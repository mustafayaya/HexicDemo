using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hexic.Elements
{
    public class Hexagon : Cell
    {
        public Color color;

        public override void OnReuse()
        {
            base.OnReuse();
            var colorblock = colors;
            
            colors = new UnityEngine.UI.ColorBlock() { normalColor = color,disabledColor = colorblock.disabledColor*color,highlightedColor = colorblock.highlightedColor * color,pressedColor = colorblock.pressedColor * color ,selectedColor = colorblock.selectedColor * color,colorMultiplier = colorblock.colorMultiplier, fadeDuration = colorblock.fadeDuration};
        }
    }
}
