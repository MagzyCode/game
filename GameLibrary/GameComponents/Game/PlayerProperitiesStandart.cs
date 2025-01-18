using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс с характеристиками игрока
    /// </summary>
    public class PlayerProperitiesStandart : PlayerProperities
    {
        /// <summary>
        /// Запас здоровья игрока
        /// </summary>
        public override int Health { get; set; } = 10;
        /// <summary>
        /// Боезапас
        /// </summary>
        public override int Ammo { get;  set; }
        /// <summary>
        /// Скорость
        /// </summary>
        public override float Speed { get; } = 5;
        /// <summary>
        /// Время перезарядки оружия
        /// </summary>
        public override float ReloadTime { get; } = 0.5f;
        /// <summary>
        /// Сила
        /// </summary>
        public override float Power { get; } = 1;

        protected override void DeactivateProperities()
        {

        }
    }
}
