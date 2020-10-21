using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClassLibrary1
{
    /// <summary>
    /// Klasa odpowiedzialna za poprawne działanie serwera - połączenie, odebranie, odesłanie
    /// </summary>
    public class ServerClass
    {
        /// <summary>
        /// Rozmiar tablicy bajtów - buffer
        /// </summary>
        static int sizeOfBuffer { get; set; } = 1024;
        /// <summary>
        /// Zmienna, do której przypisywana jest liczba podana przez klienta
        /// </summary>
        int n = default;
        /// <summary>
        /// Zmienna, do której przypisywana jest liczba podana przez klienta
        /// </summary>
        static int x { get; set; } = default;
        /// <summary>
        /// Numer portu, na którym serwer ma zostać uruchomiony
        /// </summary>
        static Int32 port { get; set; } = 1024;
        /// <summary>
        /// Wiadomość odebrana przez serwer od klienta
        /// </summary>
        string text { get; set; } = default;
        /// <summary>
        /// Zmienna potwierdzająca, że dana wartość jest liczbą
        /// </summary>
        bool checkMessage { get; set; } = default;
        /// <summary>
        /// Obiekt wykorzystywany przez klasę ServerClass to wykonywania niezbędnych operacji
        /// </summary>
        TcpListener tcpServer { get; set; } = new TcpListener(IPAddress.Parse("127.0.0.1"), port);


        /// <summary>
        /// Tablica bajtów przechowująca dane odebrane od klienta
        /// </summary>
        byte[] buffer { get; set; } = new byte[1024];
        /// <summary>
        /// Wiadomość o treści "Connected with server!" wysyłana przez serwer do klienta
        /// </summary>
        byte[] message1 { get; set; } = new ASCIIEncoding().GetBytes("Connected with server!");
        /// <summary>
        /// Wiadomość o treści "Enter one number (int): " wysyłana przez serwer do klienta
        /// </summary>
        byte[] message2 { get; set; } = new ASCIIEncoding().GetBytes("Enter one number (int): ");
        /// <summary>
        /// Wiadomość o treści "Disconnected!" wysyłana przez serwer do klienta
        /// </summary>
        byte[] message3 { get; set; } = new ASCIIEncoding().GetBytes("Disconnected!");
        /// <summary>
        /// Wiadomość o treści "n * 2: " wysyłana przez serwer do klienta
        /// </summary>
        byte[] message4 { get; set; } = new ASCIIEncoding().GetBytes("n * 2: ");
        /// <summary>
        /// Wiadomość stanowiąca formę przeniesienia do nowej linii w konsoli klienta
        /// </summary>
        byte[] message5 { get; set; } = new ASCIIEncoding().GetBytes(" ");

        /// <summary>
        /// Metoda klasy ServerClass odpowiedzialna za sprawdzenie czy użytkownik chce rozłączyć się z serwerem
        /// </summary>
        /// <param name="text">Tekst podawany przez użytkownika</param>
        /// <returns></returns>
        bool checkWord(string text)
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
        public void Server()
        {
            try
            {
                tcpServer.Start();

                Console.WriteLine("Waiting for a connection... ");
                Console.WriteLine("Server adress: " + tcpServer.LocalEndpoint);

                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                NetworkStream stream = tcpClient.GetStream();
                Console.WriteLine("Connected with client!");
                tcpClient.GetStream().Write(message1, 0, message1.Length);
                tcpClient.GetStream().Write(message2, 0, message2.Length);
                while (true)
                {
                    tcpClient.GetStream().Read(buffer, 0, sizeOfBuffer);
                    text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    checkMessage = int.TryParse(text, out n);
                    if (checkMessage)
                    {
                        Calculations calc = new Calculations();
                        x = calc.printResult(n);
                        string tempMessage = x.ToString();
                        byte[] recMessage = new ASCIIEncoding().GetBytes(tempMessage);
                        tcpClient.GetStream().Write(message4, 0, message4.Length);
                        tcpClient.GetStream().Write(recMessage, 0, recMessage.Length);
                        tcpClient.GetStream().Write(message5, 0, message5.Length);
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                    else if (checkWord(text))
                    {
                        tcpClient.GetStream().Write(message3, 0, message3.Length);
                        Array.Clear(buffer, 0, buffer.Length);
                        break;
                    }
                }
                tcpClient.Close();
                Console.WriteLine("Disconnected with client!");
            }
            catch (ArgumentNullException e)
            {
                TextWriter errWriter = Console.Error;
                errWriter.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                TextWriter errWriter = Console.Error;
                errWriter.WriteLine("SocketException: {0}", e);
            }
        }
    }
}

