using System.IO;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
#endif

public class ControlRecorder : MonoBehaviour
{
#if UNITY_EDITOR
    RecorderController m_RecorderController;
#endif
    public Material skybox;
    public GameObject ecliptic;
    public GameObject cusps;
    public GameObject meridian;

#if UNITY_EDITOR
    void OnEnable()
    {
        //SetupDomeRecorder();
        SetupMovieRecorder();
    }

    public void BeginRecording()
    {
        Debug.Log("begun recording");
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();
    }

    public void StopRecording()
    {
        Debug.Log("stop recording");
        m_RecorderController.StopRecording();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyUp("r"))
        {
            StartCoroutine(Record(10));
        }*/
    }

    void SetupMovieRecorder()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        // Image sequence
        var imageRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        imageRecorder.name = "My Movie Recorder";
        imageRecorder.Enabled = true;
        imageRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        imageRecorder.CaptureAlpha = false;

        var mediaOutputFolder = Path.Combine(Application.dataPath, "..", "Recordings");
        imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "video_") + DefaultWildcard.Take;

        imageRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080,

        };

        // Setup Recording
        controllerSettings.AddRecorderSettings(imageRecorder);
        controllerSettings.SetRecordModeToManual();
    }

    void SetupDomeRecorder()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        // Image sequence
        var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
        imageRecorder.name = "My Image Recorder";
        imageRecorder.Enabled = true;
        imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.PNG;
        imageRecorder.CaptureAlpha = false;

        var mediaOutputFolder = Path.Combine(Application.dataPath, "..", "DomeRecordings");
        imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "image_") + DefaultWildcard.Frame;

        imageRecorder.imageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 4096,
            OutputHeight = 4096,
        };

        // Setup Recording
        controllerSettings.AddRecorderSettings(imageRecorder);
        controllerSettings.SetRecordModeToManual();
    }

    IEnumerator Record(float seconds)
    {
        BeginRecording();

        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StopRecording();
    }
#endif
    /*
    private void Update()
    {
        if(Input.GetKeyUp("r"))
        {
            Debug.Log("begun recording");
            BeginRecording();
        }

        if (Input.GetKeyUp("t"))
           {
            Debug.Log("stop recording");
        }
    }*/
}
