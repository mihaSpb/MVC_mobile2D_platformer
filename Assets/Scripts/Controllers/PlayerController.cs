using UnityEngine;

namespace Platformer2D
{
    // Контроллер игрока, основанный на физике
    public class PlayerController
    {
        // Скорость персонажа - при управлении через физику нужно ставить скорость выше 
        private float _speed = 150.0f;
        private float _animationSpeed = 15.0f;
        private float _jumpSpeed = 1.0f;

        // 
        private float _movingTresh = 0.1f;
        private float _jumpTresh = 1.0f;

        // Единичные векторы для управления движением и поворачивает персонажа,
        // в соответствующую сторону
        private Vector3 _leftScale = new Vector3(-1, 1, 1);
        private Vector3 _rightScale = new Vector3(1, 1, 1);

        private float _xVelocity; // Ускорение
        private bool _doJump;  // Находимся ли мы в полете
        private float _xAxisInput; // Временная переменная для хранения результатов из инпута

        private LevelObjectView _view; // Ссылка на вьюшку игрока
        private SpriteAnimatorController _animatorController; // На контроллер анимации
        private ContactPooler _contactPooler; //


        // Конструктор: 
        public PlayerController(LevelObjectView view, SpriteAnimatorController spriteAnimator)
        {
            _view = view;
            _animatorController = spriteAnimator;
            _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Idle, true, _animationSpeed);
            _contactPooler = new ContactPooler(_view._collider); // инициализируем контакт пулер для дальнейшего использования
        }


        public void UpdateStateObject()
        {

            _animatorController.Update(); // Вызов метода обновления анимации из контроллера анимации
            _contactPooler.Update(); // Вызов контакт пулера

            _doJump = Input.GetAxis("Vertical") > 0; // Отслеживаем состояние инпута по оси Y
            _xAxisInput = Input.GetAxis("Horizontal");  // Отслеживаем состояние инпута по горизонтальной оси
            bool Move = Mathf.Abs(_xAxisInput) > _movingTresh; // Проверка: есть ли движение в инпуте (нажата ли кнопка вправо/влево)




            if (!Move) // Если не двигаемся, то включаем анимацию Idle
            {
                _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Idle, true, _animationSpeed);
            }

            if (Move) // Если можем двигаться, то двигаемся
            {
                MoveTowards();
            }


            // Если мы стоим на земле, то включаем анимацию движения
            if(_contactPooler.IsGrounded)
            {
                _animatorController.StartAnimaton(_view._spriteRenderer, Move ? AnimState.Run : AnimState.Idle, true, _animationSpeed);

                // Проверяем прыжок, через velocity по оси Y (через риджитбоди)
                if (_doJump && _view._rigidbody.velocity.y <= _jumpTresh)
                {
                    // прыгаем через AddForce (вектор2 вверх - прыгаем вверх и умножаем на скорость прыжка)
                    _view._rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else
            {
                // Если мы в полете: через velocity по оси Y (через риджитбоди)
                if (Mathf.Abs(_view._rigidbody.velocity.y) > _jumpTresh)
                {
                    // Запуск анимации полета/прыжка
                    _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Jump, true, _animationSpeed);
                }
            }
        }

        private void MoveTowards()
        {
            // Перемещение - есть или нет (velocity) и установка направления (через назначение 1 или -1, в зависимости от нажатой кнопки)
            // fixedDeltaTime - работает более адекватно при перемещении по физике
            _xVelocity = Time.fixedDeltaTime * _speed * (_xAxisInput < 0 ? -1 : 1);
            // Далее обращаемся к риджитбоди и назначим изменение velocity по оси Х
            _view._rigidbody.velocity = _view._rigidbody.velocity.Change(x: _xVelocity);
            // Изменение направления персонажа
            _view._transform.localScale = (_xAxisInput < 0 ? _leftScale : _rightScale);
        }
    }
}