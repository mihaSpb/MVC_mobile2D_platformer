using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class CannonView : MonoBehaviour
    {
        // Точка, вокруг которой поворачиваем ствол пушки
        public Transform _muzzleTransform;
        // Точка эмиттера, откуда вылетают пули
        public Transform _emitterTransform;
        // Лист с пулями
        public List<LevelObjectView> _bullets;
    }
}