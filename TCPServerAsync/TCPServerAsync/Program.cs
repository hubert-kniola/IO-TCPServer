using System;
using ServerLibrary;

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
            ServerClass serv = new ServerClassAPM();
            serv.Server();

            Console.ReadKey();
        }
    }
}