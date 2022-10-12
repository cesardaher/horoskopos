using UnityEngine;

public class ReflectionBaker : MonoBehaviour
{
    public ReflectionProbe reflectionProbe;

    void BakeReflection()
    {
        //reflectionProbe.RenderProbe();
    }


#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            //BakeReflection();
        }
    }

#endif
}
