using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D
{

    public interface IQuest : IDisposable
    {
        // Событие для завершения квеста
        event EventHandler<IQuest> Completed;

        bool IsCompleted { get; } // Переменная состояния квеста - выполнен/не выполнен
        void ResetIQuest(); // Метод для перезапуска квеста
    }
}