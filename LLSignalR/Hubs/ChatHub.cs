using LLSignalR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LLSignalR.Hubs
{
    //[Authorize(Policy = "Hubs")]
    public class ChatHub : Hub
    {
        /// <summary>
        /// 暂用静态代替上下文
        /// </summary>
        public static ChatHubDbContext chatHubDbContext = new ChatHubDbContext();

        #region 发生信息
        /// <summary>
        /// 对所有人发生信息
        /// </summary>
        /// <returns></returns>
        public async Task SendAllMessage(ChatMessage chatMessage)
        {

            await Clients.All.SendAsync("revicemsg", chatMessage.Sender, chatMessage.Message);
        }

        #endregion

        /// <summary>
        /// 客户端连接连接事情
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {



            string connectionId = Context.ConnectionId;

            #region 获取用户信息
            HttpContext httpContext = Context.GetHttpContext();
            string userNmae = httpContext.Request.Query["userNmae"];
            #endregion

            var charUser = chatHubDbContext.chatUser.FirstOrDefault(d => d.UserConnectId == connectionId);
            if (charUser == null)
            {
                //添加用户
                chatHubDbContext.chatUser.Add(new ChatUser { UserConnectId = connectionId });
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string connectionId = Context.ConnectionId;
            var charUser = chatHubDbContext.chatUser.FirstOrDefault(d => d.UserConnectId == connectionId);
            if (charUser != null)
            {
                chatHubDbContext.chatUser.Remove(charUser);
            }
            await base.OnDisconnectedAsync(ex);
        }

    }
}
