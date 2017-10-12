

using System;
using System.Collections.Generic;

namespace AStarPathFinding
{
	public class Path {
		
		private readonly List<Step> _steps = new List<Step>();

		public Path()
		{
			
		}

		public int GetLength()
		{
			return _steps.Count;
		}

		public Step GetStep(int index)
		{
			return _steps[index];
		}

		public int GetX(int index)
		{
			return _steps[index].GetX();
		}

		public int GetY(int index)
		{
			return _steps[index].GetY();
		}

		public void AppendStep(int x, int y)
		{
			_steps.Add(new Step(x, y));
		}

		public void PrependStep(int x, int y)
		{
			_steps.Insert(0, new Step(x, y));
		}
		public bool Contains(int x, int y)
		{
			return _steps.Contains(new Step(x, y));
		}
		
		public class Step
		{
			private readonly int _x;
			private readonly int _y;

			public Step(int x, int y)
			{
				_x = x;
				_y = y;
			}

			public int GetX()
			{
				return _x;
			}

			public int GetY()
			{
				return _y;
			}

			public override int GetHashCode()
			{
				return _x * _y;
			}

			public override bool Equals(object other)
			{
				var step = other as Step;
				if (step != null)
				{
					if (step._x == _x && step._y == _y)
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}
