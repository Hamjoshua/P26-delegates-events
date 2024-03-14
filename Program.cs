using System;
using app.Matrix.Extensions;
using app.Matrix;
using app.Chain;

namespace program
{
    

    public class Program
    {
        public static void Main(string[] args)
        {
            string inputString;
            ChainApplication app = new ChainApplication();            

            do
            {
                Console.WriteLine("Введите операцию. /help для вывода всех допустимых операций");
                inputString = Console.ReadLine();
                if(inputString == "/help")
                {
                    PrintHelp();
                }
                else
                {
                    app.Run(inputString);                    
                }
            }            
            while (!string.IsNullOrEmpty(inputString));            
        }

        private static void PrintHelp()
        {
            Console.WriteLine("create - создание матрицы со случайно сгенерированными элементами 3х3. Пример: ");
            Console.WriteLine("transpose [] - транспонирование матрицы");
            Console.WriteLine("diagonalize [] - приведение матрицы к диагональному виду");
            Console.WriteLine("discriminant [] - дискриминант матрицы");
            Console.WriteLine("trace [] - след матрицы");
            Console.WriteLine("reverse [] - обратная матрица (при отрицательном дискриминанте просто выводит матрицу)");
            Console.WriteLine("[1] +\\-\\* [2] - операция между матрицами");
            Console.WriteLine("[1] /\\* 2 - операция между матрицей и числом");
        }
    }

}
