using AStarPathFinding;
using UnityEngine;
using System.Collections.Generic;

public class PathTest : MonoBehaviour {

    private GameMap _map = new GameMap();
    private IPathFinder _finder;

    private Path _path;
    
    private void Start()
    {
        _finder = new AStarPathFinder(_map, 500, false);

        _path = _finder.FindPath(3, 3, 6, 5);

        for (var i = 0; i < _path.GetLength(); i++)
        {
            //print("X: " + _path.GetStep(i).GetX() + " Y: " + _path.GetStep(i).GetY());
        }
    }

}
