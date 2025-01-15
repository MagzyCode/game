﻿using System;

namespace Network.Data
{
    /// <summary>
    /// Данные, передаваемые по сети между игроками.
    /// </summary>
    [Serializable]
    public sealed class NetworkData
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public float[] PlayerPosition { get; set; } = new float[2];

        /// <summary>
        /// Направление персонажа на основе спрайта
        /// True - налево, false - направо
        /// </summary>
        public bool IsPlayerSpriteFlip { get; set; }

        public bool IsPlayerShooting { get; set; }

        public int BulletCount { get; set; } = 10;

        public int HealthCount { get; set; } = 10;

        public int CoinCount { get; set; }
    }
}


