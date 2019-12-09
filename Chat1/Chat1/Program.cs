using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chat1
{
    class Program
    {
        static string remoteAddress;
        static int localPort;
        static int remotePort;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите порт для прослушивания:");
                localPort = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Введите удаленный адрес для подключения:");
                remoteAddress = Console.ReadLine();
                Console.WriteLine("Введите порт для подключения:");
                remotePort = Int32.Parse(Console.ReadLine());
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void SendMessage()
        {
            UdpClient sender = new UdpClient();
            try
            {
                while(true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, remoteAddress, remotePort);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
               sender.Close();
            }
        }
        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort);
            IPEndPoint remoteIP = null;
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIP);
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine("Собеседник: {0}", message);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
