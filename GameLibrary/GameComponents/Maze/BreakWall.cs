using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс ломающейся стены
    /// </summary>
    public class BreakWall : ObjectScript
    {
        private const float timeToRespawn = 4f;

        private float respawnTime;

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            Animation animation = new Animation(RenderingSystem.LoadAnimation("Resources/MazeElements/BreakWall ", 1), 10f, true);
            gameObject.Sprite.AddAnimation("idleWall", animation);
            animation = new Animation(RenderingSystem.LoadAnimation("Resources/MazeElements/BreakWall ", 3), 0.2f, false);
            gameObject.Sprite.AddAnimation("breakWall", animation);
            gameObject.Sprite.SetAnimation("idleWall");
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            if(gameObject.Sprite.Bitmap == null && gameObject.IsActive)
            {
                gameObject.IsActive = false;
                respawnTime = Time.CurrentTime + timeToRespawn;
            }

            if(!gameObject.IsActive)
            {
                if(respawnTime < Time.CurrentTime)
                {
                    gameObject.IsActive = true;

                    if (!gameObject.Collider.CheckIntersection("Blue Player","Red Player"))
                    {
                        gameObject.Sprite.SetAnimation("idleWall");
                    }
                    else
                    {
                        gameObject.IsActive = false;
                    }
                }
            }
        }

        /// <summary>
        /// Уничтожении стены
        /// </summary>
        public void DestroyWall()
        {
            gameObject.Sprite.SetAnimation("breakWall");
        }
    }
}
