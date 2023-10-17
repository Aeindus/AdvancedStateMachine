using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedStateMachine {
    public class StateController<TTrigger, TState>
            where TTrigger : struct
            where TState : struct {

        private readonly TState _state;
        private Func<Transition<TTrigger, TState>, CancellationToken, Task<TTrigger?>>? _entryFunction;
        private Dictionary<TTrigger, List<TriggerCondition<TState>>> _triggers;

        public StateController(TState state) {
            _state = state;
            _triggers = new Dictionary<TTrigger, List<TriggerCondition<TState>>>();
        }

        public void AddEntryAction(Func<Transition<TTrigger, TState>, CancellationToken, Task<TTrigger?>> function) {
            _entryFunction = function;
        }

        public void AddTransition(TTrigger trigger, TState newState) {
            InternalAddTransition(trigger, new TriggerCondition<TState>(newState));
        }

        public void AddTransition(TTrigger trigger, TState newState, Func<CancellationToken, Task<bool>> checkFunction) {
            InternalAddTransition(trigger, new TriggerCondition<TState>(newState, checkFunction));
        }

        private void InternalAddTransition(TTrigger trigger, TriggerCondition<TState> condition) {
            if (!_triggers.TryGetValue(trigger, out var triggerConditions)) {
                triggerConditions = new List<TriggerCondition<TState>>();
                _triggers.Add(trigger, triggerConditions);
            }

            triggerConditions.Add(condition);
        }

        public async Task<TState?> GetNextState(TTrigger trigger, CancellationToken token) {
            if (!_triggers.ContainsKey(trigger)) return null;

            var conditions = _triggers[trigger];

            foreach (var condition in conditions) {
                var canTransition = await condition.CanTransition(token);

                if (!canTransition) continue;
                return condition.NewState;
            }

            return null;
        }

        public async Task<TTrigger?> EnterAsync(Transition<TTrigger, TState> transition, CancellationToken actionToken) {
            if (_entryFunction != null)
                return await _entryFunction(transition, actionToken);

            return null;
        }
    }
}
