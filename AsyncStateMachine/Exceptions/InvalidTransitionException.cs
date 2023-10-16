using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine.Exceptions {
    internal class InvalidTransitionException : Exception {
        public InvalidTransitionException() : base("Cannot transition") { }
    }
}
