using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Weapons;
using SharpDX;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс фабрики создания персонажа
    /// </summary>
    public class PlayerConstructor
    {
        /// <summary>
        /// Игровой Объект игрока
        /// </summary>
        public GameObject PlayerGameObject { get; private set; }
        /// <summary>
        /// Тег плеера
        /// </summary>
        public string PlayerTag { get; private set; }
        /// <summary>
        /// Начальная позиция игрока в лабиринте
        /// </summary>
        public Vector2 StartPosition { get; set; }

        /// <summary>
        /// Создание игрового объекта персонажа
        /// </summary>
        /// <returns>Игровой объект</returns>
        public GameObject CreatePlayer(string path)
        {
            PlayerTag = path;

            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(StartPosition, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/" + PlayerTag + "/left idle 1.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.5f, 0.5f), new Vector2(0, 0.2f)));
            gameObject.GameObjectTag = PlayerTag;

            Player playerScript = new Player();
            gameObject.InitializeObjectScript(playerScript);

            PlayerGameObject = gameObject;

            return gameObject;
        }

        /// <summary>
        /// Создание игрового объектиа оружия
        /// </summary>
        /// <returns>Игровой объект</returns>
        public GameObject CreateWeapon(Weapon weapon, string nameOfweapon)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/" + PlayerTag + "/" + nameOfweapon  + " Weapon/" + nameOfweapon + " left idle 1.png")));
            gameObject.GameObjectTag = PlayerGameObject.GameObjectTag;

            gameObject.ParentGameObject = PlayerGameObject;

            weapon.PlayerFactory = this;
            gameObject.InitializeObjectScript(weapon);

            Player playerScript = PlayerGameObject.Script as Player;
            playerScript.SetChildGameObject(gameObject);

            GameEvents.ChangeWeapon?.Invoke(PlayerTag, nameOfweapon);

            return gameObject;
        }
    }
}
