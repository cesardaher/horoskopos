using UnityEditor;
using UnityEngine;
using AstroResources;

public class CardinalInitializer : MonoBehaviour
{

    public enum Cardinal {
    North,
    East,
    South,
    West
    }

    [SerializeField] Cardinal cardinal;


    private void OnValidate()
    {
        PositionCardinal();
    }

    // Initializes the postion of each cardinal on the screen
    void PositionCardinal()
    {
        float azimuth;

        // assigns an azimuth value based on the chosen cardinal
        switch (cardinal)
        {
            case Cardinal.North:
                azimuth = 180; 
                break;
            case Cardinal.East:
                azimuth = 270;
                break;
            case Cardinal.South:
                azimuth = 0;
                break;
            case Cardinal.West:
                azimuth = 90;
                break;
            default:
                azimuth = 0;
                break;
        }

        // calculates cartesian position based on horizontal coordinates
        var position = AstroFunctions.HorizontalToCartesian(azimuth, 0);

        // positions cardinal slighty below horizon for legibility
        position.y = position.y - 500;

        // final position
        transform.position = position;
        
    }
}
