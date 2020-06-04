using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoveEffectTool
{
    public abstract class BasePress : MonoBehaviour
    {
        private Vector3 _defaultPos;
        private Vector3 _defaultScale;
        private Vector3 _defaultEuler;

        protected  void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Effect);
            _defaultPos = transform.position;
            _defaultScale = transform.localScale;
            _defaultEuler = transform.eulerAngles;
        }

        protected virtual void Effect()
        {
            transform.DOKill();
            ResetData();
        }

        private void ResetData()
        {
            transform.position = _defaultPos;
            transform.localScale = _defaultScale;
            transform.eulerAngles = _defaultEuler;
        }
    }
}
