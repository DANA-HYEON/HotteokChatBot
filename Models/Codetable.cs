using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Models
{
    //코드테이블
    public class Codetable
    {
        [Key]
        public string CodeId { get; set; } //코드유형ID
       
        public string Type { get; set; } //코드유형명
        
        public int Number { get; set; } //코드번호
        
        public int Order { get; set; } //정렬순서
        
        public int Status { get; set; } //사용여부

        public string Codename { get; set; } //코드명
    }
}
