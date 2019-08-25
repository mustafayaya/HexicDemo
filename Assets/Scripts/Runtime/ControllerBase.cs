using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hexic.Runtime
{
    [RequireComponent(typeof(GameController))]
    [RequireComponent(typeof(PoolController))]
    public class ControllerBase : MonoBehaviour
    {
        public GameController gameController;
        public PoolController poolController;

        public void Start()
        {
            gameController = GameController._instance;
            poolController = PoolController._instance;

        }

    }


}
