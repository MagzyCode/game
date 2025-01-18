using GameLibrary.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Decorators
{
    public class HealthDecorator : DecoratorProperty
    {
        public HealthDecorator(PlayerProperitiesStandart playerProperities) : base(playerProperities)
        {
            timeDeactivate = 10;
        }

        public override int Health { get => playerProperities.Health + 5; }

        protected override void DeactivateProperities()
        {
            player.SetProperty(new PlayerProperitiesStandart());
        }
    }
}
