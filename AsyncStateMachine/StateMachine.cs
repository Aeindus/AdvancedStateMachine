namespace AsyncStateMachine {


    public class StateMachine<TTrigger, TState>
        where TTrigger : class
        where TState : class {

        private readonly Dictionary<TState, List<TriggerToStateTransition<TTrigger, TState>>> _rules;

        public StateMachine(Dictionary<TState, List<TriggerToStateTransition<TTrigger, TState>>> rules) {
            _rules = rules;
        }
    }
}