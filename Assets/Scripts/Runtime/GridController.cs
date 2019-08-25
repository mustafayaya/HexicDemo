using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;


namespace Hexic.Runtime
{
    public class GridController : MonoBehaviour
    {

        public static GridController _instance //Singleton here
        {
            get
            {
                return FindObjectOfType<GridController>();
            }
            set
            {
                _instance = FindObjectOfType<GridController>();
            }
        }


        public Vector2 gridSize = new Vector2(8,9);
        public float cellSpacing = 3f;
        public Vector2 cellSize = new Vector2(63, 50); //Cell size of explosive like hexagon, bomb etc... For regular hexagon x/y constant is 1.16 
        public Object hexagonPrefab;
        public RectTransform gridRectTransform;
        public Vector3 rectOffset;

        Dictionary<Vector2, Cell> gridCellData = new Dictionary<Vector2, Cell>(); //Grid'de bulunan hücrelerin datalarını saklar


    

        public void InitializeGrid()
        {
            PoolController._instance.PoolCells(cellSize); //Prepare pool for initialization of grid
            StartCoroutine(Draw());
        }

        private IEnumerator Draw() //Draw game area
        {

            WaitForSeconds wait = new WaitForSeconds(GameController._instance.animationWaitTime); 

            for (int i = 0; i < gridSize.y; i++)
            {
                for (int j = 0; j < gridSize.x; j++)
                {

                    var _cell = ReuseHexagon();


                    InstertCellToGrid(new Vector2(j, i), _cell);
                    yield return wait;//Wait for initialization effect

                }
            }
        }

        void InstertCellToGrid(Vector2 gridCell, Cell cell)
        {
            var gridWidth = (gridSize.x - 1) * cellSize.x * 3 / 4 + (gridSize.x - 1) * cellSpacing; // 3/4 constant must be used for drawing honeycomb pattern 
            var gridHeight = (gridSize.y - 1) * cellSize.y + (gridSize.y - 1) * cellSpacing;

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
            Color randomHexagonColor = GameController._instance.hexagonTypes[Random.Range(0, GameController._instance.hexagonTypes.Count)].color;

            return PoolController._instance.ReuseHexagon(randomHexagonColor);

        }

    }
}
