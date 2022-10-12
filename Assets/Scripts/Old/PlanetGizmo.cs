using UnityEngine;

public class PlanetGizmo : MonoBehaviour
{
    float radius = 50;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, radius);

        Vector3 line = transform.position + (transform.forward * radius);
        var rotatedLine = Quaternion.AngleAxis(transform.rotation.x, transform.up) * line;
        Gizmos.DrawLine(Vector3.zero, rotatedLine);
    }
}
