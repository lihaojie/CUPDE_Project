using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class CanvasEvent : MonoBehaviour
{

    public void Finish()
    {
        SimpleEventSystem.Publish(new CanvasAnimationFinish());
    }

}
