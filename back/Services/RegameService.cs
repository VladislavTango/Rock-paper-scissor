using Grpc.Core;
using PlayGame;
using RestartGame;
namespace back.Services
{
    public class RegameService : RestartGame.RestartGame.RestartGameBase
    {
        private readonly RoomService roomService = new RoomService();
        public override async Task PlayMore
            (PlayMoreRequest request, IServerStreamWriter<PlayMoreResponse> responseStream, ServerCallContext context)
        {
            string result = "da";
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var roomData = roomService.GetRoom(request.RoomId);
                if (roomData.StepFirst == "null" && roomData.StepSecond == "null")
                {
                    result = "Обнуление";
                    await responseStream.WriteAsync(new PlayMoreResponse { Winner = result });
                    break;
                }
                    
            }
                await Task.Delay(TimeSpan.FromSeconds(1));       
        }
    }
}
