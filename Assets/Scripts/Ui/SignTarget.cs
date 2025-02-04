using UnityEngine;
using UnityEngine.EventSystems;

public class SignTarget : MonoBehaviour, IClickable
{
    /* This class manages interaction with 3D sky signs. It's responsible for opening information boxes when called. */

    public SignInfoBox infoBox;
    [SerializeField] MidSign sign;

    // Interaction from IClickable interface
    // called when planet is clicked
    public void Interact(Vector3 pos)
    {
        ToggleInfoBox(true, pos);
    }

    // Opens/closes information box
    // value: open/close
    // pos: cursor position, to postiion information box
    void ToggleInfoBox(bool value, Vector3 pos)
    {
        if (value)
        {
            //set up planet info
            EventManager.Instance.OpenSignBox(sign.signID, pos);
            return;
        }

        if (!value && infoBox.gameObject.activeSelf)
        {
            // turn off box if on
            infoBox.gameObject.SetActive(false);
            return;
        }
    }
}
