using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public class ReloadTimeDecorator : DecoratorProperty
    {
        public ReloadTimeDecorator(PlayerProperitiesStandart playerProperities) : base(playerProperities)
        {
            timeDeactivate = 3;
        }

        /// <summary>
        /// Скорость
        /// </summary>
        public override float ReloadTime { get => playerProperities.ReloadTime / 2; }

        protected override void DeactivateProperities()
        {
            player.SetProperty(new PlayerProperitiesStandart());
        }
    }
}
