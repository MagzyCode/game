using EngineLibrary.ObjectComponents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGameApp.Objects
{
    [TestClass]
    public class AnimationTests
    {
        [TestMethod]
        public void ResetAnimation_CorrectWork_ReturnZero()
        {
            // arrange
            var animation = new Animation();

            // act
            var result = animation.CurrentIndexInAnimation;

            // assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void ResetAnimation_ChangedIndex_ReturnTwo()
        {
            // arrange
            var animation = new Animation(2);

            // act
            var result = animation.CurrentIndexInAnimation;

            // assert
            result.Should().Be(2);
        }
    }
}
