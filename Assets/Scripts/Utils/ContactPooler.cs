using UnityEngine;


namespace Platformer2D
{

    // Класс работы с контактами игрока с другими объектами
    public class ContactPooler
    {
        // Массив контактов, сразу инициализируем его
        private ContactPoint2D[] _contacts = new ContactPoint2D[10];
        private int _contactCount; // Количество контактов
        private Collider2D _collider; // Колайдер игрока
        private float _treshHold = 0.2f; // Погрешность при наступлении контакта

        // Свойства для определения с какой стороны произошел контакт
        public bool IsGrounded { get; private set; }
        public bool LeftContact { get; private set; }
        public bool RightContact { get; private set; }

        // Конструктор назначает коллайдер
        public ContactPooler(Collider2D collider)
        {
            _collider = collider;
        }


        public void Update()
        {
            // Обнуляем поля
            IsGrounded = false;
            LeftContact = false;
            RightContact = false;

            // Считываем количество контактов из нашего коллайдера игрока
            _contactCount = _collider.GetContacts(_contacts);

            // Проходим по массиву и проверяем с какой стороны контакт
            for(int i = 0; i < _contactCount; i ++)
            {
                if (_contacts[i].normal.y > _treshHold)
                {
                    IsGrounded = true;
                }

                if (_contacts[i].normal.x > _treshHold)
                {
                    LeftContact = true;
                }

                if (_contacts[i].normal.x > -_treshHold)
                {
                    RightContact = true;
                }
            }
        }
    }
}