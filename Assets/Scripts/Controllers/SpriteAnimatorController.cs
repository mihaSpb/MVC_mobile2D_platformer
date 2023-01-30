using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{

    public class SpriteAnimatorController : IDisposable
    {

        private sealed class Animation
        {
            // Здесь будут перечисления, которые мы будем загружать
            // из конфига: трек, спрайты, скорость анимации, счетчик кадров,
            // зациклен трек или нет.
            // И лист с самими анимациями
            public AnimState Track;
            public List<Sprite> Sprites;
            public bool Loop;
            public float Speed = 10.0f;
            // Счетчик кадров
            public float Counter = 0.0f;
            // Переменная для анимаций, которые проигрываются один раз, а потом
            // впадение в сон. Проверяем - спим или нет
            public bool Sleep;


            // Тут идет переключение кадров
            public void UpdateAnimation()
            {
                if (Sleep) return;
                Counter += Time.deltaTime * Speed;

                if(Loop)
                {
                    while(Counter > Sprites.Count)
                    {
                        Counter -= Sprites.Count;
                    }
                }
                else if (Counter > Sprites.Count)
                {
                    Counter = Sprites.Count;
                    Sleep = true;
                }
            }
        }


        // Создаем словарь и загружаем конфиг
        private SpriteAnimatorConfig _config;
        // Словарь будет содержать ключ/значение.
        // В качестве ключа будет SpriteRenderer, а в качестве значения объект Animation
        // Сразу инициализирует пустой словарь
        private Dictionary<SpriteRenderer, Animation> _activeAnimations = new Dictionary<SpriteRenderer, Animation>();


        // Конструктор, в который передаем только конфиг
        public SpriteAnimatorController(SpriteAnimatorConfig config)
        {
            _config = config;
        }


        // Методы для запуска и остановки анимации
        // На старте анимации мы проверяем: есть ли у нас такой ключ. Если его
        // нет, то мы его создаем. Если он есть, то мы его должны запустить
        public void StartAnimaton(SpriteRenderer spriteRenderer, AnimState track, bool loop, float speed)
        {
            // Проверяем - а есть ли этот ключ. Выводим этот ключ через out (передача аргумента по ссылке,
            // как выходной параметр, при этом переменную animation не обязательно инициализировать)
            if(_activeAnimations.TryGetValue(spriteRenderer, out var animation))
            {
                // Назначаем пришедшие параметры
                animation.Loop = loop;
                animation.Speed = speed;

                // Если трек анимации не равен тому, который передан, то ставим нужный трек
                if(animation.Track != track)
                {
                    // Назначаем трек
                    animation.Track = track;
                    // Загружаем последовательность спрайтов из конфига, найдя, совпадающую с переданной, секвенцию 
                    animation.Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites;
                    animation.Counter = 0.0f;
                }
            }
            else //Если нет анимации, которая нам нужна, то:
            {
                // Добавляем ее и сразу создаем объект Animation, а затем сразу его заполняем данными
                _activeAnimations.Add(spriteRenderer, new Animation()
                {
                    Track = track,
                    Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites,
                    Loop = loop,
                    Speed = speed
                });
            }
        }

        // Мы просто удаляем секвенцию спрайтов
        public void StopAnimation(SpriteRenderer sprite)
        {
            // Мы проверяем наличие ключа - если данный ключ есть, то мы его удаляем
            if(_activeAnimations.ContainsKey(sprite))
            {
                _activeAnimations.Remove(sprite);
            }
        }


        public void Update()
        {
            // Перебираем значения animation из _activeAnimation
            foreach(var animation in _activeAnimations)
            {
                animation.Value.UpdateAnimation();

                if(animation.Value.Counter < animation.Value.Sprites.Count)
                {
                    // Перебираем кадры анимации и явно приводим к инту, потому что Counter float
                    animation.Key.sprite = animation.Value.Sprites[(int)animation.Value.Counter];
                }
            }

        }

        // Очистка памяти
        public void Dispose()
        {
            _activeAnimations.Clear();
        }
    }
}