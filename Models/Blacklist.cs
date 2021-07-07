using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Models
{
    //금칙어
    public class Blacklist
    {
        [Key]
        public long Id { get; set; } //금칙어ID
        
        public string  Blocked_Word{get;set;} //금칙어

        public int Check_Count { get; set; } //금칙어 사용 가능한 횟수
        
        public int Timeout_Length { get; set; } //채팅금지시간
       
        public int Silent { get; set; } //금칙어 사용 시 안내메시지 활성화 여부
        
        public string Message { get; set; } //금칙어 사용 시 안내메시지

        public long User_Id { get; set; } //User User_Id

    }
}
