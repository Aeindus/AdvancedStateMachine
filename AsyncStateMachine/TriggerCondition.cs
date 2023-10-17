using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedStateMachine {
    public class TriggerCondition<TState> {
        private readonly TState _newState;
        private Func<CancellationToken, Task<bool>>? _checkFunction;

        public TState NewState => _newState;

        public TriggerCondition(TState newState) {
            _newState = newState;
        }

        public TriggerCondition(TState newState, Func<CancellationToken, Task<bool>> checkFunction) {
            _newState = newState;
            _checkFunction = checkFunction;
        }

        public async Task<bool> CanTransition(CancellationToken token) {
            if (_checkFunction == null) return true;
            return await _checkFunction(token);
        }
    }
}
