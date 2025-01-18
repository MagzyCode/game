using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Weapons;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс подбираемых бонусов в лабиринте
    /// </summary>
    public class PrizeSpawn : ObjectScript
    {
        private MazeScene maze;
        private PlayerProperities dropOutPrize;
        private float cuurentTimeToDisappear;

        /// <summary>
        /// Инициализация места подбираемых бонусов
        /// </summary>
        /// <param name="disappearTime">Время исчезнование места</param>
        public void InitializeWeaponSpawn(PlayerProperities playerProperities, float disappearTime)
        {
            dropOutPrize = playerProperities;
            cuurentTimeToDisappear = Time.CurrentTime + disappearTime;
        }

        /// <summary>
        /// Инициализация места подбираемых зарядов
        /// </summary>
        /// <param name="disappearTime">Время исчезнование места</param>
        public void InitializeWeaponSpawn(float disappearTime)
        {
            cuurentTimeToDisappear = Time.CurrentTime + disappearTime;
        }

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start()
        {
            maze = MazeScene.instance;
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update()
        {
            if(cuurentTimeToDisappear < Time.CurrentTime)
            {
                maze.RemoveObjectFromScene(gameObject);
            }

            if (gameObject.Collider.CheckIntersection(out GameObject player,"Blue Player","Red Player"))
            {
                var enemyPrizeCaptured = maze.Client.EnemyCharacter.IsPlayerTryGetPrize;

                if ((player.GameObjectTag == "Blue Player" && maze.PlayerId == "1" && Input.GetButtonDawn((player.Script as Player).Control.GetKey))
                    || (player.GameObjectTag == "Red Player" && maze.PlayerId == "2" && Input.GetButtonDawn((player.Script as Player).Control.GetKey)))
                {
                    maze.Client.MyCharacter.IsPlayerTryGetPrize = true;

                    if (dropOutPrize == null)
                        (player.Script as Player).Property.SetProperty(TypeProperty.Ammo, 10);
                    else
                        (player.Script as Player).SetProperty(dropOutPrize);

                    maze.RemoveObjectFromScene(gameObject);
                }
                else if (enemyPrizeCaptured && ((player.GameObjectTag == "Blue Player" && maze.PlayerId == "2")
                    || (player.GameObjectTag == "Red Player" && maze.PlayerId == "1")))
                {
                    if (dropOutPrize == null)
                        (player.Script as Player).Property.SetProperty(TypeProperty.Ammo, 10);
                    else
                        (player.Script as Player).SetProperty(dropOutPrize);

                    maze.RemoveObjectFromScene(gameObject);
                }
                //if (player.GameObjectTag == "Blue Player" && Input.GetButtonDawn((player.Script as Player).Control.GetKey))
                //{
                //    if (dropOutPrize == null)
                //        (player.Script as Player).Property.SetProperty(TypeProperty.Ammo, 10);
                //    else
                //        (player.Script as Player).SetProperty(dropOutPrize);

                //    maze.RemoveObjectFromScene(gameObject);
                //}
                //else if (player.GameObjectTag == "Red Player" && Input.GetButtonDawn((player.Script as Player).Control.GetKey))
                //{
                //    if (dropOutPrize == null)
                //        (player.Script as Player).Property.SetProperty(TypeProperty.Ammo, 10);
                //    else
                //        (player.Script as Player).SetProperty(dropOutPrize);

                //    maze.RemoveObjectFromScene(gameObject);
                //}
            }
        }
    }
}
