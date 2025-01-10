using EngineLibrary.ObjectComponents;

namespace GameLibrary.Effects.EffectFactories
{
    /// <summary>
    /// Абстрактная фабрика создания эффектов
    /// </summary>
    public abstract class EffectFactory
    {
        /// <summary>
        /// Создание эффекта
        /// </summary>
        /// <param name="player">Игровой объект игрока</param>
        /// <returns>Игровой объект</returns>
        public abstract GameObject CreateEffect(GameObject player, string tag = null, float power = 1);
    }
}
