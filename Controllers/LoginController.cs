using HotteokChatBot.Data;
using HotteokChatBot.Models;
using HotteokChatBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Controllers
{
    public class LoginController : Controller
    {
        private OauthLogin oauthLogin;

        //객체 주입 
        public LoginController(OauthLogin oauthLoginService)
        {
            oauthLogin = oauthLoginService;
        }


        public IActionResult Index()
        {
            string Access_Token = HttpContext.Request.Cookies["access_token"];
            if(Access_Token != null)
            {
                return RedirectToAction("Index", "ChatBot");
            }

            return View();
        }

       
       [Route("/Login/Login"),HttpGet]
       public IActionResult Login()
        {
            string Access_Token = HttpContext.Request.Cookies["access_token"];
            if (Access_Token != null)
            {
                return RedirectToAction("Index", "ChatBot");
            }


            //url에서 ?code= 값 가져오기
            string Code = Request.Query["code"];

            //코드값이 없으면
            if (string.IsNullOrEmpty(Code))
            {
                string Redirected_Url = oauthLogin.Get_AuthorizeCode(); //트위치에 인증코드 요청
                return Redirect(Redirected_Url); //받은 리디렉션 url로 Redirect
            }
            else 
            {
                //토큰 요청
                JObject token = oauthLogin.Get_Token(Code);

                Access_Token = token["access_token"].ToString();
                string Refresh_Token = token["refresh_token"].ToString();

                //로그인 인증정보 요청
                JObject User = oauthLogin.Get_User(Access_Token);

                long User_Id = User["user_id"].ToObject<long>(); //트위치 external key
                string User_Login = User["login"].ToString(); //channel name
                
                using (var db = new HotteokDbContext())
                {
                    var user = db.User.FirstOrDefault(u => u.User_Id.Equals(User_Id));
                    if(user == null)
                    {
                        User model = new User();
                        model.User_Id = User_Id;
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
                Response.Cookies.Append("User_Id", User["user_id"].ToString()); //쿠키에 enxternal key 저장

                return RedirectToAction("Index","ChatBot");
            }
        }

        public IActionResult Logout()
        {
            //쿠키 삭제
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("channel_name");
            Response.Cookies.Delete("User_Id");

            return RedirectToAction("Index", "Home");
        }


    }
}
