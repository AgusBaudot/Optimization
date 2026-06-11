namespace Sim.Foundation
{
    public struct InputFrame
    {
        //Readonly enforce that a frame is immutable once created. Immutable mid-frame. 
        public readonly SimVec2 MovementInput;
        public readonly SimVec2 MousePosition;

        public InputFrame(SimVec2 movementInput, SimVec2 mousePosition)
        {
            MovementInput = movementInput;
            MousePosition = mousePosition;
        }
    }
}