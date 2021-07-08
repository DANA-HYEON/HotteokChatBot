using HotteokChatBot.Data;
using HotteokChatBot.Models;
using HotteokChatBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotteokChatBot.Controllers
{
    public class ChatBotController : Controller
    {
        private TwitchBot TwitchBotService;
        private readonly object thisLock = new object(); //critical section lock

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
            TwitchBot a = ChatBot_Thread();
            TwitchBot b = ChatBot_Thread2();

            Thread ChatBot = new Thread(new ThreadStart(a.BotStart));
            Thread ChatBot2 = new Thread(new ThreadStart(b.BotStart));
            ChatBot.IsBackground = true; //메인 쓰레드가 종료되면 백그라운드 쓰레드가 작업을 중지하고 종료한다.
            ChatBot.Start();
            ChatBot2.Start();

            TempData["entry_status"] = "start";

            //하드코딩 수정 예정
            return RedirectToAction("Manager","ChatBot");
        }


        public IActionResult ChatBotStop()
        {
            TwitchBot a = ChatBot_Thread();
            a.BotStop();

            TempData["entry_status"] = "stop";

            return RedirectToAction("Manager", "ChatBot");
        }


        /// <summary>
        /// 채팅봇 실행
        /// </summary>
        public TwitchBot ChatBot_Thread()
        {
            string _broadcasterName;

            string _botName = "Hotteok"; //채팅봇 이름

            lock (thisLock)
            {
                _broadcasterName = HttpContext.Request.Cookies["channel_name"]; //스트리머 채널 이름 //lock section 
            }

            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotService = new TwitchBot(_botName, _broadcasterName, _twitchOAuth);

            return TwitchBotService;
            //TwitchBotService.BotStart();
        }

        public TwitchBot ChatBot_Thread2()
        {
            string _botName = "Hotteok"; //채팅봇 이름
            string _broadcasterName = "wlh9566"; //스트리머 채널 이름
            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotService = new TwitchBot(_botName, _broadcasterName, _twitchOAuth);

            return TwitchBotService;
            //TwitchBotService.BotStart();
        }
    }
}
