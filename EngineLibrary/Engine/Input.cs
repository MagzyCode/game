using SharpDX.DirectInput;
using System;

namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Класс, позволяющий управлять вводом с клавиатуры
    /// </summary>
    public static class Input
    {
        private static InputHandler inputHandler;

        /// <summary>
        /// Установка обработчика ввода с клавиатуры
        /// </summary>
        /// <param name="input">Обработчик ввода с клавиатуры</param>
        public static void SetupInputHandler(InputHandler input)
        {
            inputHandler = input;
        }

        /// <summary>
        /// Метод, возращающий значение ввода основных осей направления
        /// </summary>
        /// <param name="axis">Ось направления</param>
        /// <returns>Положительное или отрицательное значение оси</returns>
        public static int GetAxis(AxisOfInput axis)
        {
            if (inputHandler == null) return 0;

            int move = 0;

            if (inputHandler.KeyboardUpdated)
            {
                switch(axis)
                {
                    case AxisOfInput.Horizontal:
                        if (inputHandler.KeyboardState.IsPressed(Key.D)) move++;
                        if (inputHandler.KeyboardState.IsPressed(Key.A)) move--;
                        break;
                    case AxisOfInput.Vertical:
                        if (inputHandler.KeyboardState.IsPressed(Key.W)) move++;
                        if (inputHandler.KeyboardState.IsPressed(Key.S)) move--;
                        break;
                    case AxisOfInput.AlternativeHorizontal:
                        if (inputHandler.KeyboardState.IsPressed(Key.Right)) move++;
                        if (inputHandler.KeyboardState.IsPressed(Key.Left)) move--;
                        break;
                    case AxisOfInput.AlternativeVertical:
                        if (inputHandler.KeyboardState.IsPressed(Key.Up)) move++;
                        if (inputHandler.KeyboardState.IsPressed(Key.Down)) move--;
                        break;
                }
            }

            return move;
        }

        /// <summary>
        /// Метод, возращающий реакцию на нажатие клавиши ввода
        /// </summary>
        /// <param name="key">Клавиша ввода</param>
        /// <returns>Реакция true или false</returns>
        public static bool GetButtonDawn(Key key)
        {
            if (inputHandler == null) return false;

            if (inputHandler.KeyboardUpdated)
            {
                return inputHandler.KeyboardState.IsPressed(key);
            }

            return false;
        }

    }

    /// <summary>
    /// Ось направления ввода
    /// </summary>
    public enum AxisOfInput
    {
        /// <summary>
        /// Горизонтальная ось
        /// </summary>
        Horizontal = 0,
        /// <summary>
        /// Вертикальная ось
        /// </summary>
        Vertical = 1,
        /// <summary>
        /// Альтернативная горизонтальная ось
        /// </summary>
        AlternativeHorizontal = 2,
        /// <summary>
        /// Альтернативная вертикальная ось 
        /// </summary>
        AlternativeVertical = 3,
    }
}
