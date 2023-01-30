using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public class Main : MonoBehaviour
    {
        // Это основная точка входа.
        // SpriteAnimatorConfig - загружает наши картинки, там храняться данные и
        // на основе этого конфига мы создадим ScripttableObject (в котором содержатся настройки и ссылки на сами картинки)
        // Controller - это все будет связывать вместе, а сам контроллер будет иницциализироваться в Main


        // Ссылки на нужные нам объекты
        [SerializeField] private SpriteAnimatorConfig _playerConfig;
        [SerializeField] private SpriteAnimatorConfig _coinConfig;

        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private List<LevelObjectView> _coinViews;
        [SerializeField] private CannonView _cannonView;
        [SerializeField] private GeneratorLevelView _generatorLevelView;

        [SerializeField] private QuestView _questView;


        // Ссылки на объекты управляющие параллаксом
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private Transform _backGround;
        [SerializeField] private Transform _middleGround;

        // Инициализация контроллеров
        private SpriteAnimatorController _playerAnimator;
        private SpriteAnimatorController _coinAnimator;
        private ParalaxController _paralaxController;
        private CameraController _cameraController;
        private PlayerController _playerController;
        private CannonAimController _cannonAimController;
        private BulletsEmitterController _bulletsEmitterController;
        private CoinsController _coinsController;
        private GeneratorController _generatorController;
        private QuestConfiguratorController _questConfiguratorController;


        
        void Awake()
        {
            // Загружаем конфиг, создаем экземпляр PlayerAnimator и запускаем анимацию
            _playerConfig = Resources.Load<SpriteAnimatorConfig>("PlayerAnimCfg");
            _coinConfig = Resources.Load<SpriteAnimatorConfig>("CoinAnimCfg");

            _playerAnimator = new SpriteAnimatorController(_playerConfig);
            // 2 Урок. Теперь анимацией мы управляем из PlayerTransformController и эта строчка переехала в этот класс
            // _playerAnimator.StartAnimaton(_playerView._spriteRenderer, AnimState.Run, true, _animationSpeed);
            _coinAnimator = new SpriteAnimatorController(_coinConfig);

            _paralaxController = new ParalaxController(_mainCamera, _backGround, _middleGround);
            _cameraController = new CameraController(_playerView, Camera.main.transform);

            // Создание экземпляра класса контроллера движения
            _playerController = new PlayerController(_playerView, _playerAnimator);

            _cannonAimController = new CannonAimController(_cannonView._muzzleTransform, _playerView._transform);
            _bulletsEmitterController = new BulletsEmitterController(_cannonView._bullets, _cannonView._emitterTransform);

            _coinsController = new CoinsController(_playerView, _coinViews, _coinAnimator);

            _generatorController = new GeneratorController(_generatorLevelView);
            _generatorController.Init();

            _questConfiguratorController = new QuestConfiguratorController(_questView);
            _questConfiguratorController.Init();

        }



        void Update()
        {
            // Урок 2 - анимация теперь вызывается внутри контроллера игрока (_playerController)
            //_playerAnimator.Update();

            _paralaxController.ParalaxUpdate();
            _cameraController.Update();

            // Вызов метода
            _playerController.UpdateStateObject();

            _cannonAimController.Update();
            _bulletsEmitterController.Update();

            _coinAnimator.Update();
        }
    }
}