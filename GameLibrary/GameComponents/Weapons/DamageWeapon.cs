using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Spells.SpellFactories;
using GameLibrary.Game;
using SharpDX;

namespace GameLibrary.Weapons
{
    /// <summary>
    /// Класс оружия урона
    /// </summary>
    public class DamageWeapon : Weapon
    {
        /// <summary>
        /// Загрузка оружия
        /// </summary>
        protected override void LoadAnimation()
        {
            Animation animation;

            animation = new Animation(RenderingSystem.LoadAnimation("Resources/Monsters/damage Weapon/damage left idle ", 1), 0.2f, true);
            gameObject.Sprite.AddAnimation("idleLeft", animation);
            gameObject.Sprite.AddAnimation("runLeft", animation);
            gameObject.Sprite.AddAnimation("idleRight", animation);
            gameObject.Sprite.AddAnimation("runRight", animation);
            gameObject.Sprite.SetAnimation("idleLeft");
        }

        /// <summary>
        /// Создание заряда из фабрики
        /// </summary>
        /// <param name="position">Позиция создания</param>
        /// <param name="direction">Направление заряда</param>
        protected override void SpawnSpell(Vector2 position, Vector2 direction, float power = 1)
        {
            DamageSpellFactory factory = new DamageSpellFactory();
            maze.AddObjectOnScene(factory.CreateSpell(position, direction, gameObject.ParentGameObject.GameObjectTag, power));    
        }
    }
}
