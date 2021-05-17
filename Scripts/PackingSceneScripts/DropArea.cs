using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IPointerEnterHandler, IDropHandler
{
    public static bool onDropArea = false;
    private PackingManager packingManager;

    private void Awake()
    {
        packingManager = GameObject.Find("PackingManager").GetComponent<PackingManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.tag == "BoxLid")
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            packingManager.EnableMemo();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onDropArea = true;
    }
}
