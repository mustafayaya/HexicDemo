using Hexic.Elements;
using Hexic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hexic.Models
{
   public class HexagonTrio
    {
        public Hexagon h1;
        public Hexagon h2;
        public Hexagon h3;

        public Vector2 h1GridCoordinates;
        public Vector2 h2GridCoordinates;
        public Vector2 h3GridCoordinates;


        public Vector2 center;

        public HexagonTrio(Hexagon _h1, Hexagon _h2, Hexagon _h3)
        {
            h1 = _h1;
            h2 = _h2;
            h3 = _h3;
            h1GridCoordinates = h1.gridCoordinates;
            h2GridCoordinates = h2.gridCoordinates;
            h3GridCoordinates = h3.gridCoordinates;

            Vector3 equilibrium1 = (h1.transform.position + ((h2.transform.position - h1.transform.position) / 2));

            center = equilibrium1+((h3.transform.position - equilibrium1 ) / 2);
        }

       public IEnumerator TurnClockwise() //Turns clockwise 1 unit
        {
            var elapsedTime = 0f;
        
            var _h1 = (Hexagon)GridController._instance.gridCellData[h1GridCoordinates];//Get cells from grid data
            var _h2 = (Hexagon)GridController._instance.gridCellData[h2GridCoordinates];
            var _h3 = (Hexagon)GridController._instance.gridCellData[h3GridCoordinates];
            _h1.interactable = false;
            _h2.interactable = false;
            _h3.interactable = false;
            GameController._instance.interactable = false;

            var startingPos = _h1.transform.position;
            var startingPos2 = _h2.transform.position;
            var startingPos3 = _h3.transform.position;
            while (elapsedTime < 2)
            {
                Debug.Log(elapsedTime);

                _h1.transform.position = Vector3.Lerp(startingPos, startingPos2, (elapsedTime / 2));//Turn cells
                _h2.transform.position = Vector3.Lerp(startingPos2, startingPos3, (elapsedTime / 2));
                _h3.transform.position = Vector3.Lerp(startingPos3, startingPos, (elapsedTime / 2));

                elapsedTime +=  GameController._instance.swipeAnimationSpeed* Time.deltaTime;
                yield return new WaitForSeconds(0.05f);
            }

            _h1.transform.position = startingPos2;
            _h2.transform.position = startingPos3;
            _h3.transform.position = startingPos;

            GridController._instance.InsertCellToGrid(h1GridCoordinates, _h3);//Insert new states to grid data
            GridController._instance.InsertCellToGrid(h2GridCoordinates, _h1);
            GridController._instance.InsertCellToGrid(h3GridCoordinates, _h2);
            _h1.interactable = true;
            _h2.interactable = true;
            _h3.interactable = true;
            GameController._instance.interactable = true;

        }


    }
}
