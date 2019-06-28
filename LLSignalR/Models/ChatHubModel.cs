using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LLSignalR.Models
{
    public class ChatHubModel
    {

    }

    public class ChatUser
    {
        /// <summary>
        /// 记录用户连接id
        /// </summary>
        public string UserConnectId { get; set; }
    }

    /// <summary>
    ///  ChatHub上下文
    /// </summary>
    public class ChatHubDbContext
    {
        public List<ChatUser> chatUser;
        public ChatHubDbContext()
        {
            chatUser = new List<ChatUser>();
        }
    }
}
