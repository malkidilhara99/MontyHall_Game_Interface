using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzelgame.Tests
{
    internal class MontyControllerTests
    {

        private readonly MontyController _controller;

        public MontyHallGameControllerTests()
        {
            // Arrange: Initialize the controller
            _controller = new MontyController();
        }

        [Fact]
        public void Simulate_ShouldReturnBadRequest_WhenNumberOfGamesIsZero()
        {
            // Arrange
            var request = new SimulationRequest { NumberOfGames = 0, SwitchDoor = true };

            // Act
            var result = _controller.Simulate(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Simulate_ShouldReturnOk_WhenNumberOfGamesIsPositive()
        {
            // Arrange
            var request = new SimulationRequest { NumberOfGames = 1000, SwitchDoor = true };

            // Act
            var result = _controller.Simulate(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Simulate_ShouldReturnCorrectWinningAndStayingProbabilities()
        {
            // Arrange
            var request = new SimulationRequest { NumberOfGames = 1000, SwitchDoor = true };

            // Act
            var result = _controller.Simulate(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var resultValue = result.Value as dynamic;
            Assert.NotNull(resultValue.WinningProbability);
            Assert.NotNull(resultValue.StayingProbability);
        }

        [Fact]
        public void Simulate_ShouldReturnDifferentResults_WhenSwitchDoorIsToggled()
        {
            // Arrange
            var requestWithSwitch = new SimulationRequest { NumberOfGames = 1000, SwitchDoor = true };
            var requestWithoutSwitch = new SimulationRequest { NumberOfGames = 1000, SwitchDoor = false };

            // Act
            var resultWithSwitch = _controller.Simulate(requestWithSwitch) as OkObjectResult;
            var resultWithoutSwitch = _controller.Simulate(requestWithoutSwitch) as OkObjectResult;

            // Assert
            Assert.NotNull(resultWithSwitch);
            Assert.NotNull(resultWithoutSwitch);

            var switchResultValue = resultWithSwitch.Value as dynamic;
            var stayResultValue = resultWithoutSwitch.Value as dynamic;

            Assert.NotEqual(switchResultValue.WinningProbability, stayResultValue.WinningProbability);
        }



    }
}
