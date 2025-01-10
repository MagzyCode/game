using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Maze;

namespace GameLibrary.Effects
{
    /// <summary>
    /// Асбтрактный класс эффекта
    /// </summary>
    public abstract class Effect : ObjectScript
    {
        /// <summary>
        /// Время дейтсвия эффекта
        /// </summary>
        public abstract float EffectTime { get; }
        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;
        /// <summary>
        /// Ссылка на сценарий игрока, на которого действует эффект
        /// </summary>
        protected Player playerScript;
        /// <summary>
        /// Ссылка на игровой объект, на которого действует эффект
        /// </summary>
        protected GameObject playerGameObject;

        protected float currentEffectTime;

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            maze = MazeScene.instance;
            currentEffectTime = Time.CurrentTime + EffectTime;

            Initialize();
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            if (currentEffectTime < Time.CurrentTime)
                DeactivateEffect();

            BehaviorOnScene();
        }

        /// <summary>
        /// Активация эффекта
        /// </summary>
        /// <param name="player">Игровой объект, на который будет наложен эффект</param>
        public abstract void ActivateEffect(GameObject player, string tag = null, float power = 1);
        /// <summary>
        /// Инициализация эффекта
        /// </summary>
        protected abstract void Initialize();
        /// <summary>
        /// Деактивация эффекта
        /// </summary>
        protected abstract void DeactivateEffect();
        /// <summary>
        /// Поведение на сцене
        /// </summary>
        protected abstract void BehaviorOnScene();
    }
}
