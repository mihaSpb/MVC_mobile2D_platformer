//using System;
//using UnityEngine;

//namespace TestParallax

//{
//    public class UnevirseHandler : MonoBehaviour
//    {

//        // https://ru.kihontekina.dev/posts/endless_space_part_one/
//        // Ссылки на объекты сцены
//        [SerializeField] private Camera mainCamera = null;
//        [SerializeField] private GameObject ship = null;
//        [SerializeField] private GameObject space = null;

//        // Радиус возможного обзора камеры
//        private float _spaceCircleRadius = 0;

//        // Исходные размеры объекта фона
//        private float _backgroundOriginalSizeX = 0;
//        private float _backgroundOriginalSizeY = 0;

//        // Направление движения
//        private Vector3 _moveVector;
//        // Скорость поворота в радианах
//        private float _rotationSpeed = 1f;

//        // Вспомогательные переменные
//        private bool _mousePressed = false;
//        private float _halfScreenWidth = 0;

//        void Start()
//        {
//            // Стартовое направление движения
//            _moveVector = new Vector3(0, 1.5f, 0);
//            // Используется для определения направления поворота
//            _halfScreenWidth = Screen.width / 2f;

//            // Исходные размеры фона
//            SpriteRenderer sr = space.GetComponent<SpriteRenderer>();
//            var originalSize = sr.size;
//            _backgroundOriginalSizeX = originalSize.x;
//            _backgroundOriginalSizeY = originalSize.y;

//            // Высота камеры равна ортографическому размеру
//            float orthographicSize = mainCamera.orthographicSize;
//            // Ширина камеры равна ортографическому размеру, помноженному на соотношение сторон
//            float screenAspect = (float)Screen.width / (float)Screen.height;
//            // Радиус окружности, описывающей камеру
//            _spaceCircleRadius = Mathf.Sqrt(orthographicSize * screenAspect * orthographicSize * screenAspect + orthographicSize * orthographicSize);

//            // Конечный размер фона должен позволять сдвинуться на один базовый размер фона в любом направлении + перекрыть радиус камеры также во всех направлениях
//            sr.size = new Vector2(_spaceCircleRadius * 2 + _backgroundOriginalSizeX * 2, _spaceCircleRadius * 2 + _backgroundOriginalSizeY * 2);
//        }

//        void Update()
//        {
//            // Изменение направления движения по клику кнопки мыши
//            if (Input.GetMouseButtonDown(0))
//            {
//                _mousePressed = true;
//            }

//            if (Input.GetMouseButtonUp(0))
//            {
//                _mousePressed = false;
//            }

//            if (_mousePressed)
//            {
//                // Направление поворота определяется в зависимости от стороны экрана, по которой произошёл клик
//                int rotation = Input.mousePosition.x >= _halfScreenWidth ? -1 : 1;

//                // Расчёт поворота вектора направления
//                float xComp = (float)(_moveVector.x * Math.Cos(_rotationSpeed * rotation * Time.deltaTime) - _moveVector.y * Math.Sin(_rotationSpeed * rotation * Time.deltaTime));
//                float yComp = (float)(_moveVector.x * Math.Sin(_rotationSpeed * rotation * Time.deltaTime) + _moveVector.y * Math.Cos(_rotationSpeed * rotation * Time.deltaTime));
//                _moveVector = new Vector3(xComp, yComp, 0);

//                // Поворот спрайта корабля и камеры вдоль вектора направления
//                float rotZ = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
//                ship.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
//                mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
//            }

//            // Сдвигаем фон в противоположном движению направлении
//            space.transform.Translate(-_moveVector.x * Time.deltaTime, -_moveVector.y * Time.deltaTime, 0);

//            // При достижении фоном сдвига равного исходному размеру фона в каком-либо направлении, возвращаем его в исходную точно по этому направлению
//            if (space.transform.position.x >= _backgroundOriginalSizeX)
//            {
//                space.transform.Translate(-_backgroundOriginalSizeX, 0, 0);
//            }
//            if (space.transform.position.x <= -_backgroundOriginalSizeX)
//            {
//                space.transform.Translate(_backgroundOriginalSizeX, 0, 0);
//            }
//            if (space.transform.position.y >= _backgroundOriginalSizeY)
//            {
//                space.transform.Translate(0, -_backgroundOriginalSizeY, 0);
//            }
//            if (space.transform.position.y <= -_backgroundOriginalSizeY)
//            {
//                space.transform.Translate(0, _backgroundOriginalSizeY, 0);
//            }
//        }

//        private void OnDrawGizmos()
//        {
//            // Окружность, описывающая камеру
//            UnityEditor.Handles.color = Color.yellow;
//            UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.back, _spaceCircleRadius);

//            // Направление движения
//            UnityEditor.Handles.color = Color.green;
//            UnityEditor.Handles.DrawLine(Vector3.zero, _moveVector);
//        }
//    }
//}