using System.Collections.Generic;
using SharpDX.Direct2D1;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Класс спрайта игровго объекта
    /// </summary>
    public class SpriteComponent
    {
        /// <summary>
        /// Изображение игрового объекта
        /// </summary>
        public Bitmap Bitmap { get; set; }

        /// <summary>
        /// Ширина спрайта
        /// </summary>
        public float WidthOfSprite { get; private set; }

        /// <summary>
        /// Высота спрайта
        /// </summary>
        public float HeightOfSprite { get; private set; }

        /// <summary>
        /// Инверсия по оси X
        /// </summary>
        public bool IsFlipX { get; set; }

        private readonly Dictionary<string, Animation> animations;
        private string currentAnimation = "";

        /// <summary>
        /// Конструктор класса спрайта
        /// </summary>
        /// <param name="sprite">Изображение</param>
        public SpriteComponent(Bitmap sprite)
        {
            Bitmap = sprite;
            WidthOfSprite = sprite.Size.Width;
            HeightOfSprite = sprite.Size.Height;
            animations = new Dictionary<string, Animation>();
            animations.Add("inactive", new Animation(null, 1f, true));
        }

        public SpriteComponent() 
        { }

        /// <summary>
        /// Добавление анимации
        /// </summary>
        /// <param name="name">Ключ анимации</param>
        /// <param name="animation">Анимация</param>
        public void AddAnimation(string name, Animation animation)
        {
            animations.Add(name, animation);
        }

        /// <summary>
        /// Проигрывание анимации
        /// </summary>
        public void PlayAnimation()
        {
            if (currentAnimation == "") return;

            Bitmap = animations[currentAnimation].GetSpriteFromAnimation();
        }

        /// <summary>
        /// Установка текущей анимации
        /// </summary>
        /// <param name="name">Ключ анимации</param>
        public void SetAnimation(string name)
        {
            if (currentAnimation == name) return;

            if(currentAnimation != "")
                animations[currentAnimation].ResetAnimation();

            currentAnimation = name;
        }
    }
}
