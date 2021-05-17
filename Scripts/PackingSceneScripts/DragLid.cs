using UnityEngine;
using UnityEngine.EventSystems;

public class DragLid : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private float xBound;
    private float yBound;
    private bool dragging;
    public static Vector2 lidInitialPos;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void GetBounds()
    {
        xBound = canvas.GetComponent<RectTransform>().sizeDelta.x / 2;
        yBound = canvas.GetComponent<RectTransform>().sizeDelta.y / 2;

        lidInitialPos = GetComponent<RectTransform>().position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        dragging = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform.anchoredPosition.x > -xBound && rectTransform.anchoredPosition.x < xBound
            && rectTransform.anchoredPosition.y > -yBound && rectTransform.anchoredPosition.y < yBound && dragging)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else if (dragging)
        {
            dragging = false;

            eventData.pointerDrag.GetComponent<RectTransform>().position = lidInitialPos;
        }

        if (DropArea.onDropArea)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0.6f; ;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        dragging = false;
    }
}
