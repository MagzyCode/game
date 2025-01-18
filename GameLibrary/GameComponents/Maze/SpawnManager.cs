using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;
using System;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс игрового менеджера
    /// </summary>
    public class SpawnManager : ObjectScript
    {
        private const float timeToSpawn = 1f;
        private float currentTimeToSpawn;

        private MazeScene maze;
        private PrizeFactory spawnFactory;

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            maze = MazeScene.instance;
            currentTimeToSpawn = Time.CurrentTime;
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            if (currentTimeToSpawn < Time.CurrentTime)
            {
                if (maze.PlayerId == "1")
                {
                    Random random = new Random();

                    // int chance = random.Next(0, 101);
                    var prizeRandomizedOption = random.Next(0, 4);
                    int chance = random.Next(0, 5);

                    if (maze.CountEmptyBlocks() == 0) return;

                    Vector2 position = maze.GetRandomPosition();

                    switch (prizeRandomizedOption)
                    {
                        case 0:
                            spawnFactory = new SpeedPrizeFactory();
                            break;
                        case 1:
                            spawnFactory = new SpellPowerPrizeFactory();
                            break;
                        case 2:
                            spawnFactory = new ReloadTimePrizeFactory();
                            break;
                        case 3:
                            spawnFactory = new AmmoPrizeFactory();
                            break;
                    }

                    maze.Client.MyCharacter.PrizeSpawnPosition = position.ToArray();
                    maze.Client.MyCharacter.PrizeSpawnType = prizeRandomizedOption;

                    maze.AddObjectOnScene(spawnFactory.CreatePrize(position));
                }
                else if (maze.Client.EnemyCharacter.PrizeSpawnType >= 0 && maze.Client.EnemyCharacter.PrizeSpawnPosition != null)
                {
                    switch (maze.Client.EnemyCharacter.PrizeSpawnType)
                    {
                        case 0:
                            spawnFactory = new SpeedPrizeFactory();
                            break;
                        case 1:
                            spawnFactory = new SpellPowerPrizeFactory();
                            break;
                        case 2:
                            spawnFactory = new ReloadTimePrizeFactory();
                            break;
                        case 3:
                            spawnFactory = new AmmoPrizeFactory();
                            break;
                    }

                    maze.AddObjectOnScene(spawnFactory.CreatePrize(new Vector2(maze.Client.EnemyCharacter.PrizeSpawnPosition)));
                }

                currentTimeToSpawn += timeToSpawn;
            }
        }
    }
}
