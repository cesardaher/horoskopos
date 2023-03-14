using AstroResources;
using SwissEphNet;
using System.Collections.Generic;
using UnityEngine;

public class CuspExtender : EllipseRenderer, IAzalt
{

    /* This class defines an object that defines points in 3D to draw house cusps using a line renderer.
     * Normally, this is used within a prefab instantiated by HouseDrawer.
     */

    // id/name of house
    public int houseId;

    // material for line renderer
    [SerializeField] Material houseMat;
    
    // cartesian points given to line renderer
    List<Vector3> cuspPoints = new List<Vector3>();

    // number of vertices used
    [Range(12, 72)]
    [SerializeField] int vertexCount;

    // variables for positional calculations
    double[] x2 = new double[6];
    double[] xaz = new double[6];

    // number of points to be used in calculations
    // half the number is used during animations to reduce strain
    const int staticVertexCount = 72;
    const int animVertexCount = 36;

    void Start()
    {
        // assign events to EventManager
        EventManager.Instance.OnAnimationStart += AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd += StaticVertexCount;

        // setup high vertex count from start
        StaticVertexCount();

        // assign appropriate material
        lineRenderer.material = houseMat;
    }

    void OnDestroy()
    {
        // remove events to EventManager
        EventManager.Instance.OnAnimationStart -= AnimationVertexCount;
        EventManager.Instance.OnAnimationEnd -= StaticVertexCount;
    }

    // Extends House cusp lines (default)
    // projects the cusp onto the ecliptic sphere
    // uses longitude as azimuth and altitude from 0 to 360
    public void ExtendHouseCusp(double longitude)
    {
        // clears existing points to avoid overflow
        cuspPoints.Clear();

        // define the distance between each point of the circle
        // given the amount of points
        float arcStep = 360 / vertexCount;

        // iterates through all the vertices
        // uses <= instead of <, so that the circle is completed
        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            // longitude is gotten from cusps
            // latitude goes from 0 to 360
            x2[0] = longitude; 
            x2[1] = i * arcStep;

            // overflowing values go back to the first point
            if (x2[0] > 360) x2[0] = 0; 

            // calculates horizontal coordinates for each point
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, x2, xaz);

            // xaz[0] = azimuth
            // xaz[1] = true altitude
            // calculate cartesian position and add to vertices
            cuspPoints.Add(AstroFunctions.HorizontalToCartesian(xaz[0], xaz[1]));
        }

        // draws ellipse (from EllipseRenderer class)
        DrawEllipse(cuspPoints);
    }

    // Extends House cusp lines (Campanus)
    // projects the cusp on a sphere with poles on the North/South cardinal directions
    // the cusps divide this sphere equally (30 degree distance), based on the horizon
    public void ExtendCuspCampanus()
    {
        cuspPoints.Clear();
        float arcStep = 360 / vertexCount;

        
        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            double azimuth = i * arcStep;

            // rotate
            RotateAzimuth(azimuth);
            RotateX(30 * houseId);

            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX(-30 * houseId);
            RotateAzimuth(-azimuth);

        }

        DrawEllipse(cuspPoints);
    }

    public void ExtendCuspRegiomontanus()
    {
        cuspPoints.Clear();
        float arcStep = 360 / vertexCount;

        double altitude = NorthSouthAzimuth();

        for (int i = 0; i <= vertexCount; i++) // IMPORTANT: <= instead of <
        {
            double azimuth = i * arcStep;

            // rotate
            RotateAzimuth(azimuth);
            RotateX((float)altitude);

            cuspPoints.Add(pointer.position);

            // reverse rotation to prepare for next iteration
            RotateX((float)-altitude);
            RotateAzimuth(-azimuth);

        }

        DrawEllipse(cuspPoints);

        double NorthSouthAzimuth()
        {
            double[] cuspPos = new double[6];
            cuspPos[0] = HouseData.instance.houseDataList[houseId].Longitude;
            double[] cuspPosHor = new double[6];

            // HORIZONTAL COORDINATE FROM ECLIPTIC CUSP
            SwissEphemerisManager.swe.swe_azalt(GeoData.ActiveData.Tjd_ut, SwissEph.SE_ECL2HOR, GeoData.ActiveData.Geopos, 0, 0, cuspPos, cuspPosHor);

            // CUSP TO CARTESIAN 
            Vector3 cartesianCusp = AstroFunctions.HorizontalToCartesian(cuspPosHor[0], cuspPosHor[1]);

            double[] horizontalSphCoordinates = AstroFunctions.CartesianToHorizonSpherical(cartesianCusp.x, cartesianCusp.y, cartesianCusp.z);

            // take azimuth
            return horizontalSphCoordinates[1] * Mathf.Rad2Deg;
        }
    }


    void AnimationVertexCount()
    {
        vertexCount = animVertexCount;
    }

    void StaticVertexCount()
    {
        vertexCount = staticVertexCount;
    }

    // rotate on the Y axis
    // this represents a rotation on
    public void RotateAzimuth(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;

        rotationVector.y += (float)rotation + 180;

        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    //rotates on the Z axis
    public void RotateAltitude(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z += (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    // rotate on the WORLD'S X axis
    public void RotateX(float rotation)
    {
        //add new rotation
        transform.Rotate(Vector3.right, rotation, Space.World);

    }
}

