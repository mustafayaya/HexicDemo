using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hexic.Elements;

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
                for (int i = 0; i < GridController._instance.gridSize.x * GridController._instance.gridSize.y + GridController._instance.gridSize.x; i++) //Grid'deki toplam hücre sayısı kadar + bir rowdaki yatay hücre sayısı kadar hücre poolla
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
                cellPool.Add(CellType.Hexagon,new Queue<Cell>());
                PoolCells(_cellSize);
            }


                
        }


        public Hexagon ReuseHexagon(Color color)
        {

                    var _hex = (Hexagon)cellPool[CellType.Hexagon].Dequeue();
                    _hex.color = color;
                    _hex.OnReuse();
                    return _hex;

        }
    }
}
