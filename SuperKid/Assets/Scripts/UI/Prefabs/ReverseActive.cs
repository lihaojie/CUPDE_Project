using UnityEngine;

public class ReverseActive : MonoBehaviour
{

    public void SetReverseActive(bool reverseActive)
    {
        gameObject.SetActive(!reverseActive);
    }
}