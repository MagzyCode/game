using SharpDX;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Компонент перемещения игрового объекта
    /// </summary>
    public class TransformComponent
    {
        private const float accelerationOfGravity = 0.0981f;

        /// <summary>
        /// Позиция игрового объекта
        /// </summary>
        public Vector2 Position { get => position; set => position = value; }
        private Vector2 position;

        /// <summary>
        /// Размер игрового объекта
        /// </summary>
        public Size2F Scale { get => scale; set => scale = value; }
        private Size2F scale;

        /// <summary>
        /// Использование гравитации
        /// </summary>
        public bool IsUseGravitation { get; set; } = true;

        private Vector2 movementInCurrentFrame;

        /// <summary>
        /// Конструктор компонента
        /// </summary>
        /// <param name="position">начальная позиция</param>
        /// <param name="scale">Начальный размер</param>
        public TransformComponent(Vector2 position, Size2F scale)
        {
            movementInCurrentFrame = Vector2.Zero;
            this.position = position;
            this.scale = scale;
        }

        /// <summary>
        /// Перемещение объкта
        /// </summary>
        /// <param name="movement">Вектор перемещения</param>
        public void SetMovement(Vector2 movement)
        {            
            movementInCurrentFrame = movement;

            position.X += movement.X;
            position.Y -= movement.Y;
        }

        /// <summary>
        /// Возврат позиция в этом кадре
        /// </summary>
        public void ResetMovement()
        {
            position.X -= movementInCurrentFrame.X;
            if(!IsUseGravitation)
                position.Y += movementInCurrentFrame.Y;
        }

        /// <summary>
        /// Добавление силы тяжести
        /// </summary>
        public void AddGravitation()
        {
            if(IsUseGravitation)
                position.Y += accelerationOfGravity;
        }

        /// <summary>
        /// Возрат силы тяжести
        /// </summary>
        public void ResetGravitation()
        {
            if (IsUseGravitation)
                position.Y -= accelerationOfGravity;
        }
    }
}
