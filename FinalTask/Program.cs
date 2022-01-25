using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string fPath = "C:/_МОЯ/Студенты/Students.dat";

            DeserializeList(fPath);

            Console.WriteLine("Для завершения работы программы нажмите любую клавишу");
            Console.ReadKey();
        }


        static void DeserializeList(string fPath)
        {
            var students = new Student[] { };

            // Вытащить список студентов из файла

            using (FileStream fs = new FileStream(fPath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    students = (Student[])formatter.Deserialize(fs);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Не удалось выполнить десериализацию. Причина: " + e.Message);
                }
            }

            // Создать на рабочем столе директорию Students.

            string newPath = @"/Users/Ольга/Desktop/Students";

            DirectoryInfo dirInfo = new DirectoryInfo(newPath);

            try
            {
                if (dirInfo.Exists)
                {
                    dirInfo.Delete(true); // Удаление со всем содержимым
                }

                dirInfo.Create(); // Создать папку
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании каталога Students: {ex.Message}");
            }

            // Раскидать всех студентов из файла по группам (каждая группа - отдельный текстовый файл),
            // в файле группы студенты перечислены построчно в формате "Имя, дата рождения".

            foreach (Student aStudent in students)
            {
                try
                {
                    var fileInfo = new FileInfo(newPath + "/" + aStudent.Group + ".txt");

                    if (fileInfo.Exists)
                    {
                        using (StreamWriter sw = fileInfo.AppendText()) // Добавляем запись
                        {
                            sw.WriteLine($"{aStudent.Name}, {aStudent.DateOfBirth.ToString("dd.MM.yyyy")}");
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = fileInfo.CreateText()) // Создаем файл
                        {
                            sw.WriteLine($"{aStudent.Name}, {aStudent.DateOfBirth.ToString("dd.MM.yyyy")}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи данных студента: {ex.Message}");
                }

            }

        }
    }
}
