using AsyncStateMachine.Exceptions;

namespace AsyncStateMachine {
    public class StateMachine<TTrigger, TState>
        where TTrigger : struct
        where TState : struct {

        private enum InternalState {
            Idle = 1,
            Transitioning = 2,
            Pumping = 4
        }

        private class TransitionResult {
            public bool Transitioned { get; set; }
            public TTrigger? NextTrigger { get; set; }
        }

        private class PatternData {
            public PatternExpresion<TState> Expression { get; set; }
            public TTrigger Trigger { get; set; }
            public TaskCompletionSource CompletionSource { get; set; }
        }

        private TState _currentState;
        private readonly Dictionary<TState, StateController<TTrigger, TState>> _rules;
        private InternalState _internalState = InternalState.Idle;
        private List<PatternData> _patterns;

        public StateMachine(TState initialState, Dictionary<TState, StateController<TTrigger, TState>> rules) {
            _currentState = initialState;
            _rules = rules;
            _patterns = new List<PatternData>();
        }


        /// <summary>
        /// Runs a pump until the machine stabilizes.
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="transitionCompleted">Pass a task source to be signaled when first transition finishes</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task InternalPump(TTrigger trigger, TaskCompletionSource<bool> transitionCompleted, CancellationToken token) {
            TransitionResult result;

            _internalState |= InternalState.Pumping;

            try {
                result = await InternalTransitionSingle(trigger, token);

                transitionCompleted.SetResult(result.Transitioned);
            } catch (Exception ex) {
                transitionCompleted.SetException(ex);

                _internalState = InternalState.Idle;
                throw;
            }

            if (!result.Transitioned) {
                _internalState = InternalState.Idle;
                return;
            }

            try {
                // We need to pump triggers on success
                while (true) {
                    var matchedPattern = MatchPattern();

                    if (matchedPattern != null) {
                        _patterns.Remove(matchedPattern);

                        result = await InternalTransitionSingle(matchedPattern.Trigger, CancellationToken.None);

                        matchedPattern.CompletionSource.SetResult();
                    } else {
                        if (result.NextTrigger == null) break;

                        var nextTrigger = result.NextTrigger;

                        result = await InternalTransitionSingle(nextTrigger.Value, CancellationToken.None);

                        if (!result.Transitioned) break;
                    }
                }
            } finally {
                _internalState = InternalState.Idle;
            }
        }
        private async Task<TransitionResult> InternalTransitionSingle(TTrigger trigger, CancellationToken token) {
            if (!_rules.TryGetValue(_currentState, out var stateController))
                throw new MissingConfigurationException();

            // This can be cancelled and will throw
            var nextState = await stateController.GetNextState(trigger, token);

            if (nextState == null)
                return new TransitionResult() { Transitioned = false };

            _internalState |= InternalState.Transitioning;
            try {
                // This can be cancelled and will throw
                var nextTrigger = await EnterNewState(trigger, nextState.Value, token);

                _currentState = nextState.Value;

                return new TransitionResult() {
                    Transitioned = true,
                    NextTrigger = nextTrigger
                };
            } finally {
                _internalState ^= InternalState.Transitioning;
                _internalState |= InternalState.Idle;
            }
        }

        private async Task<TTrigger?> EnterNewState(TTrigger trigger, TState newState, CancellationToken token) {
            if (!_rules.TryGetValue(_currentState, out var stateController))
                throw new MissingConfigurationException();

            var transition = new Transition<TTrigger, TState>(_currentState, trigger, newState);
            return await stateController.EnterAsync(transition, token);
        }

        /// <summary>
        /// Advances all patterns and returns the first matching one.
        /// In case of multiple matches all others are reset.
        /// </summary>
        private PatternData? MatchPattern() {
            PatternData? matchingExpression = null;

            foreach (var pattern in _patterns) {
                var result = pattern.Expression.Advance(_currentState);

                if (result) {
                    if (matchingExpression != null) {
                        matchingExpression = pattern;
                    } else {
                        pattern.Expression.Reset();
                    }
                }
            }

            return matchingExpression;
        }

        public bool IsInState(TState state) {
            if (!_internalState.HasFlag(InternalState.Idle)) return false;
            return _currentState.Equals(state);
        }
        public bool IsRunning() {
            return _internalState.HasFlag(InternalState.Pumping);
        }


        public async Task<bool> Fire(TTrigger trigger, CancellationToken token) {
            if (IsRunning())
                return false;

            var transitionCompleted = new TaskCompletionSource<bool>();

            InternalPump(trigger, transitionCompleted, token);

            return await transitionCompleted.Task;
        }

        /// <summary>
        /// Apply the trigger when the given pattern of states is found
        /// </summary>
        public async Task Match(List<TState> statesPattern, TTrigger trigger) {
            var patternData = new PatternData() {
                Expression = new PatternExpresion<TState>(statesPattern),
                Trigger = trigger,
                CompletionSource = new TaskCompletionSource()
            };

            _patterns.Add(patternData);

            await patternData.CompletionSource.Task;
        }
    }
}