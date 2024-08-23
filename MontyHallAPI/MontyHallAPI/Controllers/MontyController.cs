using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MontyHallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MontyController : ControllerBase
    {



        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] SimulationRequest request)
        {
            int wins = 0;
            int losses = 0;
            Random random = new Random();

            for (int i = 0; i < request.NumberOfGames; i++)
            {
                int carDoor = random.Next(3); // The door where the car is placed.
                int initialPick = random.Next(3); // The door initially picked by the player.
                int goatDoorToReveal = GetGoatDoorToReveal(carDoor, initialPick, random); // The door revealed by the host.

                bool result = MontyHallPick(initialPick, request.SwitchDoor, carDoor, goatDoorToReveal);
                if (result)
                    wins++;
                else
                    losses++;
            }

            var totalGames = request.NumberOfGames;
            var switchProbability = $"{wins}/{totalGames}";
            var stayProbability = $"{losses}/{totalGames}";

            return Ok(new
            {
                WinningProbability = switchProbability,
                StayingProbability = stayProbability
            });
        }

        private static bool MontyHallPick(int initialPick, bool switchDoor, int carDoor, int goatDoorToReveal)
        {
            // If the player decides to switch
            if (switchDoor)
            {
                int switchToDoor = 3 - initialPick - goatDoorToReveal; // Pick the door that isn't the initial pick or the revealed goat door.
                return carDoor == switchToDoor;
            }
            else
            {
                return carDoor == initialPick;
            }
        }

        private static int GetGoatDoorToReveal(int carDoor, int initialPick, Random random)
        {
            // The host reveals a door that:
            // 1. The player did not pick.
            // 2. Does not have the car behind it.
            int revealDoor = random.Next(3);
            while (revealDoor == carDoor || revealDoor == initialPick)
            {
                revealDoor = random.Next(3);
            }
            return revealDoor;

        }
    }
    public class SimulationRequest
    {
        public int NumberOfGames { get; set; }
        public bool SwitchDoor { get; set; }
    }
}
