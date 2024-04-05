using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HW_Client_4
{
    internal class Program
    {
        static IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        async static Task Main(string[] args)
        {
            Console.Write("Enter recive port: ");
            if (!int.TryParse(Console.ReadLine(), out var recPort)) return;
            Console.Write("Enter send port: ");
            if (!int.TryParse(Console.ReadLine(), out var sendPort)) return;
            Console.WriteLine("\n");



            await sendMsg();

            async Task sendMsg()
            {
                using (UdpClient sender = new UdpClient(recPort))
                {
                    string send = "";
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"\t\tRound: {i}");
                        while (true)
                        {
                            Console.WriteLine("Enter your choose: Rock, paper, scissors");
                            send = Console.ReadLine();
                            if (send.ToLower() == "rock" || send.ToLower() == "paper" || send.ToLower() == "scissors")
                            {
                                break;
                            }
                        }
                        byte[] bytes = Encoding.UTF8.GetBytes(send);
                        sender.Send(bytes, bytes.Length, new IPEndPoint(ipAddr, sendPort));
                        var res = await sender.ReceiveAsync();
                        string answer = Encoding.UTF8.GetString(res.Buffer);
                        Console.WriteLine(answer);
                    }
                    sender.Close();
                }
            }
        }
    }
}
