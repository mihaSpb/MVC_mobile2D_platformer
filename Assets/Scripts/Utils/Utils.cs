using UnityEngine;

// Расширение для облегчения работы с Vector3/Vector2
namespace Platformer2D
{
    public static class Utils
    {
        // Сюда мы передаем Вектор3 (вообще эта конструкция довольно дорогая, но если ее не выполнять в RunTime, то норм) - здесь
        // происходит Boxing/OnBoxing, т.к. мы оборачиваем данные Вектор3 в объекты - Вектор3 будет передаваться,
        // как три объекта: object.x, object.y, object.z
        // 
        public static Vector3 Change(this Vector3 org, object x = null, object y = null, object z = null)
        {
            // Если переданный сюда Х равен null (или мы его вообще не передали), то возвращаем Х, который был, в обратном случае
            // возвращаем новый Х - заменяем его. При этом явно приводим его к float
            // Для остальных координат та же история. То же самое и для Вектор2
            return new Vector3(x == null ? org.x : (float)x, y == null ? org.y : (float)y, z == null ? org.z : (float)z);
        }

        public static Vector2 Change(this Vector2 org, object x = null, object y = null)
        {
            return new Vector2(x == null ? org.x:(float)x, y == null ? org.y:(float)y);
        }
    }
}