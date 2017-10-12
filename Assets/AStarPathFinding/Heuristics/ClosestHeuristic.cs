
using UnityEngine;

namespace AStarPathFinding.Heuristics
{
	public class ClosestHeuristic : IAStarHeuristic {
		public float GetCost(ITileBasedMap map, int x, int y, int tx, int ty)
		{
			return Mathf.Abs(tx - x) + Mathf.Abs(ty - y);
		}
	}
}
