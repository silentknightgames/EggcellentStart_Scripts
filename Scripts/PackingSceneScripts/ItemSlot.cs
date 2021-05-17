using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool occupied = false;
    public DragDrop occupiedItem;

    private void Awake()
    {
        EventBroker.slots.Add(this);
    }

    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag != null && !occupied)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            occupied = true;
            occupiedItem = eventData.pointerDrag.GetComponent<DragDrop>();
        }
        else
        {
            bool firstSlot = true;

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

}
