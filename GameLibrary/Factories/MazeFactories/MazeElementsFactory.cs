using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Weapons;
using GameLibrary.Monsters;
using SharpDX;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс фабрики создания элементов лабиринта 
    /// </summary>
    public class MazeElementsFactory
    {
        /// <summary>
        /// Создает элемент лабиринта
        /// </summary>
        /// <param name="position">Позиция объекта на сцене</param>
        /// <param name="TagName">Тег игрового объекта</param>
        /// <returns>Созданный игровой объект</returns>
        public GameObject CreateMazeElement(Vector2 position, string TagName)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/" + TagName + ".png")));

            //Загрузка элементов
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(1f, 1f)));

            gameObject.GameObjectTag = TagName;

            if (TagName == "BreakWall")
                gameObject.InitializeObjectScript(new BreakWall());

            return gameObject;
        }

        /// <summary>
        /// Создание аптечки в лабиринте
        /// </summary>
        /// <param name="position">Позиция объекта на сцене</param>
        /// <returns>Игровой объект</returns>
        public GameObject CreateHealthKit(Vector2 position)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/HealthKit.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(1f, 1f)));

            gameObject.GameObjectTag = "HealthKit";

            gameObject.InitializeObjectScript(new HealthKit());

            return gameObject;
        }

        /// <summary>
        /// Создание монстра в лабиринте
        /// </summary>
        /// <param name="position">Позиция объекта на сцене</param>
        /// <returns>Игровой объект</returns>
        public GameObject CreateMonsters(Vector2 position)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/" + "Monsters" + "/left idle 1.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.5f, 0.5f), new Vector2(0, 0.2f)));

            gameObject.GameObjectTag = "Monster";

            var monsterScript = new Monster();
            gameObject.InitializeObjectScript(monsterScript);

            return gameObject;
        }

        /// <summary>
        /// Допавление игрового объекта оружия монстру
        /// </summary>
        /// <returns>Игровой объект</returns>
        public GameObject CreateWeapon(Weapon weapon, string nameOfweapon, GameObject monster)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Monsters/damage Weapon/damage left idle 1.png")));
            gameObject.GameObjectTag = "Weapon";

            gameObject.ParentGameObject = monster;

            gameObject.InitializeObjectScript(weapon);

            Monster playerScript = monster.Script as Monster;
            playerScript.SetChildGameObject(gameObject);

            return gameObject;
        }
    }
}
