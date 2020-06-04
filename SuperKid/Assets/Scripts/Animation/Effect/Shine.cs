using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 星星闪耀的特效
/// </summary>
public class Shine : MonoBehaviour {

    private Image img;
    public float speed=4;
    private bool add;

    public bool isShine = true;
    public bool isRotate = true;
    public bool isRotateControll = false;//旋转是否收到控制
    public void Awake()
    {
        img = GetComponent<Image>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isRotate)
        {
            if (isRotateControll)
            {
                transform.Rotate(Vector3.forward * speed, Space.World);
            }
            else
            {
                transform.Rotate(Vector3.forward * 4, Space.World);

            }
        }

        if (isShine)
        {
	         if (!add)
                    {
                        img.color -= new Color(0, 0, 0, Time.deltaTime * speed);
                        if (img.color.a <=0.2f)
                        {
                            add = true;
                        }
                    }
                    else
                    {
                        img.color += new Color(0, 0, 0, Time.deltaTime * speed);
                        if (img.color.a >=0.8f)
                        {
                            add = false;
                        }
                    }
        }
	}
}
