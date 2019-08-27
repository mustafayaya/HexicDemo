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

        public virtual void Translate(Vector2 position)
        {

                GetComponent<RectTransform>().localPosition = position;
                return;

        }

        public virtual void Translate(Vector2 startPosition, Vector2 targetPosition)
        {
            GetComponent<RectTransform>().localPosition = startPosition;
            StartCoroutine(PositionTransition(targetPosition));
        }


        public virtual void OnExplode()
        {
            GameManager._instance.AddScore(GameManager._instance.explosionPoints);
            StartCoroutine( ExplosionCoroutine());
        }

        IEnumerator PositionTransition(Vector2 position)
        {
            var elapsedTime = 0f;
            var animationTime = 2;
            var startPosition = GetComponent<RectTransform>().localPosition;

            while (elapsedTime < animationTime)
            {
                GetComponent<RectTransform>().localPosition = Vector2.Lerp(startPosition,position,elapsedTime/animationTime);

                elapsedTime += GameManager._instance.swipeAnimationSpeed * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }

            GetComponent<RectTransform>().localPosition = position;

        }

        IEnumerator ExplosionCoroutine()
        {

            var startColor = image.color;
            var elapsedTime = 0f;
            var animationTime = 2;

            while (elapsedTime < animationTime)
            {
                image.color = Color.Lerp(startColor,new Color(1,1,1,0.5f),elapsedTime/ animationTime);

                elapsedTime += GameManager._instance.swipeAnimationSpeed * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            gameObject.SetActive(false);

        }
    }
}