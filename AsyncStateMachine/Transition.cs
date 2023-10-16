using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class Transition<TTrigger, TState> {
        private readonly TState _sourceState;
        private readonly TTrigger _trigger;
        private readonly TState _destinationState;

        #region Properties
        public TState SourceState => _sourceState;
        public TTrigger Trigger => _trigger;
        public TState DestinationState => _destinationState;
        #endregion

        public Transition(TState sourceState, TTrigger trigger, TState destinationState) {
            _sourceState = sourceState;
            _trigger = trigger;
            _destinationState = destinationState;
        }
    }
}
