using EngineLibrary.ObjectComponents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace TestGameApp.Objects
{
    [TestClass]
    public class GameObjectTests
    {
        [TestMethod]
        public void InitializeObjectComponent_NullComponent_NullProperties()
        {
            // arrange
            var gameObject = new GameObject();

            // act
            gameObject.InitializeObjectComponent(null);

            // assert
            gameObject.Collider.Should().BeNull();
            gameObject.Sprite.Should().BeNull();
            gameObject.Transform.Should().BeNull();
        }

        [TestMethod]
        public void InitializeObjectComponent_ColliderComponent_ColliderPropertyPopulated()
        {
            // arrange
            var gameObject = new GameObject();

            // act
            gameObject.InitializeObjectComponent(new ColliderComponent(new GameObject(), new Size2F()));

            // assert
            gameObject.Collider.Should().NotBeNull();
            gameObject.Sprite.Should().BeNull();
            gameObject.Transform.Should().BeNull();
        }

        [TestMethod]
        public void InitializeObjectComponent_SpriteComponent_SpritePropertyPopulated()
        {
            // arrange
            var gameObject = new GameObject();

            // act
            gameObject.InitializeObjectComponent(new SpriteComponent());

            // assert
            gameObject.Collider.Should().BeNull();
            gameObject.Sprite.Should().NotBeNull();
            gameObject.Transform.Should().BeNull();
        }

        [TestMethod]
        public void InitializeObjectComponent_TransformComponent_TransformPropertyPopulated()
        {
            // arrange
            var gameObject = new GameObject();

            // act
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(), new Size2F(1f, 1f)));

            // assert
            gameObject.Collider.Should().BeNull();
            gameObject.Sprite.Should().BeNull();
            gameObject.Transform.Should().NotBeNull();
        }
    }
}
