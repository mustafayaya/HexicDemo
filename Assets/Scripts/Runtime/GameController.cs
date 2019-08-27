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

        [Header("Runtime")]
        HexagonTrio selectedHexagonTrio;
        public bool interactable = false; //Set this false while initializing, calculating matches etc.
        private void Start()
        {
            GridController._instance.InitializeGrid();
        }
        void Update()
        {
                HexagonSelectionHandler();
            if (interactable)
            {
                SwipeHandler();

            }
            if (Input.GetKey(KeyCode.K)) {
                GridController._instance.MatchingQuery();
            }

        }

        void HexagonSelectionHandler()
        {
            Vector3 touchedPosition ;

            if (InputController._instance.GetScreenTouch(out touchedPosition))
            {
               


                if (selectedHexagonTrio == GetClosestHexagonTrio(new Vector2(touchedPosition.x, touchedPosition.y)))
                {
                    Debug.Log( "Match : "+ selectedHexagonTrio.CheckMatch() + ((Hexagon)GridController._instance.gridCellData[ selectedHexagonTrio.hexagon1GridCoordinates]).color + ((Hexagon)GridController._instance.gridCellData[selectedHexagonTrio.hexagon2GridCoordinates]).color+ ((Hexagon)GridController._instance.gridCellData[selectedHexagonTrio.hexagon3GridCoordinates]).color);

                }
                else
                {
                    selectedHexagonTrio = GetClosestHexagonTrio(new Vector2(touchedPosition.x, touchedPosition.y));
                    trioCursorImage.transform.position = selectedHexagonTrio.center;
                }
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
                }
            }
        }

        

        IEnumerator TryToMatch(bool clockwise,HexagonTrio selectedHexagonTrio) 
        {
            interactable = false;
            if (clockwise)
            {

                StartCoroutine(selectedHexagonTrio.TurnClockwise());//First turn


                yield return new WaitWhile(() =>!selectedHexagonTrio.turnTrigger);


                if (GridController._instance.MatchingQuery())
                {

                    StopCoroutine(lastMatchCoroutine);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);

                    StartCoroutine(selectedHexagonTrio.TurnClockwise());//Second turn

                }

                yield return new WaitWhile(() => !selectedHexagonTrio.turnTrigger);
                if (GridController._instance.MatchingQuery())
                {

                    StopCoroutine(lastMatchCoroutine);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);

                    StartCoroutine(selectedHexagonTrio.TurnClockwise());//Second turn

                }


            }
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