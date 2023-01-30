using UnityEngine;


namespace Platformer2D
{
    public class CannonAimController
    {
        // Трансформ ствола
        private Transform _muzzleTransform;
        // Трансформ цели
        private Transform _targetTransform;

        // Направление к цели
        private Vector3 _dir;
        // Угол, на который поворачиваемся
        private float _angle;
        // Ось, вокруг которой поворачиваемся
        private Vector3 _axis;


        // Конструктор
        public CannonAimController(Transform muzzleTransform, Transform _playerTransform)
        {
            _muzzleTransform = muzzleTransform;
            _targetTransform = _playerTransform;
        }

        // 
        public void Update()
        {
            // Находим направление: из точки финиша вычитаем точку старта
            _dir = _targetTransform.position - _muzzleTransform.position;

            // У нашей пушки, по умолчанию, ствол направлен вниз поэтому используем Vector3.down
            // В случае, если пушки устанавливаются в других местах, то необходимо делать некий векторСтарт,
            // в конструкторе получать направление ствола при инициализациии его уже присвавать к векторСтарт
            // и дальше от него двигаться
            _angle = Vector3.Angle(Vector3.down, _dir);

            // Тут получаем ось поворота
            _axis = Vector3.Cross(Vector3.down, _dir);
            // Тут рассчитываем угол поворота
            _muzzleTransform.rotation = Quaternion.AngleAxis(_angle, _axis);
        }
    }
}