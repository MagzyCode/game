using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;
using SharpDX.DirectInput;
using GameLibrary.Maze;
using System.Linq;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс, описывающий сценарий поведения игрока
    /// </summary>
    public class Player : ObjectScript
    {
        /// <summary>
        /// Управление игроком
        /// </summary>
        public PlayerControl Control { get; private set; }
        /// <summary>
        /// Возможность двигаться у игрока
        /// </summary>
        public bool IsCanMove { get; set; } = true;
        /// <summary>
        /// Полученные монеты
        /// </summary>
        public static int RPCoins { get; private set; } = 0;
        public static int BPCoins { get; private set; } = 0;

        bool chekDeath = false;

        private GameObject childGameObject;

        public PlayerProperities Property { get; set; }

        public void SetProperty(PlayerProperities property)
        {
            property.SetProperty(TypeProperty.Health, Property.Health);
            property.SetProperty(TypeProperty.Ammo, Property.Ammo);
            property.SetPlayer(this);

            Property = property;
        }

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            Animation animation;

            Property = new PlayerProperitiesStandart();
            Property.SetProperty(TypeProperty.Health, 10);
            Property.SetProperty(TypeProperty.Ammo, 10);
            Property.SetPlayer(this);

            if (gameObject.GameObjectTag == "Blue Player")
            {
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Blue Player/left idle ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("idleLeft", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Blue Player/left run ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("runLeft", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Blue Player/right idle ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("idleRight", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Blue Player/right run ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("runRight", animation);

                Control = new PlayerControl(AxisOfInput.Horizontal, AxisOfInput.Vertical, Key.Space, Key.E);

                if (MazeScene.instance.PlayerId == "2")
                {
                    GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, MazeScene.instance.Client.EnemyCharacter.HealthCount);
                    BPCoins = MazeScene.instance.Client.EnemyCharacter.CoinCount;
                }

                GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, Property.Health);
                GameEvents.ChangeCoins?.Invoke(gameObject.GameObjectTag, BPCoins);
            }
            else
            {
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Red Player/left idle ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("idleLeft", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Red Player/left run ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("runLeft", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Red Player/right idle ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("idleRight", animation);
                animation = new Animation(RenderingSystem.LoadAnimation("Resources/Red Player/right run ", 5), 0.2f, true);
                gameObject.Sprite.AddAnimation("runRight", animation);

                Control = new PlayerControl(AxisOfInput.AlternativeHorizontal, AxisOfInput.AlternativeVertical, Key.RightShift, Key.RightControl);


                if (MazeScene.instance.PlayerId == "1")
                {
                    GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, MazeScene.instance.Client.EnemyCharacter.HealthCount);
                    RPCoins = MazeScene.instance.Client.EnemyCharacter.CoinCount;
                }

                GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, Property.Health);
                GameEvents.ChangeCoins?.Invoke(gameObject.GameObjectTag, RPCoins);
            }

            gameObject.Sprite.SetAnimation("idleLeft");
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
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            if (!((MazeScene.instance.PlayerId == "1" && gameObject.GameObjectTag.Equals("Blue Player")) || (MazeScene.instance.PlayerId == "2" && gameObject.GameObjectTag.Equals("Red Player"))))
            {
                GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, MazeScene.instance.Client.EnemyCharacter.HealthCount);
                GameEvents.ChangeCount?.Invoke(gameObject.GameObjectTag, MazeScene.instance.Client.EnemyCharacter.SpellCount);
                GameEvents.ChangeCoins?.Invoke(gameObject.GameObjectTag, MazeScene.instance.Client.EnemyCharacter.CoinCount);
                //Property.Ammo = MazeScene.instance.Client.EnemyCharacter.SpellCount;
            }

            DetectEffect();

            if (gameObject.IsActive && IsCanMove)
                Move();

            if (Property != null)
                Property.UpdateTime();
        }

        /// <summary>
        /// Изменение значения характеристик игрока
        /// </summary>
        /// <param name="value"></param>
        public void ChangeStatsValue(float value)
        {
            if (gameObject.Collider.CheckIntersection("Spell") || Property.Health <= 10)
            {
                var isMyCharacter = (MazeScene.instance.PlayerId == "1" && gameObject.GameObjectTag.Equals("Blue Player")) || (MazeScene.instance.PlayerId == "2" && gameObject.GameObjectTag.Equals("Red Player"));

                if (!isMyCharacter)
                {
                    Property.SetProperty(TypeProperty.Health, MazeScene.instance.Client.EnemyCharacter.HealthCount + value);
                }
                else
                {
                    Property.SetProperty(TypeProperty.Health, Property.Health + value);
                    MazeScene.instance.Client.MyCharacter.HealthCount = Property.Health;
                }

                GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, Property.Health);
            }
        }

        public void ChangeStatsValue(float value, string gametag)
        {
            if (gametag == "Death")
            {
                chekDeath = true;
                Property.SetProperty(TypeProperty.Health, 10);
                Property.SetProperty(TypeProperty.Ammo, 10);

                var isMyCharacter = (MazeScene.instance.PlayerId == "1" && gameObject.GameObjectTag.Equals("Blue Player")) || (MazeScene.instance.PlayerId == "2" && gameObject.GameObjectTag.Equals("Red Player"));

                if (isMyCharacter)
                {
                    MazeScene.instance.Client.MyCharacter.HealthCount = Property.Health;
                    MazeScene.instance.Client.MyCharacter.SpellCount = Property.Ammo;
                }
                else
                {
                    MazeScene.instance.Client.EnemyCharacter.HealthCount = Property.Health;
                    MazeScene.instance.Client.EnemyCharacter.SpellCount = Property.Ammo;
                }

                GameEvents.ChangeHealth?.Invoke(gameObject.GameObjectTag, Property.Health);
            }
        }

        /// <summary>
        /// Метод движения игрока
        /// </summary>
        private void Move()
        {
            if (chekDeath) chekDeath = false;

            int directionX = 0, directionY = 0;

            directionX = Input.GetAxis(Control.HorizontalAxis);

            if (gameObject.Collider.CheckIntersection("Stair"))
            {
                directionY = Input.GetAxis(Control.VerticalAxis);
                gameObject.Transform.IsUseGravitation = gameObject.Collider.CheckIntersection("Wall");
            }
            else
            {
                gameObject.Transform.IsUseGravitation = true;
            }

            Vector2 direction;

            if (directionX == 0)
            {
                if(childGameObject != null)
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

            Vector2 movement = direction * Property.Speed * Time.DeltaTime;
         
            gameObject.Transform.SetMovement(movement);

            DetectCollision();
        }

        private void DetectEffect()
        {
            if (!chekDeath)
            { 
                if (Property.Speed > 5)
                    GameEvents.ChangeEffect?.Invoke(gameObject.GameObjectTag, "Speed");
                else if (Property.ReloadTime < 0.5f)
                    GameEvents.ChangeEffect?.Invoke(gameObject.GameObjectTag, "Reload");
                else if (Property.Power == 2)
                    GameEvents.ChangeEffect?.Invoke(gameObject.GameObjectTag, "Power");
                else
                    GameEvents.ChangeEffect?.Invoke(gameObject.GameObjectTag, " ");
            }

            GameEvents.ChangeCount?.Invoke(gameObject.GameObjectTag, Property.Ammo);
        }

        /// <summary>
        /// Распознавание столкновений и реакция на них
        /// </summary>
        private void DetectCollision()
        {
            if (gameObject.Collider.CheckIntersection("Wall","BreakWall"))
            {
                gameObject.Transform.ResetMovement();
            }

            if (gameObject.GameObjectTag == "Blue Player" || gameObject.GameObjectTag == "Red Player")
                gameObject.Transform.AddGravitation();

            string tag =  "";

            if (gameObject.Collider.CheckIntersection("Wall", tag))
            {
                gameObject.Transform.ResetGravitation();
            }
        }

        public static void SetCoins(string tag, int value)
        {
            var playerId = MazeScene.instance.PlayerId;

            //if (tag == "Red Player")
            //{
            //    if 

            //    RPCoins += value;
            //    GameEvents.ChangeCoins?.Invoke(tag, value);
            //}
            //else
            //{
            //    BPCoins += value;
            //    GameEvents.ChangeCoins?.Invoke(tag, value);
            //}

            if (tag == "Red Player")
            {
                if (playerId == "2")
                {
                    RPCoins += value;
                    MazeScene.instance.Client.MyCharacter.CoinCount = RPCoins;
                }
                else if (MazeScene.instance.Client.EnemyCharacter.CoinCount != RPCoins)
                {
                    RPCoins = MazeScene.instance.Client.EnemyCharacter.CoinCount;
                    RPCoins += value;
                    MazeScene.instance.Client.EnemyCharacter.CoinCount = RPCoins;
                }

                GameEvents.ChangeCoins?.Invoke(tag, value);
            }
            else
            {
                if (playerId == "1")
                {
                    BPCoins += value;
                    MazeScene.instance.Client.MyCharacter.CoinCount = BPCoins;
                }
                else if (MazeScene.instance.Client.EnemyCharacter.CoinCount != BPCoins)
                {
                    BPCoins = MazeScene.instance.Client.EnemyCharacter.CoinCount;
                    BPCoins += value;
                    MazeScene.instance.Client.EnemyCharacter.CoinCount = BPCoins;
                }
                //BPCoins += value;
                GameEvents.ChangeCoins?.Invoke(tag, value);
            }
        }

    }

    /// <summary>
    /// Структура игрового управления персонажа
    /// </summary>
    public struct PlayerControl
    {
        /// <summary>
        /// Горизонтальная ось передвижения
        /// </summary>
        public AxisOfInput HorizontalAxis { get; private set; }
        /// <summary>
        /// Вертикальная ось передвижения
        /// </summary>
        public AxisOfInput VerticalAxis { get; private set; }
        /// <summary>
        /// Кнопка стрельбы
        /// </summary>
        public Key ShootKey { get; private set; }
        /// <summary>
        /// Кнопка 
        /// </summary>
        public Key GetKey { get; private set; }

        /// <summary>
        /// Конструктор структуры
        /// </summary>
        /// <param name="horizontalAxis">Горизонтальная ось передвижения</param>
        /// <param name="verticalAxis"> Вертикальная ось передвижения</param>
        /// <param name="shootKey">Кнопка стрельбы</param>
        public PlayerControl(AxisOfInput horizontalAxis, AxisOfInput verticalAxis, Key shootKey, Key getKey)
        {
            HorizontalAxis = horizontalAxis;
            VerticalAxis = verticalAxis;
            ShootKey = shootKey;
            GetKey = getKey;
        }
    }
}
