using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D
{
    [CreateAssetMenu(fileName = "QuestCfg", menuName = "Configs/ Quest Cfg", order = 1)]
    public class QuestConfig : ScriptableObject
    {
        public int id; // ID квеста
        public QuestType questType; // Тип квеста 
        
    }

    public enum QuestType
    {
        Coins = 0
    }
}