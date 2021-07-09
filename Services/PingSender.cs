using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotteokChatBot.Services
{
    public class PingSender
    {
        private IrcClient _irc; //트위치 connect 연결
        private Thread pingSender;


        //thread만드는 기본 생성자
        public PingSender(IrcClient irc)
        {
            _irc = irc;
            pingSender = new Thread(new ThreadStart(this.Run));
        }

        //thread 시작
        public void Start()
        {
            pingSender.IsBackground = true; //메인 쓰레드가 종료되면 백그라운드 쓰레드가 작업을 중지하고 종료한다.
            pingSender.Start();
        }


        // 서버에 ping하여 이 봇이 채팅에 계속 연결되어 있는지 확인
        // 서버는 pong하여 이 봇에 응답
        // 형식: ":tmi.twitch.tv PONG tmi.twitch.tv :irc.twitch.tv"
        public void Run()
        {
            while (true)
            {
                _irc.SendIrcMessage("PING irc.twitch.tv");
                Thread.Sleep(300000); // 5 분 주기로 전송
            }
        }
    }

}

