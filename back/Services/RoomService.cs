using back.Models;
using StackExchange.Redis;
using System.Text;

namespace back.Services
{
    public class RoomService
    {
        private readonly ConnectionMultiplexer redis;

        private readonly IDatabase db;

        public RoomService()
        {
            redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            db = redis.GetDatabase();
        }
        public List<string> GetAllRooms()
        {
            var server = db.Multiplexer.GetServer(db.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: "*" ).ToList();
            return keys.Select(key => key.ToString()).ToList();
        }
        public void CreateRoom(Room room)
        {
            var hashEntries = new HashEntry[]
            {
                new HashEntry("UserFirstId", room.UserFirstId),
                new HashEntry("UserSecondId", "null"),
                new HashEntry("StepFirst", "null"),
                new HashEntry("StepSecond", "null"),
            };

            db.HashSet(room.RoomId, hashEntries);
        }
        public string ConncetToRoom(string roomId,string userId) 
        { 
            var room = GetRoom(roomId);
            if (room.UserSecondId != "null") return "такой пользователь уже есть";

            var hashEntries = new HashEntry[]
            {
                new HashEntry("UserSecondId",userId)
            };

            db.HashSet(room.RoomId, hashEntries);
            return "норм";
        }
        public string GenerateRoomId()
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(Convert.ToChar(rnd.Next(97, 122)));
            }
            return sb.ToString();
        }
        public Room GetRoom(string roomId)
        {
            var hashEntries = db.HashGetAll(roomId);
            return new Room
            {
                RoomId = roomId,
                UserFirstId = hashEntries[0].Value.ToString(),
                UserSecondId =  hashEntries[1].Value.ToString() ,
                StepFirst =  hashEntries[2].Value.ToString(),
                StepSecond = hashEntries[3].Value.ToString() 
            };
        }

        public void SetTurn(string Choose,string roomId , string userId) {
            var room = GetRoom(roomId);
            if (room.UserFirstId == userId)
            {
                room.StepFirst = Choose;
                db.HashSet(roomId, "StepFirst", Choose);
            }
            else
            {
                room.StepSecond = Choose;
                db.HashSet(roomId, "StepSecond", Choose);
            }
        }

        public void Regame(string roomId, string userId) {
            var room = GetRoom(roomId);
            if (room.UserFirstId == userId)
            {
                db.HashSet(roomId, "StepFirst", "null");
            }
            else
            {
                db.HashSet(roomId, "StepSecond", "null");
            }
        }
        public void DeleteRoom(string roomId) {
            db.KeyDelete(roomId);
        }



    }
}
