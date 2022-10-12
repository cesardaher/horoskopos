using System.Collections;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    [SerializeField] ControlRecorder controlRecorder;
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyUp("R"))
        {
            StartCoroutine(RotateCamera(6, 360));
        }*/
    }
    

    IEnumerator RotateCamera(float seconds, float endRotation)
    {
        controlRecorder.BeginRecording();

        float elapsedTime = 0;
        Vector3 startingPos = transform.eulerAngles;
        Vector3 finalRotation = new Vector3(startingPos.x, endRotation, startingPos.z);
        while (elapsedTime < seconds)
        {
            transform.eulerAngles = Vector3.Lerp(startingPos, finalRotation, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.eulerAngles = finalRotation;

        controlRecorder.StopRecording();
    }
#endif
}
