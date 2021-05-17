using System;
using System.Collections;
using System.Collections.Generic;

public delegate IEnumerator CoroutineHandler();

public class EventBroker 
{
    public static event Action GameOver;

    public static List<DragDrop> items = new List<DragDrop>();
    public static List<ItemSlot> slots = new List<ItemSlot>();

    public static void CallGameOver()
    {
        if (GameOver == null)
        {
            return;
        }
        GameOver();
    }

    public static void UnsubscribeFromGameOver()
    {
        if (GameOver != null)
        {
            foreach (var d in GameOver.GetInvocationList())
            {
                GameOver -= (d as Action);
            }
        }
    }

}

