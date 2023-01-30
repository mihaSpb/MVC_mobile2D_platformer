using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    [CreateAssetMenu(fileName = "QuestStoryCfg", menuName = "Configs/ Quest Story Cfg", order = 1)]
    public class QuestStoryConfig : ScriptableObject
    {
        public QuestConfig[] quests; // Список квестов
        public QuestStoryType type; // Тип квеста
    }

    public enum QuestStoryType
    {
        Common = 0, // Одноразовый
        Resettable = 1 // Перезапускаемый
    }
}