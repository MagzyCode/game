using System;
using System.Collections.Generic;
using System.Linq;
using EngineLibrary.ObjectComponents;
using NetworkLib;
using SharpDX;
using SharpDX.Direct2D1;

namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Абстрактный класс сцены
    /// </summary>
    public abstract class Scene
    {
        private WindowRenderTarget renderTarget;
        private float worldScale;

        /// <summary>
        /// Список текущих игровых объектов для отрисовки
        /// </summary>
        public List<GameObject> gameObjects = new List<GameObject>();
        /// <summary>
        /// Список игровых объектов для добалвения в список отрисовки
        /// </summary>
        protected List<GameObject> gameObjectsToAdd = new List<GameObject>();
        /// <summary>
        /// Список игровых объектов для удаления их списка отрисовки
        /// </summary>
        protected List<GameObject> gameObjectsToRemove = new List<GameObject>();
        
        /// <summary>
        /// Состояния отрисовки сцены
        /// </summary>
        public bool IsDrawScene = true;

        public Client Client { get; set; }

        public string PlayerId { get; set; }

        /// <summary>
        /// Инициализация сцены и игровых объектов
        /// </summary>
        /// <param name="target">Окно отрисовки</param>
        /// <param name="scale">Относительный размер объектов</param>
        public void InitializeScene(WindowRenderTarget target, float scale)
        {
            renderTarget = target;
            worldScale = scale;

            CreateGameObjectsOnScene();
            InitializeEngineSettingsToGameObjects(gameObjects);
        }

        /// <summary>
        /// Установка опций рендеринга игровым объектам
        /// </summary>
        private void InitializeEngineSettingsToGameObjects(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.InitalizeEngineSettings(renderTarget, worldScale);
        }

        /// <summary>
        /// Создание игровых объектов на сцене
        /// </summary>
        protected abstract void CreateGameObjectsOnScene();

        /// <summary>
        /// Отрисовка сцены (игровых объектов)
        /// </summary>
        public void DrawScene()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GameObjectTag.EndsWith("Player") && (gameObject.Transform.Position.X != 0.0f || gameObject.Transform.Position.Y != 0.0f))
                {
                    if (PlayerId == "1" && gameObject.GameObjectTag == "Blue Player")
                    {
                        Client.MyCharacter.PlayerPosition = new float[] { gameObject.Transform.Position.X, gameObject.Transform.Position.Y };
                        Client.MyCharacter.IsPlayerSpriteFlip = gameObject.Sprite.IsFlipX;
                    }
                    else if (PlayerId == "1" && gameObject.GameObjectTag == "Red Player")
                    {
                        gameObject.Transform.Position = new Vector2(Client.EnemyCharacter.PlayerPosition[0], Client.EnemyCharacter.PlayerPosition[1]);
                        gameObject.Sprite.IsFlipX = Client.EnemyCharacter.IsPlayerSpriteFlip;
                    }

                    if (PlayerId == "2" && gameObject.GameObjectTag == "Red Player")
                    {
                        Client.MyCharacter.PlayerPosition = new float[] { gameObject.Transform.Position.X, gameObject.Transform.Position.Y };
                        Client.MyCharacter.IsPlayerSpriteFlip = gameObject.Sprite.IsFlipX;
                    }
                    else if (PlayerId == "2" && gameObject.GameObjectTag == "Blue Player")
                    {
                        gameObject.Transform.Position = new Vector2(Client.EnemyCharacter.PlayerPosition[0], Client.EnemyCharacter.PlayerPosition[1]);
                        gameObject.Sprite.IsFlipX = Client.EnemyCharacter.IsPlayerSpriteFlip;
                    }
                }

                gameObject.Draw();
            }

            AddRenderGameObjects();
            RemoveRenderGameObjects();

            if (!IsDrawScene)
                gameObjects.Clear();
        }

        /// <summary>
        /// Добавление игрового объекта для отрисовки
        /// </summary>
        private void AddRenderGameObjects()
        {
            InitializeEngineSettingsToGameObjects(gameObjectsToAdd);
            gameObjects.AddRange(gameObjectsToAdd);
            gameObjectsToAdd.Clear();
        }

        /// <summary>
        /// Удаление игрового объекта из отрисовки
        /// </summary>
        private void RemoveRenderGameObjects()
        {
            foreach(GameObject removeGameObject in gameObjectsToRemove)
            {
                gameObjects.Remove(removeGameObject);
            }

            gameObjectsToRemove.Clear();
        }

        /// <summary>
        /// Поведения при окончании сцены
        /// </summary>
        protected virtual void EndScene()
        {
            IsDrawScene = false;
        }
    }
}
