using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ErrorWarningFilter
{
    public class TexFilter
    {
        public void CopyErrorMessages(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Папка {sourceDir} не существует.");
                return;
            }

            string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                try
                {
                    if (File.Exists(file))
                    {
                        string content = File.ReadAllText(file);

                        if (!string.IsNullOrEmpty(content))
                        {
                            string errorContent = ExtractErrorContent(content);

                            if (!string.IsNullOrEmpty(errorContent))
                            {
                                string relativePath = GetRelativePath(file, sourceDir);
                                string destFile = Path.Combine(destDir, relativePath);
                                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                                File.WriteAllText(destFile, errorContent);
                                Console.WriteLine($"Сообщения об ошибках скопированы в файл {destFile}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Файл {file} пустой.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка. Файл {file} - не обработан.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке файла {file}: {ex.Message}");
                }
            }
        }
        /* не последовательно 
        private string ExtractErrorContent(string content)
        {
            StringBuilder resultBuilder = new StringBuilder();

            Regex warningRegex = new Regex(@"\[WARNING\].*?(?=\[WARNING\]|\[ERROR\]|\z)", RegexOptions.Singleline);
            Regex errorRegex = new Regex(@"\[ERROR\].*?(?=\[WARNING\]|\[ERROR\]|\z)", RegexOptions.Singleline);

            // Извлечение всех совпадений [WARNING]
            foreach (Match match in warningRegex.Matches(content))
            {
                resultBuilder.AppendLine(match.Value);
            }

            // Извлечение всех совпадений [ERROR]
            foreach (Match match in errorRegex.Matches(content))
            {
                resultBuilder.AppendLine(match.Value);
            }

            return resultBuilder.ToString();
        }
        */

        private string ExtractErrorContent(string content)
        {
            StringBuilder resultBuilder = new StringBuilder();

            Regex combinedRegex = new Regex(@"((\[WARNING\].*?)(?=\[ERROR\]|(\[WARNING\](?![\s\S]*?\[ERROR\]))|$)|(\[ERROR\].*?)(?=\[WARNING\]|(\[ERROR\](?![\s\S]*?\[WARNING\]))|$))", RegexOptions.Singleline);

            foreach (Match match in combinedRegex.Matches(content))
            {
                resultBuilder.AppendLine(match.Value);
            }

            return resultBuilder.ToString();
        }

        private string GetRelativePath(string fullPath, string basePath)
        {
            string relativePath = Path.GetRelativePath(basePath, fullPath);
            return relativePath.Replace(Path.GetFileName(basePath), "");
        }
    }
}