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
        private TwitchBotDemo TwitchBotDemoService;
        private readonly object thisLock = new object();

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
            Thread ChatBot = new Thread(new ThreadStart(ChatBot_Thread));
            Thread ChatBot2 = new Thread(new ThreadStart(ChatBot_Thread2));
            ChatBot.IsBackground = true; //메인 쓰레드가 종료되면 백그라운드 쓰레드가 작업을 중지하고 종료한다.
            ChatBot.Start();
            ChatBot2.Start();

            //하드코딩 수정 예정

            return RedirectToAction("Index","ChatBot");
        }


        public IActionResult ChatBotStop()
        {

            return RedirectToAction("Index", "ChatBot");
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
                _broadcasterName = HttpContext.Request.Cookies["channel_name"]; //스트리머 채널 이름 dada4202 //lock section 
            }

            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotDemoService = new TwitchBotDemo(_botName, _broadcasterName, _twitchOAuth);

            TwitchBotDemoService.BotStart();
        }

        public void ChatBot_Thread2()
        {
            string _botName = "Hotteok"; //채팅봇 이름
            string _broadcasterName = "wlh9566"; //스트리머 채널 이름
            string _twitchOAuth = "oauth:rmme7sswerrdkrt81140rlfjvra8l6"; //투나빵셔틀(호떡) access_token

            TwitchBotDemoService = new TwitchBotDemo(_botName, _broadcasterName, _twitchOAuth);

            TwitchBotDemoService.BotStart();
        }
    }
}
