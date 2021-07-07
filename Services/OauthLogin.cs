using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotteokChatBot.Services
{
    public class OauthLogin
    {
        string Client_Id = "sxcnv1raabwrh5m720t5dhk1xll9rp";
        string Client_Secret = "2j11j1hvdzhx2rfhtcdtalw0oyi5rw";
        string Redirect_Uri = "https://localhost:44383/Login/Login";
        string Scope = "user:edit chat:read chat:edit";

        
        /// <summary>
        /// access_token을 얻기위한 인증코드 얻기
        /// </summary>
        /// <returns></returns>
        public string Get_AuthorizeCode()
        {
            string Url = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={Client_Id}&redirect_uri={Redirect_Uri}&scope={Scope}";

            WebRequest request = WebRequest.Create(Url);
            request.Method = "GET";
            StringBuilder sb = new StringBuilder();
            using(WebResponse response = request.GetResponse())
            {
                sb.Append(response.ResponseUri);
            }

            //redirection_url로 code가 온다
            return sb.ToString();
        }


        /// <summary>
        /// access_token, refresh_token 얻기
        /// </summary>
        /// <returns></returns>
        public JObject Get_Token(string Code)
        {
            var Wb = new WebClient();
            var Data = new NameValueCollection();
            string Url = "https://id.twitch.tv/oauth2/token";

            Data["client_id"] = Client_Id;
            Data["client_secret"] = Client_Secret;
            Data["code"] = Code;
            Data["grant_type"] = "authorization_code";
            Data["redirect_uri"] = Redirect_Uri;

            var Response = Wb.UploadValues(Url, "POST", Data);
            string Token = Encoding.UTF8.GetString(Response);

            //stirng to json
            return JObject.Parse(Token);
        }


        /// <summary>
        /// access_token을 이용해 트위치로부터 로그인 인증정보 요청
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <returns></returns>
        public JObject Get_User(string Access_Token)
        {
            string url = "https://id.twitch.tv/oauth2/validate";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + Access_Token;

            string user = string.Empty;

            using (WebResponse response = request.GetResponse())
            using (Stream dataStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(dataStream))
            {
                user = reader.ReadToEnd();
            }

            //string to json
            return JObject.Parse(user);
        }
    }
}
