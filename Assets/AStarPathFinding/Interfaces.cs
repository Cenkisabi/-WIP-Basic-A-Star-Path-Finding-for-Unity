
namespace AStarPathFinding
{
	public interface ITileBasedMap
	{
		int GetWidthInTiles();
		int GetHeightInTiles();
		bool Blocked(int x, int y);
		float GetCost(int sx, int sy, int tx, int ty);
		void PathFinderVisited(int x, int y);
	}

	public interface IPathFinder
	{
		Path FindPath(int sx, int sy, int tx, int ty);
	}

	public interface IAStarHeuristic
	{
		float GetCost(ITileBasedMap map, int x, int y, int tx, int ty);
	}
}
