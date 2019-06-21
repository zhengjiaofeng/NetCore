using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LLSignalR.Hubs
{
    public class MessageHub : Hub
    {
        public List<string> connectionList = new List<string>();
        public async Task SendRoandMessage()
        {
            int count = 0;
            do
            {
                Thread.Sleep(1000);
                count++;
                int randNum = new Random().Next(0, 100000);
                await Clients.All.SendAsync("recupdate", randNum);
            }
            while (count < 11);

            await Clients.All.SendAsync("recfinish");

        }

        /// <summary>
        /// 客户端连接连接事情
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            if (!connectionList.Contains(connectionId))
            {
                connectionList.Add(connectionId);
            }
            //连接上 调用发送客户端信息
            await SendRoandMessage();
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端失去连接事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string connectionId = Context.ConnectionId;
            if (!connectionList.Contains(connectionId))
            {
                connectionList.Remove(connectionId);
            }
            await base.OnDisconnectedAsync(ex);
        }
    }
}
