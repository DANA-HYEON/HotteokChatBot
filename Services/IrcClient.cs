using System;
using System.IO;
using System.Net.Sockets;

namespace HotteokChatBot.Services
{
    // Reference: https://www.youtube.com/watch?v=Ss-OzV9aUZg
    public class IrcClient
    {
        private string userName; //채팅봇 이름
        private string channel; //join할 채널 이름
        private string password; //oauth token(호떡)

        private TcpClient _tcpClient; //클라이언트에서는 서버에 연결 요청을 하는 역할
                                      //서버에서는 클라이언트의 요청을 수락하면 클라이언트와의 통신에 사용할 수 있는 TcpClient의 인스턴스가 반환
        private StreamReader _inputStream;
        private StreamWriter _outputStream;


        /// <summary>
        /// 트위치로 연결
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="channel"></param>
        public IrcClient(string ip, int port, string userName, string password, string channel)
        {
            try
            {
                this.userName = userName;
                this.channel = channel;

                _tcpClient = new TcpClient(ip, port);
                _inputStream = new StreamReader(_tcpClient.GetStream());
                _outputStream = new StreamWriter(_tcpClient.GetStream());

                // 채널로 Join요청
                _outputStream.WriteLine("PASS " + password); //oauth token
                _outputStream.WriteLine("NICK " + userName); //Twitch username(login name)
                _outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
                _outputStream.WriteLine("JOIN #" + channel);
                _outputStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendIrcMessage(string message)
        {
            try
            {
                _outputStream.WriteLine(message);
                _outputStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// join한 채널로 메시지 전송
        /// </summary>
        /// <param name="message"></param>
        public void SendPublicChatMessage(string message)
        {
            try
            {
                string toSend = (":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
                _outputStream.WriteLine(toSend);
                _outputStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// join한 채널로부터 메시지 읽기
        /// </summary>
        /// <returns></returns>
        public string ReadMessage()
        {
            try
            {
                string message = _inputStream.ReadLine();
                return message;
            }
            catch (Exception ex)
            {
                return "Error receiving message: " + ex.Message;
            }
        }
    }
}
