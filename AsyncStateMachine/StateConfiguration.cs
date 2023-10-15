using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class StateConfiguration<TTrigger, TState> {
        private readonly TState _state;

        public StateConfiguration(TState newState) {
            _state = newState;
        }

        public StateConfiguration<TTrigger, TState> OnEntryAsync(Func<TState, TTrigger, CancellationToken, TState> function) {
            return this;
        }
    }
}
