using System.Collections.Generic;
using UnityEngine;

public class EllipseRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform pointer;

    public void DrawEllipse(List<Vector3> positions)
    {
        Vector3[] pos = new Vector3[positions.Count];

        for(int i = 0; i < positions.Count; i++)
        {
            pos[i] = positions[i];
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(pos);
        

    }
}
