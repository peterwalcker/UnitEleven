using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkBot.Utilites
{
    internal static class Count
    {
        public static int GetCount(string str)
        {
            try
            {
                return str.Length;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.Message);
                throw new Exception("Неверные входящие данные.");
            }
            
        }
    }
}
