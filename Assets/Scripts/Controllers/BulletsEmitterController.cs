using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class BulletsEmitterController
    {
        //Лист пуль, состоящий из контроллеров пуль. Сразу инициализируем его
        private List<BulletController> _bullets = new List<BulletController>();
        private Transform _transform;

        //Счетчик пуль
        private int _index;
        //Время ожидания до следующей пули
        private float _timeKillNextBull;

        private float _delay = 1.0f;
        private float _startSpeed = 3.0f;

        //Принимает лист пуль. В конструкторе создаем пули и инициализируем классы и добавляем
        //их в лист, которым в последствии будем пользоваться
        public BulletsEmitterController(List<LevelObjectView> bulletViews, Transform transform)
        {
            _transform = transform;

            //Перебираем лист LevelObjectView
            foreach (LevelObjectView BulletView in bulletViews)
            {
                //Добавляем экземпляры класса BulletController, в которые будем передавать
                //тот самый BulletView, который мы перебираем
                _bullets.Add(new BulletController(BulletView));
            }
        }

       
        public void Update()
        {
            //Проверка времени жизни пули
            if(_timeKillNextBull > 0)
            {
                //Обращаемся к текущей пуле
                _bullets[_index].Active(false);
                //Уменьшаем время таймера
                _timeKillNextBull -= Time.deltaTime;
            }
            else
            {
                //сбрасываем таймер
                _timeKillNextBull = _delay;
                //Вызываем из текущей пули метод Trow, в него передаем позицию. Т.к. пушка смотрит
                //вниз, то приходится передавать -_transform.up (инвертированный вектор вверх)
                _bullets[_index].Trow(_transform.position, -_transform.up * _startSpeed);
                _index++;

                if(_index >= _bullets.Count)
                {
                    _index = 0;
                }
            }
        }
    }
}