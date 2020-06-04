using System.Collections.Generic; 
using UnityEngine; 


public class ItemCanvasList : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> mItemCanvasList = new List<GameObject>();

    public static List<GameObject> ItemCanvasObjList { get; private set; }

    private void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            mItemCanvasList.Add(childTransform.gameObject);
        }

        ItemCanvasObjList = mItemCanvasList;
    }

  
}
