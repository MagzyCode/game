using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;
using System;
using System.Drawing;
using System.Collections.Generic;
using GameLibrary.Game;
using GameLibrary.Monsters;
using GameLibrary.Weapons;
using NetworkLib;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс лабиринта
    /// </summary>
    public class MazeScene : Scene
    {
        /// <summary>
        /// Статическая ссылка на класс
        /// </summary>
        public static MazeScene instance = null;
        /// <summary>
        /// Фабрика создания элементов лабиринта
        /// </summary>
        public MazeElementsFactory ElementsFactory { get; private set; }
        /// <summary>
        /// Конструктор синего игрока
        /// </summary>
        public PlayerConstructor BluePlayerFactory { get; set; }
        /// <summary>
        /// Конструктор красного игрока
        /// </summary>
        public PlayerConstructor RedPlayerFactory { get; set; }

        private readonly List<Vector2> emptyBlocks = new List<Vector2>();

        public int CountEmptyBlocks()
        {
            return emptyBlocks.Count;
        }

        /// <summary>
        /// Создание игровых объектов на сцене
        /// </summary>

        protected override void CreateGameObjectsOnScene()
        {
            if (instance == null)
                instance = this;

            ElementsFactory = new MazeElementsFactory();
            BluePlayerFactory = new PlayerConstructor();
            RedPlayerFactory = new PlayerConstructor();

            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(27, 15)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Back.png")));
            gameObject.GameObjectTag = "Background";

            gameObjects.Add(gameObject);

            gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(1, 1)));
            gameObject.GameObjectTag = "GameManager";
            gameObject.InitializeObjectScript(new SpawnManager());

            gameObjects.Add(gameObject);

            CreateMaze();

            GameObject player = BluePlayerFactory.CreatePlayer("Blue Player");

            gameObjects.Add(player);
            //Client.MyCharacter.PlayerPosition = player.Transform.Position.ToArray();
            gameObjects.Add(BluePlayerFactory.CreateWeapon(new DamageWeapon(), "damage"));

            player = RedPlayerFactory.CreatePlayer("Red Player");
            gameObjects.Add(player);
            //Client.EnemyCharacter.PlayerPosition = player.Transform.Position.ToArray();
            gameObjects.Add(RedPlayerFactory.CreateWeapon(new DamageWeapon(), "damage"));
        }

        /// <summary>
        /// Метод создания лабиринта
        /// </summary>
        public void CreateMaze()
        {
            Random random = new Random();

            string text = "Resources/Mazes/Maze " + 4 + ".bmp";

            Bitmap bitmap = new Bitmap(text);

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    System.Drawing.Color color = bitmap.GetPixel(j, i);

                    GameObject gameObject = null;

                    if (color.R == 0 && color.G == 0 && color.B == 0)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "Wall");
                    else if (color.R == 255 && color.G == 0 && color.B == 0)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "BreakWall");
                    else if (color.R == 0 && color.G == 0 && color.B == 255)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "Stair");
                    //else if (color.R == 255 && color.G == 255 && color.B == 0)                 
                       //gameObject = ElementsFactory.CreateHealthKit(new Vector2(j, i));                 
                    else if (color.R == 0 && color.G == 255 && color.B == 255)
                    {
                        gameObject = ElementsFactory.CreateMonsters(new Vector2(j, i));
                        gameObjects.Add(ElementsFactory.CreateWeapon(new DamageWeapon(), "damage", gameObject));
                    }
                    else if (color.R == 125 && color.G == 0 && color.B == 0)
                    {
                        RedPlayerFactory.StartPosition = new Vector2(j, i);
                        if (PlayerId == "2")
                        {
                            Client.MyCharacter.PlayerPosition = new float[] { RedPlayerFactory.StartPosition[0], RedPlayerFactory.StartPosition[1] };
                        }
                        else
                        {
                            Client.EnemyCharacter.PlayerPosition = new float[] { RedPlayerFactory.StartPosition[0], RedPlayerFactory.StartPosition[1] };
                        }
                    }
                    else if (color.R == 0 && color.G == 0 && color.B == 125)
                    {
                        BluePlayerFactory.StartPosition = new Vector2(j, i);
                        if (PlayerId == "1")
                        {
                            Client.MyCharacter.PlayerPosition = new float[] { BluePlayerFactory.StartPosition[0], BluePlayerFactory.StartPosition[1] };
                        }
                        else
                        {
                            Client.EnemyCharacter.PlayerPosition = new float[] { BluePlayerFactory.StartPosition[0], BluePlayerFactory.StartPosition[1] };
                        }
                    }
                        
                    else
                        emptyBlocks.Add(new Vector2(j, i));

                    if (gameObject != null)
                        gameObjects.Add(gameObject);
                }
            }
        }

        /// <summary>
        /// Добавление объекта в лист отрисовки
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        public void AddObjectOnScene(GameObject gameObject)
        {
            gameObjectsToAdd.Add(gameObject);
        }

        /// <summary>
        /// Удаления объекта из листа отрисовки
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        public void RemoveObjectFromScene(GameObject gameObject)
        {
            int count = 0;
            
            if (gameObject.GameObjectTag == "Spawn")
                emptyBlocks.Add(gameObject.Transform.Position);

            gameObjectsToRemove.Add(gameObject);

            foreach (var monsterObject in gameObjects)
            {
                if (monsterObject.Script is Monster)
                    count++;
                
            }

            if (count == 0)
                EndScene();
            
        }

        /// <summary>
        /// Рандомное место в лабиринте
        /// </summary>
        /// <returns>Позицию</returns>
        public Vector2 GetRandomPosition()
        {
            Random random = new Random();

            int index = random.Next(0, emptyBlocks.Count);

            Vector2 position = emptyBlocks[index];

            emptyBlocks.Remove(position);

            return position;
        }

        /// <summary>
        /// Поведение при завершении сцены
        /// </summary>
        protected override void EndScene()
        {
            base.EndScene();

            string winPlayer;

            if (Player.BPCoins < Player.RPCoins)
                winPlayer = RedPlayerFactory.PlayerTag;
            else if (Player.BPCoins > Player.RPCoins)
                winPlayer = BluePlayerFactory.PlayerTag;
            else
                winPlayer = "Friendship";

                GameEvents.EndGame?.Invoke(winPlayer);
        }
    }
}
