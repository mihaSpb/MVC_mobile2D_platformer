using UnityEngine;


namespace Platformer2D
{
    public class BulletController
    {
        // Ускорение полета пули
        private Vector3 _velocity;

        // Вьюшка
        private LevelObjectView _view;

        // Конструктор, вызывается при старте
        public BulletController (LevelObjectView view)
        {
            _view = view;
            Active(false);
        }


        public void Active(bool val)
        {
            _view.gameObject.SetActive(val);
        }

        // Устанавливаем направление полета пули, в целом делаем то же, что и
        // в CannonAimController в методе Update
        private void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
            float angle = Vector3.Angle(Vector3.left, _velocity);
            Vector3 axis = Vector3.Cross(Vector3.left, _velocity);
            _view.transform.rotation = Quaternion.AngleAxis(angle, axis);
        }

        // Выброс пули. Принимает он стартовую позицию и направление
        public void Trow(Vector3 position, Vector3 velocity)
        {
            Active(true);
            SetVelocity(velocity);

            // Обращаемся к трансформу вьюшки для получения позиции
            _view.transform.position = position;

            // Обнуляем velocity, перед тем как назначать ее в AddForce
            _view._rigidbody.velocity = Vector3.zero;
            _view._rigidbody.angularVelocity = 0.0f;

            _view._rigidbody.AddForce(velocity, ForceMode2D.Impulse);
        }

    }
}