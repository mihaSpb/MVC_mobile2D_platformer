using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer2D
{
    public class GeneratorLevelView : MonoBehaviour
    {
        // Ссылка на тайловую палетку
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tile _groundTile; // Ссылка на конкретный тайл из палетки для отрисовки земли
        [SerializeField] private int _mapHeight; // Ширина будущей карты
        [SerializeField] private int _mapWight;  // Высота будущей карты
        [SerializeField] private bool _borders; // Будут ли бордюры (стенки) у карты
        [SerializeField] [Range(0,100)] private int _fillPercent; // Процент заполнения карты
        [SerializeField] [Range(0,100)] private int _factorSmooth; // Процент сглаживания - устанавливают правила перехода из одной ячейки в другую

        // Для доступа к private полям мы инкапсулируем их, как свойства
        public Tilemap Tilemap { get => _tilemap; set => _tilemap = value; }
        public Tile GroundTile { get => _groundTile; set => _groundTile = value; }
        public int MapHeight { get => _mapHeight; set => _mapHeight = value; }
        public int MapWight { get => _mapWight; set => _mapWight = value; }
        public bool Borders { get => _borders; set => _borders = value; }
        public int FillPercent { get => _fillPercent; set => _fillPercent = value; }
        public int FactorSmooth { get => _factorSmooth; set => _factorSmooth = value; }
    }
}