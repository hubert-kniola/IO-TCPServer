﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerLibrary
{
    public class ServerClassAPM : ServerClass
    {
        public string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public delegate void TransmissionDataDelegate(TcpClient tcpClient);
        public ServerClassAPM(){}

        public void makeSomething(TcpClient tcpClient)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = 60000;
                Console.WriteLine("Connected with client!");


                _sendToClient(stream, "Connected with server!");
                _sendToClient(stream, "Enter login (string):");
                
                stream.Read(buffer, 0, sizeOfBuffer);
                var login = _toString(buffer);
                //var login = login1.Substring(0, login1.Length - 2);
                _sendToClient(stream, "Enter password (string):");
                stream.Read(buffer, 0, sizeOfBuffer);
                Array.Clear(buffer, 0, buffer.Length);
                stream.Read(buffer, 0, sizeOfBuffer);
                var password = _toString(buffer);
                if (_checkPass(stream, login, password))
                {
                    _sendToClient(stream, "Correct! Please get one number:");
                }
                else
                {
                    _sendToClient(stream, "Incorrect! Forced shutdown!");
                }
                
                
                while (true)
                {
                    stream.Read(buffer, 0, sizeOfBuffer);

                    text = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Trim('\0');
                    if (string.IsNullOrEmpty(text))
                    {
                        return;
                    }
                    checkMessage = int.TryParse(text, out n);
                    if (checkMessage)
                    {
                        x = Calculations.printResult(n);
                        string tempMessage = x.ToString();
                        byte[] recMessage = new ASCIIEncoding().GetBytes(tempMessage);
                        _sendToClient(stream, "n * 2 = ");
                        stream.Write(recMessage, 0, recMessage.Length);
                        _sendToClient(stream, " ");
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                    else if (checkWord(text))
                    {
                        _sendToClient(stream, "Disconnected!");
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
            catch (IOException e)
            {
                TextWriter errWriter = Console.Error;
                errWriter.WriteLine("IOException: {0}", e);
            }
        }

        public override void Server()
        {
            tcpServer.Start();

            Console.WriteLine("Waiting for a connection... ");
            Console.WriteLine("Server adress: " + tcpServer.LocalEndpoint);

            while (true)
            {
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(makeSomething);
                transmissionDelegate.BeginInvoke(tcpClient, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            Console.WriteLine("Forced shutdown!");
            Console.WriteLine("Cleaning...");
        }

        private bool _isInBase(string log)
        {
            Regex rx = new Regex(@"" + log, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string text = System.IO.File.ReadAllText(Path.Combine(docPath, "dataBase.txt"));
            if (rx.IsMatch(text) == true)
                return true;
            return false;
        }

        private string _toString(byte[] buffer)
        {
            string text = "";
            text = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Trim('\0');
            Array.Clear(buffer, 0, buffer.Length);
            return text;
        }

        private bool _checkPass(NetworkStream stream, string login, string password)
        {
            string textRegex = @"l: " + login + " p: " + password;
            var y = _isInBase(textRegex);
            if (y == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void _sendToClient(NetworkStream stream, string message) =>
            stream.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
    }
}
