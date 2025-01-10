using EngineLibrary.EngineComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public abstract class PlayerProperities
    {
        protected Player player;

        public float timeDeactivate = 5;
        protected float timer = 0;

        /// <summary>
        /// Запас здоровья игрока
        /// </summary>
        public abstract int Health { get; protected set; }
        /// <summary>
        /// Боезапас
        /// </summary>
        public abstract int Ammo { get; set; }
        /// <summary>
        /// Скорость
        /// </summary>
        public abstract float Speed { get; }
        /// <summary>
        /// Время перезарядки
        /// </summary>
        public abstract float ReloadTime { get; }
        /// <summary>
        /// Сила
        /// </summary>
        public abstract float Power { get; }

        public virtual void SetPlayer(Player player)
        {
            this.player = player;
        }

        public virtual void SetProperty(TypeProperty type, float value)
        {
            switch (type)
            {
                case TypeProperty.Health:
                    Health = (int)value;
                    break;
                case TypeProperty.Ammo:
                    Ammo = (int)value;
                    break;
            }
        }

        public virtual void UpdateTime()
        {
            timer += Time.DeltaTime;

            if (timer >= timeDeactivate)
            {
                timer = 0;
                DeactivateProperities();
            }
        }

        protected abstract void DeactivateProperities();
    }
}
