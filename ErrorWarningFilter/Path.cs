using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorWarningFilter;
namespace ErrorWarningFilter
{
    public class PathProgram
    {
        public static string GetValidDirectoryPath(string message)
        {
            string path;
            bool validPath = false;

            do
            {
                Console.WriteLine(message);
                path = Console.ReadLine();


                if (Directory.Exists(path))
                {
                    validPath = true;
                    if (message.Length == 34) Directory.CreateDirectory(path += @"\output");
                    path = CreateNewFolderIfNeeded(path);
                }
                else
                {
                    Console.WriteLine("Указанный путь не существует.");

                    Console.WriteLine("Хотите создать полный путь и папки каталога? (Y/N)");

                    string input = Console.ReadLine();
                    if (input.ToUpper() == "Y")
                    {
                        try
                        {
                            if (message.Length == 34) Directory.CreateDirectory(path += @"\output");
                            Directory.CreateDirectory(path);
                            validPath = true;
                            Console.WriteLine("\nПапки успешно созданы.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nОшибка при создании папок: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nВведите путь каталога повторно."); //Возврат к вводу пути каталога.
                    }
                }

            } while (!validPath);

            Console.WriteLine("\nПуть к каталогу успешно считан.");
            return path;
        }
        static string CreateNewFolderIfNeeded(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
            {

                // Проверяем наличие файлов в папке 1 способ 
                FileInfo[] files = directory.GetFiles();
                bool output = path.Contains("output");
                if (output && (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0))
                {
                    Console.WriteLine("Папка содержит файлы. Создать новую папку? (Y/N)");
                    string input = Console.ReadLine();
                    if (input.ToUpper() == "Y")
                    {
                        // Создаем новую папку с датой создания в имени
                        string newFolderName = $"{directory.Name}-{DateTime.Now:yyyy-MM-dd_HH-mm}";
                        string newPath = Path.Combine(directory.Parent.FullName, newFolderName);
                        Directory.CreateDirectory(newPath); // Создаем новую папку
                        Console.WriteLine($"Новая папка создана: {newPath}");
                        return path;
                    }
                    else
                    {
                        Console.WriteLine($"Используем папку:{path}");
                    }
                }
                else
                {
                    //Console.WriteLine("Папка не содержит файлов.");
                }
            }
            else
            {
                Console.WriteLine("Указанная папка не существует.");
            }
            return path;
        }
    }
}

