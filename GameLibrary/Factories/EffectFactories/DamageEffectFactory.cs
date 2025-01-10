using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Monsters;
using SharpDX;

namespace GameLibrary.Effects.EffectFactories
{
    /// <summary>
    /// Фабрика создания эффекта смерти
    /// </summary>
    public class DamageEffectFactory : EffectFactory
    {
        /// <summary>
        /// Создание могилы
        /// </summary>
        /// <returns>Игровой объект</returns>
        public override GameObject CreateEffect(GameObject gameObj, string tag = null, float power = 1)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(gameObj.Transform.Position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/Effects/damage idle 1.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.8f, 0.8f)));
            gameObject.GameObjectTag = "Effect";
            DamageEffect damageEffect = new DamageEffect();
            damageEffect.ActivateEffect(gameObj, tag, power);

            switch (gameObj.Script)
            {
                case Monster scriptMonster:
                    if(scriptMonster.Health > 0)
                        gameObject.Sprite.Bitmap = null;
                    break;
                case Player scriptPlayer:
                    if (scriptPlayer.Property.Health > 0)
                        gameObject.Sprite.Bitmap = null;
                    break;
            }

            gameObject.InitializeObjectScript(damageEffect);

            return gameObject;
        }
    }
}
