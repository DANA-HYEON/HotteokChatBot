using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Models
{
    //명령어
    public class Commands
    {
        [Key]
        public long Id { get; set; } //명령어ID
        
        public int Define { get; set; } //0:기본명령어, 1:맞춤명령어
        
        public string Commands_Title { get; set; } //명령어제목

        public string Message { get; set; }//맞춤명령어일때 사용(출력 메세지)
       
        public string Guide { get; set; }//기본명령어일때 사용(명령어 설명)
       
        public int Status { get; set; }//0:명령어 비활성 1:명령어 활성
       
        public int Cooltime { get; set; } //쿨타임

        public long User_Id { get; set; } //User User_Id
    }
}
