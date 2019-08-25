using UnityEngine;
using System.Collections;
using System;
namespace Hexic.Models { 
[Serializable]
public class HexagonModel : CellModel
{
    public Color color;
    public HexagonModel(Vector2 _cellSize, Color _color) : base(_cellSize)
    {
        displayImage = (Texture2D)Resources.Load("Hexagon");
        color = _color;
        cellSize = _cellSize;
    }
    public override void OnExplode()
    {
        base.OnExplode();
    }
}
}
