using Hexic.Elements;
using System;
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
    }
}
