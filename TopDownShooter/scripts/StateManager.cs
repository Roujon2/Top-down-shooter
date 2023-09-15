using Godot;
using System.Collections.Generic;

namespace StateMachine
{
    public class StateManager : Node
    {

        // Dictionary of states
        private readonly Dictionary<string, State> states = new Dictionary<string, State>();

        private State CurrentState{get; set;}
        private State InitialState{get; set;}


        // When the state manager enters
        public override void _Ready()
        {
            // Loops through the children states
            foreach (var child in GetChildren())
            {
                if (!(child is State state)) continue;
                
                // Sets the iniial state as the first state that is found (it shouldn't matter as it' changed later)
                if (states.Count == 0)
                {
                InitialState = state;
                }

                // Updates states dictionary and the reference of the states statemanager
                states[state.Name] = state;
                state.StateManager = this;
            }

        }


        // State manager's process
        public override void _Process(float delta)
        {

            if (states.Count == 0 || InitialState == null)
            {
                GD.PrintErr("State machine has no states!");
                return;
            }

            if (CurrentState == null)
            {
                ChangeState(InitialState);
            }

            CurrentState.OnProcess?.Invoke(delta);
        }


        // State manager's physics process
        public override void _PhysicsProcess(float delta)
        {
            
            if (states.Count == 0 || InitialState == null)
            {
                GD.PrintErr("State machine has no states!");
                return;
            }

            if (CurrentState == null)
            {
                ChangeState(InitialState);
            }

            CurrentState.OnPhysicsProcess?.Invoke(delta);
        }


        // Change state
        private void ChangeState(State newState)
        {
            if (newState == null)
            {
                GD.PrintErr("Cannot transition to a null state!");
                return;
            }

            // Invoke the states on exit method
            CurrentState?.OnExit?.Invoke();

            CurrentState = newState;
            
            // Invokes the new state
            newState.OnEnter?.Invoke();
        }

        
        // Same as the change state but with the states name
        public void ChangeState(string name)
        {
            if (states.ContainsKey(name) == false)
            {
                GD.PrintErr($"State {name} does not exist!");
                return;
            }

            ChangeState(states[name]);
        }

        public void UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);

            // Sends any unhandled input to the state's OnInput function
            CurrentState?.OnInput?.Invoke(@event);
        }
    }
}
