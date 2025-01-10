using EngineLibrary.ObjectComponents;
using GameLibrary.Effects.EffectFactories;

namespace GameLibrary.Spells
{
    /// <summary>
    /// Класс заряда урона
    /// </summary>
    public class DamageSpell : Spell
    {
        /// <summary>
        /// Скорость заряда
        /// </summary>
        public override float Speed => 10f;

        /// <summary>
        /// Взаимодействие с игроком
        /// </summary>
        public override void PlayerInteraction(GameObject playerGameObject)
        {
            DamageEffectFactory factory = new DamageEffectFactory();
            maze.AddObjectOnScene(factory.CreateEffect(playerGameObject, tag, power));
        }
    }
}
