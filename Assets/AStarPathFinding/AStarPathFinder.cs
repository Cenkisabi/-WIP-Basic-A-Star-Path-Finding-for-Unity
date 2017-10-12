using System;
using System.Collections.Generic;
using AStarPathFinding.Heuristics;
using UnityEngine;

namespace AStarPathFinding
{
	public class AStarPathFinder : IPathFinder {
		
		/* The set of nodes that have been searched through */
		private List<Node> _closed = new List<Node>();
		/* The set of nodes that we do not yet consider fully searched */
		private SortedList<Node> _open = new SortedList<Node>();
		
		/* The map being searched */
		private ITileBasedMap _map;
		/* The maximum depth of search we're willing to accept before giving up */
		private int _maxSearchDistance;
		
		/* The complete set of nodes across the map */
		private Node[,] _nodes;
		/* True if we allow diaganol movement */
		private bool _allowDiagMovement;
		/* The heuristic we're applying to determine which nodes to search first */
		private IAStarHeuristic _heuristic;

		public static int Unit;
		
		/// <summary>
		/// Create a path finder with the default heuristic - closest to target.
		/// </summary>
		/// <param name="map">The map to be searched</param>
		/// <param name="maxSearchDistance">The maximum depth we'll search before giving up</param>
		/// <param name="allowDiagMovement">True if the search should try diaganol movement</param>
		public AStarPathFinder(ITileBasedMap map, int maxSearchDistance, bool allowDiagMovement) : this(map, maxSearchDistance, allowDiagMovement, new ClosestHeuristic())
		{
			
		}
		
		/// <summary>
		/// Create a path finder
		/// </summary>
		/// <param name="map">The map to be searched</param>
		/// <param name="maxSearchDistance">The maximum depth we'll search before giving up</param>
		/// <param name="allowDiagMovement">True if the search should try diaganol movement</param>
		/// <param name="heuristic">The heuristic used to determine the search order of the map</param>
		public AStarPathFinder(ITileBasedMap map, int maxSearchDistance, bool allowDiagMovement, IAStarHeuristic heuristic)
		{
			Unit = GameMap.Soldier;
			
			_map = map;
			_maxSearchDistance = maxSearchDistance;
			_allowDiagMovement = allowDiagMovement;
			_heuristic = heuristic;
			
			_nodes = new Node[_map.GetWidthInTiles(), _map.GetHeightInTiles()];

			for (var i = 0; i < _map.GetWidthInTiles(); i++)
			{
				for (var j = 0; j < _map.GetHeightInTiles(); j++)
				{
					_nodes[i,j] = new Node(i,j);
				}
			}
			
		}
		
		public Path FindPath(int sx, int sy, int tx, int ty)
		{
			if (_map.Blocked(tx, ty))
			{
				return null;
			}

			_nodes[sx, sy].Cost = 0;
			_nodes[sx, sy].Depth = 0;
			_closed.Clear();
			_open.Clear();
			_open.Add(_nodes[sx, sy]);

			_nodes[tx, ty].Parent = null;

			var maxDepth = 0;

			while (maxDepth < _maxSearchDistance && _open.Count() != 0)
			{
				var current = _open.First();

				if (current == _nodes[tx, ty])
				{
					break;
				}
				
				_open.Remove(current);
				_closed.Add(current);

				for (var x = -1; x < 2; x++)
				{
					for (var y = -1; y < 2; y++)
					{
						// Not a neighbour, it is current tile
						if (x == 0 && y == 0) continue;

						if (!_allowDiagMovement)
						{
							if (x != 0 && y != 0) continue;
						}
						
						// determine the location of the neighbour and evaluate it
						var xp = x + current.X;
						var yp = y + current.Y;

						if (IsValidLocation(sx, sy, xp, yp))
						{
							var nextStepCost = current.Cost + _map.GetCost(sx, sy, xp, yp);
							var neighbour = _nodes[xp, yp];
							_map.PathFinderVisited(xp, yp);

							if (nextStepCost < neighbour.Cost)
							{
								if (_open.Contains(neighbour))
								{
									_open.Remove(neighbour);
								}
								if (_closed.Contains(neighbour))
								{
									_closed.Remove(neighbour);
								}
							}

							if (!_open.Contains(neighbour) && !_closed.Contains(neighbour))
							{
								neighbour.Cost = nextStepCost;
								neighbour.Heuristic = _heuristic.GetCost(_map, xp, yp, tx, ty);
								maxDepth = Math.Max(maxDepth, neighbour.SetParent(current));
								_open.Add(neighbour);
							}
						}
					}
				}
			}

			if (_nodes[tx, ty].Parent == null)
			{
				return null;
			}
			
			var path = new Path();
			var target = _nodes[tx, ty];

			while (target != _nodes[sx, sy])
			{
				path.PrependStep(target.X, target.Y);
				target = target.Parent;
			}
			path.PrependStep(sx, sy);

			return path;
		}

