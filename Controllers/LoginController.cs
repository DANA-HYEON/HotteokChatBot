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
                string Toekn_Info = OauthLoginService.Get_Token(); //토큰 요청
                JObject Root = JObject.Parse(Toekn_Info); //string to json

                string Access_Token = Root["access_token"].ToString();

                //쿠키에 액세스토큰 저장
                CookieOptions Options = new CookieOptions();
                Response.Cookies.Append("access_token", Access_Token);

                //로그인 인증정보 요청
                string User = OauthLoginService.Get_User(Access_Token);
                JObject 
            }
           
            return View();
        }
    }
}
