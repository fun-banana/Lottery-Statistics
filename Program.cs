using System;
using System.IO;

namespace LotoEachPos
{
    class Program
    {
        static string LinkLotoFile = "https://www.pais.co.il/Lotto/lotto_resultsDownload.aspx";
        static string path = "Lotto.csv";
        static string resultPath = "Result.csv";

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

        public static int GetNumber(string str) // 0 = undefined symbol
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

        public static Number[] FillArray(Number[] numbers)
        { 
            for (int i = 0; i < 37; i++)     
            {
                numbers[i] = new Number(i + 1);
            }
            return numbers;
        }

        public static void DownloadLotoFile()
        {
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(LinkLotoFile,path);
            }
        }

        public static void WriteResult(Number[] numbers, int index)
        {
            StreamWriter sw = File.AppendText(resultPath);                
            {
                sw.Write("|" + index + "| ");
                foreach (Number number in numbers)
                {
                    sw.Write(number.num + "\t"); 
                }
                sw.WriteLine();   
            }
            sw.Close();
        }

        public static void ClearResultFile()
        {
            System.IO.File.WriteAllText(resultPath,string.Empty);
        }

        static void Main(string[] args)
        {
            DownloadLotoFile();
            ClearResultFile();
            string[] lines = System.IO.File.ReadAllLines(path);
            
            Console.WriteLine("\n\nStart\n\n");

            for (int i = 2; i < 9; i++)
            {
                Number[] numbers = FillArray(new Number[37]);

                foreach(string line in lines)
                {
                    string[] columns = line.Split(',');

                    int temp = GetNumber(columns[i]);

                    if (temp != 0)
                    {
                        numbers[temp-1].count++;
                    }

                    if (columns[1].CompareTo("03/01/2012") == 0) { break; } // "03/01/2012"
                }

                numbers = SortNumbers(numbers);
                WriteResult(numbers, i - 1);

                Console.WriteLine("\nLine: " + ( i - 1 ) + "\n");

                for (int k = 0; k < numbers.Length; k++)
                {
                    Console.WriteLine(numbers[k].num + "\t:\t" + numbers[k].count);
                }
            }

            File.Delete(path);

            Console.WriteLine("\n\nDone\n\n");
            Console.ReadLine();
        }
    }
}
