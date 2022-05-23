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
        if(PathFinding.instance.path.Count > 0 && showLine)
        {
            List<Node> path = PathFinding.instance.path;
            lineRenderer.SetVertexCount(path.Count);
            for(int i = 0; i < path.Count; i++)
            {
                lineRenderer.enabled = showLine;
                lineRenderer.SetPosition(i, path[i].worldPos- Vector3.up*2);
            }
        }

    }
}
