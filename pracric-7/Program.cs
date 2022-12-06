namespace pracric_7
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    namespace terminalExplorer
    {
        class Program
        {
            static string path;
            static string[] files, Folder;
            static int len = 1;
            static List<string> prevPath = new List<string>(len + 1);
            static void OpenPath(string p, bool prev = true) /*переходы в меню*/
            {
                if (p != "" && prev)
                {
                    if (prevPath.Count == len)
                        prevPath.RemoveAt(0);
                    prevPath.Add(p);
                    
                }
                if (p == "")
                {
                    Console.Clear();
                    path = menu(System.IO.Directory.GetLogicalDrives(), "-----------------Проводник-----------------"); /*диски*/
                    
                    if (prev)
                    {
                        if (prevPath.Count == len)
                            prevPath.RemoveAt(0);
                        prevPath.Add(path);
                    }
                    
                    files = File(System.IO.Directory.GetFiles(path)); /*показ файлов*/
                    Folder = File(System.IO.Directory.GetDirectories(path)); /*показ папок*/

                }
                else
                {
                    path = p;
                    while (path[path.Length - 1] == '\\')
                        path = path.Remove(path.Length - 1);
                    if (path.Length < 3) path += '\\';
                    
                    try
                    {
                        files = File(System.IO.Directory.GetFiles(path));
                        Folder = File(System.IO.Directory.GetDirectories(path));
                    }
                    catch
                    {
                        if (prevPath.Count != 0)
                            prevPath.RemoveAt(prevPath.Count - 1);
                        if (prevPath.Count == 0) OpenPath("");
                        else OpenPath(prevPath[prevPath.Count - 1]);
                    }
                }
                
            }

            static void Main(string[] args) /*директории*/
            {
                
                if (args.Length > 0 && System.IO.Directory.Exists(args[0]))
                    OpenPath(args[0]);
                else
                    OpenPath("");
                while (true)
                {
                    int selected = 0, last = 0, totalLength = Folder.Length + files.Length;
                    bool rewrite = false;
                    Console.Title = path;
                    Console.Clear();
                    Console.WriteLine("=====Папки======\n");
                    for (int i = 0; i < Folder.Length; i++)
                        Console.WriteLine((i == selected ? "      ->" : "") + Folder[i] + (i == selected ? " :<-" : ""));
                    Console.WriteLine("\n=====Файлы======\n");
                    for (int i = 0; i < files.Length; i++)
                        Console.WriteLine((selected >= Folder.Length && i == selected - Folder.Length ? "      ->" : " ") + files[i] + (selected >= Folder.Length && i == selected - Folder.Length ? "<-" : ""));
                    while (true)
                    {
                        ConsoleKeyInfo clavisha = Console.ReadKey(true);
                        if (clavisha.Key == ConsoleKey.DownArrow)
                        {
                            selected++;
                            if (selected > totalLength - 1)
                                selected = 0;
                        }
                        else if (clavisha.Key == ConsoleKey.UpArrow)
                        {
                            selected--;
                            if (selected < 0)
                                selected = totalLength - 1;
                        }
                        else if (clavisha.Key == ConsoleKey.Enter)
                        {
                            if (selected < Folder.Length)
                            {
                                OpenPath((path.Length > 3) ? path + '\\' + Folder[selected] : path + Folder[selected]);
                                break;
                            }
                            else
                                try
                                {
                                    System.Diagnostics.Process.Start(path + '\\' + files[selected - Folder.Length]);
                                }
                                catch { }
                        }
                        else if (clavisha.Key == ConsoleKey.Escape)
                        {
                            if (path.Length > 3)
                            {
                                OpenPath(Back(path));
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                OpenPath("");
                                break;
                            }
                        }

                        else if (clavisha.Key == ConsoleKey.Tab)
                        {
                            bool ex = false;
                            string podmenu = menu(new string[] { "Назад", "Путь", "информация", "Copy", "открыть в проводнике", "Выход мз приложения" }, "для выбора нажмите ентер\n");
                            {
                                if (podmenu == "Назад")
                                    ex = true;
                                else if (podmenu == "Путь")
                                {
                                    Console.Clear();
                                    ex = true;
                                    Console.WriteLine(path);
                                    Console.Write("для выхода нажмите на любую клавишу");
                                    Console.ReadKey(true);
                                }
                                else if (podmenu == "информация")
                                {
                                    Console.Clear();
                                    ex = true;
                                    if (selected < Folder.Length)
                                    {
                                        Console.WriteLine(path + "\\" + Folder[selected]);
                                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path + "\\" + Folder[selected]);
                                        Console.WriteLine("Создано : " + di.CreationTime.ToLongDateString() + "  " + di.CreationTime.ToLongTimeString());
                                        Console.Write("для выхода нажмите на любую клавишу");
                                        Console.ReadKey(true);
                                    }
                                    else
                                    {

                                        Console.WriteLine(path + "\\" + files[selected - Folder.Length]);
                                        System.IO.FileInfo di = new System.IO.FileInfo(path + /*"\\" +*/ files[selected - Folder.Length]);
                                        float Gb = (float)(di.Length / 1024.0 / 1024.0); /*перевод в гигабайты*/
                                        Console.WriteLine("Размер : " + di.Length.ToString() + " Байт " + (Gb < 921.6 ? (Gb >= 0.9216) ? "(" + (Gb).ToString() + " Мб)" : "(" + (Gb / 1024.0).ToString() + " Гб)" : ")"));
                                        Console.WriteLine("Создано : " + di.CreationTime.ToLongDateString() + "  " + di.CreationTime.ToLongTimeString());
                                        Console.Write("для выхода нажмите на любую клавишу");
                                        Console.ReadKey(true);
                                    }
                                }

                                else if (podmenu == "открыть в проводнике")
                                {
                                    ex = true;
                                    System.Diagnostics.Process.Start(path + '\\' + Folder[selected]);
                                }

                                else if (podmenu == "Выход мз приложения")
                                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                            }
                            if (ex)
                                break;
                        }
                        else
                        {
                            if (selected < Folder.Length)
                            {
                                for (int n = (selected < Folder.Length - 1) ? selected + 1 : 0; n < Folder.Length; n++)
                                {
                                    if (Char.ToLower(Folder[n][0]) == Char.ToLower(clavisha.KeyChar))
                                        if (Char.ToLower(Folder[n][0]) == Char.ToLower(clavisha.KeyChar))
                                        {
                                            selected = n;
                                            break;
                                        }
                                        else if (n == Folder.Length - 1)
                                        {
                                            for (int m = 0; m < Folder.Length; m++)
                                                if (Char.ToLower(Folder[m][0]) == Char.ToLower(clavisha.KeyChar))
                                                {
                                                    selected = m;
                                                    break;
                                                }
                                        }

                                }
                            }
                            else
                            {

                                for (int n = (selected - Folder.Length < files.Length - 1) ? selected - Folder.Length + 1 : Folder.Length; n < files.Length; n++)
                                {
                                    if (Char.ToLower(files[n][0]) == Char.ToLower(clavisha.KeyChar))
                                    {
                                        selected = Folder.Length + n;
                                        break;
                                    }
                                    else if (n == files.Length - 1)
                                    {
                                        for (int m = 0; m < files.Length; m++)
                                            if (Char.ToLower(files[m][0]) == Char.ToLower(clavisha.KeyChar))
                                            {
                                                selected = m + Folder.Length;
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                        if (last == selected && !rewrite)
                        {
                            continue;
                        }
                        if (selected < Folder.Length)
                        {
                            Console.SetCursorPosition(35, 2);
                            Console.Write("Назад - Escape");
                            Console.SetCursorPosition(35, 3);
                            Console.Write("Меню - Tab");
                            if (last < Folder.Length)
                            {
                                Console.SetCursorPosition(0, last + 2);
                                Console.WriteLine(EmptyString(Folder[last].Length + 21)); /*-------------------------------------------------------------------------*/
                                Console.SetCursorPosition(0, last + 2);
                                Console.Write(Folder[last]);
                            }
                            else
                            {
                                Console.SetCursorPosition(0, last + 5);
                                Console.WriteLine(EmptyString(files[last - Folder.Length].Length + 21));
                                Console.SetCursorPosition(0, last + 5);
                                Console.Write(files[last - Folder.Length]);
                            }
                            Console.SetCursorPosition(0, selected + 2);
                            Console.Write("      ->: " + Folder[selected] + " :<-");
                        }
                        else
                        {
                            if (last < Folder.Length)
                            {
                                Console.SetCursorPosition(0, last + 2);
                                Console.WriteLine(EmptyString(Folder[last].Length + 21));
                                Console.SetCursorPosition(0, last + 2);
                                Console.Write(Folder[last]);
                            }
                            else
                            {
                                Console.SetCursorPosition(0, last + 5);
                                Console.WriteLine(EmptyString(files[last - Folder.Length].Length + 21));
                                Console.SetCursorPosition(0, last + 5);
                                Console.Write(files[last - Folder.Length]);
                            }
                            Console.SetCursorPosition(0, selected + 5);
                            Console.Write("      ->: " + files[selected - Folder.Length] + " :<-");
                        }
                        last = selected;
                    }

                }
            }


            static string menu(string[] items, string title) /*меню*/
            {
                Console.Clear();
                
                Console.WriteLine(title);
                int enterCount = EntersCount(title) + 1;
                
                int selected = 0, last = 0;
                for (int i = 0; i < items.Length; i++)
                    Console.WriteLine((i == selected ? "  ->" : "") + items[i] + (i == selected ? "" : ""));
                while (true)
                {
                    Console.SetCursorPosition(35, 2);
                    Console.Write("Назад - Escape");
                    Console.SetCursorPosition(35, 3);
                    Console.Write("Меню - Tab");

                    ConsoleKeyInfo clavusha = Console.ReadKey(true);
                    if (clavusha.Key == ConsoleKey.DownArrow)
                    {
                        selected++;
                        if (selected > items.Length - 1)
                            selected = 0;
                    }
                    else if (clavusha.Key == ConsoleKey.UpArrow)
                    {
                        selected--;
                        if (selected < 0)
                            selected = items.Length - 1;
                    }
                    else if (clavusha.Key == ConsoleKey.Enter)
                    {
                        if (selected == -1)
                            return "";
                        else return items[selected];
                    }
                    if (selected != last)
                    {
                        Console.SetCursorPosition(35, 2);
                        Console.Write("Назад - Escape");
                        Console.SetCursorPosition(35, 3);
                        Console.Write("Меню - Tab");
                        Console.SetCursorPosition(0, enterCount + last);
                        Console.Write(EmptyString(12 + items[last].Length));
                        Console.SetCursorPosition(0, enterCount + last);
                        Console.Write(items[last]);
                        Console.SetCursorPosition(0, enterCount + selected);
                        Console.Write("  ->" + items[selected]);
                    }
                    last = selected;
                }
            }
            /*static int menu2(string[] items, string title)
            {
                Console.Clear();
                Console.WriteLine(title);
                int enterCount = entersCount(title) + 1;
                int selected = 0, last = 0;
                for (int i = 0; i < items.Length; i++)
                    Console.WriteLine((i == selected ? "  ->" : "") + items[i] + (i == selected));
                while (true)
                {
                    ConsoleKeyInfo ck = Console.ReadKey(true);
                    if (ck.Key == ConsoleKey.DownArrow)
                    {
                        selected++;
                        if (selected > items.Length - 1)
                            selected = 0;
                    }
                    else if (ck.Key == ConsoleKey.UpArrow)
                    {
                        selected--;
                        if (selected < 0)
                            selected = items.Length - 1;
                    }
                    else if (ck.Key == ConsoleKey.Enter)
                    {
                        return selected;
                    }
                    if (selected != last)
                    {
                        Console.SetCursorPosition(0, enterCount + last);
                        Console.Write(emptyString(12 + items[last].Length));
                        Console.SetCursorPosition(0, enterCount + last);
                        Console.Write(items[last]);
                        Console.SetCursorPosition(0, enterCount + selected);
                        Console.Write("  ->" + items[selected] + "<-");
                    }
                    last = selected;
                }
            }*/
            static string[] File(string[] v) /*имя файла и массив */
            {
                for (int i = 0; i < v.Length; i++)
                    v[i] = System.IO.Path.GetFileName(v[i]);

                return v;
            }
            static string EmptyString(int len) /*очистка после сдвига*/
            {
                string s = "";
                for (int i = 0; i < len; i++)
                    s += " ";
                return s;
            }
            static int EntersCount(string v) /*начало меню*/
            {
                int i = 0;
                foreach (char x in v)
                    if (x == '\n')
                        i++;
                return i;
            }
            static string Back(string v) /*кнопка назад*/
            {
                int i = v.Length - 1;
                while (v[i] != '\\')
                    i--;
                if (i > 0)
                    return v.Substring(0, i);
                return v;
            }
            static void Drivers() /*Инфо о дисках*/
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    Double FreeSpace = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                    Double AllSpace = drive.TotalSize / 1024 / 1024 / 1024;
                    
                }
            }
        }
    }



}
