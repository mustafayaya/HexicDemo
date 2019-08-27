using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;
using UnityEngine.UI;

namespace Hexic.Runtime
{


    public class GameController : MonoBehaviour
    {
        public static GameController _instance //Singleton here
        {
            get {                
                return FindObjectOfType<GameController>();
            }
            set
            {
                _instance = FindObjectOfType<GameController>();
            }
        }
        

        [Header("Game Settings")]
        public List<HexagonModel> hexagonTypes = new List<HexagonModel>(); //Add hexagon types here
        public float animationWaitTime;
        public float swipeAnimationSpeed = 10f;

        public Image trioCursorImage;
        public float explosionPoints = 5f;

        public bool interactable = false; //Set this false while initializing, calculating matches etc.

        [Header("Runtime")]
        float score;

        HexagonTrio selectedHexagonTrio;
        private void Start()
        {
            GridController._instance.InitializeGrid();
            AddScore(0);
        }
        void Update()
        {
            if (interactable)
            {
                HexagonSelectionHandler();

                SwipeHandler();

            }
        }
        public Text scoreText;
        public void AddScore(float points)
        {
            score += points;
            if (scoreText)
            {
                scoreText.text = score.ToString();
            }
        }

        void HexagonSelectionHandler()
        {
            Vector3 touchedPosition ;

            if (InputController._instance.GetScreenTouch(out touchedPosition))
            {
     
                    selectedHexagonTrio = GetClosestHexagonTrio(new Vector2(touchedPosition.x, touchedPosition.y));
                    trioCursorImage.transform.position = selectedHexagonTrio.center;
            }

            
        }

        HexagonTrio GetClosestHexagonTrio(Vector2 position)
        {
            CellInputQuery cellInputQuery = new CellInputQuery();

            foreach (HexagonTrio trio in GridController._instance.HexagonTrios)
            {
                var _distance = Vector3.Distance(new Vector2(position.x, position.y), trio.center);
                if (_distance < cellInputQuery.distance || cellInputQuery.distance == 0)
                {
                    cellInputQuery.distance = _distance;
                    cellInputQuery.hexagonTrio = trio;

                }
            }

            return cellInputQuery.hexagonTrio;
        }

        Coroutine lastMatchCoroutine = null;

        void SwipeHandler()
        {
            if (selectedHexagonTrio != null)
            {
                var swipeType = InputController._instance.GetScreenSwipe();
                if (swipeType != InputController.SwipeType.None)
                {
                    if (swipeType == InputController.SwipeType.Up)//Turn trio clockwise
                    {
                        lastMatchCoroutine = StartCoroutine(TryToMatch(true,selectedHexagonTrio));
                    }
                    if (swipeType == InputController.SwipeType.Down)//Turn trio clockwise
                    {
                        lastMatchCoroutine = StartCoroutine(TryToMatch(false, selectedHexagonTrio));
                    }
                }
            }
        }

        

        IEnumerator TryToMatch(bool clockwise,HexagonTrio selectedHexagonTrio) 
        {
            interactable = false;


                StartCoroutine(selectedHexagonTrio.TurnHexagonTrio(clockwise));//First turn


                yield return new WaitWhile(() =>!selectedHexagonTrio.turnTrigger);


                if (GridController._instance.MatchingQuery())
                {

                    StopCoroutine(lastMatchCoroutine);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(selectedHexagonTrio.TurnHexagonTrio(clockwise));//Second turn

                }

                yield return new WaitWhile(() => !selectedHexagonTrio.turnTrigger);
                if (GridController._instance.MatchingQuery())
                {

                    StopCoroutine(lastMatchCoroutine);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);

                    StartCoroutine(selectedHexagonTrio.TurnHexagonTrio(clockwise));//Second turn

                }
            yield return new WaitWhile(() => !selectedHexagonTrio.turnTrigger);


            interactable = true;

        }


        public void HexagonTrioTurnOneUnit()
        {

        }


        struct CellInputQuery
        {
            public float distance ;
            public HexagonTrio hexagonTrio;


        }
    }
    
}