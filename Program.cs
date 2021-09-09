using System;
using System.IO;

namespace Loto
{  
    public class Number
    {
        public int num;
        public int count;
        public Number(int num)
        {
            this.num = num;
            this.count = 0;
        }

        public Number()
        {
            this.num = 0;
            this.count = 0;
        }
    }

    class Program
    {
        static string LinkLotoFile = "https://www.pais.co.il/Lotto/lotto_resultsDownload.aspx";
        static string path = "Lotto.csv";

        public static Number[] SortNumbers(Number[] numbers)
        {
            Number temp;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int k = 0; k < numbers.Length - 1; k++)
                {
                    if (numbers[k].count > numbers[k + 1].count)
                    {
                        temp = numbers[k + 1];
                        numbers[k + 1] = numbers[k];
                        numbers[k] = temp;
                    }
                }
            }

            return numbers;
        }

        public static int GetNumber(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return 0;
            }
        }

        static void Main(string[] args)
        {
            Number[] MainNumbers = new Number[37];

            for (int i = 0; i < 37; i++)     
            {
                MainNumbers[i] = new Number(i + 1);
            }

            Number[] StrongNumbers = new Number[7];

            for (int i = 0; i < 7; i++)
            {
                StrongNumbers[i] = new Number(i + 1);
            }

            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(LinkLotoFile,path);
            }

            string[] lines = System.IO.File.ReadAllLines(path);

            Console.WriteLine("\n\nStart\n\n");

            foreach(string line in lines)
            {
                string[] columns = line.Split(',');

                for (int i = 2; i < 9; i++)
                {
                    int tempNum = GetNumber(columns[i]);
                    
                    if (tempNum == 0)
                    {

                    }
                    else if (i != 8) 
                    {
                        MainNumbers[tempNum - 1].count++;
                    }
                    else
                    {
                        StrongNumbers[tempNum - 1].count++;
                    }
                }
                if (columns[1].CompareTo("03/01/2012") == 0) { break;} // "03/01/2012"
            }

            Console.WriteLine("\n\nMain Numbers: \n\n");

            MainNumbers = SortNumbers(MainNumbers);

            for (int i = 0; i < MainNumbers.Length; i++)
            {
                Console.WriteLine(MainNumbers[i].num + "\t:\t" + MainNumbers[i].count);
            }

            Console.WriteLine("\n\nStrong Numbers: \n\n");

            StrongNumbers = SortNumbers(StrongNumbers);

            for (int i = 0; i < StrongNumbers.Length; i++)
            {
                Console.WriteLine(StrongNumbers[i].num + "\t:\t" + StrongNumbers[i].count);
            }

            File.Delete(path);

            Console.WriteLine("\n\nDone\n\n");
            Console.ReadLine();
        }
    }
}
