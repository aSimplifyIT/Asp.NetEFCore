using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Hubs
{
    [Authorize]
    public class OnlineUsersHub : Hub
    {
        private static readonly Dictionary<string, string> _onlineUsers = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _onlineUsers[Context.ConnectionId] = userId;
                UpdateOnlineUsersCount();
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception? exception)
        {
            if (_onlineUsers.ContainsKey(Context.ConnectionId))
            {
                _onlineUsers.Remove(Context.ConnectionId);
                UpdateOnlineUsersCount();
            }
            return base.OnDisconnectedAsync(exception);
        }

        private async void UpdateOnlineUsersCount()
        {
            var userCount = _onlineUsers.Values.Distinct().Count(); // Count unique users
            await Clients.All.SendAsync("ReceiveOnlineUsersCount", userCount);
        }
        //private static HashSet<string> _onlineUsers = new HashSet<string>();

        //public override Task OnConnectedAsync()
        //{
        //    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    _onlineUsers.Add(Context.ConnectionId);
        //    UpdateOnlineUsersCount();
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    _onlineUsers.Remove(Context.ConnectionId);
        //    UpdateOnlineUsersCount();
        //    return base.OnDisconnectedAsync(exception);
        //}

        //private async void UpdateOnlineUsersCount()
        //{
        //    var count = _onlineUsers.Count;
        //    await Clients.All.SendAsync("ReceiveOnlineUsersCount", count);
        //}

    }
}
