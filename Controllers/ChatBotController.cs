using HotteokChatBot.Data;
using HotteokChatBot.Models;
using HotteokChatBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotteokChatBot.Controllers
{



    public class ChatBotController : Controller
    {
        private TwitchBot TwitchBotService;
        private readonly object thisLock = new object(); //critical section lock
        private CancellationTokenSource tokensource; //task 취소 토큰 생성

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Manager()
        {
            return View();
        }



        public IActionResult ChatBotStart()
        {
            tokensource = new CancellationTokenSource();
            CancellationToken ct = tokensource.Token;

            //Task ChatBot = Task.Run(ChatBot_Thread);
            //Task ChatBot2 = Task.Run(ChatBot_Thread2);

            var task1 = Task.Factory.StartNew(ChatBot_Thread, ct);
            var task2 = Task.Factory.StartNew(ChatBot_Thread2, ct);
            //Thread ChatBot = new Thread(new ThreadStart(ChatBot_Thread));
            //Thread ChatBot2 = new Thread(new ThreadStart(ChatBot_Thread2));

            //int Threadid = ChatBot.ManagedThreadId;
            //ThreadArray.Add(Threadid, ChatBot);

            //ChatBot.IsBackground = true; //메인 쓰레드가 종료되면 백그라운드 쓰레드가 작업을 중지하고 종료한다.
            //ChatBot.Start();
            //ChatBot2.Start();

            TempData["entry_status"] = "start";

            //하드코딩 수정 예정
            return RedirectToAction("Manager","ChatBot");
        }


        public IActionResult ChatBotStop()
        {
            //로그아웃 기능 개발
            /*            Process proc = Process.GetCurrentProcess();
                        ProcessThreadCollection ptc = proc.Threads;
                        Console.WriteLine("현재 프로세스에서 실행중인 스레드 수 : " + ptc.Count);*/

            tokensource.Cancel();

            TempData["entry_status"] = "stop";

            return RedirectToAction("Manager", "ChatBot");
        }


        /// <summary>
        /// 채팅봇 실행
        /// </summary>
        public void ChatBot_Thread()
        {
            string _broadcasterName;

            string _botName = "Hotteok"; //채팅봇 이름

            lock (thisLock)
            {
                _broadcasterName = HttpContext.Request.Cookies["channel_name"]; //스트리머 채널 이름 //lock section 
            }

            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotService = new TwitchBot(_botName, _broadcasterName, _twitchOAuth);
            TwitchBotService.BotStart();
        }

        public void ChatBot_Thread2()
        {
            string _botName = "Hotteok"; //채팅봇 이름
            string _broadcasterName = "wlh9566"; //스트리머 채널 이름
            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotService = new TwitchBot(_botName, _broadcasterName, _twitchOAuth);
            TwitchBotService.BotStart();
        }
    }
}
