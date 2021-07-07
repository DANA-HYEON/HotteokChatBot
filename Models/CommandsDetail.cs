using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Models
{
    //명령어사용대상 (Commands-CommandsDetail구조)
    public class CommandsDetail
    {
        [Key]
        public long Id { get; set; } //명령어사용대상 ID

        public long Id2 { get; set; } //명령어ID

        public string CodeId { get; set; } //명령어사용대상
    }
}
