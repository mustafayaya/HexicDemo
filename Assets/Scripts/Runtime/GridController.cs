using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;
using UnityEngine.UI;

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

        public Dictionary<Vector2, Cell> gridCellData = new Dictionary<Vector2, Cell>(); //Grid'de bulunan hücrelerin datalarını saklar

        bool gridInitialized;
        Coroutine _lastErosionCoroutine = null;


        public void InitializeGrid()
        {
            PoolController._instance.PoolCells(cellSize); //Prepare pool for initialization of grid
            StartCoroutine(Draw());
        }

        private IEnumerator Draw() //Draw game area
        {

            WaitForSeconds wait = new WaitForSeconds(GameController._instance.animationWaitTime); 



            for (int i = 0; i < gridSize.x; i++)//Initialize cells
            {
                for (int j = 0; j < gridSize.y; j++)
                {

                    var _cell = InitializeHexagon(new Vector2(i, j));


                    InsertCellToGrid(new Vector2(i, j), _cell);
                    if (i == gridSize.x - 1 && j == gridSize.y - 1)
                    {
                        InitializeHexagonTrios();
                        GridInitialized();
                    }
                    yield return wait;//Wait for initialization effect
                   
                }
            }

           

        }

        public void GridInitialized()
        {
            GameController._instance.interactable = true;
            gridInitialized = true;
        }

        public List<HexagonTrio> HexagonTrios = new List<HexagonTrio>();

        public Image uiImage;
        void InitializeHexagonTrios()
        {
            for (int x = 0; x < gridSize.x - 1; x++)//Initialize trios
            {

                for (int y = 0; y < gridSize.y - 1; y++)
                {
                    if (x%2 ==0)
                    {
                        HexagonTrios.Add(new HexagonTrio((Hexagon)gridCellData[new Vector2(x, y)], (Hexagon)gridCellData[new Vector2(x + 1, y)], (Hexagon)gridCellData[new Vector2(x + 1, y + 1)]));
                        HexagonTrios.Add(new HexagonTrio((Hexagon)gridCellData[new Vector2(x, y)], (Hexagon)gridCellData[new Vector2(x + 1, y + 1)], (Hexagon)gridCellData[new Vector2(x, y + 1)]));
                    }
                    else
                    {
                        if (y != 0)//if it is not at the top level
                        {
                            HexagonTrios.Add(new HexagonTrio((Hexagon)gridCellData[new Vector2(x, y)],  (Hexagon)gridCellData[new Vector2(x + 1, y - 1)], (Hexagon)gridCellData[new Vector2(x + 1, y)]));

                        }
                        HexagonTrios.Add(new HexagonTrio((Hexagon)gridCellData[new Vector2(x, y)], (Hexagon)gridCellData[new Vector2(x + 1, y)],(Hexagon)gridCellData[new Vector2(x, y + 1)]));
                        if (y == gridSize.y - 2)//Add trios to the bottom
                        {
                            HexagonTrios.Add(new HexagonTrio((Hexagon)gridCellData[new Vector2(x, y + 1)],  (Hexagon)gridCellData[new Vector2(x + 1, y)],(Hexagon)gridCellData[new Vector2(x + 1, y + 1)]));

                        }
                    }
                   

                }
            }
        }

        public void InsertCellToGrid(Vector2 gridCell, Cell cell)
        {
            
            cell.Translate(GetCellWorldPositionAtGrid(gridCell));
            ClearGridCell(gridCell);
            gridCellData.Add(gridCell, cell);
            cell.gridCoordinates = gridCell;

        }

        public void ClearGridCell(Vector2 gridCell)
        {
            gridCellData.Remove(gridCell);
            if (gridCellData.ContainsKey(gridCell))
            {
                Debug.Log("couldn't deleted");
            }
        }
        public void InsertCellToGrid(Vector2 gridCell, Cell cell, Vector3 startpos)
        {

            cell.Translate(startpos,GetCellWorldPositionAtGrid(gridCell));
            gridCellData.Remove(gridCell);
            gridCellData.Add(gridCell, cell);
            cell.gridCoordinates = gridCell;
            cell.onClick.RemoveAllListeners();
            cell.onClick.AddListener(() => Debug.Log("I am at " + cell.gridCoordinates+" and I am " + cell.image.color));

        }
        Vector2 GetCellWorldPositionAtGrid(Vector2 gridCell)
        {
            var gridWidth = (gridSize.x - 1) * cellSize.x * 3 / 4 + (gridSize.x - 1) * cellSpacing; // 3/4 constant must be used for drawing honeycomb pattern 
            var gridHeight = (gridSize.y - 1) * cellSize.y + (gridSize.y - 1) * cellSpacing;
            Vector3 startPosition = new Vector3(-gridWidth / 2, gridHeight / 2) + rectOffset;
            if (gridCell.x % 2 == 0)
            {
                return startPosition + new Vector3(gridCell.x * cellSize.x * 3 / 4 + gridCell.x * cellSpacing, gridCell.y * -(cellSize.y + cellSpacing) - cellSize.y / 2);
            }
            else
            {
                return startPosition + new Vector3(gridCell.x * cellSize.x * 3 / 4 + gridCell.x * cellSpacing, gridCell.y * -(cellSize.y + cellSpacing));

            }
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


       Hexagon InitializeHexagon(Vector2 gridCoordinates)//Use this function at the start of the game for initializing hexagons
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
                          
                            availableColors.Remove(GameController._instance.hexagonTypes[i].color);
                            continue;
                        }
                    }

                }
                
            }
           

            Color randomHexagonColor = availableColors[Random.Range(0, availableColors.Count)];

            return PoolController._instance.ReuseCell<Hexagon>(randomHexagonColor,gridCoordinates);

        }

        Hexagon SpawnHexagon(Vector2 gridCoordinates)//Use this function at the start of the game for initializing hexagons
        {
            List<Color> availableColors = new List<Color>();
            foreach (HexagonModel model in GameController._instance.hexagonTypes)
            { //Initialize available color list
                availableColors.Add(model.color);
            }
            var hexagon = PoolController._instance.ReuseCell<Hexagon>(availableColors[Random.Range(0, availableColors.Count)], gridCoordinates);
            InsertCellToGrid(gridCoordinates,hexagon,GetCellWorldPositionAtGrid(new Vector2(gridCoordinates.x,0))+ new Vector2(0,cellSpacing + cellSize.y));
            return hexagon;
        }





            public bool MatchingQuery()//Check grid for color matches
            {
            List<HexagonTrio> matchedTrios = new List<HexagonTrio>();

            foreach (HexagonTrio hexagonTrio in HexagonTrios)
            {
                if (hexagonTrio.CheckMatch())
                {
                    matchedTrios.Add(hexagonTrio);
                }
                    
            }

            if (matchedTrios.Count != 0)
            {
                foreach (HexagonTrio trio in matchedTrios)
                {
                    trio.DequeueHexagons();

                }
                _lastErosionCoroutine = StartCoroutine(ErosionCoroutine(matchedTrios));
                Debug.Log(_lastErosionCoroutine.ToString());

                return true;
            }

            return false;
        }

        IEnumerator ErosionCoroutine(List<HexagonTrio> matchedTrios) //
        {
            var emptyCells = GetEmptyCellsInGrid();
            yield return new WaitForSeconds(0.01f);

            foreach (Vector2 cellCoordinate in emptyCells) //Check every empty block for executing erosion
            {
                GameController._instance.interactable = false;

                bool stopUndergoErosion = false;
                for (int y = ((int)cellCoordinate.y+1 ); y < gridSize.y; y++)
                {
                    if (!gridCellData.ContainsKey(new Vector2(cellCoordinate.x, y)))
                    {
                        stopUndergoErosion = true;
                        break;
                    }
                }

                if (stopUndergoErosion)//If there is a empty cell at the bottom break this coroutine
                {
                    continue;
                }
                var willSpawnCells = 0;

                for (int y = ((int)cellCoordinate.y); y >= 0; y--) //calculate cell number to the top
                {
                    if (gridCellData.ContainsKey(new Vector2(cellCoordinate.x, y)))
                    {

                        for (int i = y+1; i < gridSize.y; i++)
                        {

                            if (gridCellData.ContainsKey(new Vector2(cellCoordinate.x, i)))
                            {
                                InsertCellToGrid(new Vector2(cellCoordinate.x, i-1), gridCellData[new Vector2(cellCoordinate.x, y)],GetCellWorldPositionAtGrid(new Vector2(cellCoordinate.x, y)));
                                ClearGridCell( (new Vector2(cellCoordinate.x, y)));
                                break;
                            }

                            if (i == gridSize.y -1) //If there is no cell exists, move this piece to bottom
                            {
                                InsertCellToGrid(new Vector2(cellCoordinate.x, i), gridCellData[new Vector2(cellCoordinate.x, y)], GetCellWorldPositionAtGrid(new Vector2(cellCoordinate.x, y)));
                                ClearGridCell((new Vector2(cellCoordinate.x, y)));
                            }
                            
                        }
                    }
                    else
                    {
                        willSpawnCells++; //Calculate the amount of hexagons will be spawned after erosion
                    }
                    yield return new WaitForSeconds(0.03f);

                }
                for (int i = willSpawnCells-1; i >= 0;i--)
                {
                    yield return new WaitForSeconds(0.08f);

                    var cell = SpawnHexagon(new Vector2(cellCoordinate.x, i));
                                        
                }

                
            }
            if (!MatchingQuery())
            {
                GameController._instance.interactable = true;
            }


        }

        public List<Vector2> GetEmptyCellsInGrid()
        {
            List<Vector2> emptyCells = new List<Vector2>();
            for (int x = 0; x < gridSize.x; x++)//Initialize trios
            {

                for (int y = 0; y < gridSize.y ; y++)
                {
                    if (!gridCellData.ContainsKey(new Vector2(x,y)))
                    {

                        emptyCells.Add(new Vector2(x, y));
                    }
                }
            }
            return emptyCells;
        }



    }
}
