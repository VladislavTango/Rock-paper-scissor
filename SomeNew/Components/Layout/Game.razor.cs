using back.Models;
using GameConnect;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlayGame;
namespace SomeNew.Components.Layout
{
    public partial class Game : ComponentBase
    {
        [Parameter]
        public string? Code { get; set; }
        [Parameter]
        public string? UserId { get; set; }

        string? WinnerCard;

        string play ="Wait";

        protected async Task Wait()
        {
            var chanel = GrpcChannel.ForAddress("https://localhost:7163");

            var client = new GameConnect.GameConnect.GameConnectClient(chanel);

            var request = new RoomRequest
            {
                RoomId = Code
            };

            using (var call = client.CheckRoom(request))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var userCount = call.ResponseStream.Current.UserCount;
                    //await JSRuntime.InvokeVoidAsync("console.log", userCount);
                    if (userCount == 2)
                    {
                        play = "Play";
                        StateHasChanged();
                        //await JSRuntime.InvokeVoidAsync("console.log", START);
                        break;
                    }

                }
            }
        }
        protected async Task Choose(string item) {

            if (item == "undef") return;
            HttpClient client = new HttpClient();

            var response = client.GetStringAsync
                ($"https://localhost:7163/api/Room/SetTurn?userId={UserId}&roomId={Code}&Choose={item}");

            play = "Wait";
            StateHasChanged();
            var chanel = GrpcChannel.ForAddress("https://localhost:7163");

            var GRPCclient = new PlayGame.PlayGame.PlayGameClient(chanel);
            await JSRuntime.InvokeVoidAsync("console.log", Code);
            await JSRuntime.InvokeVoidAsync("console.log", item);
            await JSRuntime.InvokeVoidAsync("console.log", UserId);

            var request = new TurnRequest
            {
                RoomId = Code
            };
            
            using (var call = GRPCclient.GetTurn(request))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    await JSRuntime.InvokeVoidAsync("console.log", call.ResponseStream.Current.Winner);
                    if (call.ResponseStream.Current.Winner != "da")
                    {
                        play = "ShowResult";
                        WinnerCard = $"Images/{call.ResponseStream.Current.Winner}.png";
                        StateHasChanged();
                        await JSRuntime.InvokeVoidAsync("console.log", WinnerCard);
                        break;
                    }

                }
            }
        }

    }
}