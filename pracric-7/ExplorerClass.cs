/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace pracric_7
{
    internal class ExplorerClass
    {
        public string[] File(string[] v) *//*имя файла и массив *//*
        {
            for (int i = 0; i < v.Length; i++)
                v[i] = System.IO.Path.GetFileName(v[i]);
            return v;
        }
        static string EmptyString(int len) *//*очистка после сдвига*//*
        {
            string s = "";
            for (int i = 0; i < len; i++)
                s += " ";
            return s;
        }
        static int EntersCount(string v) *//*начало меню*//*
        {
            int i = 0;
            foreach (char x in v)
                if (x == '\n')
                    i++;
            return i;
        }
        public string Back(string v) *//*кнопка назад*//*
        {
            int i = v.Length - 1;
            while (v[i] != '\\')
                i--;
            if (i > 0)
                return v.Substring(0, i);
            return v;
        }
        static Drivers(Double FreeSpace, Double AllSpace) *//*Инфо о дисках*//*
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                FreeSpace = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                AllSpace = drive.TotalSize / 1024 / 1024 / 1024;
                return FreeSpace + AllSpace;
            }
        }
    }*/