using System.Collections.Generic;
using EngineLibrary.EngineComponents;
using SharpDX.Direct2D1;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Класс анимации 
    /// </summary>
    public class Animation
    {
        private readonly List<Bitmap> sprites;

        private int currentIndexInAnimation;

        private float changeTime;

        private readonly float deltaTimeAnimation;

        private readonly bool isLoop;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sprites">Список спрайтов</param>
        /// <param name="changeTime">Время смены каждого спрайта</param>
        /// <param name="isLoop">Зацикленностб анимации</param>
        public Animation(List<Bitmap> sprites, float changeTime, bool isLoop)
        {
            this.sprites = sprites;
            this.changeTime = changeTime;
            this.isLoop = isLoop;
            deltaTimeAnimation = changeTime;
            currentIndexInAnimation = 0;
        }

        public Animation(int index = 0)
        {
            currentIndexInAnimation = index;
        }

      
        public int CurrentIndexInAnimation { 
            get => currentIndexInAnimation; 
            set => currentIndexInAnimation = value;
        }

        /// <summary>
        /// Сброс анимации
        /// </summary>
        public void ResetAnimation()
        {
            currentIndexInAnimation = 0;
        }

        /// <summary>
        /// Возращает текущее изображение анимации
        /// </summary>
        /// <returns>Текущее изображение в анимации</returns>
        public Bitmap GetSpriteFromAnimation()
        {
            if (sprites == null)
                return null;

            if (changeTime < Time.CurrentTime)
            {
                currentIndexInAnimation++;
                changeTime = Time.CurrentTime + deltaTimeAnimation;
            }

            if (currentIndexInAnimation >= sprites.Count)
                if (!isLoop)
                    return null;
                else
                    currentIndexInAnimation = 0;

            return sprites[currentIndexInAnimation];
        }
    }
}
