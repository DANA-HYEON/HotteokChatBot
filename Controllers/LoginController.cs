﻿using HotteokChatBot.Data;
using HotteokChatBot.Models;
using HotteokChatBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Controllers
{
    public class LoginController : Controller
    {
        private OauthLogin OauthLoginService;

        public LoginController()
        {
            OauthLoginService = new OauthLogin();
        }


        public IActionResult Index()
        {
            return View();
        }


       public IActionResult Login()
        {
            //url에서 ?code= 값 가져오기
            string Code = Request.Query["code"];

            //코드값이 없으면
            if (string.IsNullOrEmpty(Code))
            {
                string Redirected_Url = OauthLoginService.Get_AuthorizeCode(); //트위치에 인증코드 요청
                return Redirect(Redirected_Url); //받은 리디렉션 url로 Redirect
            }
            else 
            {
                //토큰 요청
                JObject token = OauthLoginService.Get_Token(Code);

                string Access_Token = token["access_token"].ToString();
                string Refresh_Token = token["refresh_token"].ToString();

                //로그인 인증정보 요청
                JObject User = OauthLoginService.Get_User(Access_Token);

                string User_Id = User["user_id"].ToString(); //트위치 external key
                string User_Login = User["login"].ToString(); //channel name
                
                using (var db = new HotteokDbContext())
                {
                    var user = db.User.FirstOrDefault(u => u.User_Id.Equals(User_Id));
                    if(user == null)
                    {
                        User model = new User();
                        model.User_Id = long.Parse(User_Id);
                        model.User_login = User_Login;
                        model.Refresh_Token = Refresh_Token;

                        db.User.Add(model); //insert
                        db.SaveChanges(); //commit
                    }
                }

                //쿠키에 액세스토큰 저장
                Response.Cookies.Append("access_token", Access_Token);
                //CookieOptions Options = new CookieOptions();
                Response.Cookies.Append("channel_name", User_Login); //쿠키에 채널이름 저장
                Response.Cookies.Append("User_Id", User_Id); //쿠키에 enxternal key 저장

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
