using UnityEngine;

public class ChangeScaleButton : MonoBehaviour
{
    public Transform parentTransform;

    private void Start()
    {
        parentTransform = transform.parent;
    }

    public void ToggleScale(bool val)
    {
        Vector3 newScale;

        if (val)
        {
            newScale = transform.parent.localScale * 1.1f;
            if (newScale.x > 1.3) newScale = new Vector3(1.3f, 1.3f, 1.3f);
            transform.parent.localScale = newScale;

            return;
        }

        newScale = transform.parent.localScale / 1.1f;
        if (newScale.x < 0.6) newScale = new Vector3(0.6f, 0.6f, 0.6f);
        transform.parent.localScale = newScale;

    }
}
