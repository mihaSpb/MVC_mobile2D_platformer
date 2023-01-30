using UnityEngine;

namespace Platformer2D
{
    // Здесь управление игроком без физики, только на векторах
    // Такая реализация (без физики) хороша для раннеров - одна пласкость и физика менее целесообразна в этом случае
    // Если в игре есть враги, стреляющие пушки, направленные на игрока, взаимодействие, то менее нагруженным будет использование физики
    // Если использовать вектора, то придется почти, что писать свой движок - логику столкновений, отслеживание игрока и прочее. Зачем, если есть
    // уже физический движок
    public class PlayerTransformController
    {
        // Скорость персонажа
        private float _speed = 4.0f;
        // Скорость анимации
        private float _animationSpeed = 10.0f;
        // Скорость прыжка
        private float _jumpSpeed = 8.0f;
        // Сила тяжести
        private float _g = -9.8f;
        // Погрешность реагирования на движение: если игрок скользит по склону или его
        // кто-то толкнул - это не должно включать анимацию движения
        private float _movingTresh = 0.1f;
        // Погрешность прыжка
        private float _jumpTresh = 1.0f;
        // Уровень земли (по оси Y), ниже которого игрок не провалиться (т.к. физики нет, то нет коллайдеров и игрок проваливается сквозь другие объекты)
        // Если писать полную имитацию физики, то лучше делать проверку пересечения границ - это будет более рационально. 
        private float _groundLevel = 0.2f;

        // Единичные векторы для управления движением и поворачивает персонажа,
        // в соответствующую сторону
        private Vector3 _leftScale = new Vector3(-1,1,1);
        private Vector3 _rightScale = new Vector3(1, 1, 1);

        // Ускорение
        private float _yVelocity;
        // Находимся ли мы в состоянии полета
        private bool _doJump;
        // Временная переменная для хранения результатов из инпута
        private float _xAxisInput;

        // Ссылки на вьюшку и контроллер анимации
        private LevelObjectView _view;
        private SpriteAnimatorController _animatorController;

        // Конструктор класса: принимает вьюшку, чтобы можно было манипулировать объектом и
        // спрайт аниматор контроллер, чтобы управлять анимацией
        public PlayerTransformController(LevelObjectView view, SpriteAnimatorController spriteAnimator)
        {
            _view = view;
            _animatorController = spriteAnimator;
            //Инициализация контроллера анимации
            _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Idle, true, _animationSpeed);
        }


        // Этот метод будет вызываться в классе Main - в нем обрабатывается движение персонажа и
        // управление анимацией в каждом кадре
        public void UpdateStateObject()
        {
            // Отслеживаем состояние инпута по оси Y
            _doJump = Input.GetAxis("Vertical") > 0;
            // Отслеживаем состояние инпута по горизонтальной оси
            _xAxisInput = Input.GetAxis("Horizontal");

            // Проверка: есть ли движение в инпуте (нажата ли кнопка вправо/влево)
            bool Move = Mathf.Abs(_xAxisInput) > _movingTresh;

            // Вызов метода обновления анимации из контроллера анимации
            _animatorController.Update();

            // Тело проверок и управления StateMashine
            // Проверки: где находится игрок - на земле или прыгает или идет. В соответствии с состоянием игрока
            // запускаем нужный трек анимации и меняем поведение игрока на сцене
            if (IsGrounded())
            {

                if (!Move)
                {
                    _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Idle, true, _animationSpeed);
                }

                if (Move)
                {
                    MoveTowards();
                    // Запуск анимации движения
                    _animatorController.StartAnimaton(_view._spriteRenderer, Move ? AnimState.Run : AnimState.Idle, true, _animationSpeed);
                }

                // Начало прыжка
                if(_doJump && _yVelocity == 0)
                {
                    _yVelocity = _jumpSpeed;
                }

                // Прыжок закончился
                else if(_yVelocity < 0)
                {
                    _yVelocity = 0;

                    // Заземляем игрока - меняем координату Y и присваиваем ей значение Y _groundLevel
                    _view._transform.position.Change(y: _groundLevel);
                }
            }
            // Находимся не на земле: проверяем движемся (летим) или нет, если да - то включаем управление полетом
            // или, как в нашем случае - управление обычным движением, без включения анимации хотьбы

            // Второй ИФ для включения гравитации и плавного снижения на землю после прыжка
            else
            {
                if(Move)
                {
                    MoveTowards();
                }

                // Проверяем, а не превышен ли порог прыжка, если превышает, то запускаем анимацию прыжка
                if(Mathf.Abs(_yVelocity) > _jumpTresh)
                {
                    _animatorController.StartAnimaton(_view._spriteRenderer, AnimState.Jump, true, _animationSpeed);
                }

                // Гравитация - плавное снижение
                _yVelocity += _g * Time.deltaTime;
                _view._transform.position += Vector3.up * (Time.deltaTime * _yVelocity);
            }
        }

        // Метод перемещения вправо или влево
        private void MoveTowards()
        {
            // Само перемещение
            _view._transform.position += Vector3.right * (Time.deltaTime * _speed * (_xAxisInput < 0 ? -1 : 1));
            // Перемена направления персонажа (LocalScale), в зависимости от направления его движения
            _view._transform.localScale = (_xAxisInput < 0 ? _leftScale : _rightScale);
        }


        // State Mashine - проверяем игрок находиться на земле или в воздухе (прыгнули или упали)
        public bool IsGrounded()
        {
            // Возвращает результат сравнения трансформ позиции вьюшки по оси Y, относительно установленного уровня земли и ускорения
            return _view._transform.position.y <= _groundLevel && _yVelocity <= 0;
        }
    }
}