		protected bool IsValidLocation(int sx, int sy, int x, int y)
		{
			var invalid = x < 0 || y < 0 || x > _map.GetWidthInTiles() || y > _map.GetHeightInTiles();

			if (!invalid && (sx != x || sy != y))
			{
				invalid = _map.Blocked(x, y);
			}
			//Debug.Log("IsValid: " + x + " " + y + " " + !invalid);
			return !invalid;
		}
		
		/// <summary>
		/// A simple sorted list
		/// </summary>
		/// <typeparam name="T">Type of Sorted List</typeparam>
		private class SortedList<T>
		{
			/// <summary>
			/// The list of elements
			/// </summary>
			private readonly List<T> _list = new List<T>();

			/// <summary>
			/// Retrieve the first element from the list
			/// </summary>
			/// <returns>The first element from the list</returns>
			public T First()
			{
				return _list[0];
			}

			/// <summary>
			/// Empty the list
			/// </summary>
			public void Clear()
			{
				_list.Clear();
			}

			/// <summary>
			/// Add an element to the list - causes sorting
			/// </summary>
			/// <param name="obj">The element to add</param>
			public void Add(T obj)
			{
				_list.Add(obj);
				_list.Sort();
			}

			/// <summary>
			/// Remove an element from the list
			/// </summary>
			/// <param name="obj">The element to remove</param>
			public void Remove(T obj)
			{
				_list.Remove(obj);
			}

			/// <summary>
			/// Get the number of elements in the list
			/// </summary>
			/// <returns>The number of element in the list</returns>
			public int Count()
			{
				return _list.Count;
			}

			/// <summary>
			/// Check if an element is in the list
			/// </summary>
			/// <param name="obj">The element to search for</param>
			/// <returns>True if the element is in the list</returns>
			public bool Contains(T obj)
			{
				return _list.Contains(obj);
			}
		}
		
		/// <summary>
		/// A single node in the search graph
		/// </summary>
		private class Node : IComparable
		{
			/** The x coordinate of the node */
			public int X { get; private set; }
			/** The y coordinate of the node */
			public int Y { get; private set; }
			/** The path cost for this node */
			public float Cost { get; set; }
			/** The parent of this node, how we reached it in the search */
			public Node Parent { get; set; }
			/** The heuristic cost of this node */
			public float Heuristic { get; set; }
			/** The search depth of this node */
			public int Depth { get; set; }
			
			
			/// <summary>
			/// Create a new node
			/// </summary>
			/// <param name="x">The x coordinate of the node</param>
			/// <param name="y">The y coordinate of the node</param>
			public Node(int x, int y)
			{
				X = x;
				Y = y;
			}

			/// <summary>
			/// Set the parent of this node
			/// </summary>
			/// <param name="parent">The parent node which lead us to this node</param>
			/// <returns>The depth we have no reached in searching</returns>
			public int SetParent(Node parent)
			{
				Depth = parent.Depth + 1;
				Parent = parent;

				return Depth;
			}

			public int CompareTo(object obj)
			{
				var node = obj as Node;

				if (node == null) return -1;
				
				var totalCost = Heuristic + Cost;
				var nodeTotalCost = node.Heuristic + node.Cost;

				if (totalCost < nodeTotalCost) return -1;
				else if (totalCost > nodeTotalCost) return 1;
				else return 0;
			}
		}
	}
}
