using System;
using System.IO;

namespace lab4_ak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Usage: file.exe <directory> [attribute1 attribute2 ...]");
            Console.WriteLine("To add an argument: +argument");
            Console.WriteLine("To remove an arugemnt: -argument");
            Console.WriteLine("Available arguments: hidden, readonly, archive, system");

            if (args.Length < 2)
            {
                Console.WriteLine("You must enter at least 2 parameters.");
                Environment.Exit(1);
            }

            string directoryPath = args[0];
            string[] attributes = new string[args.Length - 1];
            Array.Copy(args, 1, attributes, 0, args.Length - 1);

            try
            {
                string[] files;
                string searchPattern;

                // Перевірка наявності патерну у шляху
                if (Path.HasExtension(directoryPath))
                {
                    // Якщо є розширення, використовуємо каталог і патерн
                    searchPattern = Path.GetFileName(directoryPath);
                    directoryPath = Path.GetDirectoryName(directoryPath);
                    files = Directory.GetFiles(directoryPath, searchPattern);
                }
                else
                {
                    // Якщо немає розширення, використовуємо весь каталог
                    files = Directory.GetFiles(directoryPath);
                }

                foreach (string file in files)
                {
                    FileAttributes fileAttributes = File.GetAttributes(file);

                    foreach (string attribute in attributes)
                    {
                        if (attribute.StartsWith('+'))
                        {
                            // Додавання атрибуту
                            if (attribute.Contains("hidden"))
                                fileAttributes |= FileAttributes.Hidden;
                            else if (attribute.Contains("readonly"))
                                fileAttributes |= FileAttributes.ReadOnly;
                            else if (attribute.Contains("archive"))
                                fileAttributes |= FileAttributes.Archive;
                            else if (attribute.Contains("system"))
                                fileAttributes |= FileAttributes.System;
                        }
                        else if (attribute.StartsWith('-'))
                        {
                            // Видалення атрибуту
                            if (attribute.Contains("hidden"))
                                fileAttributes &= ~FileAttributes.Hidden;
                            else if (attribute.Contains("readonly"))
                                fileAttributes &= ~FileAttributes.ReadOnly;
                            else if (attribute.Contains("archive"))
                                fileAttributes &= ~FileAttributes.Archive;
                            else if (attribute.Contains("system"))
                                fileAttributes &= ~FileAttributes.System;
                        }
                    }

                    // Оновлення атрибутів файлу
                    File.SetAttributes(file, fileAttributes);
                }

                Console.WriteLine("File attributes have been changed successfully.");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}