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

                    var _cell = InitializeHexagon(new Vector2(i, j));


                    InstertCellToGrid(new Vector2(i, j), _cell);

                    yield return wait;//Wait for initialization effect

                }
            }
        }

        void InstertCellToGrid(Vector2 gridCell, Cell cell)
        {
            var gridWidth = (gridSize.x ) * cellSize.x * 3 / 4 + (gridSize.x - 1) * cellSpacing; // 3/4 constant must be used for drawing honeycomb pattern 
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

        List<List<Vector2>> evenQueryVectors = new List<List<Vector2>>() {
            new List<Vector2>(){new Vector2(1, 0),new Vector2(1, 1)},
            new List<Vector2>(){new Vector2(0, 1),new Vector2(1, 1)},
            new List<Vector2>(){new Vector2(0, 1),new Vector2(-1, 1)},
            new List<Vector2>(){new Vector2(-1, 0),new Vector2(-1, 1) },
            new List<Vector2>(){new Vector2(-1, 0),new Vector2(0, -1) },
            new List<Vector2>(){new Vector2(0,-1),new Vector2(1, 0) }
        };
        List<List<Vector2>> oddQueryVectors = new List<List<Vector2>>() {
            new List<Vector2>(){new Vector2(0,-1),new Vector2(1, -1) },
            new List<Vector2>(){new Vector2(1, 0),new Vector2(1, -1) },
            new List<Vector2>(){new Vector2(1,0),new Vector2(0,1)},
            new List<Vector2>(){new Vector2(0, 1),new Vector2(-1,0)},
            new List<Vector2>(){new Vector2(-1, 0),new Vector2(-1, -1) },
            new List<Vector2>(){new Vector2(-1, -1),new Vector2(0, -1) },
        };

        Hexagon InitializeHexagon(Vector2 gridCoordinates)
        {

            List<Color> availableColors = new List<Color>();
            List<List<Vector2>> queryVectors;
            foreach (HexagonModel model in GameController._instance.hexagonTypes) { //Initialize available color list
                availableColors.Add(model.color);
            }

            for (int i = 0; i < GameController._instance.hexagonTypes.Count; i++)
            {

                if (gridCoordinates.x % 2 == 0) //if it is even
                {
                    queryVectors = evenQueryVectors;
                }
                else
                {
                    queryVectors = oddQueryVectors;

                }

                foreach (List<Vector2> vectors in queryVectors)
                {
                    if (gridCellData.ContainsKey(gridCoordinates + vectors[0]) && gridCellData.ContainsKey(gridCoordinates + vectors[1]))
                    {
                        if (((Hexagon)gridCellData[gridCoordinates + vectors[0]]).color == ((Hexagon)gridCellData[gridCoordinates + vectors[1]]).color && ((Hexagon)gridCellData[gridCoordinates + vectors[0]]).color == GameController._instance.hexagonTypes[i].color)
                        {
                            foreach (Vector2 v in vectors)
                            {
                                Debug.Log(v);

                            }

                            availableColors.Remove(GameController._instance.hexagonTypes[i].color);
                            continue;
                        }
                    }

                }
                
            }
           

            Color randomHexagonColor = availableColors[Random.Range(0, availableColors.Count)];

            return PoolController._instance.ReuseHexagon(randomHexagonColor);

        }

    }
}
