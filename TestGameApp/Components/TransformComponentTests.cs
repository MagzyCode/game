using EngineLibrary.ObjectComponents;
using FluentAssertions;
using GameLibrary.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGameApp.Components
{
    [TestClass]
    public class TransformComponentTests
    {
        [TestMethod]
        public void PowerProperty_NoPositionChange_ReturnValue()
        {
            // arrange
            var transformComponent = new TransformComponent(new Vector2(), new Size2F(1f, 1f));

            // act
            transformComponent.SetMovement(new Vector2());

            // assert
            transformComponent.Position.X.Should().BeApproximately(0f, 0.0001f);
            transformComponent.Position.Y.Should().BeApproximately(0f, 0.0001f);
        }

        [TestMethod]
        public void PowerProperty_PositionChanged_ReturnValue()
        {
            // arrange
            var transformComponent = new TransformComponent(new Vector2(), new Size2F(1f, 1f));

            // act
            transformComponent.SetMovement(new Vector2(1f, 1f));

            // assert
            transformComponent.Position.X.Should().BeApproximately(1f, 0.0001f);
            transformComponent.Position.Y.Should().BeApproximately(-1f, 0.0001f);
        }
    }
}
