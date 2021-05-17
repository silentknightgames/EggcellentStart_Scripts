using UnityEngine;
using DigitalRuby.Tween;
using UnityEngine.UI;
using TMPro;

public class PackingManager : MonoBehaviour
{
    [SerializeField] private GameObject tartPrefab, boxBottom, tarts, dragTarts, trayAndSlots, lid, okButton, patternColor,
        ribbon, patterns, continueButton, memo, okButtonMemo, finalGiftBox;
    [SerializeField] private DragLid dragLid;
    [SerializeField] private TextMeshProUGUI dragText, patternText;

    private void Start()
    {
        if (QuestionGenerator.tartNo > 9)
        {
            QuestionGenerator.tartNo = 9;
        }

        for (int i = 0; i < QuestionGenerator.tartNo; i++)
        {
            Vector3 spawnPos = EventBroker.slots[i].transform.position;
            EventBroker.slots[i].occupiedItem
                = Instantiate(tartPrefab, spawnPos, Quaternion.identity, tarts.transform).GetComponent<DragDrop>();
            EventBroker.items.Add(EventBroker.slots[i].occupiedItem);
            EventBroker.slots[i].occupied = true;
        }

        dragLid.enabled = false;
    }

    public void EnableDragTarts()
    {
        boxBottom.GetComponent<Image>().color = ColorChanger.finalColor;

        System.Action<ITween<Vector3>> updateDragPos = (t) =>
        {
            dragTarts.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> dragMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
        };

        Vector2 newPos = new Vector2(0, dragTarts.transform.position.y);
        dragTarts.Tween("MoveDragTarts", dragTarts.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateDragPos, dragMoveCompleted);

    }

    public void DeleteTartsOnTray()
    {
        foreach (var item in EventBroker.items)
        {
            foreach (var slot in EventBroker.slots)
            {
                if (item.GetComponent<DragDrop>() == slot.occupiedItem)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }

    public void RemoveTray()
    {
        EventBroker.slots.Clear();
        EventBroker.items.Clear();

        System.Action<ITween<Vector3>> updateTrayPos = (t) =>
        {
            trayAndSlots.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> trayMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            Destroy(trayAndSlots);
        };

        Vector3 offset = new Vector3(15, 0);
        trayAndSlots.Tween("MoveTray", trayAndSlots.transform.position, trayAndSlots.transform.position + offset, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateTrayPos, trayMoveCompleted);

        EnableLid();
        dragText.text = "Drag the lid to the box";
        okButton.SetActive(false);
    }

    private void EnableLid()
    {
        lid.GetComponent<Image>().color = ColorChanger.finalColor;

        System.Action<ITween<Vector3>> updateLidPos = (t) =>
        {
            lid.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> lidMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            dragLid.enabled = true;
            dragLid.GetBounds();
        };

        Vector2 newPos = new Vector2(3, lid.transform.position.y);
        lid.Tween("MoveLid", lid.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateLidPos, lidMoveCompleted);
    }

    public void RemoveDragTarts()
    {
        System.Action<ITween<Vector3>> updateTrayPos = (t) =>
        {
            dragTarts.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> trayMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            Destroy(dragTarts);
        };

        Vector3 offset = new Vector3(-20, 0);
        dragTarts.Tween("MoveTray", dragTarts.transform.position, dragTarts.transform.position + offset, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateTrayPos, trayMoveCompleted);
    }

    public void EnableMemo()
    {
        dragText.text = "Let's leave a message!";
        
        System.Action<ITween<Vector3>> updateLidPos = (t) =>
        {
            memo.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> lidMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
            okButtonMemo.SetActive(true);
        };

        Vector2 newPos = new Vector2(4, memo.transform.position.y);
        memo.Tween("MoveMemo", memo.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateLidPos, lidMoveCompleted);
    }

    public void EnableRibbonColor()
    {
        Vector2 offset = new Vector2(0, -0.2f);
        Vector2 boxPos = GameObject.FindGameObjectWithTag("GiftBox").GetComponent<RectTransform>().position;
        patterns.GetComponent<RectTransform>().position += Vector3.down * 0.8f;
        ribbon.GetComponent<RectTransform>().position = boxPos + offset;

        ribbon.SetActive(true);

        System.Action<ITween<Vector3>> updateLidPos = (t) =>
        {
            patternColor.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> lidMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
        };

        Vector2 newPos = new Vector2(0, patternColor.transform.position.y);
        patternColor.Tween("MovePalette", patternColor.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateLidPos, lidMoveCompleted);
    }

    public void FinishRibbon()
    {
        System.Action<ITween<Vector3>> updateBoxPos = (t) =>
        {
            finalGiftBox.transform.position = t.CurrentValue;
        };

        System.Action<ITween<Vector3>> boxMoveCompleted = (t) =>
        {
            Debug.Log("move completed");
        };

        Vector2 newPos = new Vector2(finalGiftBox.transform.position.x, -0.5f);
        finalGiftBox.Tween("MoveBox", finalGiftBox.transform.position, newPos, 1.5f,
            TweenScaleFunctions.CubicEaseOut, updateBoxPos, boxMoveCompleted);

        patterns.GetComponent<AudioSource>().Play();
        patternText.text = "The gift is ready!";
        continueButton.SetActive(true);
    }

    public void ToEndScene()
    {
        QuestionGenerator.tartNo = 0;
        StartCoroutine(SceneLoader.Instance.LoadNextScene("EndScene"));
    }
}
