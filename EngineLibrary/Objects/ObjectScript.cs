namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Абстрактный класс сценария поведения игрового объекта
    /// </summary>
    public abstract class ObjectScript
    {
        /// <summary>
        /// Игровой объект, которым управляет сценарий
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// Инициализация сценария
        /// </summary>
        /// <param name="gameObject">Игровой объект, который выполняет сценарий</param>
        public void Initialize(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
        
        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public abstract void Update();
    }
}
