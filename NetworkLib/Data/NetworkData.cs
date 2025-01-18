using System;

namespace Network.Data
{
    /// <summary>
    /// Данные, передаваемые по сети между игроками.
    /// </summary>
    [Serializable]
    public sealed class NetworkData
    {
        public float[] PlayerPosition { get; set; } = new float[2];

        /// <summary>
        /// Направление персонажа на основе спрайта
        /// True - налево, false - направо
        /// </summary>
        public bool IsPlayerSpriteFlip { get; set; }

        public bool IsPlayerShooting { get; set; }

        public int SpellCount { get; set; } = 10;

        public int HealthCount { get; set; } = 10;

        public int CoinCount { get; set; }

        public float[] PrizeSpawnPosition { get; set; }

        public int PrizeSpawnType { get; set; }

        public bool IsPlayerTryGetPrize { get; set; }
    }
}


