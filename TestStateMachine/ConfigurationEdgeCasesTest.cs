using AsyncStateMachine;
using AsyncStateMachine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStateMachine {
    [TestClass]
    public class ConfigurationEdgeCasesTest {
        private enum Trigger {
            LoadData,
            LoadFirst,
            UndefinedTrigger
        }
        private enum State {
            Initial,
            Load,
            First,
            Second
        }

        [TestMethod]
        public async Task Test_Missing_Configuration_For_Initial_State() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);
            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());

            await Assert.ThrowsExceptionAsync<MissingConfigurationException>(async () => {
                await machine.Fire(Trigger.LoadData, CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Test_Missing_Configuration_For_Destination_State() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .Permit(Trigger.LoadData, State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());

            await Assert.ThrowsExceptionAsync<MissingConfigurationException>(async () => {
                await machine.Fire(Trigger.LoadData, CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Test_Simplest_Configuration_For_Destination_State() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);
            
            config.Configure(State.Initial)
                .Permit(Trigger.LoadData, State.Load);
            config.Configure(State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());

            Assert.IsTrue(await machine.Fire(Trigger.LoadData, CancellationToken.None));

            Assert.IsTrue(machine.IsInState(State.Load));
            Assert.IsFalse(machine.IsRunning());
        }
    }
}
