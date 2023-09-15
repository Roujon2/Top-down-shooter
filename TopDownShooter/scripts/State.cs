using Godot;
using System;

namespace StateMachine{
    public class State : Node
    {
        public Action<float> OnProcess { get; set; }
        public Action<float> OnPhysicsProcess { get; set; }
        public Action OnEnter { get; set; }
        public Action OnExit { get; set; }
        public Action<InputEvent> OnInput { get; set; }

        public StateManager StateManager { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}