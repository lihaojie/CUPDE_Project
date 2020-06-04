using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class LoadingManager : MonoBehaviour {
    
    private static LoadingManager Instance;

    public static LoadingManager GetInstance() {
        return Instance;
    }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject LoadingPrefab;
    public Transform Parent;
    private GameObject Loading;

    //通过这里来创建Loading预制体
    public void CreatLoading(string str = null)
    {
        var y = Parent.transform.position.y;
        if (Parent.childCount < 1)
        {
            Loading = Instantiate(LoadingPrefab, Parent.transform.position, Parent.transform.rotation, Parent.transform);
            Loading.SetActive(true);
        }
    }

    public void DismissLoading()
    {
        if (Loading.IsNotNull())
        {
            Destroy(Loading);
        }
    }
}