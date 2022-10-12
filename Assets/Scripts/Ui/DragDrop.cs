using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas _mainCanvas;
    [SerializeField] RectTransform _infoBoxRectTransform;
    [SerializeField] Image _dragArea;
    [SerializeField] [Range(1, 1.5f)] float _dragScale;

    bool isButtonClicked = false;

    Vector3 ogScale;
    Vector2 topRightCorner;
    Vector2 bottomLeftCorner;

    void Awake()
    {
        _mainCanvas = transform.parent.parent.parent.GetComponent<Canvas>();
        
        if (_dragArea is null) _dragArea = transform.parent.parent.GetComponent<Image>();
    }

    void Start()
    {
        ogScale = _infoBoxRectTransform.localScale;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && isButtonClicked)
        {
            isButtonClicked = false;
            _infoBoxRectTransform.localScale = ogScale;
        }

        KeepBoxWithinBounds();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonClicked = true;
        ogScale = _infoBoxRectTransform.localScale;
        _infoBoxRectTransform.localScale *= _dragScale;

        // put box on top of other boxes
        transform.parent.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _infoBoxRectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;

        GetCorners();

        if(topRightCorner.x >_dragArea.rectTransform.rect.xMax ||
            topRightCorner.y > _dragArea.rectTransform.rect.yMax ||
            bottomLeftCorner.x < _dragArea.rectTransform.rect.xMin ||
            bottomLeftCorner.y < _dragArea.rectTransform.rect.yMin)
        {
            _infoBoxRectTransform.anchoredPosition -= eventData.delta / _mainCanvas.scaleFactor;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isButtonClicked = false;
        _infoBoxRectTransform.localScale = ogScale;
    }

    void GetCorners()
    {
        // get corner positions
        Vector3[] corners = new Vector3[4];
        _infoBoxRectTransform.GetWorldCorners(corners);

        // get position of top right corner
        // get position of bottom left corner
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragArea.rectTransform, corners[0], Camera.current, out bottomLeftCorner);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragArea.rectTransform, corners[2], Camera.current, out topRightCorner);
    }

    void KeepBoxWithinBounds()
    {
        GetCorners();

        // VERTICAL AXIS
        if (topRightCorner.y > _dragArea.rectTransform.rect.yMax)
        {
            float deltaY = topRightCorner.y - _dragArea.rectTransform.rect.yMax;
            Vector3 finalPos = new Vector3(_infoBoxRectTransform.anchoredPosition.x, _infoBoxRectTransform.anchoredPosition.y - deltaY);
            _infoBoxRectTransform.anchoredPosition = finalPos;
        }
        
        if (bottomLeftCorner.y < _dragArea.rectTransform.rect.yMin)
        {
            float deltaY = _dragArea.rectTransform.rect.yMin - bottomLeftCorner.y;
            Vector3 finalPos = new Vector3(_infoBoxRectTransform.anchoredPosition.x, _infoBoxRectTransform.anchoredPosition.y + deltaY);
            _infoBoxRectTransform.anchoredPosition = finalPos;
        }

        // HORIZONTAL AXIS
        if (topRightCorner.x > _dragArea.rectTransform.rect.xMax)
        {
            float deltaX = topRightCorner.x - _dragArea.rectTransform.rect.xMax;
            Vector3 finalPos = new Vector3(_infoBoxRectTransform.anchoredPosition.x - deltaX, _infoBoxRectTransform.anchoredPosition.y);
            _infoBoxRectTransform.anchoredPosition = finalPos;
        }

        if (bottomLeftCorner.x < _dragArea.rectTransform.rect.xMin)
        {
            float deltaX = _dragArea.rectTransform.rect.xMin - bottomLeftCorner.x;
            Vector3 finalPos = new Vector3(_infoBoxRectTransform.anchoredPosition.x + deltaX, _infoBoxRectTransform.anchoredPosition.y);
            _infoBoxRectTransform.anchoredPosition = finalPos;
        }
    }

}
