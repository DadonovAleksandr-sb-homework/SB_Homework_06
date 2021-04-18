using System;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;

namespace Homework_06
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Домашнее задание
            Разработайте программу, которая будет разбивать числа от 1 до N на группы, 
            при этом числа в каждой отдельно взятой группе не делятся друг на друга. 
            Число N хранится в файле, поэтому его необходимо сначала оттуда прочитать. 
            Это число может изменяться от единицы до одного миллиарда.

            После получения числа N необходимо начать поиск групп неделящихся друг на друга чисел. 
            Сделать это можно различными способами. Например, для N = 50 группы могут получиться такими:

            Группа 1: 1.
            Группа 2: 2 3 5 7 11 13 17 19 23 29 31 37 41 43 47.
            Группа 3: 4 6 9 10 14 15 21 22 25 26 33 34 35 38 39 46 49.
            Группа 4: 8 12 18 20 27 28 30 42 44 45 50.
            Группа 5: 16 24 36 40.
            Группа 6: 32 48.

            А для N = 10, такими:

            Группа 1: 1.
            Группа 2: 2 7 9.
            Группа 3: 3 4 10.
            Группа 4: 5 6 8.

            Кроме распределения чисел реализуйте возможность получить только количество групп. 
            После получения групп их надо записать на диск. 
            Если пользователь захотел рассчитать только их количество, записывать его на диск не нужно, достаточно вывести на экран.

            После записи групп на диск необходимо спросить пользователя, хочет ли он поместить файл с группами в архив. 
            В случае положительного ответа заархивируйте этот файл и выведите информацию о его размере до и после архивации.

            Советы и рекомендации

            Заметьте, что группы можно рассчитать совершенно разными способами, при этом неважно, 
            какой способ расчёта вы выбрали ― до тех пор, пока числа в группах не делятся друг на друга, задача считается решённой.
            Для расчёта только количества групп необязательно проходить все числа. Попробуйте сделать этот расчёт с помощью формулы.
            При N равному миллиарду вам может не хватить оперативной памяти для хранения всех групп. Тогда стоит отказаться от их хранения.
            Не стоит использовать цикл тройной вложенности, так как это решение будет слишком медленным.
            После архивации расширение исходного файла может потеряться. Стоит предусмотреть этот момент.
            Если расчёт для миллиарда чисел идёт более 20 минут (с поправкой на слабое оборудование), вам стоит поменять алгоритм их поиска.
            Задачу можно решить с помощью одного цикла и без использования массивов.

            Что оценивается

            Число N прочитано из файла. Если данного числа там нет, а также если оно выходит за рамки заданного диапазона или не может быть прочитано, 
            пользователю выводится сообщение об ошибке.
            Группы чисел рассчитаны, при этом в каждой группе находятся только те числа, которые не делятся друг на друга.
            Пользователю предлагается выбрать: рассчитать все группы или только посмотреть их количество для заданного N.
            После расчётов группы чисел записываются в файл, по строке на группу.
            Пользователю предлагается поместить файл с рассчитанными группами в архив. При его положительном ответе архив сформирован, 
            а статистика по размеру обоих файлов выведена на экран.
            Расчёт групп для N = 1_000_000_000 не должен превышать 20 минут, какое бы оборудование ни использовалось.
                                    
            */

            Console.WriteLine("Разработайте программу, которая будет разбивать числа от 1 до N на группы,\n"+ 
                "при этом числа в каждой отдельно взятой группе не делятся друг на друга.\n"+ 
                "Число N хранится в файле, поэтому его необходимо сначала оттуда прочитать.\n"+ 
                "Это число может изменяться от единицы до одного миллиарда.");
            var data = string.Empty;
            if(!GetDataFromFile("number.txt", out data))
            {
                Console.ReadKey();
                return;
            }
            var number = 0;
            if (!ParseStringToInt(data, out number))
            {
                Console.ReadKey();
                return;
            }
            if(!CheckNumberLimits(number, 1, 1_000_000_000))
            {
                Console.ReadKey();
                return;
            }
            // расчет кол-ва взимно неделимых групп
            var groupsCnt = GetGroupCount(number);
            Console.WriteLine($"Для числа {number} кол-во групп взаимно неделимых чисел: {groupsCnt}");
            if (CalcGroupsUserChoice())
            {
                var startDate = DateTime.Now;
                var fileName = "result.txt";
                SaveToFile(number, fileName);
                var timeSpan = DateTime.Now.Subtract(startDate);
                Console.WriteLine($"Расчет занял {timeSpan.TotalSeconds} секунд");
                if (ArchivingUserChoise(fileName))
                {
                    Compress(fileName, "result.zip");
                }
                        
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Чтение данных из файла.
        /// </summary>
        /// <returns />
        private static bool GetDataFromFile(string fileName, out string data)
        {
            data = String.Empty;
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл {fileName} не существует!");
                return false;
            }
            data = File.ReadAllText(fileName);
            return true;
        }
        
        /// <summary>
        /// Проверка, что строковые данные являются числом
        /// </summary>
        /// <param name="data"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool ParseStringToInt(string data, out int number)
        {
            number = 0;
            if (int.TryParse(data, out int num))
            {
                number = num;
                return true;
            }
            else
            {
                Console.WriteLine($"Данные не возможно интерпретировать как число!");
                return false;
            }
        }
        
        /// <summary>
        /// Проверка числа на соответствие диапазону
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool CheckNumberLimits(int number, int min, int max)
        {
            if (number >= min && number <= max)
            {
                return true;
            }
            Console.WriteLine($"Введенное число имеет не корректное значение: {number}");
            return false;
        }
        
        /// <summary>
        /// Выбор пользователем алгоритма работы
        /// </summary>
        public static bool CalcGroupsUserChoice()
        {
            var result = false;
            do
            {
                Console.Write("Рассчитать группы? (д/н): ");
                var userChoice = Console.ReadLine().Trim().ToLower();
                if (userChoice == "д")
                {
                    result = true;
                    break;
                }
                else if(userChoice == "н")
                {
                    result = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Невозможно распознать ответ. Пожалуйста введите \"д\" или \"н\".");
                }
            } while (true);
            
            return result;
        }
        
        /// <summary>
        /// Выбор пользователем необходимости архивации файла
        /// </summary>
        /// <returns></returns>
        private static bool ArchivingUserChoise(string fileName)
        {
            var result = false;
            do
            {
                Console.Write($"Архивировать файл {fileName}? (д/н): ");
                var userChoice = Console.ReadLine().Trim().ToLower();
                if (userChoice == "д")
                {
                    result = true;
                    break;
                }
                else if(userChoice == "н")
                {
                    result = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Невозможно распознать ответ. Пожалуйста введите \"д\" или \"н\".");
                }
            } while (true);

            return result;
        }
        
        /// <summary>
        /// Вычисление необходимого кол-ва групп для разложения числа на взаимно некратные числа 
        /// </summary>
        /// <param name="number">Исходное число</param>
        /// <returns>Кол-во групп</returns>
        public static int GetGroupCount(int number)
        {
            return (int)Math.Ceiling(Math.Log(number, 2));
        }
        
        /// <summary>
        /// Сохранение в файл
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fileName"></param>
        private static void SaveToFile(int number, string fileName)
        {
            var groupCnt = GetGroupCount(number);
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine($"Результат для N = {number}");
                for (int i = 1; i <= groupCnt; i++)
                {
                    sw.Write($"Группа {i}:");
                    var startNum = GetStartOfGroup(i);
                    var endNum = GetEndOfGroup(i);
                    for (int j = startNum; j <= endNum; j++)
                    {
                        sw.Write($" {j}");
                    }
                    sw.WriteLine();
                }
            }    
        }

        private static int GetEndOfGroup(int groupNum)
        {
            var result = (int)Math.Pow(2, groupNum);
            return result - 1;
        }

        private static int GetStartOfGroup(int groupNum)
        {
            var result = (int)Math.Pow(2, groupNum-1);
            return result;
        }
        
        /// <summary>
        /// Архивация файла
        /// </summary>
        /// <param name="fileName"></param>
        private static void Compress(string sourceFileName, string compressedFileName)
        {
            using (FileStream ss = new FileStream(sourceFileName, FileMode.OpenOrCreate))
            {
                using (FileStream ts = File.Create(compressedFileName))   // поток для записи сжатого файла
                {
                    // поток архивации
                    using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                    {
                        ss.CopyTo(cs); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Было: {1}  стало: {2}.",
                            sourceFileName,
                            ss.Length,
                            ts.Length);
                    }
                }
            }
        }

    }
}
