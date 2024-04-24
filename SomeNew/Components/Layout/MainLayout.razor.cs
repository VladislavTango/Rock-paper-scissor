#if ANDROID
using Android.Content;
#endif
using back.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using Grpc.Net.Client;
using GameConnect;
using System.Text;
using Microsoft.AspNetCore.Components.Web;


namespace SomeNew.Components.Layout
{
    public partial class MainLayout  
    {
        string? Code;

         List<string>? Rooms=new List<string>();
         List<string>? SearchRooms=new List<string>();


        string? TextToSearch="";
        string UserId = GenerateUserId();

        private void PerformSearch()
        {
            SearchRooms = Rooms.Where(item => item.Contains(TextToSearch)).ToList();
        }
        protected override async Task OnInitializedAsync() {
            GetRooms();
        }
        private async Task GetRooms() {
            HttpClient client = new HttpClient();
            Rooms = await client.GetFromJsonAsync<List<string>>($"https://localhost:7163/api/Room/GetAllRooms");
          
        }
        private  async Task CreateRoom()
        {
            HttpClient client = new HttpClient();

            var response = client.GetStringAsync($"https://localhost:7163/api/Room/CreateRoom?userId={UserId}");

            Code = response.Result.ToString();
        }
        public async Task JoinGame(string roomId)
        {
            HttpClient client = new HttpClient();

            var response =  client.GetStringAsync
                ($"https://localhost:7163/api/Room/ConnectToRoom?userId={UserId}&roomId={roomId}");

            Code = response.Result.ToString();
        }

        public static string GenerateUserId()
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 25; i++)
            {
                sb.Append(Convert.ToChar(rnd.Next(97, 122)));
            }
            return sb.ToString();
        }
    }
}