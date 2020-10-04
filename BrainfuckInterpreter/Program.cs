﻿using System;
using System.IO;

namespace BrainfuckInterpreter
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                return End("Usage: BrainfuckInterpreter.exe <filename> [memorysize: int]");
            }

            var filename = args[0];

            if (!File.Exists(filename))
            {
                return End("File not found");
            }

            var size = 0;

            if (args.Length > 1 && !int.TryParse(args[1], out size))
            {
                return End("Usage: BrainfuckInterpreter.exe <filename> [memorysize: int]");
            }

            new Interpreter(filename, size).Start();

            return End("Program finished", 0);
        }

        static int End(string message, int code = 1)
        {
            Console.WriteLine(message);

            return code;
        }
    }
}
