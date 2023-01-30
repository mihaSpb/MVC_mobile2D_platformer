using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{
    public interface IQuestStory : IDisposable
    {
        // Завершена история или нет
        bool IsDone { get; }
    } 
}