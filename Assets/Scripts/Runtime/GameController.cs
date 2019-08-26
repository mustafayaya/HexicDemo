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
            if (interactable)
            {
                HexagonSelectionHandler();
                SwipeHandler();

            }


        }

        void HexagonSelectionHandler()
        {
            Vector3 touchedPosition ;

            if (InputController._instance.GetScreenTouch(out touchedPosition))
            {
               
                selectedHexagonTrio = GetClosestHexagonTrio(new Vector2(touchedPosition.x,touchedPosition.y));

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


        void SwipeHandler()
        {
            if (selectedHexagonTrio != null)
            {
                if (InputController._instance.GetScreenSwipe() != InputController.SwipeType.None)
                {

                }
            }
        }

       

        struct CellInputQuery
        {
            public float distance ;
            public HexagonTrio hexagonTrio;


        }
    }
    
}