using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SampleRunner
{
    using System;
    using System.IO;
    using NameValidation;
    class Program
    {
        static void Main(string[] args)
        {
            var inputParser = new Parser();
            var registryParser = new RegistryParser();
            var validator = new Validator(new[]
            {
                new [] {"BOB", "ROB", "ROBERT"},
                new [] {"DEBBIE", "DEBORAH", "DEBRA"},
                new [] {"BECKY", "REBECCA", "BECKI"},
                new [] {"CATHY", "CATHERINE", "KATHY", "KATE", "KATHERINE"},
                new [] {"CINDY", "CYNTHIA", "CINDI", "CYNDY"},
                new [] {"SUE", "SUSAN", "SUZANNE", "SUZANNA", "SUSANNA"},
                new [] {"KRISTY", "CHRISTINE"},
                new [] {"NIKI", "NIKKI", "NICOLE"},
                new [] {"SANDRA", "SANDY", "CASANDRA"},
                new [] {"BETH", "ELIZABETH", "BETTIE", "BETTY"},
                new [] {"WILLIAM", "BILL","BILLY"},
                new [] {"JUDY", "JUDITH"},
                new [] {"CONSTANCE", "CONNIE"}
            });

            var total = 0;
            var totalProcessed = 0;
            var totalFailed = 0;
            var regex = new Regex("\\\"(.*?)\\\"");

            using (var stream = new FileStream(@"C:\Users\christopher.hyne\Desktop\roe_report_data.csv", FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    total++;
                    var line = reader.ReadLine();

                    if (line == null)
                    {
                        continue;
                    }

                    line = regex.Replace(line, m => m.Value.Replace(',', ' '));

                    var entries = line.Split(",".ToCharArray());

                    if (entries.Length != 4)
                    {
                        Console.WriteLine(line);
                        continue;
                    }

                    var result = validator.Validate(inputParser.Parse(entries[0], entries[1]),
                        registryParser.Parse(entries[2], entries[3]));

                    totalProcessed++;

                    if (IsFail(result.BestSurnameResult) && IsFail(result.BestGivenNameResults.OrderByDescending(r => r.Proximity).FirstOrDefault()))
                    {
                        var reversed = validator.Validate(inputParser.Parse(entries[1], entries[0]),
                            registryParser.Parse(entries[2], entries[3]));

                        if (IsFail(reversed.BestSurnameResult) && IsFail(reversed.BestGivenNameResults.OrderByDescending(r => r.Proximity).FirstOrDefault()))
                        {
                            Console.WriteLine(line);
                            totalFailed++;
                        }
                    }

                }
            }

            Console.WriteLine($"Total: {total}; Total Processed: {totalProcessed}; Total Failed: {totalFailed}");
            Console.Write("Done! Press ENTER.");
            Console.ReadLine();
        }

        private static bool IsFail(ValidationResult result)
        {
            if (result == null)
            {
                return true;
            }

            return result.Proximity < 50 && !(result.NameFromRegistry?.Contains(result.NameFromInput) ?? false);
        }
    }
}
