namespace AsyncStateMachine {
    public class StateMachine<TTrigger, TState>
        where TTrigger : struct
        where TState : struct {

        private readonly Dictionary<TState, StateController<TTrigger, TState>> _rules;

        public StateMachine(Dictionary<TState, StateController<TTrigger, TState>> rules) {
            _rules = rules;
        }
    }
}