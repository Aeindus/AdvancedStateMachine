﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine.Exceptions {
    public class MissingConfigurationException : Exception {
        public MissingConfigurationException() : base("Missing state or trigger configuration") {
        }
    }
}
