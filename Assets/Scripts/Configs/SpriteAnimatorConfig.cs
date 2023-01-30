using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{

    [CreateAssetMenu(fileName ="SpriteAnimatorConfig", menuName = "Configs/Animation", order = 1)]

    public class SpriteAnimatorConfig : ScriptableObject
    {
        [Serializable]
        public sealed class SpriteSequence
        {
            // Это треки типов анимации: Idle, Run или Jump, тот тип анимации, который проигрывается в настоящее время
            public AnimState Track; 
            // Это лист, в котором храняться наши спрайты, сразу проинициализированный
            public List<Sprite> Sprites = new List<Sprite>();
        }

        // Лист для секвенций видов движений (набор спрайтов для Idle это одна секвенция, для прыжка - другая)
        public List<SpriteSequence> Sequences = new List<SpriteSequence>();

    }
}