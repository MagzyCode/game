using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Monsters;

namespace GameLibrary.Effects
{
    /// <summary>
    /// Эффект после урона
    /// </summary>
    public class DamageEffect : Effect
    {
        /// <summary>
        /// Время действия эффекта
        /// </summary>
        public override float EffectTime => 5f;

        /// <summary>
        /// Активация эффекта
        /// </summary>
        /// <param name="player">Игровой объект, на который будет наложен эффект</param>
        public override void ActivateEffect(GameObject player, string tag = null, float power = 1)
        {
            playerGameObject = player;
            playerGameObject.IsActive = true;

            if (playerGameObject.Script is Player)
            {
                // Нанесение урона игроку
                if ((playerGameObject.Script as Player).Property.Health > 0)
                {
                    
                    (playerGameObject.Script as Player).ChangeStatsValue(-power);
                }

                // Проверка, если здоровье игрока меньше или равно 0, то игрок погиб
                if ((playerGameObject.Script as Player).Property.Health <= 0)
                {
                    playerGameObject.IsActive = false;

                    // Обработка смерти игрока и вычет очков (RPCoins или BPCoins)
                    if (playerGameObject.GameObjectTag == "Red Player" && Player.RPCoins > 0)
                    {
                        (playerGameObject.Script as Player).ChangeStatsValue((Player.RPCoins), "Death");
                        Player.SetCoins("Red Player", -10);
                    }
                    else if (playerGameObject.GameObjectTag == "Blue Player" && Player.BPCoins > 0)
                    {
                        (playerGameObject.Script as Player).ChangeStatsValue((Player.BPCoins), "Death");
                        Player.SetCoins("Blue Player", -10);
                    }
                    else
                        (playerGameObject.Script as Player).ChangeStatsValue(-power, "Death");

                    
                    if (Player.RPCoins < 0)
                        Player.SetCoins("Red Player", 10);
                    else if (Player.BPCoins < 0)
                        Player.SetCoins("Blue Player", 10);

                    // Вызов события изменения эффекта на сцене
                    GameEvents.ChangeEffect?.Invoke(playerGameObject.GameObjectTag, "Death");
                }
            }

            if (playerGameObject.Script is Monster)
            {
                // Нанесение урона монстру
                if ((playerGameObject.Script as Monster).Health > 0)
                {
                    (playerGameObject.Script as Monster).ChangeStatsValue(-power);
                    playerGameObject.Sprite.Bitmap = null;
                }

                // Проверка, если здоровье монстра меньше или равно 0, то монстр погибает
                if ((playerGameObject.Script as Monster).Health <= 0)
                {
                    playerGameObject.IsActive = false;
                    playerGameObject.Collider.IsInactive = true;

                    // Добавление очков (tag) при убийстве монстра
                    if (tag != null)
                        Player.SetCoins(tag, 10);

                    // Вызов события изменения эффекта на сцене
                    GameEvents.ChangeEffect?.Invoke(playerGameObject.GameObjectTag, "Death");
                }
            }
        }

        /// <summary>
        /// Инициализация эффекта
        /// </summary>
        protected override void Initialize() { }

        /// <summary>
        /// Деактивация эффекта
        /// </summary>
        protected override void DeactivateEffect()
        {
            if (!playerGameObject.IsActive)
            {
                // Восстановление позиции игрока после деактивации эффекта
                if (playerGameObject.GameObjectTag == "Blue Player")
                {
                    if (maze.CountEmptyBlocks() != 0)
                        playerGameObject.Transform.Position = maze.GetRandomPosition();
                    else
                        playerGameObject.Transform.Position = maze.BluePlayerFactory.StartPosition;
                }
                else
                {
                    if (maze.CountEmptyBlocks() != 0)
                        playerGameObject.Transform.Position = maze.GetRandomPosition();
                    else
                        playerGameObject.Transform.Position = maze.RedPlayerFactory.StartPosition;
                }
                playerGameObject.IsActive = true;
            }
            // Удаление объекта из сцены
            maze.RemoveObjectFromScene(gameObject);
            // Вызов события изменения эффекта на сцене
            GameEvents.ChangeEffect?.Invoke(playerGameObject.GameObjectTag, "");
        }

        /// <summary>
        /// Поведение на сцене
        /// </summary>
        protected override void BehaviorOnScene()
        {
            // Проверка столкновения с другим объектом (игроком)
            if (gameObject.Collider.CheckIntersection(out GameObject player) && gameObject.IsActive)
            {
                // Если столкнулись с объектом того же тега, то деактивируем эффект
                if (playerGameObject.GameObjectTag == player.GameObjectTag)
                    gameObject.IsActive = false;
            }

            // Если игровой объект активен, удаляем его из сцены
            if (playerGameObject.IsActive)
                maze.RemoveObjectFromScene(gameObject);
        }
    }
}
