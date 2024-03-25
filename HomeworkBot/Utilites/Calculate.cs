using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkBot.Utilites
{
    internal class Calculate
    {
        public static int Sum(string str)
        {
            string[] strings = str.Split(' ');
            int sum = 0;

            try
            {
                foreach (string s in strings)
                {
                    sum += Convert.ToInt32(s);
                }
                return sum;
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
                throw new Exception("Необходимо ввести числа.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw exception;
            }
        }
    }
}
