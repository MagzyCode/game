
using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class AmmoPrizeFactory : PrizeFactory
    {
        /// <summary>
        /// Подбираемые заряды
        /// </summary>
        /// <param name="position">Позиция появления</param>
        /// <returns>Игровой объект</returns>
        public override GameObject CreatePrize(Vector2 position)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/Prize/ammo.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.8f, 0.8f)));
            gameObject.GameObjectTag = "Spawn";

            PrizeSpawn speedPrize = new PrizeSpawn();
            speedPrize.InitializeWeaponSpawn(50f);

            gameObject.InitializeObjectScript(speedPrize);

            return gameObject;
        }
    }
}
