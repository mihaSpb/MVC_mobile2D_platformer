using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;


namespace Platformer2D
{

    // Это наша вьюшка и в методе Main мы будем перекидывать эти ссылки в контроллер, который уже будет управлять объектом на сцене
    public class LevelObjectView : MonoBehaviour
    {
        // Здесь мы прописываем ссылки
        // Нам понадобиться трансформ - кешируем его, чтобы не использовать прожорливый GetComponent
        public Transform _transform;
        // Обращаемся к SpriteRenderer, т.к. мы работаем со спрайтами
        public SpriteRenderer _spriteRenderer;
        // Обращение к физике
        public Collider2D _collider;
        public Rigidbody2D _rigidbody;

        // Детектор столкновений (контактов объектов) и на основании этого сделаем
        // событие на которое будем подписываться/отписываться
        public Action<LevelObjectView> OnLevelObjectContact { get; set; }

        // Событие
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Здесь получаем GAmeObject из коллизии и запрашиваем у него компонент LevelObjectView
            LevelObjectView LevelObject = collision.gameObject.GetComponent<LevelObjectView>();
            // Через инвок передаем LevelObject
            OnLevelObjectContact?.Invoke(LevelObject);
        }

    }
}


