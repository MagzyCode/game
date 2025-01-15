using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using GameLibrary.Monsters;
using SharpDX;

namespace GameLibrary.Weapons
{
    /// <summary>
    /// Абстрактный класс оружия
    /// </summary>
    public abstract class Weapon : ObjectScript
    {
        /// <summary>
        /// Фабрика персонажа, к которому прикреплено оружие
        /// </summary>
        public PlayerConstructor PlayerFactory { get; set; }

        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        private Player playerScript;
        private float currentReloadTime;
        private Monster monsterScript;

        /// <summary>
        /// Поведение на момент создания игрового объекта
        /// </summary>
        public override void Start()
        {
            try
            {
                playerScript = ((Player)gameObject.ParentGameObject.Script);
                currentReloadTime = Time.CurrentTime + playerScript.Property.ReloadTime;
            }
            catch
            {
                
                monsterScript = ((Monster)gameObject.ParentGameObject.Script);
                currentReloadTime = Time.CurrentTime + monsterScript.ReloadTime;
            }

            maze = MazeScene.instance;

            if (playerScript != null)
                GameEvents.ChangeCount?.Invoke(playerScript.gameObject.GameObjectTag, playerScript.Property.Ammo);

            LoadAnimation();
        }

        protected abstract void LoadAnimation();

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            var isCurrentGameObjectCurrentCharacter = (maze.PlayerId == "1" && gameObject.ParentGameObject.GameObjectTag.Equals("Blue Player")) || (maze.PlayerId == "2" && gameObject.ParentGameObject.GameObjectTag.Equals("Red Player"));

            if (playerScript != null && !isCurrentGameObjectCurrentCharacter)
            {
                GameEvents.ChangeCount?.Invoke(playerScript.gameObject.GameObjectTag, maze.Client.EnemyCharacter.SpellCount);
            }

            if (playerScript != null && (playerScript.IsCanMove && Input.GetButtonDawn(playerScript.Control.ShootKey) && currentReloadTime < Time.CurrentTime && playerScript.Property.Ammo > 0 || (!isCurrentGameObjectCurrentCharacter && maze.Client.EnemyCharacter.IsPlayerShooting)))
            {
                // Позиция создания заряда
                //Vector2 spellSpawnPosition = isCurrentGameObjectCurrentCharacter
                //    ? gameObject.ParentGameObject.Transform.Position
                //    : new Vector2(maze.Client.EnemyCharacter.PlayerPosition);
                Vector2 spellSpawnPosition = gameObject.ParentGameObject.Transform.Position;

                // Направление заряда (вправо или влево в зависимости от ориентации спрайта)
                //Vector2 spellDirection = isCurrentGameObjectCurrentCharacter
                //    ? new Vector2(gameObject.Sprite.IsFlipX ? -1 : 1, 0)
                //    : new Vector2(maze.Client.EnemyCharacter.IsPlayerSpriteFlip ? -1 : 1, 0);
                Vector2 spellDirection = new Vector2(gameObject.Sprite.IsFlipX ? -1 : 1, 0);

                // Создание заряда с указанной позицией, направлением и силой (если применимо)
                SpawnSpell(spellSpawnPosition, spellDirection, playerScript.Property.Power);


                //if (!isCurrentGameObjectCurrentCharacter && maze.Client.EnemyCharacter.BulletCount != playerScript.Property.Ammo)
                //{
                //    playerScript.Property.Ammo = maze.Client.EnemyCharacter.BulletCount;
                //}



                playerScript.Property.SetProperty(TypeProperty.Ammo, (isCurrentGameObjectCurrentCharacter ? playerScript.Property.Ammo : maze.Client.EnemyCharacter.SpellCount) - 1);

                // При быстрой перезарядке заряды не отнимаются
                if (playerScript.Property.ReloadTime < 0.5f)
                    playerScript.Property.Ammo += 1;


                //if (isCurrentGameObjectCurrentCharacter)
                //{       
                //    // Уменьшение количества патронов у игрока
                //    playerScript.Property.SetProperty(TypeProperty.Ammo, playerScript.Property.Ammo - 1);

                //    // При быстрой перезарядке заряды не отнимаются
                //    if (playerScript.Property.ReloadTime < 0.5f)
                //        playerScript.Property.Ammo += 1;

                //    maze.Client.MyCharacter.BulletCount = playerScript.Property.Ammo;
                //}
                //else
                //{
                //    maze.Client.EnemyCharacter.IsPlayerShooting = false;
                //    // maze.Client.EnemyCharacter.CoinCount -= 1;
                //    //playerScript.Property.Ammo = maze.Client.EnemyCharacter.BulletCount;
                //    // Уменьшение количества патронов у игрока
                //    playerScript.Property.SetProperty(TypeProperty.Ammo, playerScript.Property.Ammo - 1);

                //    // При быстрой перезарядке заряды не отнимаются
                //    if (playerScript.Property.ReloadTime < 0.5f)
                //        playerScript.Property.Ammo += 1;

                //    maze.Client.EnemyCharacter.BulletCount = playerScript.Property.Ammo;
                //}

                if (isCurrentGameObjectCurrentCharacter)
                {
                    maze.Client.MyCharacter.IsPlayerShooting = true;
                }

                currentReloadTime = Time.CurrentTime + playerScript.Property.ReloadTime;

                if (playerScript != null && playerScript.Property.Ammo >= 0)
                {
                    //if (isCurrentGameObjectCurrentCharacter)
                    //{
                    //    maze.Client.MyCharacter.IsPlayerShooting = true;
                    //    maze.Client.MyCharacter.BulletCount = playerScript.Property.Ammo;
                    //}
                    //else
                    //{
                    //    maze.Client.EnemyCharacter.BulletCount = playerScript.Property.Ammo;
                    //}
                    if (isCurrentGameObjectCurrentCharacter)
                    {
                        maze.Client.MyCharacter.SpellCount = playerScript.Property.Ammo;
                    }

                    GameEvents.ChangeCount?.Invoke(playerScript.gameObject.GameObjectTag, playerScript.Property.Ammo);
                }
                    // Изменение количества патронов и вызов события изменения количества
                    
            }
            else if (monsterScript != null && monsterScript.IsCanMove && monsterScript.CollisionPlayer() && currentReloadTime < Time.CurrentTime)
            {
                // Позиция создания заряда
                Vector2 spellSpawnPosition = gameObject.ParentGameObject.Transform.Position;
                // Направление заряда (вправо или влево в зависимости от ориентации спрайта)
                Vector2 spellDirection = new Vector2(gameObject.Sprite.IsFlipX ? -1 : 1, 0);
                // Создание заряда с указанной позицией и направлением
                SpawnSpell(spellSpawnPosition, spellDirection);

                currentReloadTime = Time.CurrentTime + monsterScript.ReloadTime;
            }
        }

        /// <summary>
        /// Создание заряда из фабрики
        /// </summary>
        /// <param name="position">Позиция создания</param>
        /// <param name="direction">Направление заряда</param>
        protected abstract void SpawnSpell(Vector2 position, Vector2 direction, float power = 1);
    }
}
