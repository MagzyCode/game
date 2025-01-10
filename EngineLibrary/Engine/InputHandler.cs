using System;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Обработчик ввода с клавиатуры
    /// </summary>
    public class InputHandler : IDisposable
    {
        private DirectInput directInput;
        private Keyboard keyboard;

        /// <summary>
        /// Состояние клавиатуры
        /// </summary>
        public virtual KeyboardState KeyboardState { get => keyboardState; }
        private KeyboardState keyboardState;

        /// <summary>
        /// Удалось ли обновить состоние клавиатуры
        /// </summary>
        public virtual bool KeyboardUpdated { get; private set; }

        private bool keyboardAcquired;
        
        // Для тестирования
        public InputHandler() 
        { }

        /// <summary>
        /// Конструктор класса, инциализирующий компоненты обработчика
        /// </summary>
        /// <param name="renderForm">Form отрисовки</param>
        public InputHandler(RenderForm renderForm)
        {
            directInput = new DirectInput();

            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 16;
            AcquireKeyboard();

            keyboardState = new KeyboardState();
        }

        /// <summary>
        /// Попытка получить доступ к клавиатуре
        /// </summary>
        private void AcquireKeyboard()
        {
            try
            {
                keyboard.Acquire();
                keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    keyboardAcquired = false;
            }
        }

        /// <summary>
        /// Обновление состония клавиатуры
        /// </summary>
        public void UpdateKeyboardState()
        {
            if (!keyboardAcquired) AcquireKeyboard();

            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                keyboard.GetCurrentState(ref keyboardState);
                KeyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                KeyboardUpdated = false;
            }

            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                keyboardAcquired = false;
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref keyboard);
            Utilities.Dispose(ref directInput);
        }
    }
}
