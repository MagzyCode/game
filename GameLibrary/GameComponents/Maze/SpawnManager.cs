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
                Random random = new Random();

                int chance = random.Next(0, 101);

                if (maze.CountEmptyBlocks() == 0) return;

                Vector2 position = maze.GetRandomPosition();

                if (chance < 20)
                {
                    spawnFactory = new SpeedPrizeFactory();
                }
                else if (chance > 20 && chance <= 40)
                {
                    spawnFactory = new SpellPowerPrizeFactory();
                }
                else if (chance > 40 && chance <= 70)
                {
                    spawnFactory = new ReloadTimePrizeFactory();
                }
                else
                    spawnFactory = new AmmoPrizeFactory();


                maze.AddObjectOnScene(spawnFactory.CreatePrize(position));

                currentTimeToSpawn += timeToSpawn;
            }
        }
    }
}
