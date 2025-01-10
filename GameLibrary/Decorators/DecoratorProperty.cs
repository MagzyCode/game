using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public class DecoratorProperty : PlayerProperities
    {
        protected PlayerProperitiesStandart playerProperities;
        /// <summary>
        /// Конструктор объекта
        /// </summary>
        /// <param name="playerProperities"></param>
        public DecoratorProperty(PlayerProperitiesStandart playerProperities)
        {
            this.playerProperities = playerProperities;
        }


        /// <summary>
        /// Запас здоровья игрока
        /// </summary>
        public override int Health { get => playerProperities.Health; protected set => playerProperities.SetProperty( TypeProperty.Health,value); }
        /// <summary>
        /// Боезапас
        /// </summary>
        public override int Ammo { get => playerProperities.Ammo;  set => playerProperities.SetProperty(TypeProperty.Ammo, value); }
        /// <summary>
        /// Скорость
        /// </summary>
        public override float Speed { get => playerProperities.Speed; }
        /// <summary>
        /// Время перезарядки
        /// </summary>
        public override float ReloadTime { get => playerProperities.ReloadTime; }
        /// <summary>
        /// Сила
        /// </summary>
        public override float Power { get => playerProperities.Power; }

        protected override void DeactivateProperities()
        {
        }
    }
}
