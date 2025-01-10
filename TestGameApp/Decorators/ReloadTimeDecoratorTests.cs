using FluentAssertions;
using GameLibrary.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp.Decorators
{
    [TestClass]
    public class ReloadTimeDecoratorTests
    {
        private ReloadTimeDecorator _reloadTimeDecorator;

        [TestMethod]
        public void ReloadTimeProp_CorrectWork_ReturnHalfValue()
        {
            // arrange
            _reloadTimeDecorator = new ReloadTimeDecorator(new PlayerProperitiesStandart());

            // act
            var result = _reloadTimeDecorator.ReloadTime;

            // assert
            result.Should().BeApproximately(0.25f, 0.0001f);
        }
    }
}
