Import-Module ..\bin\Debug\netstandard2.0\Perry.dll -Force

Update-MarkdownHelp -Path $PSScriptRoot -UpdateInputOutput
Update-MarkdownHelpModule -Path  $PSScriptRoot -RefreshModulePage
