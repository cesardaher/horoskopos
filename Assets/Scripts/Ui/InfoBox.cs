using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public InfoBoxSpawner spawner;
    public int astroID;
    protected RectTransform activeBox;

    ContentSizeFitter contentSizeFitter;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    public virtual int AstroID
    {
        get { return astroID; }
        set
        {
            astroID = value;
        }
    }

    [SerializeField] protected GameObject infoBoxPrefab;
    [SerializeField] protected RectTransform positionalInfo;
    [SerializeField] protected RectTransform descriptionInfo;
    [SerializeField] protected GameObject expandBox;
    [SerializeField] protected RectTransform collapseBox;

    protected void Awake()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StartCoroutine(StartBoxes());
    }

    public virtual void ViewPositionInfo()
    {
        StartCoroutine(TransitionBoxes(descriptionInfo, positionalInfo, 0.3f));
    }

    public virtual void ViewDescriptionInfo()
    {
        StartCoroutine(TransitionBoxes(positionalInfo, descriptionInfo, 0.3f));
    }

    public void ExpandBox()
    {
        StartCoroutine(ExpandBox(0.3f));
    }

    public void CollapseBox()
    {
        StartCoroutine(CollapseBox(0.3f));
    }

    IEnumerator StartBoxes()
    {
        float duration = 0.5f;
        // make box invisible
        canvasGroup.alpha = 0;

        // open up boxes to get sizes
        descriptionInfo.gameObject.SetActive(true);
        positionalInfo.gameObject.SetActive(true);
        collapseBox.gameObject.SetActive(true);
        expandBox.SetActive(true);
        yield return null;

        // close boxes to initiate animation
        descriptionInfo.gameObject.SetActive(false);
        positionalInfo.gameObject.SetActive(false);
        collapseBox.gameObject.SetActive(false);
        expandBox.SetActive(false);
        yield return null;

        // collect positions and enable content size fitter
        canvasGroup.alpha = 1;
        contentSizeFitter.enabled = false;
        Vector2 startRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        Vector2 endRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + positionalInfo.rect.height + collapseBox.rect.height);

        float elapsedTime = 0;
        float progress = 0;
        while (progress < duration)
        {
            rectTransform.sizeDelta = Vector3.Lerp(startRect, endRect, progress);

            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }

        canvasGroup.alpha = 1;
        rectTransform.sizeDelta = endRect;
        contentSizeFitter.enabled = true;

        yield return null;

        // end
        positionalInfo.gameObject.SetActive(true);
        collapseBox.gameObject.SetActive(true);
        
    }

    IEnumerator TransitionBoxes(RectTransform startBox, RectTransform endBox, float time)
    {
        startBox.gameObject.SetActive(false);
        endBox.gameObject.SetActive(false);
        collapseBox.gameObject.SetActive(false);

        float deltaY = -startBox.rect.height + endBox.rect.height;
        Vector2 startRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        Vector2 endRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + deltaY);
        contentSizeFitter.enabled = false;

        float elapsedTime = 0;
        float progress = 0;
        while (progress < time)
        {
            rectTransform.sizeDelta = Vector3.Lerp(startRect, endRect, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / time;
            yield return null;
        }

        rectTransform.sizeDelta = endRect;

        yield return null;

        endBox.gameObject.SetActive(true);
        collapseBox.gameObject.SetActive(true);
        contentSizeFitter.enabled = true;

    }

    IEnumerator CollapseBox(float duration)
    {

        // save active box
        if (positionalInfo.gameObject.activeSelf) activeBox = positionalInfo;
        else if (descriptionInfo.gameObject.activeSelf) activeBox = descriptionInfo;
        else
        {
            Debug.LogWarning("Cannot collapse box. No ActiveBox found.");
            yield break;
        }

        // calculate start and end heights
        float deltaY = -activeBox.rect.height;
        Vector2 startRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        Vector2 endRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + deltaY);

        // turn off objects
        collapseBox.gameObject.SetActive(false);
        activeBox.gameObject.SetActive(false);
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

        // start animation
        float elapsedTime = 0;
        float progress = 0;
        while (progress < duration)
        {
            rectTransform.sizeDelta = Vector3.Lerp(startRect, endRect, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }

        rectTransform.sizeDelta = endRect;

        // activate expand box and adapt size
        expandBox.SetActive(true);
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    IEnumerator ExpandBox(float duration)
    {
        if(activeBox == null)
        {
            Debug.LogWarning("Cannot expand box. No ActiveBox found.");
            yield break;
        }

        // calculate start and end heights
        float deltaY = activeBox.rect.height;
        Vector2 startRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        Vector2 endRect = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + deltaY);

        // turn off objects
        expandBox.SetActive(false);
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

        // start animation
        float elapsedTime = 0;
        float progress = 0;
        while (progress < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startRect, endRect, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        rectTransform.sizeDelta = endRect;

        // activate expand box and adapt size
        collapseBox.gameObject.SetActive(true);
        activeBox.gameObject.SetActive(true);
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
