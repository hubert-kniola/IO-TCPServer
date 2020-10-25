using System;
using ServerLibrary;

namespace TCPServer
{
    class Program
    {
        /// <summary>
        /// Funkcja główna aplikacji - wywołanie programu
        /// Imie i nazwisko: Hubert Knioła
        /// Grupa: I2-2
        /// Index: 139644
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