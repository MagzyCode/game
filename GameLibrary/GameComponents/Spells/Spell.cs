using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using SharpDX;

namespace GameLibrary.Spells
{
    /// <summary>
    /// Абстрактный класс заряда
    /// </summary>
    public abstract class Spell : ObjectScript
    {
        /// <summary>
        /// Скорость заряда
        /// </summary>
        public abstract float Speed { get; }

        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        private Vector2 flyDirection;
        private string[] interactionTag = new string[2];
        protected string tag;
        protected float power;

        /// <summary>
        /// Установление направления полета заряда
        /// </summary>
        /// <param name="direction">Вектор направления</param>
        /// <param name="tag">Тег игрового объекта, создающий заряд</param>
        public void SetSettings(Vector2 direction, string tag, float power)
        {
            this.tag = tag;
            flyDirection = direction;
            this.power = power;

            if (tag == "Blue Player")
                interactionTag[0] = "Red Player";
            else if(tag == "Red Player")
                interactionTag[0] = "Blue Player";
            else
            {
                interactionTag[0] = "Blue Player";
                interactionTag[1] = "Red Player";
            }

        }

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            maze = MazeScene.instance;
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            Vector2 movement = flyDirection * Speed * Time.DeltaTime;

            gameObject.Transform.SetMovement(movement);

            if (gameObject.Collider.CheckIntersection("Wall"))
            {
                maze.RemoveObjectFromScene(gameObject);
            }

            if (gameObject.Collider.CheckIntersection(out BreakWall wall))
            {
                maze.RemoveObjectFromScene(gameObject);
                wall.DestroyWall();
            }

            if (gameObject.Collider.CheckIntersection(out GameObject playerGameObject, interactionTag))
            {
                PlayerInteraction(playerGameObject);
                maze.RemoveObjectFromScene(gameObject);
            }

            if (gameObject.Collider.CheckIntersection(out GameObject monsterGameObject, "Monster"))
            {
                if(tag != monsterGameObject.GameObjectTag)
                {
                    PlayerInteraction(monsterGameObject);
                    maze.RemoveObjectFromScene(gameObject);
                }
            }
        }

        /// <summary>
        /// Взаимодействие с игроком
        /// </summary>
        public abstract void PlayerInteraction(GameObject playerGameObject);
    }
}
