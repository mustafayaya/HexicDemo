using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Hexic.Runtime
{


    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance //Singleton here
        {
            get {                
                return FindObjectOfType<GameManager>();
            }
            set
            {
                _instance = FindObjectOfType<GameManager>();
            }
        }
        

        [Header("Game Settings")]
        public List<HexagonModel> hexagonTypes = new List<HexagonModel>(); //Add hexagon types here
        public float animationWaitTime;
        public float swipeAnimationSpeed = 10f;
        public Image trioCursorImage;
        public Text gameOverText;

        public float explosionPoints = 5f;
        public float bombSpawnScore = 1000f;
        public bool interactable = false; //Set this false while initializing, calculating matches etc.

        [Header("Runtime")]
        public float score;

        public HexagonTrio selectedHexagonTrio;


        void Start()
        {
            StartGame();
        }
        public void StartGame()
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
        HexagonTrio lastTrio;
        public Coroutine lastHighlightCoroutine;
        void HexagonSelectionHandler()
        {
            Vector3 touchedPosition ;

            if (InputController._instance.GetScreenTouch(out touchedPosition))
            {
     
                    selectedHexagonTrio = GetClosestHexagonTrio(new Vector2(touchedPosition.x, touchedPosition.y));
                    trioCursorImage.transform.position = selectedHexagonTrio.center;
                if (lastTrio != null)
                {
                    lastTrio.SelectedHighlight();
                }
                    lastTrio = selectedHexagonTrio;
                    selectedHexagonTrio.SelectedHighlight();
                
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
        int moves;
        public void CountMove() //Gets triggered when player makes a move
        {
            Debug.Log("Player move");
            var hexagonBombs = FindObjectsOfType<HexagonBomb>();
            foreach (HexagonBomb bomb in hexagonBombs)
            {
                bomb.Countdown();
            }
            moves++;
        }

        IEnumerator TryToMatch(bool clockwise,HexagonTrio selectedHexagonTrio) 
        {
            interactable = false;


                StartCoroutine(selectedHexagonTrio.TurnHexagonTrio(clockwise));//First turn


                yield return new WaitWhile(() =>!selectedHexagonTrio.turnTrigger);


                if (GridController._instance.MatchingQuery())
                {
                       CountMove();
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

                CountMove();

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


        struct CellInputQuery
        {
            public float distance ;
            public HexagonTrio hexagonTrio;


        }

        public void FinishGame()
        {
            interactable = false;
            StopAllCoroutines();
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(GridController._instance.DissolveGrid());

            StartCoroutine(Restart());
        }

        public IEnumerator Restart()
        {
            yield return new WaitForSeconds(10f);
            SceneManager.LoadScene(0);
        }
    }
    
}