using System;


namespace Platformer2D
{
    // Этот класс отвечает за менеджмент квеста, реализовывать интерфейс IQuest
    // и отвечать за активацию, сброс, выполнение квеста и иметь метод Dispose
    public class QuestController : IQuest
    {
        // 
        public event EventHandler<IQuest> Completed;

        public bool IsCompleted { get; private set; } // Переменная, показывающая закончен квест или нет

        private QuestObjectView _view; // Вьюшка квестового предмета
        private bool _active; // Активен наш квест или нет
        private IQuestModel _model; // Ссылка на модель


        // Конструктор, принимает вьюшку и модель
        public QuestController(QuestObjectView view, IQuestModel model)
        {
            _view = view;
            _model = model;
        }


        // Обработчик события - проверяет можем ли мы завершать квест или нет
        private void OnContact(LevelObjectView arg)
        {
            // Эту переменную получаем на основе модели (модель содержит метод TryComplete)
            bool complete = _model.TryComplete(arg.gameObject);

            if(complete)
            {
                Complete();
            }
        }


        // Метод выполнение квеста
        public void Complete()
        {
            if(!_active)
            {
                return;
            }

            _active = false;
            _view.OnLevelObjectContact -= OnContact; // Отписываемся от события
            _view.ProcessComplete(); // Выполнение действия окончания квеста (в нашем случае - перекрашивание цвета монеты)
            Completed?.Invoke(this, this); // Выполнение события о том, что квест завершился
        }


        // Перезапуск квеста
        public void ResetIQuest()
        {
            if (_active)
            {
                return;
            }

            _active = true;
            _view.OnLevelObjectContact += OnContact; // Подписываемся на событие "контакт"
            _view.ProcessActivate(); // Активация квеста
        }


        // Отписываемся от контакта
        public void Dispose()
        {
            _view.OnLevelObjectContact -= OnContact;
        }

    }
}