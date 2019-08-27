using Hexic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Hexic.Elements
{
    public class HexagonBomb : Hexagon
    {
        public Text countdownText;
        public float countdown;

        public override void OnReuse()
        {
            base.OnReuse();
            countdown = 10f;
           
        }
        public override void OnExplode()
        {
            base.OnExplode();
        }

        public virtual void Countdown()
        {
            countdown--;
            countdownText.text = countdown.ToString();
            if (countdown == 0)
            {
                GameManager._instance.FinishGame();
            }
            
        }
    }
}
