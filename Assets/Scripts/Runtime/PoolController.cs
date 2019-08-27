using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hexic.Elements;
using System;

namespace Hexic.Runtime
{

public class PoolController : MonoBehaviour
{
        public static PoolController _instance 
        {
            get
            {
                return FindObjectOfType<PoolController>();
            }
            set
            {
                _instance = FindObjectOfType<PoolController>();
            }
        }


        public enum CellType
        {
            Hexagon,
            Bomb
        }

        public Dictionary<CellType, Queue<Cell>> cellPool = new Dictionary<CellType, Queue<Cell>>();


        public void PoolCells(Vector2 _cellSize) //Initialize pooling here at the start of the game
        {
            
            if (cellPool.ContainsKey(CellType.Hexagon))
            {
                var queue = cellPool[CellType.Hexagon];
                for (int i = 0; i < GridController._instance.gridSize.x * GridController._instance.gridSize.y + GridController._instance.gridSize.x; i++) //Grid'deki toplam hücre sayısının iki katı kadar obje poolla
                {
                    GameObject go = (GameObject)GameObject.Instantiate(GridController._instance.hexagonPrefab, GridController._instance.gridRectTransform.transform);
                    var _cell = go.GetComponent<Hexagon>();
                    queue.Enqueue(_cell);
                    _cell.SetSize(_cellSize);
                    go.SetActive(false);
                }
            }
            else
            {
                cellPool.Add(CellType.Hexagon, new Queue<Cell>());
                PoolCells(_cellSize);
            }
            if (cellPool.ContainsKey(CellType.Bomb))
            {
                var queue = cellPool[CellType.Bomb];
                for (int i = 0; i < GridController._instance.gridSize.x; i++) //Pool bombs
                {
                    GameObject go = (GameObject)GameObject.Instantiate(GridController._instance.hexagonBombPrefab, GridController._instance.gridRectTransform.transform);
                    var _cell = go.GetComponent<Hexagon>();
                    queue.Enqueue(_cell);
                    _cell.SetSize(_cellSize);
                    go.SetActive(false);
                }
            }
            else
            {
                cellPool.Add(CellType.Bomb, new Queue<Cell>());
                PoolCells(_cellSize);
            }
        }


        public T ReuseCell<T>(Color color, Vector2 _gridCoordinates) where T : Cell
        {
            Type cellType = typeof(T);
            Cell cell = null;
            if (cellType == typeof(Hexagon))
            {
                cell = (Cell)cellPool[CellType.Hexagon].Dequeue();
                cellPool[CellType.Hexagon].Enqueue(cell);
            }
            if (cellType == typeof(HexagonBomb))
            {
                cell = (Cell)cellPool[CellType.Bomb].Dequeue();
                cellPool[CellType.Bomb].Enqueue(cell);
            }


            if (cell.gameObject.activeSelf)//return new hexagon if object is using
                      {
                       return ReuseCell<T>(color, _gridCoordinates);
                      }

                      ((Hexagon)cell).image.color = Color.white;

                       ((Hexagon)cell).color = color;
                  ((Hexagon)cell).colors = UnityEngine.UI.ColorBlock.defaultColorBlock;

                   ((Hexagon)cell).gridCoordinates = _gridCoordinates;

                      cell.OnReuse();
                    return cell as T;

        }
    }
}
