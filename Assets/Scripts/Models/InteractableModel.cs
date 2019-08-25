using UnityEngine;
using System.Collections;
using System;

namespace Hexic.Models
{
    [Serializable]
    public class InteractableModel  : IInteractable
    {
        IInteractable interactable;
        public Vector2 cellSize;

        public InteractableModel(Vector2 _cellSize)
        {
            cellSize = _cellSize;
        }
        public InteractableModel(Vector2 _cellSize,IInteractable _interactable)
        {
            interactable = _interactable;
            cellSize = _cellSize;
        }


        public void OnClick()
        {
            interactable.OnClick();
        }
    }
    public interface IInteractable {
         void OnClick();
    }

    
}

