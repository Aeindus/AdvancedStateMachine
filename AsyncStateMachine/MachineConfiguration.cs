using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class MachineConfiguration<TTrigger, TState>
        where TTrigger : class
        where TState : class {

        private readonly List<StateConfiguration<TTrigger, TState>> _stateConfigurations =
            new List<StateConfiguration<TTrigger, TState>>();


        public MachineConfiguration() {

        }

        public StateConfiguration<TTrigger, TState> Configure(TState newState) {
            var temp = new StateConfiguration<TTrigger, TState>(newState);

            _stateConfigurations.Add(temp);
            return temp;
        }

        public StateMachine<TTrigger, TState> Build() {
            throw new NotImplementedException();
        }
    }
}
