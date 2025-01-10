using EngineLibrary.ObjectComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;

namespace TestGameApp
{
    [TestClass]
    public class TestCollision
    {
        [TestMethod]
        public void TestCollider()
        {
            GameObject firstObject = new GameObject();
            firstObject.InitializeObjectComponent(new TransformComponent(new Vector2(1f, 1f), new Size2F(1, 1)));
            firstObject.InitializeObjectComponent(new ColliderComponent(firstObject, new Size2F(1, 1)));
            firstObject.GameObjectTag = "Test";

            GameObject secondObject = new GameObject();
            secondObject.InitializeObjectComponent(new TransformComponent(new Vector2(1f, 3f), new Size2F(1, 1)));
            secondObject.InitializeObjectComponent(new ColliderComponent(secondObject, new Size2F(1, 1)));
            secondObject.GameObjectTag = "Test";

            Assert.IsFalse(firstObject.Collider.CheckIntersection("Test"));

            secondObject.Transform.Position = new Vector2(1f, 1.5f);

            Assert.IsTrue(firstObject.Collider.CheckIntersection("Test"));

            secondObject.GameObjectTag = "";

            Assert.IsFalse(firstObject.Collider.CheckIntersection("Test"));
        }
    }
}
