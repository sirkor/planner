﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestProject
{
    class Program
    {
         interface ITaskManager
        {
            List<Task> Tasks { get; set; }
            void AddTask();
            void DelTask(int id);
            void EditTask(int id);
        }

        class TaskManager : ITaskManager
        {
            public List<Task> Tasks { get; set; }
            public int LastId { get; set; }
            public TaskManager(List<Task> lst)
            {
                Tasks = lst;
                if(Tasks.Count > 0)
                {
                    LastId = Tasks.Last().Id;
                }
                else
                {
                    LastId = 0;
                }
                
            }

            public void AddTask()
            {
                Console.Clear();
                Console.WriteLine("Введите название задачи:");
                string name = Console.ReadLine();
                Console.WriteLine("Введите дату начала выполнения задачи в формате дд мм гггг");
                string day = Console.ReadLine();
                string month = Console.ReadLine();
                string year = Console.ReadLine();
                DateTime dateStart = new DateTime();
                try
                {
                    dateStart = new DateTime(Convert.ToInt16(year), Convert.ToInt16(month),
                        Convert.ToInt16(day));
                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    if (e.Source != null)
                    {
                        Console.WriteLine("Проверьте вводимые данные");
                        Console.ReadLine();
                        return;
                    }
                }
                Console.WriteLine("Введите дату завершения выполнения задачи в формате дд мм гггг");
                day = Console.ReadLine();
                month = Console.ReadLine();
                year = Console.ReadLine();
                DateTime dateEnd = new DateTime();
                try
                {
                    dateEnd = new DateTime(Convert.ToInt16(year), Convert.ToInt16(month),
                        Convert.ToInt16(day));
                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    if (e.Source != null)
                    {
                        Console.WriteLine("Проверьте вводимые данные");
                        Console.ReadLine();
                        return;
                    }
                }

                Console.WriteLine("Введите описание к задаче");
                string discribe = Console.ReadLine();
                LastId += 1;
                Task newTask = new Task(name, dateStart, dateEnd, discribe, LastId);
                Tasks.Add(newTask);
            }
            public void DelTask( int id)
            {
                foreach (Task task in Tasks)
                {
                    if (task.Id == id)
                    {
                        Tasks.Remove(task);
                        Console.WriteLine("Задача была удалена");
                        break;
                    }
                }
            }
            public void EditTask(int id)
            {
                foreach (Task task in Tasks)
                {
                    if (id == task.Id)
                    {
                        char s = ' ';
                        while (s != '*')
                        {
                            Console.WriteLine("Выберите пункт для редактирования:");
                            Console.WriteLine("1 - Название");
                            Console.WriteLine("2 - Дата начала");
                            Console.WriteLine("3 - Дата окончания");
                            Console.WriteLine("4 - Описание");
                            Console.WriteLine("* - Выход в главное меню");
                            try
                            {
                                s = Convert.ToChar(Console.ReadLine());
                            }
                            catch (System.FormatException e)
                            {
                                if (e.Source != null)
                                {
                                    Console.WriteLine("Неверный формат данных");
                                }
                            }
                            switch (s)
                            {
                                case '1':
                                    Console.Clear();
                                    Console.WriteLine("Введите новое название");
                                    task.Name = Console.ReadLine();
                                    break;

                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Введите новую дату начала");
                                    string day = Console.ReadLine();
                                    string month = Console.ReadLine();
                                    string year = Console.ReadLine();
                                    DateTime dateStart = new DateTime();
                                    try
                                    {
                                        dateStart = new DateTime(Convert.ToInt16(year), Convert.ToInt16(month),
                                            Convert.ToInt16(day));
                                    }
                                    catch (System.ArgumentOutOfRangeException e)
                                    {
                                        if (e.Source != null)
                                        {
                                            Console.WriteLine("Проверьте вводимые данные");
                                            Console.ReadLine();
                                            return;
                                        }
                                    }
                                    task.DateStart = dateStart;
                                    break;

                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Введите новую дату окончания");
                                    day = Console.ReadLine();
                                    month = Console.ReadLine();
                                    year = Console.ReadLine();
                                    DateTime dateEnd = new DateTime();
                                    try
                                    {
                                        dateEnd = new DateTime(Convert.ToInt16(year), Convert.ToInt16(month),
                                            Convert.ToInt16(day));
                                    }
                                    catch (System.ArgumentOutOfRangeException e)
                                    {
                                        if (e.Source != null)
                                        {
                                            Console.WriteLine("Проверьте вводимые данные");
                                            Console.ReadLine();
                                            return;
                                        }
                                    }
                                    task.DateEnd = dateEnd;
                                    break;

                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Введите новое описание");
                                    task.Discribe = Console.ReadLine();
                                    break;
                            }
                        }
                    }
                }
                
            }
        }

        [Serializable]
        class Task
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public string Discribe { get; set; }

            public Task(string name, DateTime dateStart, DateTime dateEnd, string discribe, int id)
            {
                Name = name;
                DateStart = dateStart;
                DateEnd = dateEnd;
                Discribe = discribe;
                Id = id;
            }

            public override string ToString()
            {
                return string.Format("Номер: {0}\nНазвание: {1}\nДата начала: {2}\nДата окончание: {3}" +
                    "\nОписание: {4}", Id, Name, DateStart, DateEnd, Discribe);
            }

        }

        [Serializable]
        class TaskSaver
        {
            public List<Task> Tasks { get; set; }
            string FileName { get; set; }

            public TaskSaver(List<Task> tasks, string fileName)
            {
                Tasks = tasks;
                FileName = Directory.GetCurrentDirectory() + "/" + fileName;
            }

            public void SaveToFile()
            {
                BinaryFormatter format = new BinaryFormatter();
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    format.Serialize(fs, Tasks);
                    Console.WriteLine("Список сохранён");
                }
            }
            public List<Task> LoadFromFile()
            {
                Tasks.Clear();
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    return (List<Task>)formatter.Deserialize(fs);

                }
            }

            public int GetLastId()
            {
                try
                {
                    return Tasks.Last().Id;
                }
                catch (System.InvalidOperationException e)
                {
                    if (e.Source!=null)
                    {
                        return 1;
                    }
                    else
                    {
                        return Tasks.Last().Id;
                    }
                }
            }
        }
        

    

        
        static void Main(string[] args)
        {

            List<Task> tasks = new List<Task>();
            TaskManager TManager = new TaskManager(tasks); 
            TaskSaver ts = new TaskSaver(TManager.Tasks, "inf.dat");
            char c=' ';
            while (c!='*')
                {
                Console.Clear();
                Console.WriteLine("Выберите пункт меню");
                Console.WriteLine("1 - Просмотреть задачи");
                Console.WriteLine("2 - Добавить задачу");
                Console.WriteLine("3 - Редактировать задчу");
                Console.WriteLine("4 - Удалить задачу");
                Console.WriteLine("5 - Сохранить список задач в файл");
                Console.WriteLine("6 - Загрузить список из файла");
                Console.WriteLine("* - Выйти из программы");
                try { 
                c = Convert.ToChar(Console.ReadLine());
                }
                catch (System.FormatException e)
                {
                    if (e.Source!=null)
                    {
                        Console.WriteLine("Неверный формат ввода");
                    }
                }
                switch (c)
                {
                    case '1':
                        Console.Clear();
                        foreach (Task task in TManager.Tasks )
                        {
                            Console.WriteLine("------------------");
                            Console.WriteLine(task);
                        }
                        Console.ReadLine();
                        break;
                    case '2':
                        Console.Clear();
                        TManager.AddTask();
                        break;

                    case '3':
                        Console.Clear();
                        Console.WriteLine("Введите номер изменяемого элемента:");
                        int editNum = Convert.ToInt32(Console.ReadLine());
                        TManager.EditTask(editNum);
                        break;

                    case '4':
                        Console.Clear();
                        Console.WriteLine("Введите номер удаляемого элемента:");
                        int delNum = Convert.ToInt16(Console.ReadLine());
                        TManager.DelTask(delNum);
                        break;

                    case '5':
                        Console.Clear();
                        ts.Tasks = TManager.Tasks;
                        ts.SaveToFile();
                        break;

                    case '6':
                        Console.Clear();
                        TManager.Tasks = ts.LoadFromFile();
                        TManager.LastId = TManager.Tasks.Last().Id;
                        break;
                }
            }
        }
    }
}
