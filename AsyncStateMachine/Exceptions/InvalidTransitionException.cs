using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedStateMachine.Exceptions {
    internal class InvalidTransitionException : Exception {
        public InvalidTransitionException() : base("Cannot transition") { }
    }
}
