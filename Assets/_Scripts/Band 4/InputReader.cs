using UnityEngine;
using Sim.Foundation;
using System;

namespace Sim.View
{
    public class InputReader
    { 
        public void CalculateInput(InputContainer inputContainer)
        {
            Calculate(inputContainer);
        }

        private void Calculate(InputContainer inputContainer)
        {
            SimVec2 movementInput = new SimVec2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            var mouse = Input.mousePosition;
            mouse.z = MathF.Abs(Camera.main.transform.position.z);
            mouse = Camera.main.ScreenToWorldPoint(mouse);

            SimVec2 mousePosition = new SimVec2(mouse.x, mouse.y);
            inputContainer.Input = new InputFrame(movementInput, mousePosition);
        }
    }
}