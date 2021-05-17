using UnityEngine;
using UnityEngine.EventSystems;

public class DragBackground : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public void OnDrop(PointerEventData eventData)
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

        if (eventData.pointerDrag.tag == "BoxLid")
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = DragLid.lidInitialPos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DropArea.onDropArea = false;
    }
}
