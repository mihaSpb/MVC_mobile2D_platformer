using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class QuestStoryController : IQuestStory
    {
        private List<IQuest> _questCollection = new List<IQuest>(); // Лист с квестами (квесты разные и поэтому собираем по интерфейсу IQuest)
                                                                    // и сразу инициализируем лист

        // В интерфейсе IsDone - свойство. В интерфейсе IQuest есть свойство IsCompleted и его мы можем проверять у всех квестов.
        // И если у всех квестов оно true, то и IsDone будет true.
        // Воспользуемся System.Linq для вызова всех значений квестов (первый =>)
        // через лямбду указываем (value => value.IsCompleted), что нам нужны определенные значения All - если все IsCompleted
        // являются true, то и IsDone тоже является true
        public bool IsDone => _questCollection.All(value => value.IsCompleted);


        // Конструктор принимает лист квестов и запускает метод подписки на все квесты и ресет квестов
        public QuestStoryController(List<IQuest> questCollection)
        {
            _questCollection = questCollection;
            Subscribe();
            Reset(0); // 0 - начальный элемент коллекции квестов
        }


        // Подписка на квесты
        private void Subscribe()
        {
            foreach (IQuest quest in _questCollection) // В цикле подписываемся на все квесты из листа квестов
            {
                quest.Completed += OnQuestCompleted; // И каждую итерацию цикла каждый квест подписывается на обработчик событий 
            }
        }


        private void Unsubscribe()
        {
            foreach (IQuest quest in _questCollection)
            {
                quest.Completed -= OnQuestCompleted; // Отписываемся от квеста
            }
        }

        // Обработчик завершенности квеста: проверяем IsDone, если true, то выполняется логика, после выполнения квеста
        private void OnQuestCompleted(object sender, IQuest quest)
        {
            int index = _questCollection.IndexOf(quest); // Индекс проверяемого квеста

            if (IsDone)
            {
                Debug.Log("Story is done"); // Любая логика, которая выполняется после выполнения квеста: начисление очков, вызов диалогового окна и тд
            }
            else
            {
                Reset(++index); // Сброс квеста
            }
        }


        // Обработчик сброса квеста для index квеста, коорый необходимо сбросить
        private void Reset(int index)
        {
            // Проверяем, входит ли квест в коллекцию квестов, не выходи ли он за границы диапазона
            if(index < 0 || index >= _questCollection.Count)
            {
                return;
            }

            // Временная переменная для индекса квеста
            IQuest nextQuest = _questCollection[index];

            // Если квест выполнен, то отсылаем событие, что квест завершился
            if (nextQuest.IsCompleted)
            {
                OnQuestCompleted(this, nextQuest);
                Debug.Log(nextQuest);
            }
            else
            {
                _questCollection[index].ResetIQuest(); // Сбрасываем _questCollection для квеста, перезапускаем его
            }
        }


        public void Dispose()
        {
            Unsubscribe(); // Метод отписки от квеста

            foreach (IQuest quest in _questCollection)
            {
                quest.Dispose(); // В цикле вызываем метод Dispose из квеста, для очистки памяти
            }
        }
    }
}