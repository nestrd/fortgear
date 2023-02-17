using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathCreation;

public class PathManager : MonoBehaviour
{
    public List<PathCreator> paths = new List<PathCreator>();

    public PathCreator getPath(int index)
    {
        return paths[index];
    }
}

