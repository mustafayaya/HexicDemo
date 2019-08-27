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

        public Vector2 hexagon1GridCoordinates;
        public Vector2 hexagon2GridCoordinates;
        public Vector2 hexagon3GridCoordinates;

        private Hexagon hexagon1 {
            get { return (Hexagon)GridController._instance.gridCellData[hexagon1GridCoordinates]; }
        }
        private Hexagon hexagon2
        {
            get { return (Hexagon)GridController._instance.gridCellData[hexagon2GridCoordinates]; }
        }
        private Hexagon hexagon3
        {
            get { return (Hexagon)GridController._instance.gridCellData[hexagon3GridCoordinates]; }
        }

        public bool turnTrigger;
        public bool selected;

        public Vector2 center;

        public HexagonTrio(Hexagon _h1, Hexagon _h2, Hexagon _h3)
        {

            hexagon1GridCoordinates = _h1.gridCoordinates;
            hexagon2GridCoordinates = _h2.gridCoordinates;
            hexagon3GridCoordinates = _h3.gridCoordinates;

            Vector3 equilibrium1 = (_h1.transform.position + ((_h2.transform.position - _h3.transform.position) / 2));

            center = equilibrium1+((_h3.transform.position - equilibrium1 ) / 2);
        }

        public bool CheckMatch()//Check colors, if they are all the same return true
        {
            var _h1 = (Hexagon)GridController._instance.gridCellData[hexagon1GridCoordinates];//Get cells from grid data
            var _h2 = (Hexagon)GridController._instance.gridCellData[hexagon2GridCoordinates];
            var _h3 = (Hexagon)GridController._instance.gridCellData[hexagon3GridCoordinates];

            if (_h1.color.Equals(_h2.color) && _h1.color.Equals(_h3.color) && _h2.color.Equals(_h3.color))
            {
                _h1.image.color = Color.grey;
                _h2.image.color = Color.grey;
                _h3.image.color = Color.grey;
                return true;
            }

            return false;
        }


        public bool DequeueHexagons()//dequeue, dissappear and remove hexagons at this trio
        {
            if (GridController._instance.gridCellData.ContainsKey(hexagon1GridCoordinates))
            {
                var _h1 = (Hexagon)GridController._instance.gridCellData[hexagon1GridCoordinates];//Get cells from grid data
                GridController._instance.gridCellData.Remove(hexagon1GridCoordinates);
                _h1.OnExplode();


            }
            if (GridController._instance.gridCellData.ContainsKey(hexagon2GridCoordinates))
            {
                var _h2 = (Hexagon)GridController._instance.gridCellData[hexagon2GridCoordinates];//Get cells from grid data
                GridController._instance.gridCellData.Remove(hexagon2GridCoordinates);
                _h2.OnExplode();


            }
            if (GridController._instance.gridCellData.ContainsKey(hexagon3GridCoordinates))
            {
                var _h3 = (Hexagon)GridController._instance.gridCellData[hexagon3GridCoordinates];//Get cells from grid data
                GridController._instance.gridCellData.Remove(hexagon3GridCoordinates);
                _h3.OnExplode();


            }

            return true;
        }

        Coroutine lastSelectedHighlightCoroutine;

       public void SelectedHighlight()//Start highlight coroutine
        {

            GameManager._instance.lastHighlightCoroutine = GameManager._instance.StartCoroutine(GameManager._instance.selectedHexagonTrio.HighlightCoroutine());
        }

        IEnumerator HighlightCoroutine()
        {
            var _h1 = (Hexagon)GridController._instance.gridCellData[hexagon1GridCoordinates];//Get cells from grid data
            var _h2 = (Hexagon)GridController._instance.gridCellData[hexagon2GridCoordinates];
            var _h3 = (Hexagon)GridController._instance.gridCellData[hexagon3GridCoordinates];
            var elapsedTime1 = 0f;
            var elapsedTime2 = 0f;

            while (elapsedTime1 < 1f)
            {
                _h1.image.color = Color.Lerp(GameManager._instance.interactableDefaultColorBlock.normalColor, GameManager._instance.interactableDefaultColorBlock.highlightedColor, (elapsedTime1 / 1));
                _h2.image.color = Color.Lerp(GameManager._instance.interactableDefaultColorBlock.normalColor, GameManager._instance.interactableDefaultColorBlock.highlightedColor, (elapsedTime1 / 1));
                _h3.image.color = Color.Lerp(GameManager._instance.interactableDefaultColorBlock.normalColor, GameManager._instance.interactableDefaultColorBlock.highlightedColor, (elapsedTime1 / 1));

                elapsedTime1 += 3 * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }

            while (elapsedTime2 < 1f)
            {
                _h1.image.color = Color.Lerp( GameManager._instance.interactableDefaultColorBlock.highlightedColor, GameManager._instance.interactableDefaultColorBlock.normalColor, (elapsedTime2 / 1));
                _h2.image.color = Color.Lerp(GameManager._instance.interactableDefaultColorBlock.highlightedColor, GameManager._instance.interactableDefaultColorBlock.normalColor,(elapsedTime2 / 1));
                _h3.image.color = Color.Lerp( GameManager._instance.interactableDefaultColorBlock.highlightedColor, GameManager._instance.interactableDefaultColorBlock.normalColor,(elapsedTime2 / 1));

                elapsedTime2 += 3 * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }



            GameManager._instance.lastHighlightCoroutine = GameManager._instance.StartCoroutine(GameManager._instance.selectedHexagonTrio.HighlightCoroutine());


        }

        public IEnumerator TurnHexagonTrio(bool clockwise) //Turns clockwise 1 unit
        {
            turnTrigger = false;

            var elapsedTime = 0f;


            var _h1 = (Hexagon)GridController._instance.gridCellData[hexagon1GridCoordinates];//Get cells from grid data
            var _h2 = (Hexagon)GridController._instance.gridCellData[hexagon2GridCoordinates];
            var _h3 = (Hexagon)GridController._instance.gridCellData[hexagon3GridCoordinates];



          

            _h1.interactable = false;
            _h2.interactable = false;
            _h3.interactable = false;

            if (clockwise )
            {

                var startingPos = _h1.transform.position;
                var startingPos2 = _h2.transform.position;
                var startingPos3 = _h3.transform.position;
                while (elapsedTime < 2)
                {

                    _h1.transform.position = Vector3.Lerp(startingPos, startingPos2, (elapsedTime / 2));//Turn cells
                    _h2.transform.position = Vector3.Lerp(startingPos2, startingPos3, (elapsedTime / 2));
                    _h3.transform.position = Vector3.Lerp(startingPos3, startingPos, (elapsedTime / 2));

                    elapsedTime += GameManager._instance.swipeAnimationSpeed * Time.deltaTime;
                    yield return new WaitForSeconds(0.01f);
                }

                _h1.transform.position = startingPos2;
                _h2.transform.position = startingPos3;
                _h3.transform.position = startingPos;

                GridController._instance.InsertCellToGrid(hexagon1GridCoordinates, _h3);//Insert new states to grid data
                GridController._instance.InsertCellToGrid(hexagon2GridCoordinates, _h1);
                GridController._instance.InsertCellToGrid(hexagon3GridCoordinates, _h2);
                turnTrigger = true;
            }
            if (!clockwise )
            {
                var startingPos = _h1.transform.position;
                var startingPos2 = _h2.transform.position;
                var startingPos3 = _h3.transform.position;
                while (elapsedTime < 2)
                {

                    _h1.transform.position = Vector3.Lerp(startingPos, startingPos3, (elapsedTime / 2));//Turn cells
                    _h2.transform.position = Vector3.Lerp(startingPos2, startingPos, (elapsedTime / 2));
                    _h3.transform.position = Vector3.Lerp(startingPos3, startingPos2, (elapsedTime / 2));

                    elapsedTime += GameManager._instance.swipeAnimationSpeed * Time.deltaTime;
                    yield return new WaitForSeconds(0.01f);
                }

                _h1.transform.position = startingPos3;
                _h2.transform.position = startingPos;
                _h3.transform.position = startingPos2;

                GridController._instance.InsertCellToGrid(hexagon1GridCoordinates, _h2);//Insert new states to grid data
                GridController._instance.InsertCellToGrid(hexagon2GridCoordinates, _h3);
                GridController._instance.InsertCellToGrid(hexagon3GridCoordinates, _h1);
                turnTrigger = true;
            }

            _h1.interactable = true;
            _h2.interactable = true;
            _h3.interactable = true;

        }

        public bool HasLegalMoves()
        {
            if (GridController._instance.GetMatchableColor(hexagon1GridCoordinates) == hexagon2.color || GridController._instance.GetMatchableColor(hexagon1GridCoordinates) == hexagon3.color)
            {
                return true;
            }
            if (GridController._instance.GetMatchableColor(hexagon2GridCoordinates) == hexagon1.color || GridController._instance.GetMatchableColor(hexagon1GridCoordinates) == hexagon3.color)
            {
                return true;
            }
            if (GridController._instance.GetMatchableColor(hexagon3GridCoordinates) == hexagon2.color || GridController._instance.GetMatchableColor(hexagon1GridCoordinates) == hexagon1.color)
            {
                return true;
            }
            return false;

        }

    }
}
