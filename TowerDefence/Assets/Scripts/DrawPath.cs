using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    [SerializeField] bool showLine = true;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PathFinding.path.Count > 0 && showLine)
        {
            lineRenderer.SetVertexCount(PathFinding.path.Count);
            for(int i = 0; i < PathFinding.path.Count; i++)
            {
                lineRenderer.SetPosition(i, PathFinding.path[i].worldPos);
            }
        }

    }
}
