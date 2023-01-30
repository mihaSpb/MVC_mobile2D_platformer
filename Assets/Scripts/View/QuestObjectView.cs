using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class QuestObjectView : LevelObjectView
    {
        [SerializeField] private int _id; //Принадлежность вьюшки к определенному квесту
        [SerializeField] private Color _completedColor; //При выполнении квеста будет меняться цвет 
        [SerializeField] private Color _defaultColor; //у объекта (можно сделать запись в журнале или еще что-то другое)

        public int Id { get => _id; set => _id = value; }

        private void Awake()
        {
            _defaultColor = _spriteRenderer.color;
        }


        public void ProcessComplete()
        {
            _spriteRenderer.color = _completedColor;
        }


        public void ProcessActivate()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}