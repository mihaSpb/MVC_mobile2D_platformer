using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    // Здесь происходит инициализация всех квестов
    public class QuestConfiguratorController
    {
        private QuestObjectView _singleQuestView; // Вьюшка для сингквеста, она нужна для инициализации
        private QuestController _singleQuestController; // Контроллер синглквеста 
        private CoinQuestModel _model; // Модель абстрактного квеста

        private QuestStoryConfig[] _questStoryConfig; // Массив для конфигов квестовых историй
        private QuestObjectView[] _questObjects; // Массив для квестовых объектов

        private List<IQuestStory> _questStories; // Пока нет квестовых историй, то пока прописываем лист интерфесов IQuestStory 

        // Конструктор, принимает вьюшку квеста
        public QuestConfiguratorController(QuestView view)
        {
            _singleQuestView = view._singleQuest; // Получаем вьюшку квеста 
            _model = new CoinQuestModel(); // Создаем модель
            _questStoryConfig = view._questStoryConfig; // Вьюшка конфигуратора цепочки квестов
            _questObjects = view._questObjects; // Вьюшка для квестовых объектов
        }


        // Словарь для хранения (по ключ-значению) квестов. Ключ: тип квеста, значение: модель, для того чтобы создавать разные типы фабрик
        // В нашем случае модель только одна. Func - для инициализации словаря
        private readonly Dictionary<QuestType, Func<IQuestModel>> _questFactory = new Dictionary<QuestType, Func<IQuestModel>>(1); // Один элемент в словаре
        // Словарь для хранения (по ключ-значению) квестовых цепочек. Ключ: тип цепочек квестов, значение: лист квестов (IQuest - интерфейс квеста и IQuestStory - интерфейс квестовой истории)
        private readonly Dictionary<QuestStoryType, Func<List<IQuest>, IQuestStory>> _questStoryFactory = new Dictionary<QuestStoryType, Func<List<IQuest>, IQuestStory>>(2); // Два элемента - у нас пока всего два типа квестов


        // Метод инициализации, для того чтобы его можно было вызывать отдельно, если понадобиться перезапускать часть квестовой системы
        public void Init()
        {
            // Создаем квест контроллер (передаем вьюшку синглквеста и модель)
            _singleQuestController = new QuestController(_singleQuestView, _model);
            _singleQuestController.ResetIQuest(); // Активация квеста через метод Reset


            // Добавляем тип квеста Common и questCollection (создаем экземпляр QuestStoryController и в него передаем questCollection)
            _questStoryFactory.Add(QuestStoryType.Common, questCollection => new QuestStoryController(questCollection));
            _questFactory.Add(QuestType.Coins, () => new CoinQuestModel()); // Добавляем в _questFactory ТипКвеста (Coin) и инициализируем модель CoinQuestMod, в качестве значения

            _questStories = new List<IQuestStory>(); // Лист квестовых историй, инициализируем его

            // Создаем квестовые истории на основе конфига квестовой истории
            foreach(QuestStoryConfig questStCfg in _questStoryConfig)
            {
                _questStories.Add(CreateQuestStory(questStCfg)); // Инициализация квестовой истории
            }
        }


        // Метод создания квеста
        private IQuest CreateQuest(QuestConfig config)
        {
            int questID = config.id; // Запрашиваем ID квеста
            // Запрашиваем вьюшку квеста: ищем через FirstOrDefault, в условиях: если value.Id == config.id, то мы нашли нужный нам квест
            // и прикрепляем его к questView
            QuestObjectView questView = _questObjects.FirstOrDefault(value => value.Id == config.id);

            // Проверка, а есть ли вьюшка
            if(questView == null)
            {
                Debug.Log("No views");
                return null;
            }

            // Запрашиваем тип квеста и получаем фабрику 
            if(_questFactory.TryGetValue(config.questType, out var factory))
            {
                IQuestModel questModel = factory.Invoke(); // Создаем модель квеста
                return new QuestController(questView, questModel); // Создаем квест контроллелер и возвращаем его
            }

            Debug.Log("No model");
            return null;
        }


        // Метод создания квестовой истории
        private IQuestStory CreateQuestStory(QuestStoryConfig cfg)
        {
            List<IQuest> quests = new List<IQuest>(); // Лист квестов

            // Заполнение листа квестов
            foreach(QuestConfig questCFG in cfg.quests)
            {
                IQuest quest = CreateQuest(questCFG);

                if(quest == null)
                {
                    continue;
                }

                quests.Add(quest);
                Debug.Log("AddQuest " + quest);
            }

            // Возвращаем созданную квест стори
            return _questStoryFactory[cfg.type].Invoke(quests);
        }
    }
}