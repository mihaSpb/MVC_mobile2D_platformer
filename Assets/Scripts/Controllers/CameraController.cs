using UnityEngine;


namespace Platformer2D
{
    // По оси Х _xAxisInput, а по Y - _yAxisVelocity, т.к. при горизонтальном перемещении
    // мы дежим кнопки нажатыми, а для прыжка используется только одно нажатие.
    // Если вертикальное перемещение тоже завязано на удержании кнопок, то нужно
    // использовать _yAxisInput
    public class CameraController
    {
        private LevelObjectView _playerView; // Вьюшка игрока
        private Transform _playerTransform; // Трансформ игрока
        private Transform _cameraTransform; // Трансформ камеры

        private float _camSpeed = 2.0f; // Скорость перемещения камеры

        private float X; // Переменные для работы с погрешностями
        private float Y; // перемещения камеры

        private float offsetX; // Сами погрешности
        private float offsetY;

        private float _xAxisInput; // Сторона, в которую персонаж идет
        private float _yAxisVelocity; // перемещение игрока по вертикали

        private float _treshHold; // Погрешность


        // Передаем самого игрока и трансформ камеры
        public CameraController (LevelObjectView player, Transform camera)
        {
            _playerView = player;
            _cameraTransform = camera;
            _playerTransform = _playerView._transform; // Получаем ссылку на трансформ игрока
            _treshHold = 0.2f; // инициализация погрешности
        }
        
        public void Update()
        {
            _xAxisInput = Input.GetAxis("Horizontal"); // Получаем направление игрока
            _yAxisVelocity = _playerView._rigidbody.velocity.y; // Получаем движение вверх игрока, через риджибоди вьюшки по оси Y

            X = _playerTransform.position.x; // Позиция игрока по Х
            Y = _playerTransform.position.y; // Позиция игрока по Y

            // Камера движется за игроком, плавно и слегка опережая его (на величину offset по осям)
            if(_xAxisInput > _treshHold)
            {
                offsetX = 0.1f;
            }
            else if(_xAxisInput < -_treshHold)
            {
                offsetX = -0.1f;
            }
            else
            {
                offsetX = 0;
            }

            if (_yAxisVelocity > _treshHold)
            {
                offsetY = 0.1f;
            }
            else if (_yAxisVelocity < -_treshHold)
            {
                offsetY = -0.1f;
            }
            else
            {
                offsetY = 0;
            }

            // Само движении камеры: Lerp (откуда, куда и за какое время) - линейная интерполяция от позиции камеры
            // до нового вектора с координатами игрока + офсет по осям, умноженное на скорость камеры
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                new Vector3(X + offsetX, Y + offsetY, _cameraTransform.position.z), Time.deltaTime * _camSpeed);
        }
    }
}