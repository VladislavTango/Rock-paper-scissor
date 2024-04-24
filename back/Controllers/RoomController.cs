using back.Models;
using back.Services;
using GameConnect;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly RoomService roomService;

        public RoomController()
        {
            roomService = new RoomService();
        }

        [HttpGet("GetAllRooms")]
        public IEnumerable<string> GetAllRooms()
        {
            return roomService.GetAllRooms();
        }

        //[HttpPost("CreateRoom")]
        [HttpGet("CreateRoom")]
        public IActionResult CreateRoom([FromQuery]string userId)
        {
            var room = new Room
            {
                RoomId = roomService.GenerateRoomId(),
                UserFirstId = userId
            };
            roomService.CreateRoom(room);
            return Ok(room.RoomId);
        }

        [HttpGet("ConnectToRoom")]
        public IActionResult ConnectToRoom([FromQuery] string userId, [FromQuery] string roomId) 
        {
            if (roomService.ConncetToRoom(roomId, userId) != "норм") return BadRequest("):");
            return Ok(roomId);
        }

        [HttpGet("SetTurn")]
        public IActionResult SetTurn([FromQuery] string userId, [FromQuery] string roomId,[FromQuery] string Choose)
        {
            roomService.SetTurn(Choose, roomId, userId);
            return Ok();
        }
        [HttpGet("Regame")]
        public IActionResult Regame([FromQuery] string userId, [FromQuery] string roomId)
        {
            roomService.Regame(roomId, userId);
            return Ok();
        }


    }
}
