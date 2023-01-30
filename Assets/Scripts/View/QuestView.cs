using UnityEngine;


namespace Platformer2D
{
    public class QuestView : MonoBehaviour
    {
        public QuestObjectView _singleQuest; // Вьшка для сингквеста

        public QuestStoryConfig[] _questStoryConfig; // Массив для сториконфиг
        public QuestObjectView[] _questObjects; // Массив квестовых предметов
    }
}