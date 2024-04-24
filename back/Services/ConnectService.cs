using GameConnect;
using Grpc.Core;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace back.Services
{
    public class ConnectService : GameConnect.GameConnect.GameConnectBase
    {
        private readonly RoomService roomService=new RoomService();

      
        public override async Task<int> CheckRoom
            (RoomRequest request, IServerStreamWriter<RoomResponse> responseStream, ServerCallContext context)
        {
            var userCount = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var roomData = roomService.GetRoom(request.RoomId);
                if (roomData.UserFirstId!="null") userCount++;
                if (roomData.UserSecondId!="null") userCount++;

                await responseStream.WriteAsync(new RoomResponse { UserCount = userCount });

                if (userCount == 2)
                {
                    break;
                }
                userCount = 0;
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            return userCount;
        }
    }
}
