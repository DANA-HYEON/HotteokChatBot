using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HotteokChatBot.Services
{
    public class TwitchBotDemo
    {
        // Bot settings
        private string _botName; //채팅봇 이름
        private string _broadcasterName; //스트리머 채널 이름
        private string _twitchOAuth; // oauth 코드


        public TwitchBotDemo(string _botName, string _broadcasterName, string _twitchOAuth)
        {
            this._botName = _botName;
            this._broadcasterName = _broadcasterName;
            this._twitchOAuth = _twitchOAuth;
        }

        public void BotStart()
        {
            // Initialize and connect to Twitch chat
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, _botName, _twitchOAuth, _broadcasterName);

            // 서버에 ping하여 이 봇이 채팅에 계속 연결되어 있는지 확인
            // 서버는 pong하여 이 본에 응답
            // Example: ":tmi.twitch.tv PONG tmi.twitch.tv :irc.twitch.tv"
            PingSender ping = new PingSender(irc);
            ping.Start();


            //프로그램이 종료될때까지 챗방 읽기
            while (true) //연결중인걸 파악할 수 있는 조건 으로 돌리기
            {
                // 챗방으로부터 메세지 읽기
                string message = irc.ReadMessage();
                //Console.WriteLine(message); // irc메시지 출력

                //message가 null일때 실행할 코드 삽입

                if (message.Contains("PRIVMSG"))
                {
                    //유저가보낸 메세지는 아래 형식으로 보여진다.
                    // 형식: ":[user]![user]@[user].tmi.twitch.tv PRIVMSG #[channel] :[message]"

                    // Modify message to only retrieve user and message
                    int intIndexParseSign = message.IndexOf('!');
                    string userName = message.Substring(1, intIndexParseSign - 1); // parse username from specific section (without quotes)
                                                                                   // Format: ":[user]!"
                                                                                   // Get user's message
                    intIndexParseSign = message.IndexOf(" :");
                    message = message.Substring(intIndexParseSign + 2);

                    Console.WriteLine(message); // Print parsed irc message (debugging only)

                    // 스트리머 명령어
                    if (userName.Equals(_broadcasterName))
                    {
                        if (message.Equals("!exitbot"))
                        {
                            irc.SendPublicChatMessage("Bye! Have a beautiful time!");
                            Environment.Exit(0); // Stop the program
                        }
                    }

                    // 누구나 사용할 수 있는 명령어
                    if (message.Equals("!hello"))
                    {
                        irc.SendPublicChatMessage("Hello World!");
                    }
                }
            }
        }
    }

    // Reference: https://www.youtube.com/watch?v=Ss-OzV9aUZg
    public class IrcClient
    {
        public string userName; //채팅봇 이름
        private string channel; //join할 채널 이름

        private TcpClient _tcpClient; //클라이언트에서는 서버에 연결 요청을 하는 역할
                                      //서버에서는 클라이언트의 요청을 수락하면 클라이언트와의 통신에 사용할 수 있는 TcpClient의 인스턴스가 반환
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

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

        public void SendPublicChatMessage(string message)
        {
            try
            {
                SendIrcMessage(":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

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

    //Class that sends PING to irc server every 5 minutes
    public class PingSender
    {
        private IrcClient _irc;
        private Thread pingSender;

        // Empty constructor makes instance of Thread
        public PingSender(IrcClient irc)
        {
            _irc = irc;
            pingSender = new Thread(new ThreadStart(this.Run)); //클라이언트의 소켓을 담아 쓰레드 생성
        }

        // Starts the thread
        public void Start()
        {
            pingSender.IsBackground = true; //메인 쓰레드가 종료되면 백그라운드 쓰레드가 작업을 중지하고 종료한다.
            pingSender.Start();
        }

        // Send PING to irc server every 5 minutes
        public void Run()
        {
            while (true)
            {
                _irc.SendIrcMessage("PING irc.twitch.tv");
                Thread.Sleep(300000); // 5 minutes
            }
        }
    }
}
