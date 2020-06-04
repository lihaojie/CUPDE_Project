using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class ToastManager : MonoBehaviour {
    
    private static ToastManager Instance;

    public static ToastManager GetInstance() {
        return Instance;
    }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ToastPrefab;
    public List<ToastHandler> ToastList = new List<ToastHandler>();

    private float Timer=0;
    public float Interval;

    public Transform Parent;


    //通过这里来创建Toast预制体
    public void CreatToast(string str)
    {
        if (ToastList.IsNotNull() && ToastList.Count > 0)
        {
            if (ToastList[ToastList.Count - 1].text.text.Equals(str))
            {
                return;
            }
        }
        var y = Parent.transform.position.y;
        var Toa = Instantiate(ToastPrefab, Parent.transform.position, Parent.transform.rotation, Parent.transform);
        var comp = Toa.GetComponent<ToastHandler>();
        ToastList.Insert(0, comp);
        comp.InitToast(str,()=> {
            ToastList.Remove(comp);
        });
        Timer = 0;
        //有新的Toast出现，之前的Toast向上移动
        ToastMove(0.2f);
    }

    public void ToastMove(float speed) {
        for (int i = 0; i < ToastList.Count; i++)
        {
            ToastList[i].Move(speed,i+1);
        }
    }
}