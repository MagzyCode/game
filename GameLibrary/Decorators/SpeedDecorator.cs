using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public class SpeedDecorator : DecoratorProperty
    {
        public SpeedDecorator(PlayerProperitiesStandart playerProperities) : base(playerProperities)
        {
            timeDeactivate = 7;
        }

        /// <summary>
        /// Скорость
        /// </summary>
        public override float Speed { get => playerProperities.Speed * 1.5f; }

        protected override void DeactivateProperities()
        {
            player.SetProperty(new PlayerProperitiesStandart());
        }
    }
}
