using System;
using System.Collections;
using System.Collections.Generic;

namespace Perry.Helpers
{
    public static class HashsetHelpers
    {
        public static Dictionary<string, string> ConvertToDictionary(this Hashtable hashtable)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (DictionaryEntry entry in hashtable)
            {
                var key = entry.Key as string;
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Invalid name");

                if (entry.Value != null && !(entry.Value is string))
                    throw new ArgumentException($"Invalid value: {key}");
                var value = entry.Value == null ? "" : (string)entry.Value;
                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}
