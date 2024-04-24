using Grpc.Core;
using PlayGame;

namespace back.Services
{
    public class PlayingService : PlayGame.PlayGame.PlayGameBase
    {
        private readonly RoomService roomService = new RoomService();
        public override async Task GetTurn
            (TurnRequest request, IServerStreamWriter<TurnResponse> responseStream, ServerCallContext context)
        {
            string result = "da";
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var roomData = roomService.GetRoom(request.RoomId);
                if (roomData.StepFirst != "null" && roomData.StepSecond != "null")
                {

                    if (roomData.StepFirst == roomData.StepSecond)
                    {
                        result = "tie";
                        
                    }

                    else if ((roomData.StepFirst == "Rock" && roomData.StepSecond == "Scissors") ||
                        (roomData.StepFirst == "Scissors" && roomData.StepSecond == "Paper") ||
                        (roomData.StepFirst == "Paper" && roomData.StepSecond == "Rock"))
                    {
                        result = roomData.StepFirst;
                        
                    }

                    else
                    {
                        result = roomData.StepSecond;
                    }
                    await responseStream.WriteAsync(new TurnResponse { Winner = result });
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }      
    }
}