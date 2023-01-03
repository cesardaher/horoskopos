using UnityEngine;

public class VSyncDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 100;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        
        if(QualitySettings.vSyncCount != 0)
        {
            QualitySettings.vSyncCount = 0;
        }

    }

}
