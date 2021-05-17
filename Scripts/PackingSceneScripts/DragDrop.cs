using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler
{
    private GameObject dropArea;
    private PackingManager packingManager;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private float xBound;
    private float yBound;
    private bool dragging;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        packingManager = GameObject.Find("PackingManager").GetComponent<PackingManager>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        dropArea = GameObject.Find("DropArea");

        xBound = canvas.GetComponent<RectTransform>().sizeDelta.x / 2;
        yBound = canvas.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        dragging = true;

        if (EventBroker.slots != null)
        {
            foreach (var slot in EventBroker.slots)
            {
                if (slot.occupiedItem == this)
                {
                    slot.occupied = false;
                    slot.occupiedItem = null;
                }
            }
        }
        
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

            ToAnotherSlot(eventData);
        }

        if(DropArea.onDropArea)
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

    public void OnDrop(PointerEventData eventData)
    {
        if(EventBroker.slots != null)
        {
            foreach (var slot in EventBroker.slots)
            {
                if (slot.occupiedItem == this)
                {
                    ToAnotherSlot(eventData);
                }
            }
        }

        if (eventData.pointerDrag.tag == "BoxLid")
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = dropArea.GetComponent<RectTransform>().position;
            packingManager.EnableMemo();
        }

    }

    private void ToAnotherSlot(PointerEventData eventData)
    {
        bool firstSlot = true;

        if (EventBroker.slots != null)
        {
            foreach (var slot2 in EventBroker.slots)
            {
                if (!slot2.occupied && firstSlot)
                {
                    eventData.pointerDrag.GetComponent<RectTransform>().position = slot2.GetComponent<RectTransform>().position;
                    slot2.occupied = true;
                    slot2.occupiedItem = eventData.pointerDrag.GetComponent<DragDrop>();

                    firstSlot = false;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DropArea.onDropArea = true;
    }
}
