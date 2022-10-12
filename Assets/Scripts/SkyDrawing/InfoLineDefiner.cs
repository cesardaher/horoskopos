using UnityEngine;

public class InfoLineDefiner : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform symbolTransform;
    [SerializeField] Transform planetTransform;

    Vector3 planetPos;
    Vector3 symbolPos;
    Vector3[] positions = new Vector3[2];

    private void Start()
    {
        positions[0] = planetPos = planetTransform.position;
        positions[1] = symbolPos = symbolTransform.position;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }

    private void Update()
    {
        if (planetPos != transform.position || symbolPos != symbolTransform.position)
        {
            positions[0] = planetPos = planetTransform.position;
            positions[1] = symbolPos = symbolTransform.position;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }
    }

}
