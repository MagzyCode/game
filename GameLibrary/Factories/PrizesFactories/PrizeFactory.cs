using EngineLibrary.ObjectComponents;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public abstract class PrizeFactory
    {
        public abstract GameObject CreatePrize(Vector2 position);
    }
}
