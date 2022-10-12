using UnityEngine;

public class UiClamp : MonoBehaviour
{
    public GameObject nameLabel;

    // Update is called once per frame
    void Update()
    { 
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);

        /*
        if (namePos.z <= 0)
            nameLabel.SetActive(false);
        else nameLabel.SetActive(true);*/


        namePos.z = 0;
        nameLabel.transform.position = namePos;
    }
}
