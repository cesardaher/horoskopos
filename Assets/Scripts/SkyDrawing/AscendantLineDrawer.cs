using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendantLineDrawer : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _pointer;

    private void Update()
    {
        Vector3[] positions = new Vector3[2];

        positions[0] = Vector3.zero;
        positions[1] = _pointer.position;

        _lineRenderer.SetPositions(positions);    
    }
}
