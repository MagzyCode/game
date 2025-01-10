using SharpDX;
using System.Collections.Generic;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Класс компонента, описывающий твердое тело
    /// </summary>
    public class ColliderComponent
    {
        private static List<GameObject> collidersOfGameObjects;

        private readonly Vector2[] boundCorners;

        private readonly GameObject gameObject;

        private Size2F colliderScale;

        private Vector2 offsetCollider;

        /// <summary>
        /// Неактивность элемента
        /// </summary>
        public bool IsInactive { get; set; }

        /// <summary>
        /// Свойство CollidersOfGameObjects
        /// </summary>
        public static List<GameObject> CollidersOfGameObjects
        {
            get => collidersOfGameObjects;
            set => collidersOfGameObjects = value;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="gameObject">Игровой объект, которому принадлежит компонент</param>
        /// <param name="scale">Размер коллайдера</param>
        /// <param name="offset">Cмещения коллайдера от центра</param>
        public ColliderComponent(GameObject gameObject, Size2F scale, Vector2 offset = new Vector2())
        {
            if (collidersOfGameObjects == null)
                collidersOfGameObjects = new List<GameObject>();

            collidersOfGameObjects.Add(gameObject);
            this.gameObject = gameObject;

            boundCorners = new Vector2[4];
            colliderScale = scale;
            offsetCollider = offset;
            IsInactive = false;
        }

        /// <summary>
        /// Обновление границ твердого тела
        /// </summary>
        private void UpdateBounds()
        {
            Vector2 position = gameObject.Transform.Position;

            float offsetWidth = colliderScale.Width / 2;
            float offsetHeight = colliderScale.Height / 2;

            boundCorners[0] = new Vector2(position.X + offsetCollider.X - offsetWidth, position.Y + offsetCollider.Y - offsetHeight);
            boundCorners[1] = new Vector2(position.X + offsetCollider.X - offsetWidth, position.Y + offsetCollider.Y + offsetHeight);
            boundCorners[2] = new Vector2(position.X + offsetCollider.X + offsetWidth, position.Y + offsetCollider.Y + offsetHeight);
            boundCorners[3] = new Vector2(position.X + offsetCollider.X + offsetWidth, position.Y + offsetCollider.Y - offsetHeight);
        }

        /// <summary>
        /// Проверка на пересечние компонента твердого тела с другими компонентами твердого тела, имеющие тег у игрового объекта
        /// </summary>
        /// <param name="tagNames">Теги игровых объектов, с которыми ожидается столкновение</param>
        /// <returns>Реакция на проверку</returns>
        public bool CheckIntersection(params string[] tagNames)
        {
            foreach (GameObject otherGameObject in collidersOfGameObjects)
            {
                if (otherGameObject == gameObject || otherGameObject.Collider.IsInactive) continue;

                bool hasTag = false;

                for (int i = 0; i < tagNames.Length && !hasTag; i++)
                {
                    hasTag = otherGameObject.GameObjectTag == tagNames[i];
                }

                if(hasTag)
                {
                    if (CheckGameObjectIntersection(otherGameObject))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Проверка на пересечние компонента твердого тела с другими компонентами твердого тела, имеющие тег у игрового объекта
        /// </summary>
        /// <param name="intersecredGameObject">Пересаемый объект</param>
        /// <param name="tagNames">Теги игровых объектов, с которыми ожидается столкновение</param>
        /// <returns>Реакция на проверку</returns>
        public bool CheckIntersection(out GameObject intersecredGameObject, params string[] tagNames)
        {
            foreach (GameObject otherGameObject in collidersOfGameObjects)
            {
                if (otherGameObject == gameObject || otherGameObject.Collider.IsInactive) continue;

                bool hasTag = false;

                for (int i = 0; i < tagNames.Length && !hasTag; i++)
                {
                        hasTag = otherGameObject.GameObjectTag == tagNames[i];
                }

                if (hasTag)
                {
                    if (otherGameObject.GameObjectTag != null && CheckGameObjectIntersection(otherGameObject))
                    {
                        intersecredGameObject = otherGameObject;
                        return true;
                    }
                }
            }

            intersecredGameObject = null;
            return false;
        }

        /// <summary>
        /// Проверка на пересечение компонента твердого тела с другими компонентами твердого тела, имеющие конкретный сценарий выполнения T
        /// </summary>
        /// <typeparam name="T">Конкретный сценарий выполнения</typeparam>
        /// <param name="objectScript">Сценарий выполнения игрового объекта, с которым столкнулся</param>
        /// <returns>Реакция на столкновение</returns>
        public bool CheckIntersection<T>(out T objectScript)
            where T : ObjectScript
        {
            foreach (GameObject otherGameObject in collidersOfGameObjects)
            {
                if (otherGameObject == gameObject || otherGameObject.Script == null || otherGameObject.Collider.IsInactive) continue;

                if (otherGameObject.Script is T && CheckGameObjectIntersection(otherGameObject))
                {
                    objectScript = (T)otherGameObject.Script;
                    return true;
                }
            }

            objectScript = null;
            return false;
        }

        /// <summary>
        /// Проверка на пересечение компонента твердого тела с другим компонентам твердого тела
        /// </summary>
        /// <param name="otherGameObject">Игровой объект с компонетом твердого тела</param>
        /// <returns>Реакция на столкновение</returns>
        private bool CheckGameObjectIntersection(GameObject otherGameObject)
        {
            UpdateBounds();
            otherGameObject.Collider.UpdateBounds();

            ColliderComponent collider = otherGameObject.Collider;

            int count = boundCorners.Length + collider.boundCorners.Length;

            Vector2[] allCorners = new Vector2[count];
            boundCorners.CopyTo(allCorners, 0);
            collider.boundCorners.CopyTo(allCorners, boundCorners.Length);

            Vector2 normal;

            bool isInteresect = false;

            for (int i = 0; i < count && !isInteresect; i++)
            {
                normal = GetNormal(allCorners, i);

                Vector2 currentProjection = GetProjection(normal);
                Vector2 otherProjection = collider.GetProjection(normal);

                if (currentProjection.X < otherProjection.Y || otherProjection.X < currentProjection.Y)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Создание нормали
        /// </summary>
        /// <param name="corners">Углы двух компонетов</param>
        /// <param name="index">Номер угла</param>
        /// <returns>Нормаль</returns>
        private Vector2 GetNormal(Vector2[] corners, int index)
        {
            int next = index + 1;
            next = next == corners.Length ? 0 : next;

            Vector2 firstPoint = corners[index];
            Vector2 secondPoint = corners[next];

            Vector2 edge = new Vector2(secondPoint.X - firstPoint.X, secondPoint.Y - firstPoint.Y);

            return new Vector2(-edge.Y, edge.X);
        }

        /// <summary>
        /// Создание проекции
        /// </summary>
        /// <param name="normal">Нормаль</param>
        /// <returns>Проекцию</returns>
        private Vector2 GetProjection(Vector2 normal)
        {
            Vector2 result = new Vector2();
            bool isNull = true;

            foreach(Vector2 current in boundCorners)
            {
                float projection = normal.X * current.X + normal.Y * current.Y;

                if(isNull)
                {
                    result = new Vector2(projection, projection);
                    isNull = false;
                }

                if (projection > result.X)
                    result.X = projection;
                if (projection < result.Y)
                    result.Y = projection;
            }

            return result;
        }
    }
}
