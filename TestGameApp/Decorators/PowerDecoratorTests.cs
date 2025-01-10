using FluentAssertions;
using GameLibrary.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp.Decorators
{
    [TestClass]
    public class PowerDecoratorTests
    {
        private PowerDecorator _powerDecorator;

        [TestMethod]
        public void PowerProperty_CorrectWork_ReturnValue()
        {
            // arrange
            _powerDecorator = new PowerDecorator(new PlayerProperitiesStandart());

            // act
            var result = _powerDecorator.Power;

            // assert
            result.Should().BeApproximately(2f, 0.001f);
        }
    }
}
