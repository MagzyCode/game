using EngineLibrary.ObjectComponents;
using SharpDX;

namespace GameLibrary.Spells.SpellFactories
{
    /// <summary>
    /// Абстрактный класс фабрики создания пуль 
    /// </summary>
    public abstract class SpellFactory
    {
        /// <summary>
        /// Создание игрового объекта заряда
        /// </summary>
        /// <param name="position">Позиция появления заряда</param>
        /// <param name="direction">Направление заряда</param>
        /// <param name="tag">Тег игрового объекта, создающий заряд</param>
        /// <returns>Игровой объект</returns>
        public abstract GameObject CreateSpell(Vector2 position, Vector2 direction, string tag, float power = 1);
    }
}
