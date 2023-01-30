using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class CoinsController : IDisposable
    {
        // На данном этапе монеты просто расставлены по сцене
        // Если надо, чтобы монетки "выбивались" из ящика, то для их создания можно использовать пулинг, как с пулями
        // Если надо, чтобы единичные монеты где-то появлялись, то их можно инстатиэйтить
        private float _animationSpeed = 10.0f;
        private SpriteAnimatorController _spriteAnimator; 
        private LevelObjectView _playerView; // Вьюшка игрока
        private List<LevelObjectView> _coinViews; // Лист вьюшек монеток

        // Конструктор
        public CoinsController(LevelObjectView playerView, List<LevelObjectView> coinViews, SpriteAnimatorController spriteAnimator )
        {
            _playerView = playerView;
            _coinViews = coinViews;
            _spriteAnimator = spriteAnimator;

            // Подписываемся на событие (метод обработчик контакта)
            _playerView.OnLevelObjectContact += OnLevelObjectContact;

            // Запуск анимации всех монеток в листе
            foreach(LevelObjectView coin in coinViews)
            {
                _spriteAnimator.StartAnimaton(coin._spriteRenderer, AnimState.Run, true, _animationSpeed);
            }
        }

        // Обработчик событий, принимает вьюшку
        private void OnLevelObjectContact(LevelObjectView contactView)
        {
            // Проверяем, что пришло в контакт и что с ним делать
            if(_coinViews.Contains(contactView))
            {
                // Если в контакте игрок, то останавливаем анимацию этой моетки
                _spriteAnimator?.StopAnimation(contactView._spriteRenderer);

                // И уничтожаем эту монетку (объект не возобнавляемый, поэтому удаляем таким образом)
                // Мы не наследуемся от монобеха, поэтому обращаемся к геймобджекту (GameObject.Destroy)
                // А это указывает на то, что удаляем данный геймобджект - contactView.gameObject
                GameObject.Destroy(contactView.gameObject);
                _coinViews.Remove(contactView); // Очистка списка монеток из объекта Main
            }
        }

        // Отписка от метода
        public void Dispose()
        {
            _playerView.OnLevelObjectContact -= OnLevelObjectContact;
        }

    }
}