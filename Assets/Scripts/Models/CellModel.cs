using UnityEngine;
using System.Collections;


namespace Hexic.Models
{

    public class CellModel : InteractableModel
    {
        public Texture2D displayImage;

        public CellModel(Vector2 _cellSize) : base(_cellSize)
        {
            cellSize = _cellSize;

        }

        public virtual void OnExplode()
        {

        }
    }
}