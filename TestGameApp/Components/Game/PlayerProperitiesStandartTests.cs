using FluentAssertions;
using GameLibrary.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp.Components.Game
{
    [TestClass]
    public class PlayerProperitiesStandartTests
    {
        [TestMethod]
        public void SetProperty_ChangeHealth_HealthIncrease()
        {
            // arrange
            var playerProps = new PlayerProperitiesStandart();

            // act
            playerProps.SetProperty(GameLibrary.TypeProperty.Health, 5f);

            // assert
            playerProps.Health.Should().Be(5);
        }

        [TestMethod]
        public void SetProperty_ChangeAmmo_AmmoIncrease()
        {
            // arrange
            var playerProps = new PlayerProperitiesStandart();

            // act
            playerProps.SetProperty(GameLibrary.TypeProperty.Ammo, 55f);

            // assert
            playerProps.Ammo.Should().Be(55);
        }
    }
}
