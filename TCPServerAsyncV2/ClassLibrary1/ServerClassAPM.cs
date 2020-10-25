using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Data.SQLite;
using DataBaseLibrary;
using static DataBaseLibrary.Data;

namespace ServerLibrary
{
    public class ServerClassAPM : ServerClass
    {
        #region Fields
        /// <summary>
        /// Przypisanie do zmiennej ścieżki do pulpitu
        /// </summary>
        public string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Tworzenie delegata
        /// </summary>
        /// <param name="tcpClient">Aktywne połączenie z klientem</param>
        public delegate void TransmissionDataDelegate(TcpClient tcpClient);
        #endregion

        #region Metods
        /// <summary>
        /// Konstruktor domyślny klasy
        /// </summary>
        public ServerClassAPM() { }
        /// <summary>
        /// Główna metoda serwera odpowiadająca za logowanie i wykonywanie operacji matematycznych
        /// </summary>
        /// <param name="tcpClient">Aktywne połączenie z klientem</param>
        public void MakeSomething(TcpClient tcpClient)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = 60000;
                Console.WriteLine("Connected with client!");

                var dataBase = new Data();
                dataBase.setConn(new SQLiteConnection("Data Source=C:\\Users\\Dell\\Desktop\\TCPServerAsyncV2\\dataBase.db"));
                dataBase.CreateDatabase();
                var listOfAccounts = dataBase.Accounts;

                _SendToClient(stream, "Connected with server!\r\n");
                _SendToClient(stream, "Enter login (string):");
                stream.Read(buffer, 0, sizeOfBuffer);
                var login = _ToString(buffer);

                //var login = login1.Substring(0, login1.Length - 2);
                _SendToClient(stream, "Enter password (string):");
                stream.Read(buffer, 0, sizeOfBuffer);
                Array.Clear(buffer, 0, buffer.Length);
                stream.Read(buffer, 0, sizeOfBuffer);
                var password = _ToString(buffer);

                if (_CheckLog(listOfAccounts, login, password))
                {
                    _SendToClient(stream, "Correct! Please get 2 numbers:\r\n");
                }
                else
                {
                    _SendToClient(stream, "Incorrect! Forced shutdown!");
                    tcpClient.Close();
                }

                while (true)
                {
                    int numberOneI = 0, numberTwoI = 0;
                    var numberOneS = _GetNumber(stream, buffer); //string
                    var checkText1 = Int32.TryParse(numberOneS, out numberOneI); //bool
                    if (!checkText1)
                        throw new IOException();

                    var numberTwoS = _GetNumber(stream, buffer); //string
                    var checkText2 = Int32.TryParse(numberTwoS, out numberTwoI); //bool

                    if (checkText1 && checkText2)
                    {
                        var intTable = Calculations.calculator(numberOneI, numberTwoI);
                        var stringTable = Calculations.printCalculator(numberOneI, numberTwoI, intTable);

                        _SendCalculator(stream, stringTable);
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                    else if (checkWord(numberOneS) || checkWord(numberTwoS))
                    {
                        _SendToClient(stream, "Disconnected!");
                        Array.Clear(buffer, 0, buffer.Length);
                        tcpClient.Close();
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
            catch (IOException e)
            {
                TextWriter errWriter = Console.Error;
                errWriter.WriteLine("IOException: {0}", e);
            }
            catch (ObjectDisposedException e)
            {
                TextWriter errWriter = Console.Error;
                errWriter.WriteLine("IOException: {0}", e);
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za utrzymanie połączenia z wszystkimi klientami
        /// </summary>
        public override void Server()
        {
            tcpServer.Start();

            Console.WriteLine("Waiting for a connection... ");
            Console.WriteLine("Server adress: " + tcpServer.LocalEndpoint);

            while (true)
            {
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(MakeSomething);
                transmissionDelegate.BeginInvoke(tcpClient, TransmissionCallback, tcpClient);
            }
        }

        /// <summary>
        /// Metoda wywoływana w momencie rozłączenia się klienta
        /// </summary>
        /// <param name="ar"></param>
        private void TransmissionCallback(IAsyncResult ar)
        {
            Console.WriteLine("Forced shutdown!");
            Console.WriteLine("Cleaning...");
        }

        /// <summary>
        /// Metoda sprawdzająca dane podane przez użytkownika
        /// </summary>
        /// <param name="list">Lista kont użytkowników</param>
        /// <param name="login">Login podany przez użytkownika</param>
        /// <param name="password">Hasło podane przez użytkownika</param>
        /// <returns></returns>
        private bool _CheckLog(List<Account> list, string login, string password)
        {
            foreach (var element in list)
            {
                if (element.Login == login && element.Password == password)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda zamieniająca ciąg bajtów na zmienną typu string
        /// </summary>
        /// <param name="stream">Strumień danych</param>
        /// <param name="buffer">Ciąg danych</param>
        /// <returns></returns>
        private string _GetNumber(NetworkStream stream, byte[] buffer)
        {
            _SendToClient(stream, "Enter number (int):");
            stream.Read(buffer, 0, sizeOfBuffer);
            stream.Read(buffer, 0, sizeOfBuffer);
            var stringNum = _ToString(buffer);
            return stringNum;
        }

        /// <summary>
        /// Metoda odpowiedzialna za wysłanie wyników operacji matematycznych do klienta
        /// </summary>
        /// <param name="stream">Strumień danych</param>
        /// <param name="tbs">Tablica stringów zawierająca wyniki obliczeń</param>
        private void _SendCalculator(NetworkStream stream, string[] tbs)
        {
            foreach (var element in tbs)
            {
                byte[] temp = new ASCIIEncoding().GetBytes(element);
                stream.Write(temp, 0, temp.Length);
            }
        }

        /// <summary>
        /// Metoda zamieniająca ciąg bajtów na zmienną typu string
        /// </summary>
        /// <param name="buffer">Ciąg danych</param>
        /// <returns></returns>
        private string _ToString(byte[] buffer)
        {
            string text = "";
            text = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Trim('\0');
            Array.Clear(buffer, 0, buffer.Length);
            return text;
        }

        /// <summary>
        /// Metoda wysyłająca wiadomość z serwera do klienta
        /// </summary>
        /// <param name="stream">Strumień danych</param>
        /// <param name="message">Wiadomośc do klienta</param>
        private static void _SendToClient(NetworkStream stream, string message) => stream.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
        #endregion

        //private bool _IsInBase(string log)
        //{
        //    Regex rx = new Regex(@"" + log, RegexOptions.Compiled);
        //    string text = System.IO.File.ReadAllText(Path.Combine(docPath, "dataBase.txt"));
        //    if (rx.IsMatch(text) == true)
        //        return true;
        //    return false;
        //}
        //private bool _CheckPass(NetworkStream stream, string login, string password)
        //{
        //    string textRegex = @"l: " + login + " p: " + password;
        //    var y = _IsInBase(textRegex);
        //    if (y == true)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }
}
