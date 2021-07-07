using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Models
{
    //회원
    public class User
    {
        [Key]
        public long User_Id { get; set; } //트위치 external key

        [Required]
        public string Refresh_Token { get; set; } //트위치 refresh token
        
        [Required]
        public string User_login{get;set;} //접속한 스트리머의 channel name
        
        public int Blacklist_Status { get; set; } //금칙어 사용유무
        
        public int Auto_Greet { get; set; } //자동인사 사용유무
        
        public DateTime RegDate { get; set; } //호떡 사용 시작일자
    }
}
