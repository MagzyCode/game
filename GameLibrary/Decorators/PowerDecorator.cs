using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public class PowerDecorator : DecoratorProperty
    {
        public PowerDecorator(PlayerProperitiesStandart playerProperities) : base(playerProperities)
        {
            timeDeactivate = 5; 
        }

        /// <summary>
        /// Сила
        /// </summary>
        public override float Power { get => playerProperities.Power * 2; }

        protected override void DeactivateProperities()
        {
            player.SetProperty(new PlayerProperitiesStandart());
        }
    }
}
