using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Hexic.Runtime;

namespace Hexic.Elements
{
    public class Cell : Button, IReusable
    {
        public Vector2 gridCoordinates;

        public void SetSize(Vector2 size)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size.x);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

        }
        public virtual void OnReuse()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnExplode()
        {
            StartCoroutine( ExplosionCoroutine());
        }

        IEnumerator ExplosionCoroutine()
        {

            var startColor = image.color;
            var elapsedTime = 0f;
            var animationTime = 2;

            while (elapsedTime < animationTime)
            {
                image.color = Color.Lerp(startColor,new Color(1,1,1,0.5f),elapsedTime/ animationTime);

                elapsedTime += GameController._instance.swipeAnimationSpeed * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            gameObject.SetActive(false);

        }
    }
}