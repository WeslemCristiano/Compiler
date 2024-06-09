using System;
using System.Collections.Generic;

namespace Compiler
{
    class Program
    {
        public static void Main(string[] args)
        {
            var interpreter = new Interpreter();
            Console.WriteLine("Calculadora de expressões aritméticas do Tianex!, digite 'exit' para sair.");
            string? command;

            do
            {
                Console.Write(">");
                command = Console.ReadLine();
                if (!string.IsNullOrEmpty(command))
                {
                    try
                    {
                        string output = interpreter.Exec(command);
                        Console.WriteLine(output);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }
            } while (!string.IsNullOrEmpty(command));

            Console.WriteLine("Encerrando o interpretador. Até mais!");
        }
    }
}
