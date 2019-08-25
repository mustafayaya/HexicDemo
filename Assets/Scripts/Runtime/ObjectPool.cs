using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hexic.Elements;

namespace Hexic.Runtime
{

public class ObjectPool : MonoBehaviour
{
    public List<Cell> cellPool = new List<Cell>();

        public Cell ReuseCell()
        {
            var _cell= cellPool[0];
            _cell.OnReuse();
            cellPool.RemoveAt(0);
            cellPool.Add(_cell);
            return _cell;
        }
    }
}
