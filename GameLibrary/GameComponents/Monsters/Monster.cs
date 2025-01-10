using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using SharpDX;
using System;

namespace GameLibrary.Monsters
{
    public class Monster : ObjectScript
    {
        /// <summary>
        /// Запас здоровья игрока
        /// </summary>
        public int Health { get; private set; } = 2;
        /// <summary>
        /// Скорость игрока
        /// </summary>
        public float Speed { get; set; } = 4;
        /// <summary>
        /// Возможность двигаться у игрока
        /// </summary>
        public bool IsCanMove { get; set; } = true;

        private GameObject childGameObject;

        private GameObject collider;

        Random random;

        int directionX = 0, directionY = 0;

        protected MazeScene maze;

        float timer = 0.5f;

        public float ReloadTime { get; } = 0.5f;

        public override void Start()
        {
            random = new Random();

            maze = MazeScene.instance;

            collider = new GameObject();
            collider.InitializeObjectComponent(new ColliderComponent(collider, new Size2F(0.1f, 0.1f)));

            Animation animation;

            animation = new Animation(RenderingSystem.LoadAnimation("Resources/Monsters/left idle ", 2), 0.2f, true);
            gameObject.Sprite.AddAnimation("idleLeft", animation);
            animation = new Animation(RenderingSystem.LoadAnimation("Resources/Monsters/left run ", 4), 0.2f, true);
            gameObject.Sprite.AddAnimation("runLeft", animation);
            animation = new Animation(RenderingSystem.LoadAnimation("Resources/Monsters/right idle ", 2), 0.2f, true);
            gameObject.Sprite.AddAnimation("idleRight", animation);
            animation = new Animation(RenderingSystem.LoadAnimation("Resources/Monsters/right run ", 4), 0.2f, true);
            gameObject.Sprite.AddAnimation("runRight", animation);

            gameObject.Sprite.SetAnimation("idleLeft");

            directionX = 1;

            //if (random.Next(0, 2) == 0)
            //    directionX = 1;
            //else
            //    directionX = -1;
        }

        public override void Update()
        {
            if (gameObject.IsActive && IsCanMove)
                Move();
        }

        /// <summary>
        /// Установка дочернего объекта 
        /// </summary>
        /// <param name="gameObject">Дочерний объект</param>
        public void SetChildGameObject(GameObject gameObject)
        {
            if (childGameObject != null)
                MazeScene.instance.RemoveObjectFromScene(childGameObject);

            childGameObject = gameObject;
        }

        /// <summary>
        /// Метод движения игрока
        /// </summary>
        private void Move()
        {

            if (gameObject.Collider.CheckIntersection("Stair"))
            {
                gameObject.Transform.IsUseGravitation = gameObject.Collider.CheckIntersection("Wall");
            }
            else
            {
                gameObject.Transform.IsUseGravitation = true;
            }

            Vector2 direction;

            if (directionX == 0)
            {
                if (childGameObject != null)
                    childGameObject.Sprite.IsFlipX = gameObject.Sprite.IsFlipX;

                if (childGameObject != null && childGameObject.Sprite.IsFlipX)
                    childGameObject.Sprite.SetAnimation("idleLeft");
                else if (childGameObject != null)
                    childGameObject.Sprite.SetAnimation("idleRight");

                if (gameObject.Sprite.IsFlipX)
                    gameObject.Sprite.SetAnimation("idleLeft");
                else
                    gameObject.Sprite.SetAnimation("idleRight");

                direction = new Vector2(0, directionY);
            }
            else
            {
                gameObject.Sprite.IsFlipX = directionX < 0;

                if (childGameObject != null)
                    childGameObject.Sprite.IsFlipX = gameObject.Sprite.IsFlipX;

                if (childGameObject != null && childGameObject.Sprite.IsFlipX)
                    childGameObject.Sprite.SetAnimation("runLeft");
                else if (childGameObject != null)
                    childGameObject.Sprite.SetAnimation("runRight");

                if (gameObject.Sprite.IsFlipX)
                    gameObject.Sprite.SetAnimation("runLeft");
                else
                    gameObject.Sprite.SetAnimation("runRight");

                direction = new Vector2(directionX, 0);
            }

            Vector2 movement = direction * Speed * Time.DeltaTime;
            gameObject.Transform.SetMovement(movement);

            DetectCollision();
        }

        /// <summary>
        /// Распознавание столкновений и реакция на них
        /// </summary>
        private void DetectCollision()
        {
            if (gameObject.GameObjectTag == "Blue Player" || gameObject.GameObjectTag == "Red Player" || gameObject.GameObjectTag == "Monster")
                gameObject.Transform.AddGravitation();

            if (gameObject.Collider.CheckIntersection("Wall", "Stair"))
            {
                gameObject.Transform.ResetGravitation();
            }

            if (gameObject.Collider.CheckIntersection("Wall", "BreakWall"))
            {
                Speed = 0;
                if (directionX == -1)
                {
                    directionX = 1;
                    gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X + 0.3f, gameObject.Transform.Position.Y);
                }
                else if (directionX == 1)
                {
                    directionX = -1;
                    gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X - 0.3f, gameObject.Transform.Position.Y);
                }
                Speed = 4;
            }

            if (gameObject.Collider.CheckIntersection(out GameObject player, "Red Player", "Blue Player"))
            {
                timer += Time.DeltaTime;

                if ((player.Script as Player).Property.Health > 0 && timer >= 0.5)
                {
                    timer = 0;
                    (player.Script as Player).ChangeStatsValue(-1);
                }

                if ((player.Script as Player).Property.Health <= 0)
                {
                    player.IsActive = false;
                    Player.SetCoins(player.GameObjectTag, -10);

                    GameEvents.ChangeEffect?.Invoke(player.GameObjectTag, "Death");
                }
            }
        }    

        /// <summary>
        /// Распознавание столкновений с игроком и реакция
        /// </summary>
        public bool CollisionPlayer()
        {
            collider.InitializeObjectComponent(new TransformComponent(gameObject.Transform.Position, new Size2F(0.1f, 0.1f)));

            while (!collider.Collider.CheckIntersection("Wall", "BreakWall"))
            {
                if (collider.Collider.CheckIntersection(out GameObject player, "Red Player", "Blue Player"))
                {
                    if ((player.Script as Player).IsCanMove)
                        return true;
                }

                collider.Transform.SetMovement(new Vector2(directionX, 0));
            }

            return false;
        }

        /// <summary>
        /// Изменение значения характеристик игрока
        /// </summary>
        /// <param name="value">Значение, которое прибавляется к текущему значению монет</param>
        public void ChangeStatsValue(float value)
        {
            if (gameObject.Collider.CheckIntersection("Spell") || Health < 2)
            {
                Health += (int) value;
            }

            if (Health <= 0)
            {
                maze.RemoveObjectFromScene(childGameObject);
                maze.RemoveObjectFromScene(gameObject);
            }
        }
    }
}
