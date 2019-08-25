using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexic.Models;
using Hexic.Elements;

namespace Hexic.Runtime
{


    public class HexagonGame : MonoBehaviour
    {
        [Header("Setup")]
        public Vector2 gridSize;
        public float cellSpacing;
        public Object cellPrefab;
        public RectTransform gridRectTransform;
        public Vector3 rectOffset;
        public ObjectPool objectPool;


        [Header("Game Settings")]
        public Vector2 cellSize = new Vector2(50, 43); //Cell size of explodables like hexagon, bomb etc...
        public List<HexagonModel> hexagonTypes = new List<HexagonModel>(); //Add hexagon types here

        [Header("Runtime")]
        Dictionary<int, Cell> gridCellData = new Dictionary<int, Cell>(); //Grid'de bulunan hücrelerin datalarını saklar

        private void Start()
        {
            PoolCells();
            Draw();
        }
        void Update()
        {

        }

        void PoolCells()
        {
            for (int i = 0; i < gridSize.x * gridSize.y + gridSize.x; i++) //Grid'deki toplam hücre sayısı kadar + bir rowdaki yatay hücre sayısı kadar hücre poolla
            {
                GameObject go = (GameObject)GameObject.Instantiate(cellPrefab, gridRectTransform.transform);
                var _cell = go.GetComponent<Cell>();
                objectPool.cellPool.Add(_cell);
                _cell.SetSize(cellSize);
                go.active = false;
            }
        }

        private void Draw() //Draw game area
        {
            var gridWidth = gridSize.x * cellSize.x * 3 / 4 -cellSize.x; // 3/4 should use for drawing hexagon pattern
            var gridHeight = gridSize.y * cellSize.y ; // 3/4 should use for drawing hexagon pattern

            Vector3 startPosition = new Vector3(-gridWidth / 2, gridHeight / 2) + rectOffset;
            
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    if (i % 2 == 0)
                    {
                        var _cell = objectPool.ReuseCell();
                        _cell.GetComponent<RectTransform>().localPosition = startPosition + new Vector3(i * cellSize.x * 3/4 , j * -cellSize.y - cellSize.y /2);
                    }
                    else
                    {
                        var _cell = objectPool.ReuseCell();
                        _cell.GetComponent<RectTransform>().localPosition = startPosition + new Vector3(i * cellSize.x * 3/4, j * -cellSize.y);

                    }
                }
            }
        }
    }
}