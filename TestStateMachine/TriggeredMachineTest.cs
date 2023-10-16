using AsyncStateMachine;

namespace TestStateMachine {
    [TestClass]
    public class TriggeredMachineTest {
        private enum Trigger {
            LoadData,
            UndefinedTrigger
        }
        private enum State {
            Initial,
            Load,
            First
        }


        private StateMachine<Trigger, State> _machine;

        [TestInitialize]
        public void Initialize() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    await Task.Delay(100);
                    return null;
                })
                .Permit(Trigger.LoadData, State.Load);

            _machine = config.Build();
        }

        [TestMethod]
        public async Task Fire_Trigger() {
            var result = await _machine.Fire(Trigger.LoadData, CancellationToken.None);

            Assert.IsTrue(result);
            Assert.IsTrue(_machine.IsInState(State.Load));
            Assert.IsTrue(_machine.IsRunning(), "Implementation detail but should stay constant: the machine still works because the continuation was executed immediatelly");
        }

        [TestMethod]
        public async Task Fire_Undefined_Trigger() {
            var result = await _machine.Fire(Trigger.UndefinedTrigger, CancellationToken.None);

            Assert.IsFalse(result);
            Assert.IsTrue(_machine.IsInState(State.Initial));
            Assert.IsFalse(_machine.IsRunning());
        }

        [TestMethod]
        public async Task Fire_With_Cancelled_Token() {
            var cts = new CancellationTokenSource(5);
            var result = await _machine.Fire(Trigger.UndefinedTrigger, cts.Token);

            Assert.IsFalse(result);
            Assert.IsTrue(_machine.IsInState(State.Initial));
            Assert.IsFalse(_machine.IsRunning());
        }
    }
}