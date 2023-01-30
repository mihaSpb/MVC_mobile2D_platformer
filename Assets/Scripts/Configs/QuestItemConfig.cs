using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    [CreateAssetMenu(fileName = "QuestItemCfg", menuName = "Configs/ Quest Item Cfg", order = 1)]
    public class QuestItemConfig : ScriptableObject
    {
        public int questId; // ID квеста
        public List<int> questItemCollection; // Коллекция квестовых предметов
    }
}