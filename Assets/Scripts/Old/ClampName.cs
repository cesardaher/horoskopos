using UnityEngine;

public class ClampName : MonoBehaviour
{
    public Renderer objRenderer;
    public GameObject nameLabel;
    public bool mouseOn;

    void Start()
    {
        mouseOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);

        // turn label on when planet is visible
        if (namePos.z >= 0 && // on screen
            objRenderer.isVisible && // is visible
            !nameLabel.activeSelf // is off
            && mouseOn) // mouse is on
        {
            nameLabel.SetActive(true);
            return;
        }

        // turn label off when planet is invisible
        if (namePos.z < 0 || // off screen
            !mouseOn || // mouse off
            !objRenderer.isVisible && nameLabel.activeSelf) //object is not visible anymore
        {
            nameLabel.SetActive(false);
            return;
        }

        namePos.z = 0;
        nameLabel.transform.position = namePos;

    }
}
