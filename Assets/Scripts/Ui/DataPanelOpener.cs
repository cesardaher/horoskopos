using UnityEngine;

public class DataPanelOpener : MonoBehaviour
{
    [SerializeField] RectTransform fullPanel;
    [SerializeField] RectTransform secPanel;

    Vector3 closedPosition;

    public bool Opened { get; private set; }

    private void Start()
    {
        Opened = false;
        closedPosition = fullPanel.anchoredPosition;
    }

    public void OpenClosePanel()
    {
        if (Opened)
        {
            fullPanel.anchoredPosition = closedPosition;
            Opened = false;
            return;
        }

        fullPanel.anchoredPosition = new Vector3(fullPanel.anchoredPosition.x, fullPanel.anchoredPosition.y + secPanel.rect.height);
        Opened = true;

    }

    public void OpenPanel()
    {
        fullPanel.anchoredPosition = new Vector3(fullPanel.anchoredPosition.x, fullPanel.anchoredPosition.y + secPanel.rect.height);
        Opened = true;
    }

    public void ClosePanel()
    {
        fullPanel.anchoredPosition = closedPosition;
        Opened = false;
    }
}
