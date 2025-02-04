using UnityEngine;

public class VSyncDisabler : MonoBehaviour
{
    [SerializeField] bool _limitFrameRate;
    // Start is called before the first frame update
    void Start()
    {
        if(_limitFrameRate)
        Application.targetFrameRate = 60;
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
