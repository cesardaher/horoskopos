using UnityEngine;

public class ChartOpener : MonoBehaviour
{
    [SerializeField] RectTransform chartHolder;

    Vector3 openPosition;
    Vector3 closedPosition;

    bool opened;

    private void Start()
    {
        opened = false;
        closedPosition = chartHolder.anchoredPosition;
        openPosition = new Vector3(chartHolder.anchoredPosition.x, chartHolder.anchoredPosition.y + chartHolder.rect.height);

    }

    public void OpenCloseChart()
    {
        if (opened)
        {
            chartHolder.anchoredPosition = closedPosition;
            opened = false;
            return;
        }

        chartHolder.anchoredPosition = openPosition;
        opened = true;

    }

    public void CheckForTutorialPrompt()
    {
        if(TutorialManager.isInTutorial && TutorialManager.Instance.openChart)
        {
            EventManager.Instance.AllowContinueTutorial();
            TutorialManager.Instance.openChart = false;
        }
    }
}
