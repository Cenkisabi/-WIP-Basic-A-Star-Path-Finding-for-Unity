
namespace AStarPathFinding
{
    public class GameMap : ITileBasedMap
    {

        /// <summary>
        /// The map width in tiles
        /// </summary>
        public static readonly int Width = 30;

        /// <summary>
        /// The map height in tiles
        /// </summary>
        public static readonly int Height = 30;

        /// <summary>
        /// Terrain Type Grass
        /// </summary>
        public static readonly int Grass = 0;

        public static readonly int Soldier = 10;
        public static readonly int Barracks = 11;
        public static readonly int PowerPlant = 12;
        
        private readonly int[,] _terrain = new int[Width,Height];
        private readonly  int[,] _units = new int[Width,Height];
        private readonly  bool[,] _visited = new bool[Width,Height];

        /// <summary>
        /// Test Map
        /// </summary>
        public GameMap()
        {
            FillArea(0,0,30,30,Grass);

            _units[4, 6] = Soldier;

            _units[4, 3] = Barracks;
            _units[4, 4] = Barracks;
            _units[5, 3] = Barracks;
            _units[5, 4] = Barracks;
        }

        
        private void FillArea(int x, int y, int width, int height, int type)
        {
            for (var i = x; i < x + width; i++)
            {
                for (var j = y; j < y + height; j++)
                {
                    _terrain[i, j] = type;
                }
            }
        }

        public void ClearVisited()
        {
            for (var i = 0; i < GetWidthInTiles(); i++)
            {
                for (var j = 0; j < GetHeightInTiles(); j++)
                {
                    _visited[i, j] = false;
                }
            }
        }

        public bool Visited(int x, int y)
        {
            return _visited[x, y];
        }

        public int GetTerrain(int x, int y)
        {
            return _terrain[x, y];
        }

        public int GetUnit(int x, int y)
        {
            return _units[x, y];
        }

        public void SetUnit(int x, int y, int unit)
        {
            _units[x, y] = unit;
        }
        
        public int GetWidthInTiles()
        {
            return Width;
        }

        public int GetHeightInTiles()
        {
            return Height;
        }

        public bool Blocked(int x, int y)
        {
            if (GetUnit(x, y) != 0) return true;

            if (AStarPathFinder.Unit == Soldier)
            {
                return _terrain[x, y] != Grass;
            }

            return true;
        }

        public float GetCost(int sx, int sy, int tx, int ty)
        {
            return 1;
        }

        public void PathFinderVisited(int x, int y)
        {
            _visited[x, y] = true;
        }
    }
}
