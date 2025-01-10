using EngineLibrary.ObjectComponents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace TestGameApp.Components
{
    [TestClass]
    public class ColliderComponentTests
    {
        [TestMethod]
        public void InitializationConstructor_NormalWay_ReturnFalseIsInactive()
        {
            // arrange
            var colliderComponent = new ColliderComponent(
                gameObject: new GameObject(),
                scale: new Size2F(1f, 1f));

            // act
            var result = colliderComponent.IsInactive;

            // assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void CheckIntersection_ZeroTags_ReturnFalse()
        {
            // arrange
            var colliderComponent = new ColliderComponent(
                gameObject: new GameObject(),
                scale: new Size2F(1f, 1f));
            ColliderComponent.CollidersOfGameObjects = new System.Collections.Generic.List<GameObject>();

            // act
            var result = colliderComponent.CheckIntersection();

            // assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void CheckIntersection_WithTags_ReturnFalse()
        {
            // arrange
            var colliderComponent = new ColliderComponent(
                gameObject: new GameObject(),
                scale: new Size2F(1f, 1f));
            ColliderComponent.CollidersOfGameObjects = new System.Collections.Generic.List<GameObject>();

            // act
            var result = colliderComponent.CheckIntersection("Wall", "Spell");

            // assert
            result.Should().BeFalse();
        }
    }
}
