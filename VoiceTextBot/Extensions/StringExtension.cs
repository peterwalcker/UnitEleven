using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTextBot.Extensions
{
    internal static class StringExtension
    {
        public static string UpperCaseFirst(string str)
        {
            if(String.IsNullOrEmpty(str)) 
                return String.Empty;

            return char.ToUpper(str[0]) + str[1..];
        }
    }
}
