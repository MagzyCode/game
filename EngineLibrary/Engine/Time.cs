using System;
using System.Diagnostics;

namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Статический класс управления временем
    /// </summary>
    public static class Time
    {
        private readonly static Stopwatch watch;

        private static long previousTicks;

        /// <summary>
        /// Текущее время с запуска приложения
        /// </summary>
        public static float CurrentTime { get; private set; }

        /// <summary>
        /// Разница во времени между кадрами
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// Конструктори статического класса
        /// </summary>
        static Time()
        {
            watch = new Stopwatch();
            Reset();
        }

        /// <summary>
        /// Обновление подсчитанных значений
        /// </summary>
        public static void UpdateTime()
        {
            long ticks = watch.Elapsed.Ticks;

            CurrentTime = (float)ticks / TimeSpan.TicksPerSecond;
            DeltaTime = (float)(ticks - previousTicks) / TimeSpan.TicksPerSecond;

            previousTicks = ticks;
        }

        /// <summary>
        /// Сброс таймера и счетчика
        /// </summary>
        public static void Reset()
        {
            watch.Reset();
            watch.Start();
            previousTicks = watch.Elapsed.Ticks;
        }
    }
}
