using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.AppUI.UI;
using UnityEngine;

namespace Assets
{
    public class ProgressBar: MonoBehaviour
    {
        [SerializeField] private Vector2 padding = new(10,3);//遮罩内边距
        [SerializeField] private RectTransform mask;
        private RectTransform maskParentRectTransform;
        private void Awake()
        {
            if (mask == null)
            {
                Debug.LogError("无遮罩");
                return;
            }
            maskParentRectTransform = mask.parent.GetComponent<RectTransform>();
        }
        public void SetProgress(float progress)
        {
            Vector2 parentSize=maskParentRectTransform.sizeDelta;
            Vector2 targetSize = parentSize-padding*2;
            targetSize.x *=Mathf.Clamp01(progress);
            mask.offsetMin = padding;
            mask.offsetMax = new Vector2(padding.x+targetSize.x-parentSize.x,-padding.y);
        }
    }
}