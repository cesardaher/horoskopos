using UnityEngine;

public class EditButton : MonoBehaviour
{
    public GameObject editBox;
    public void ActivateEditMode()
    {
        editBox.SetActive(true);
    }
}
