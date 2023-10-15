using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class TriggerCondition<TTrigger, TState> {

    }

    public class StateMangement<TTrigger, TState> {
        public 

    }

    public class TriggerToStateTransition<TTrigger, TState> : Tuple<TriggerCondition<TTrigger, TState>, StateMangement<TTrigger, TState>> {
        public TriggerToStateTransition(TriggerCondition<TTrigger, TState> item1, TState item2) :
            base(item1, item2) {
        }
    }
}
