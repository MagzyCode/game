﻿using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Decorators;
using GameLibrary.Game;
using GameLibrary.Maze;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Factories.PrizesFactories
{
    public class HealthKitFactory : PrizeFactory
    {
        public override GameObject CreatePrize(Vector2 position)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/HealthKit.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(1f, 1f)));

            gameObject.GameObjectTag = "HealthKit";

            PrizeSpawn speedPrize = new PrizeSpawn();
            speedPrize.InitializeWeaponSpawn(new HealthDecorator(new PlayerProperitiesStandart()), 50f);

            gameObject.InitializeObjectScript(speedPrize);

            return gameObject;
        }
    }
}
