using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class StateConfiguration<TTrigger, TState>
        where TTrigger : struct
        where TState : struct {

        private readonly TState _state;
        private readonly StateController<TTrigger, TState> _stateController;

        public TState State => _state;

        public StateConfiguration(TState newState, StateController<TTrigger, TState> stateController) {
            _state = newState;
            _stateController = stateController;
        }

        public StateConfiguration<TTrigger, TState> OnEntryAsync(Func<Transition<TTrigger, TState>, CancellationToken, Task<TTrigger?>> function) {
            _stateController.AddEntryAction(function);
            return this;
        }
    }
}
