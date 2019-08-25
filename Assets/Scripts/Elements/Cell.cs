using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Hexic.Elements
{
    public class Cell : Button, IReusable
    {
        public void SetSize(Vector2 size)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size.x);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

        }
        public virtual void OnReuse()
        {
            gameObject.SetActive(true);
        }
    }
}