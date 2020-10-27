using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ServerLibrary
{
    /// <summary>
    /// Klasa odpowiedzialna za poprawne działanie serwera - połączenie, odebranie, odesłanie
    /// </summary>
    public abstract class ServerClass
    {
        #region Fields
        /// <summary>
        /// Rozmiar tablicy bajtów - buffer
        /// </summary>
        protected static int sizeOfBuffer { get; set; } = 1024;
        /// <summary>
        /// Zmienna, do której przypisywana jest liczba podana przez klienta
        /// </summary>
        protected int n = default;
        /// <summary>
        /// Zmienna, do której przypisywana jest liczba podana przez klienta
        /// </summary>
        protected static int x { get; set; } = default;
        /// <summary>
        /// Numer portu, na którym serwer ma zostać uruchomiony
        /// </summary>
        protected static Int32 port { get; set; } = 1024;
        /// <summary>
        /// Wiadomość odebrana przez serwer od klienta
        /// </summary>
        protected string text { get; set; } = default;
        /// <summary>
        /// Zmienna potwierdzająca, że dana wartość jest liczbą
        /// </summary>
        protected bool checkMessage { get; set; } = default;
        /// <summary>
        /// Obiekt wykorzystywany przez klasę ServerClass to wykonywania niezbędnych operacji
        /// </summary>
        protected TcpListener tcpServer { get; set; } = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        /// <summary>
        /// Tablica bajtów przechowująca dane odebrane od klienta
        /// </summary>
        protected byte[] buffer { get; set; } = new byte[1024];
        #endregion

        #region Methods
        /// <summary>
        /// Metoda klasy ServerClass odpowiedzialna za sprawdzenie czy użytkownik chce rozłączyć się z serwerem
        /// </summary>
        /// <param name="text">Tekst podawany przez użytkownika</param>
        /// <returns></returns>
        protected bool checkWord(string text)
        {
            if (text[0] == 'e' && text[1] == 'x' && text[2] == 'i' && text[3] == 't')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metoda klasy ServerClass odpowiedzialna za obsługę działania serwera
        /// </summary>
        public abstract void Server();
        #endregion
    }
}

