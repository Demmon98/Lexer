﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Lexer("file.txt");
            new Lexer($"file.txt");

            Console.ReadKey();
        }
    }
}
