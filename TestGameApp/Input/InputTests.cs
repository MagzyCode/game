using EngineLibrary.EngineComponents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGameApp.Input
{
    [TestClass]
    public class InputTests
    {
        private Mock<InputHandler> _inputHandlerMock = new Mock<InputHandler>();

        [TestMethod]
        public void GetAxis_NullHandler_ReturnZero()
        {
            // arrange
            InputHandler handler = null;
            EngineLibrary.EngineComponents.Input.SetupInputHandler(handler);

            // act
            var result = EngineLibrary.EngineComponents.Input.GetAxis(AxisOfInput.AlternativeVertical);

            // assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void GetAxis_KeyboardUpdatedFalse_ReturnZero()
        {
            // arrange
            _inputHandlerMock.Setup(x => x.KeyboardUpdated).Returns(false);
            EngineLibrary.EngineComponents.Input.SetupInputHandler(_inputHandlerMock.Object);

            // act
            var result = EngineLibrary.EngineComponents.Input.GetAxis(AxisOfInput.AlternativeVertical);

            // assert
            result.Should().Be(0);
        }
    }
}
