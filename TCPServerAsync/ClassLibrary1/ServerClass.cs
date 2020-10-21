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
        protected byte[] buffer1 { get; set; } = new byte[1024];
        /// <summary>
        /// Wiadomość o treści "Connected with server!" wysyłana przez serwer do klienta
        /// </summary>
        protected byte[] message1 { get; set; } = new ASCIIEncoding().GetBytes("Connected with server!");
        /// <summary>
        /// Wiadomość o treści "Enter one number (int): " wysyłana przez serwer do klienta
        /// </summary>
        protected byte[] message2 { get; set; } = new ASCIIEncoding().GetBytes("Enter one number (int): ");
        /// <summary>
        /// Wiadomość o treści "Disconnected!" wysyłana przez serwer do klienta
        /// </summary>
        protected byte[] message3 { get; set; } = new ASCIIEncoding().GetBytes("Disconnected!");
        /// <summary>
        /// Wiadomość o treści "n * 2: " wysyłana przez serwer do klienta
        /// </summary>
        protected byte[] message4 { get; set; } = new ASCIIEncoding().GetBytes("n * 2: ");
        /// <summary>
        /// Wiadomość stanowiąca formę przeniesienia do nowej linii w konsoli klienta
        /// </summary>
        protected byte[] message5 { get; set; } = new ASCIIEncoding().GetBytes(" ");

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
    }
}

