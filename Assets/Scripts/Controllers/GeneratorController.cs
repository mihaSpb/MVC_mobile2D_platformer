using UnityEngine.Tilemaps;
using UnityEngine;


namespace Platformer2D
{
    public class GeneratorController 
    {
        private Tilemap _tilemap;
        private Tile _groundTile;
        private int _mapHeight;
        private int _mapWidth;
        private bool _borders;
        private int _fillPercent;
        private int _factorSmooth;


        private int[,] _map; // Матрица ячеек (двумерный массив)

        private int countWall = 4; // Количество соседних клеток для алгоритма сглаживания

        private MarshingSquaresController _squaresController;


        // Конструктор, принимающий параметры из вьюшки
        public GeneratorController(GeneratorLevelView view)
        {
            _tilemap = view.Tilemap;
            _groundTile = view.GroundTile;
            _mapHeight = view.MapHeight;
            _mapWidth = view.MapWight;
            _borders = view.Borders;
            _fillPercent = view.FillPercent;
            _factorSmooth = view.FactorSmooth;

            _map = new int[_mapWidth, _mapHeight]; // Инициализируем массив ячеек
        }


        // Метод, занимающийся инициализацией - чтобы можно было вызывать этот метод для перегенерации карты
        public void Init()
        {
            // Генерация - заполнение массива
            FillMap();

            // Сглаживание массива производим несколько раз в цикле
            for(int i = 0; i < _factorSmooth; i++)
            {
                SmoothMap();
            }

            // Отрисовка спрайтов через клеточный автомат
            // DrawTiles();

            // Отрисовка спрайтов через MarshingSquares
            _squaresController = new MarshingSquaresController();
            _squaresController.GenerateGrid(_map, 1);
            _squaresController.DrawTilesOnMap(_tilemap, _groundTile);
        }


        private void FillMap() // Метод заполнение массива 
        {
            // Заполняем массив случайными числами
            // Сначала по координате Х (ширине), а затем по Y
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y ++)
                {
                    // Если х == 0, значит мы находимся в самом начале массива или х == ширине массива (крайний правый столбец)
                    // или y == 0 или y == высоте массива и если нам нужны границы (_borders == true),
                    // то мы заполняем границы карты единицами (не пустая ячейка)
                    if (x == 0 || x == _mapWidth-1 || y == 0 || y == _mapHeight-1)
                    {
                        if(_borders)
                        {
                            _map[x, y] = 1;
                        }
                    }

                    else
                    {
                        // Остальная часть карты рандомно заполняется: если выпавшее числи меньше процента заполнения, то выпадет 0
                        _map[x, y] = Random.Range(0, 100) < _fillPercent ? 1 : 0;
                    }
                }
            }
        }


        // Метод глаживания
        private void SmoothMap()
        {
            // Снова обходим весь массив и сраниваем его с правилом
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    // Вызываем метод подсчета соседей, который возвращает найденно количество
                    int neighbour = GetNeifhbour(x,y); // В метод передаем координаты текущей клетки 

                    // Само правило: если соседей больше, чем необходимое количество стенок,
                    // то текущая клетка будет 1, иначе 0
                    if (neighbour > countWall)
                    {
                        _map[x, y] = 1;
                    }
                    else
                    {
                        _map[x, y] = 0;
                    }
                }
            }
        }


        // Отдельный метод, коорый возвращает количество соседей
        private int GetNeifhbour(int x, int y) // Передаем координаты текущей клетки
        {
            int neighbourCounter = 0; // Количество соседей

            // Обход массива соседей (если сделать gridX < x + 1, то цикл закончится на координате Х и не зайдет на соседа справа,
            // поэтому нужно ставить меньше либо равно: gridX <= x + 1)
            for (int gridX = x - 1; gridX <= x + 1; gridX++) // Продим от левого соседа до правого соседа
            {
                for(int gridY = y - 1; gridY <= y + 1; gridY++ ) // Проходим от соседа сверху до соседа снизу
                {
                    // Условие по ширине и высоте (для проверки угловых клеток, у которых нетнекоторых соседей)
                    if(gridX >= 0 && gridX < _mapWidth && gridY >= 0 && gridY < _mapHeight)
                    {
                        // Исключение из проверки текущей клетки
                        if(gridX!= x || gridY != y)
                        {
                            neighbourCounter += _map[gridX, gridY]; // Вместо значения текущей клетки добавляем значение клетки с координатами gridX, gridY
                        }
                    }
                    else
                    {
                        neighbourCounter++;
                    }
                }
            }

            return neighbourCounter;
        }


        // Отрисовка тайлов
        private void DrawTiles()
        {
            if(_map == null) // Проверяем, а не пуста ли наша карта
            {
                return;
            }

            // Проходим весь массив
            for(int x = 0; x < _mapWidth; x++)
            {
                for(int y = 0; y < _mapHeight; y ++)
                {
                    // Определяем позицию тайла, для этого используем целочисленный вектор3 инт
                    // Х.тайла == половина ширины + х.текущей ячейки (при этом ширина имеет отрицательное значение)
                    Vector3Int tilePosition = new Vector3Int( -_mapWidth/2 + x, -_mapHeight/2 + y, 0);

                    // Отрисовываем тайл, если значение текущей ячейки равно 1
                    if (_map[x,y] == 1)
                    {
                        _tilemap.SetTile(tilePosition, _groundTile);
                    }
                }
            }
        }

    }
}