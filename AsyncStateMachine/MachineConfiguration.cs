using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class MachineConfiguration<TTrigger, TState>
        where TTrigger : struct
        where TState : struct {

        private readonly TState _initialState;
        private readonly Dictionary<TState, StateController<TTrigger, TState>> _stateControllers;


        public MachineConfiguration(TState initialState) {
            _initialState = initialState;
            _stateControllers = new Dictionary<TState, StateController<TTrigger, TState>>();
        }

        private StateController<TTrigger, TState> GetStateController(TState state) {
            if (!_stateControllers.TryGetValue(state, out var manager)) {

                manager = new StateController<TTrigger, TState>(state);
                _stateControllers.Add(state, manager);
            }

            return manager;
        }


        public StateConfiguration<TTrigger, TState> Configure(TState newState) {
            var stateController = GetStateController(newState);
            return new StateConfiguration<TTrigger, TState>(newState, stateController);
        }

        public StateMachine<TTrigger, TState> Build() {
            return new StateMachine<TTrigger, TState>(_initialState, _stateControllers);
        }
    }
}
