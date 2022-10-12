using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    LineRenderer lineRenderer;
    List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetupLine(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;

        List<Vector3> positions = new List<Vector3>(points.Length);

        for(int i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }

        this.points = positions;
    }

    public void SetupLine(Vector3[] positions)
    {
        lineRenderer.positionCount = points.Count;
        List<Vector3> poses = new List<Vector3>();

        foreach(Vector3 pos in poses)
        {
            poses.Add(pos);
        }

        this.points = poses;
    }

    public void SetupLine(List<Vector3> positions)
    {
        lineRenderer.positionCount = points.Count;
        this.points = positions;
    }

    private void Update()
    {
        lineRenderer.positionCount = points.Count;

        lineRenderer.SetPositions(points.ToArray());


        /*
        for (int i = 0; i < points.Count; i++)
        {

            //lineRenderer.SetPosition(i, points[i]);
        }
        */
    }


}
