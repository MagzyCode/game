using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    /// <summary>
    /// Статический класс событий игры
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// Делегат события изменения количества здоровья
        /// </summary>
        /// <param name="tagPlayer">Тег игрового объекта игрока</param>
        /// <param name="value">Значение собранных монет</param>
        public delegate void HealthDelegate(string tagPlayer, int value);

        /// <summary>
        /// Событие изменения количества здоровья
        /// </summary>
        public static HealthDelegate ChangeHealth { get; set; }

        /// <summary>
        /// Делегат события изменения количества монет
        /// </summary>
        /// <param name="tagPlayer">Тег игрового объекта игрока</param>
        /// <param name="value">Значение собранных монет</param>
        public delegate void CoinsDelegate(string tagPlayer, int value);

        /// <summary>
        /// Событие изменения количества монет
        /// </summary>
        public static CoinsDelegate ChangeCoins { get; set; }

        /// <summary>
        /// Делегат события окончания игры
        /// </summary>
        public delegate void EndGameDelegate(string winPlayer);

        /// <summary>
        /// Событие окончания игры
        /// </summary>
        public static EndGameDelegate EndGame { get; set; }

        /// <summary>
        /// Делегат события получения эффекта игроком
        /// </summary>
        /// <param name="tagPlayer">Тег игрового объекта игрока</param>
        /// <param name="effectName">Название эффекта</param>
        public delegate void EffectDelegate(string tagPlayer, string effectName);

        /// <summary>
        /// Событие получения эффекта игроком
        /// </summary>
        public static EffectDelegate ChangeEffect { get; set; }

        /// <summary>
        /// Делегат события получения
        /// </summary>
        /// <param name="tagPlayer">Тег игрового объекта игрока</param>
        /// <param name="weaponName"></param>
        public delegate void WeaponDelegate(string tagPlayer, string weaponName);

        /// <summary>
        /// Событие получения
        /// </summary>
        public static WeaponDelegate ChangeWeapon { get; set; }

        /// <summary>
        /// Делегат события получения 
        /// </summary>
        /// <param name="tagPlayer">Тег игрового объекта игрока</param>
        /// <param name="count"></param>
        public delegate void CountSpells(string tagPlayer, int count);

        /// <summary>
        /// Событие получения
        /// </summary>
        public static CountSpells ChangeCount { get; set; }
    }
}
