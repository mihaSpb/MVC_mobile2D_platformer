using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class BulletView : LevelObjectView
    {
        [SerializeField] private TrailRenderer _trail;

        public void SetVisible(bool visible)
        {
            if(_trail)
            {
                _trail.enabled = visible;
                _trail.Clear();
            }

            _spriteRenderer.enabled = visible;
        }
    }
}