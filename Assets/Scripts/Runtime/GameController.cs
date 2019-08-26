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
            GridController._instance.InitializeGrid();
        }
        void Update()
        {

        }
        float counter;
 


    }
    
}