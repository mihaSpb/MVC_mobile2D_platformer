using UnityEngine.Tilemaps;
using UnityEngine;


namespace Platformer2D
{
    public class MarshingSquaresController
    {
        private SquareGrid _squareGrid; // Сетка квадратов
        private Tilemap _tilemap; 
        private Tile _groundTile; // Нужный нам тайл


        // Метод генерации сетки, в который передаем двумерный массив нашей карты и размер квадрата
        public void GenerateGrid(int[,] map, float squareSize)
        {
            // Инициализируем сетку
            _squareGrid = new SquareGrid(map, squareSize);
        }


        // Отрисовка тайлов
        public void DrawTilesOnMap(Tilemap tileMapGround, Tile groundTile)
        {
            // Проверяем, заполнен ли массив и сетка присутствует
            if(_squareGrid == null)
            {
                return;
            }

            _tilemap = tileMapGround;
            _groundTile = groundTile;

            // Обходим массив - запускаем отрисовку от 0 до размеров нашей сетки
            for(int x = 0; x < _squareGrid.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < _squareGrid.Squares.GetLength(1); y++)
                {
                    DrawTile(_squareGrid.Squares[x, y].TopLeft.Active, _squareGrid.Squares[x,y].TopLeft.position);
                    DrawTile(_squareGrid.Squares[x, y].TopRight.Active, _squareGrid.Squares[x, y].TopRight.position);
                    DrawTile(_squareGrid.Squares[x, y].BottomLeft.Active, _squareGrid.Squares[x, y].BottomLeft.position);
                    DrawTile(_squareGrid.Squares[x, y].BottomRight.Active, _squareGrid.Squares[x, y].BottomRight.position);
                }
            }
        }

        // Отдельный метод для проверки активна нода или нет и определения ее позиции
        public void DrawTile(bool active, Vector3 position)
        {
            if(active)
            {
                Vector3Int pos = new Vector3Int((int)position.x, (int)position.y, 0);
                _tilemap.SetTile(pos, _groundTile); // Устанавливаем тайл в нужную позицию
            }
        }

    }


    // Базовый класс
    public class Node
    {
        public Vector3 position; // Позиция конкретной ноды

        // В конструкторе назначаем позицию
        public Node(Vector3 pos)
        {
            position = pos;
        }
    }


    // Наследуется от класса Node
    public class ControlNode : Node
    {
        public bool Active; // Нода может быть активной или нет

        // Конструктор, использующий конструктор из базаовой ноды
        public ControlNode(Vector3 pos, bool active) : base(pos)
        {
            Active = active;
        }
    }


    // Метод, делающий из нод квадраты
    public class Square
    {
        // Четыре ноды, составляющие квадрат
        public ControlNode TopLeft, TopRight, BottomLeft, BottomRight;

        // Конструктор, принимающий четыре ноды - инициализирует их
        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }
    }


    // Сетка квадратов
    public class SquareGrid
    {
        public Square[,] Squares; // Двумерный массив из квадратов

        // Конструктор для квадратов - передаем в него массив квадратов и размер квадрата
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0); // Количество нод по Х
            int nodeCountY = map.GetLength(1); // Количество нод по Y


            float mapWidth = nodeCountX * squareSize; // Реальный размер квадрата, ширина
            float mapHeight = nodeCountY * squareSize; // Его высота

            float size = squareSize/2; // 

            float width = -mapWidth/2; // Делим ширину и высоту на 2, чтобы не делить впоследствии функции
            float height = -mapHeight/2;

            // Создаем контрольные ноды
            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            // Обходим массив
            for(int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    // Заполняем квадраты по позиции
                    Vector3 position = new Vector3((width + x) * squareSize + size, (height + y) * squareSize + size, 0);
                    // Инициализируем контрольные ноды
                    // Если в массиве map[x, y] единица, то нода активна, если нет - не активна
                    controlNodes[x, y] = new ControlNode(position, map[x, y] == 1);
                }
            }

            // Инициализируем квадрат
            Squares = new Square[nodeCountX - 1, nodeCountY - 1];

            // Обходим массив
            for(int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    // Заполняем квадрат - передаем четыре контрольные ноды
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
}