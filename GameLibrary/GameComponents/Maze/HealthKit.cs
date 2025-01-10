using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс аптечки
    /// </summary>
    public class HealthKit : ObjectScript
    {
        private MazeScene maze;

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
            if (gameObject.Collider.CheckIntersection(out Player player))
            {
                if (player.Property.Health < 10)
                {
                    player.ChangeStatsValue(1);
                    maze.RemoveObjectFromScene(gameObject);
                }
            }
        }
    }
}
