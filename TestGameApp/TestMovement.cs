using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineLibrary.ObjectComponents;
using SharpDX;

namespace TestGameApp
{
    [TestClass]
    public class TestMovement
    {
        [TestMethod]
        public void TestGameObjectMovement()
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(1f, 1f), new Size2F(1, 1)));

            Vector2 offset = new Vector2(1f, 1f);

            gameObject.Transform.SetMovement(offset);

            Vector2 expected = new Vector2(2f, 0f);
            Vector2 actual = gameObject.Transform.Position;

            Assert.AreEqual(expected, actual);
        }
    }
}
