using UnityEngine;
using TMPro;

public class Planet2D : MonoBehaviour, IRotatable
{
    [SerializeField] TextMeshProUGUI _symbol;
    [SerializeField] PlanetData _parentPlanet;

    public Vector3 initialPos;

    void Awake()
    {
        if(_symbol != null)
            initialPos = _symbol.transform.localPosition;
    }

    public PlanetData ParentPlanet
    {
        get
        { return _parentPlanet; }
        set
        { _parentPlanet = value; } 
    }

    public void RotateObject(double rotation)
    {
        var rotationVector = transform.localRotation.eulerAngles;
        rotationVector.z = (float)rotation;
        transform.localRotation = Quaternion.Euler(rotationVector);
    }

    public void PlaceSymbolConjunct(double midLongitude, int index, int size)
    {
        RotateObject(midLongitude);

        Vector3 pos = new Vector3(initialPos.x, initialPos.y, initialPos.z);

        if (size < 4)
        {

            pos.x += 30 * index;
            _symbol.transform.localPosition = pos;

            _symbol.transform.localScale = new Vector3(1, 1, 1);
            return;
        }

        pos.x += 20 * index;
        _symbol.transform.localPosition = pos;

        _symbol.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        return;
    }

    public void PlaceSymbol(double rotation)
    {
        _symbol.transform.localPosition = initialPos;
        RotateObject(rotation);    
    }

    public void TargetPlanetWithCamera()
    {
        EventManager.Instance.InteractWith2DPlanet(_parentPlanet.planetID);
    }
}
