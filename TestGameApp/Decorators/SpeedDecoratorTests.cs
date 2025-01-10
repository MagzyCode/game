using FluentAssertions;
using GameLibrary.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp.Decorators
{
    [TestClass]
    public class SpeedDecoratorTests
    {
        private SpeedDecorator _speedDecorator;

        [TestMethod]
        public void SpeedProperty_CorrectWork_ReturnValue()
        {
            // arrange
            _speedDecorator = new SpeedDecorator(new PlayerProperitiesStandart());

            // act
            var result = _speedDecorator.Speed;

            // assert
            result.Should().BeApproximately(7.5f, 0.001f);
        }
    }
}
