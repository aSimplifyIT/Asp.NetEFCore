//using Microsoft.AspNetCore.Http;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;
//using System;

//namespace WebApplication1.Models
//{
//    public class UserActivityMiddleware
//    {
//         private readonly RequestDelegate _next;
//            private static readonly ConcurrentDictionary<string, DateTime> UserActivity = new();

//            public UserActivityMiddleware(RequestDelegate next)
//            {
//                _next = next;
//            }

//            public async Task InvokeAsync(HttpContext context)
//            {
//                var userId = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : context.Connection.RemoteIpAddress.ToString();
//                UserActivity[userId] = DateTime.UtcNow;

//                await _next(context);
//            }

//            public static int GetNumberOfUsersOnline(int userIsOnlineTimeWindow)
//            {
//                var threshold = DateTime.UtcNow.AddMinutes(-userIsOnlineTimeWindow);
//                return UserActivity.Values.Count(lastActivity => lastActivity >= threshold);
//            }
        

//    }
//}
