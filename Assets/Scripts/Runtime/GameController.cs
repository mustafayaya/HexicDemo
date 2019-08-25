using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;

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
        public static ControllerBase controllerBase //Singleton here
        {
            get
            {

                return FindObjectOfType<ControllerBase>();
            }
            set
            {
                controllerBase = FindObjectOfType<ControllerBase>();
            }
        }
        [Header("Setup")]
        public Vector2 gridSize;
        public float cellSpacing;
        public Object hexagonPrefab;
        public RectTransform gridRectTransform;
        public Vector3 rectOffset;


        [Header("Game Settings")]
        public Vector2 cellSize = new Vector2(50, 43); //Cell size of explodables like hexagon, bomb etc...
        public List<HexagonModel> hexagonTypes = new List<HexagonModel>(); //Add hexagon types here
        public float animationWaitTime;
        [Header("Runtime")]
        Dictionary<Vector2, Cell> gridCellData = new Dictionary<Vector2, Cell>(); //Grid'de bulunan hücrelerin datalarını saklar

        private void Start()
        {
            controllerBase.poolController.PoolCells(cellSize);
            StartCoroutine(Draw());
        }
        void Update()
        {

        }
        float counter;
        
        private IEnumerator Draw() //Draw game area
        {

            WaitForSeconds wait = new WaitForSeconds(animationWaitTime);

            for (int i = 0; i < gridSize.y;i++ )
            {
                for (int j = 0; j < gridSize.x;j++ )
                {
                 
                    var _cell = ReuseHexagon();


                    InstertCellToGrid(new Vector2(j, i), _cell);
                    yield return wait;

                }
            }
        }

        void InstertCellToGrid(Vector2 gridCell,Cell cell)
        {
            var gridWidth = (gridSize.x - 1) * cellSize.x * 3 / 4 + (gridSize.x - 1) * cellSpacing; // 3/4 should use for drawing hexagon pattern
            var gridHeight = (gridSize.y - 1) * cellSize.y + (gridSize.y - 1) * cellSpacing; // 3/4 should use for drawing hexagon pattern

            Vector3 startPosition = new Vector3(-gridWidth / 2, gridHeight / 2) + rectOffset;
            if (gridCell.x % 2 == 0)
            {
                cell.GetComponent<RectTransform>().localPosition = startPosition + new Vector3(gridCell.x * cellSize.x * 3 / 4 + gridCell.x * cellSpacing, gridCell.y * -(cellSize.y + cellSpacing) - cellSize.y / 2);
            }
            else
            {
                cell.GetComponent<RectTransform>().localPosition = startPosition + new Vector3(gridCell.x * cellSize.x * 3 / 4 + gridCell.x * cellSpacing, gridCell.y * -(cellSize.y + cellSpacing));

            }

            gridCellData.Remove(gridCell);
            gridCellData.Add(gridCell, cell);
        }


        private Hexagon ReuseHexagon()
        {
            Color randomHexagonColor = hexagonTypes[Random.Range(0,hexagonTypes.Count)].color;
            
            return controllerBase.poolController.ReuseHexagon(randomHexagonColor);
            
        }

    }
    
}