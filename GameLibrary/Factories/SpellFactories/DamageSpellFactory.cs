using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;

namespace GameLibrary.Spells.SpellFactories
{
    /// <summary>
    /// Класс фабрики создания заряда урона 
    /// </summary>
    public class DamageSpellFactory : SpellFactory
    {
        /// <summary>
        /// Создание игрового объекта заряда, которая убивает
        /// </summary>
        /// <param name="position">Позиция появления заряда</param>
        /// <param name="direction">Направление заряда</param>
        /// <param name="tag">Тег игрового объекта, создающий заряд</param>
        /// <returns>Игровой объект</returns>
        public override GameObject CreateSpell(Vector2 position, Vector2 direction, string tag, float power = 1)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Spells/damage spell.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.4f, 0.4f)));
            gameObject.GameObjectTag = "Spell";

            DamageSpell spell = new DamageSpell();
            spell.SetSettings(direction, tag, power);
            gameObject.InitializeObjectScript(spell);

            return gameObject;
        }
    }
}
