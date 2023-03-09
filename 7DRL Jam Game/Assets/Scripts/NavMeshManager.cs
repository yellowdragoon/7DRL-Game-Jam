using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class NavMeshManager : MonoBehaviour
{

    public NavMeshSurface Surface2D;

    // Start is called before the first frame update
    void Start()
    {
        Surface2D.BuildNavMeshAsync();
    }

    public void updateNavMesh()
    {
        Surface2D.BuildNavMeshAsync();
    }

}
