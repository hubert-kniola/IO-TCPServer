using System;
using ClassLibrary1;

namespace TCPServer
{
    class Program
    {
        /// <summary>
        /// Funkcja główna aplikacji - wywołanie programu
        /// </summary>
        /// <param name="args">Parametr główny</param>
        static void Main(string[] args)
        {
            ServerClass serv = new ServerClass();
            serv.Server();

            Console.ReadKey();
        }
    }
}