﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Perry.Errors.Parsing;
using Perry.Options;

namespace Perry.Errors.Handlers
{
    /// <summary>
    /// Output errors to log file.
    /// </summary>
    class FileLoggingErrorHandler : IErrorHandler
    {
        public static FileLoggingErrorHandler Singleton => _singleton.Value;

        private PSCmdlet _cmdlet;
        private PerryOptions _options;

        private static bool _isInitialized;
        private static readonly object _syncInitialize = new object();
        private static readonly Lazy<FileLoggingErrorHandler> _singleton = new Lazy<FileLoggingErrorHandler>(() => new FileLoggingErrorHandler());
        private static readonly object _syncLogs = new object();

        private static readonly int _maxRolledLogCount = 3;
        private static readonly int _maxLogSize = 1024 * 1024 * 10;

        public FileLoggingErrorHandler()
        {
            if (_isInitialized) return;

            lock (_syncInitialize)
            {
                if (_isInitialized) return;
                ErrorCatcher.Singleton.ErrorEvent += ErrorHandler;
                _isInitialized = true;
            }
        }

        public void Add(PSCmdlet source, PerryOptions options)
        {
            _cmdlet = source;
            _options = options;

            if (!Directory.Exists(options.LogPath))
            {
                Directory.CreateDirectory(options.LogPath);
            }
        }

        public void Remove()
        {
            _cmdlet = null;
        }

        public void ErrorHandler(object sender, ErrorInformation errorInformation)
        {
            var cmdlet = _cmdlet;
            if (cmdlet == null || _options == null) return;

            var exceptionName = errorInformation.Exception.InnerException?.GetType().Name ?? errorInformation.Exception.GetType().FullName;

            var errorMessage = $"{DateTime.UtcNow:u} {exceptionName}{Environment.NewLine}{errorInformation.Error}";

            if (_options?.IncludeVariable == true)
            {
                var variables = VariableParser.GetVariables(cmdlet, errorInformation.Error);
                foreach (var variable in variables)
                {
                    errorMessage += $"{Environment.NewLine}   Variable: ${variable.Name} = {variable.Value}";
                }
            }

            if (_options?.IncludeException == true)
            {
                errorMessage += $"{Environment.NewLine}{errorInformation.Exception}";
            }

            var outputFile = Path.Combine(_options.LogPath, "perry.log");
            cmdlet.WriteWarning($"outputting to: {outputFile}");

            lock (_syncLogs)
            {
                try
                {
                    RollLogFiles(outputFile);
                    File.AppendAllText(outputFile, $"{errorMessage}{Environment.NewLine}", Encoding.UTF8);
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        private void RollLogFiles(string logFilePath)
        {
            try
            {
                var length = new FileInfo(logFilePath).Length;

                if (length > _maxLogSize)
                {
                    var logPath = Path.GetDirectoryName(logFilePath);
                    var wildLogName = Path.GetFileNameWithoutExtension(logFilePath) + "*" + Path.GetExtension(logFilePath);
                    var logFilePathWithoutExtension = Path.Combine(logPath, Path.GetFileNameWithoutExtension(logFilePath));

                    var logFileList = Directory.GetFiles(logPath, wildLogName, SearchOption.TopDirectoryOnly);
                    if (logFileList.Length <= 0) return;

                    // Take files like logfilename.log and logfilename.0.log, so there also can be a maximum of 10 additional rolled files (0..9)
                    var rolledLogFileList = logFileList.Where(fileName => fileName.Length == logFilePath.Length + 2).ToArray();
                    Array.Sort(rolledLogFileList, 0, rolledLogFileList.Length);
                    if (rolledLogFileList.Length >= _maxRolledLogCount)
                    {
                        File.Delete(rolledLogFileList[_maxRolledLogCount - 1]);
                        var list = rolledLogFileList.ToList();
                        list.RemoveAt(_maxRolledLogCount - 1);
                        rolledLogFileList = list.ToArray();
                    }

                    // Move remaining rolled files
                    for (int i = rolledLogFileList.Length; i > 0; --i)
                    {
                        File.Move(rolledLogFileList[i - 1], logFilePathWithoutExtension + "." + i + Path.GetExtension(logFilePath));
                    }

                    var targetPath = logFilePathWithoutExtension + ".0" + Path.GetExtension(logFilePath);
                    // Move original file
                    File.Move(logFilePath, targetPath);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
