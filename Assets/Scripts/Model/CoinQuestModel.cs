using UnityEngine;


namespace Platformer2D
{
    public class CoinQuestModel : IQuestModel
    {

        private const string Tag = "Player";

        public bool TryComplete(GameObject activator)
        {
            return activator.CompareTag(Tag); // Возвращаем результат сравнения, через тег 
        }
    }
}

// Если нужный тег столкнулся с нашим квестовым объектом, то метод возвращает true, что является сигналом завершения квеста