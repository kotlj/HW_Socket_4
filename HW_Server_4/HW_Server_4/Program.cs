using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HW_Server_4
{
    internal class UDPServ
    {
        static IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        async static Task Main(string[] args)
        {
            Console.WriteLine("Enter port:\n");
            int port = int.Parse(Console.ReadLine());
            Dictionary<int, int> playersPort = new Dictionary<int, int>();
            Dictionary<int, string> playersChoise = new Dictionary<int, string>();
            Dictionary<int, int> playersWins = new Dictionary<int, int>()
            {
                {0, 0}
            };
            try
            {
                using (UdpClient udpClient = new UdpClient(port))
                {
                    Console.WriteLine("Connected!");
                    var res1 = await udpClient.ReceiveAsync();
                    var res2 = await udpClient.ReceiveAsync();
                    string figure1 = Encoding.UTF8.GetString(res1.Buffer);
                    string figure2 = Encoding.UTF8.GetString(res2.Buffer);
                    playersPort.Add(res1.RemoteEndPoint.Port, 1);
                    playersPort.Add(res2.RemoteEndPoint.Port, 2);
                    playersChoise.Add(playersPort[res1.RemoteEndPoint.Port], figure1);
                    playersChoise.Add(playersPort[res2.RemoteEndPoint.Port], figure2);
                    playersWins.Add(playersPort[res1.RemoteEndPoint.Port], 0);
                    playersWins.Add(playersPort[res2.RemoteEndPoint.Port], 0);
                    
                    int win = compareFig(figure1, figure2);
                    playersWins[win] += 1;
                    StringBuilder answer = new StringBuilder();
                    answer.AppendLine($"Player 1 hands: {playersChoise[1]}!\nAnd Player 2 hands: {playersChoise[2]}!");
                    if (win == 0)
                    {
                        answer.AppendLine("And its draw!");
                    }
                    else
                    {
                        answer.AppendLine($"Player{win} win this round!");
                    }
                    answer.AppendLine($"\t\tScore: \tPlayer 1: {playersWins[1]}\t\tPlayer 2: {playersWins[2]}\n\t\tDraws: {playersWins[0]}");
                    byte[] bytes = Encoding.UTF8.GetBytes(answer.ToString());
                    udpClient.SendAsync(bytes, bytes.Length, res1.RemoteEndPoint);
                    udpClient.SendAsync(bytes, bytes.Length, res2.RemoteEndPoint);
                    for (int i = 0; i < 4; i++)
                    {
                        answer.Clear();


                        res1 = await udpClient.ReceiveAsync();
                        res2 = await udpClient.ReceiveAsync();
                        figure1 = Encoding.UTF8.GetString(res1.Buffer);
                        figure2 = Encoding.UTF8.GetString(res2.Buffer);
                        playersChoise[playersPort[res1.RemoteEndPoint.Port]] = figure1;
                        playersChoise[playersPort[res2.RemoteEndPoint.Port]] = figure2;
                        answer.AppendLine($"Player 1 hands: {playersChoise[1]}!\nAnd Player 2 hands: {playersChoise[2]}!");

                        if (playersPort[res1.RemoteEndPoint.Port] == 1)
                        {
                            win = compareFig(figure1, figure2);
                        }
                        else
                        {
                            win = compareFig(figure2, figure1);
                        }
                        if (win == 0)
                        {
                            answer.AppendLine("And its draw!");
                        }
                        else
                        {
                            answer.AppendLine($"Player{win} win this round!");
                        }
                        playersWins[win] += 1;
                        answer.AppendLine($"\t\tScore: \tPlayer 1: {playersWins[1]}\t\tPlayer 2: {playersWins[2]}\n\t\tDraws: {playersWins[0]}");
                        bytes = Encoding.UTF8.GetBytes(answer.ToString());
                        udpClient.SendAsync(bytes, bytes.Length, res1.RemoteEndPoint);
                        udpClient.SendAsync(bytes, bytes.Length, res2.RemoteEndPoint);

                    }
                    udpClient.Close();
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        static int compareFig(string fig1, string fig2)
        {
            if (fig1.ToLower().Equals(fig2.ToLower()))
            {
                return 0;
            }
            else if (fig1.ToLower() == "rock" && fig2.ToLower() == "scissors" || fig1.ToLower() == "scissors" && fig2.ToLower() == "paper" ||
                 fig1.ToLower() == "paper" && fig2.ToLower() == "rock")
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }
}
