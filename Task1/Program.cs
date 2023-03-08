using System;
using System.IO;
using System.Globalization;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = @"C:\Users\Omen\Documents\transactions.csv";
            string dateFormat = "yyyy-MM-dd";
            int batchSize = 10;
            string outputFilePath = @"C:\Users\Omen\Documents\total_date.csv";

            Func<string, DateTime> getDate = input => DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Func<string, double> getAmount = input => double.Parse(input.Replace(",", "."), CultureInfo.InvariantCulture);

            Action<DateTime, double> printResult = (date, total) =>
            {
                Console.WriteLine($"{date.ToString(dateFormat)}: ${total:F2}");
            };

            Action<DateTime, double> saveResult = (date, total) =>
            {
                if (!File.Exists(outputFilePath))
                {
                    using (var writer = new StreamWriter(outputFilePath))
                    {
                        writer.WriteLine("Date - Total");
                    }
                }

                using (var writer = new StreamWriter(outputFilePath, true))
                {
                    writer.WriteLine($"{date.ToString("yyyy-MM-dd")},{total.ToString("0.00", CultureInfo.InvariantCulture)}");
                }
            };

            using (var reader = new StreamReader(inputFilePath))
            {
                double total = 0;
                DateTime currentDate = DateTime.MinValue;
                int count = 0;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] fields = line.Split(',');

                    DateTime date = getDate(fields[0]);
                    double amount = getAmount(fields[1]);

                    if (currentDate == DateTime.MinValue || currentDate != date)
                    {
                        if (currentDate != DateTime.MinValue)
                        {
                            printResult(currentDate, total);
                            saveResult(currentDate, total);
                        }

                        currentDate = date;
                        total = 0;
                    }

                    total += amount;

                    if (++count == batchSize)
                    {
                        saveResult(currentDate, total);
                        count = 0;
                    }
                }

                printResult(currentDate, total);
                saveResult(currentDate, total);

            }         
        }
    }
}
