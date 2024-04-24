using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PlayGame;
using RestartGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeNew.Components.Pages
{
    public partial class WinnerCard : ComponentBase
    {
        [Parameter]
        public string? Card { get; set; }
        [Parameter]
        public string? Code { get; set; }
        [Parameter]
        public string? UserId { get; set; }
        string Regame="Winner";

        protected async Task More() 
        { 
            HttpClient client = new HttpClient();


            var response = client.GetStringAsync
                ($"https://localhost:7163/api/Room/Regame?userId={UserId}&roomId={Code}&");
            Regame = "Wait";

            StateHasChanged();
            var chanel = GrpcChannel.ForAddress("https://localhost:7163");

            var GRPCclient = new RestartGame.RestartGame.RestartGameClient(chanel);

            var request = new PlayMoreRequest
            {
                UserId = UserId,
                RoomId = Code
            };

            using (var call = GRPCclient.PlayMore(request))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    if (call.ResponseStream.Current.Winner == "Обнуление")
                    {
                        Regame = "Watch";
                        StateHasChanged();
                        break;
                    }

                }
            }
        }
    }
}
