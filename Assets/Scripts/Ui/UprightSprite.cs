using UnityEngine;

public class UprightSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = new Vector3(0, 1, 0);
    }
}
