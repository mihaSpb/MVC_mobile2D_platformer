using UnityEngine;


namespace Platformer2D
{
    public interface IQuestModel
    {
        // Проверяем, с кем мы столкнулись, для того чтобы понять
        // можем ли мы завершить квест или нет
        // В данном случае нужно, чтобы столкновение было с игроком
        bool TryComplete(GameObject actor);
    }
}