using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public Vector3 deceleration = new Vector3(1, 1, 0); //减速度
 
    private Vector2 beginP = Vector2.zero; //鼠标第一次落下点  
    private Vector2 endP = Vector2.zero; //鼠标第二次位置（拖拽位置）  
    private Vector3 speed = Vector3.zero;
    public Camera eyeCamera = null; // 视图相机
    public bool isUpdateTouch = true; //是否更新touch 

    private float startPositionX = 11.94f;
    private float endPositionX = 142.1f;
    public bool isMove = false;
    public int clickCount = 0;
    public void Start()
    {
        if (eyeCamera == null)
        {
            eyeCamera = Camera.main;
        }
    }

    public void OnGUI()
    { 

        if (Event.current.type == EventType.MouseDown)
        {
            clickCount++;
            MoveBegin(Input.mousePosition);
        }
        else if (Event.current.type == EventType.MouseDrag)
        {
            isMove = true;
            clickCount = 0;
            Moveing(Input.mousePosition);
        } else if (Event.current.type == EventType.MouseUp)
        {
            clickCount++;
        }
    }

    //移动对象
    void UpdateTargetPositon()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        if (!isUpdateTouch)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Canceled ||
                    Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    isUpdateTouch = true;
                    break;
                }
            }
        }

        if (Input.touchCount == 1)
        {
            if (isUpdateTouch)
            {
                MoveBegin(Input.GetTouch(0).position);
                isUpdateTouch = false;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Moveing(Input.GetTouch(0).position);
            }
        }
    }

    ///初始化位置，为接下来的move做准备
    void MoveBegin(Vector3 point)
    {
        beginP = point;
        speed = Vector3.zero;
    }

    ///更新目标位置
    void Moveing(Vector3 point)
    {
        //记录鼠标拖动的位置 　　  
        endP = point;
        Vector3 fir =
            eyeCamera.ScreenToWorldPoint(new Vector3(beginP.x, beginP.y, eyeCamera.nearClipPlane)); //转换至世界坐标  
        Vector3 sec = eyeCamera.ScreenToWorldPoint(new Vector3(endP.x, endP.y, eyeCamera.nearClipPlane));
        speed = sec - fir; //需要移动的 向量  
    }

    ///Move结束，清除数据
    void MoveEnd(Vector3 point)
    {
        MoveBegin(point);
    }

    public void Update()
    {
        if (speed == Vector3.zero)
        {
            isMove = false;
            return;
        }

        var x = transform.position.x; 
        x = x - speed.x; //向量偏移   

        if (x < startPositionX )
        {
            x = startPositionX;
        }

        if (x > endPositionX)
        {
             x = endPositionX;
        }
        transform.position = new Vector3(x, 0, transform.position.z);
        if (System.Math.Abs(speed.x) < 0.01f)
        {
            speed.x = 0;
        }
        else
        {
            if (speed.x > 0)
            {
                speed.x -= deceleration.x * Time.deltaTime;
                if (speed.x < 0)
                {
                    speed.x = 0;
                }
            }
            else
            {
                speed.x += deceleration.x * Time.deltaTime;
                if (speed.x > 0)
                {
                    speed.x = 0;
                }
            }
        }

        if (System.Math.Abs(speed.y) < 0.01f)
        {
            speed.y = 0;
        }
        else
        {
            if (speed.y > 0)
            {
                speed.y -= deceleration.y * Time.deltaTime;
                if (speed.y < 0)
                {
                    speed.y = 0;
                }
            }
            else
            {
                speed.y += deceleration.y * Time.deltaTime;
                if (speed.y > 0)
                {
                    speed.y = 0;
                }
            }
        }

        beginP = endP;
        if (speed.x == 0 && speed.y == 0)
        {
            speed = Vector3.zero;
        }
    }

    public void InitPosition()
    {
        transform.position = new Vector3(startPositionX, 0, transform.position.z);
    }
}