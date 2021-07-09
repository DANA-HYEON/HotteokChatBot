using System;

namespace HotteokChatBot.Services
{
    public class TwitchBot
    {
        // Bot settings
        private string _botName;//채팅봇 이름
        private string _broadcasterName; //스트리머 채널 이름
        private string _twitchOAuth; //oauth 코드

        public TwitchBot(string _botName, string _broadcasterName, string _twitchOAuth)
        {
            this._botName = _botName;
            this._broadcasterName = _broadcasterName;
            this._twitchOAuth = _twitchOAuth;
        }


        public void BotStart()
        {
            // 트위치로 연결
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, _botName, _twitchOAuth, _broadcasterName);

            PingSender ping = new PingSender(irc);
            ping.Start();


            //프로그램이 종료될때까지 챗방 읽기
            while (true) //연결중인걸 파악할 수 있는 조건 으로 돌릴수 있게 코드 수정 예정
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
}
