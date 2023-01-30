using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class ParalaxController
    {
        private Transform _camera;

        private Transform _back;
        private Transform _middle;

        private Vector3 _backStartPosition;
        private Vector3 _middleStartPosition;
        private Vector3 _cameraStartPosition;

        private float _coefBack = 0.2f;
        private float _coefMiddle = 0.25f;


        public ParalaxController(Transform camera, Transform back, Transform middle)
        {
            _camera = camera;
            _back = back;
            _middle = middle;

            _backStartPosition = _back.transform.position;
            _middleStartPosition = _middle.transform.position;
            _cameraStartPosition = _camera.transform.position;
        }


        public void ParalaxUpdate()
        {
           _back.position = _backStartPosition + (_camera.position - _cameraStartPosition) * _coefBack;
           _middle.position = _middleStartPosition + (_camera.position - _cameraStartPosition) * _coefMiddle;
        }
    }
}