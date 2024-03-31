using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;

namespace ErrorWarningFilter
{
    class Program
    {
        private static string inputPath; // Объявляем поле inputPath
        private static string outputPath; // Объявляем поле outputPath
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Введите команду: input, output, check, cleanOut");
                string command = Console.ReadLine();

                if (!string.IsNullOrEmpty(command))
                {
                    // Обработка команды
                    switch (command.ToLower())
                    {
                        case "input":
                            inputPath = PathProgram.GetValidDirectoryPath("Введите путь к входному каталогу:");
                            break;
                        case "output":
                            outputPath = PathProgram.GetValidDirectoryPath("Введите путь к выходному каталогу:");
                            break;
                        case "check":
                            TexFilter texFilter = new TexFilter();
                            texFilter.CopyErrorMessages(inputPath, outputPath);
                            break;
                        case "cleanOut":
                            try
                            {
                                // Проверяем, введена ли папка
                                if (outputPath != null)
                                {
                                    // Удаляем все файлы в папке
                                    string[] files = Directory.GetFiles(outputPath);
                                    foreach (string file in files)
                                    {
                                        File.Delete(file);
                                    }

                                    // Удаляем все подкаталоги и их содержимое
                                    string[] directories = Directory.GetDirectories(outputPath);
                                    foreach (string directory in directories)
                                    {
                                        Directory.Delete(directory, true);
                                    }

                                    Console.WriteLine($"Содержимое папки {outputPath} успешно очищено.");
                                }
                                else
                                {
                                    Console.WriteLine($"Укажите путь output. Запустите команду output и введите путь каталога");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при очистке папки {outputPath}: {ex.Message}");
                            }
                            break;

                        default:
                            Console.WriteLine("Команда не распознана. Повторите команду.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Команда введена с ошибкой. Повторите команду.");
                }
            }

        }
    }
}