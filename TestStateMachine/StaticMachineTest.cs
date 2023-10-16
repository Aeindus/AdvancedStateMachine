using AsyncStateMachine;

namespace TestStateMachine {
    [TestClass]
    public class UnitTest1 {
        private enum Trigger {
            LoadData
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

                    return null;
                })
                .Permit(Trigger.LoadData, State.Load);

            config.Configure(State.Load)
                .Permit(Trigger.LoadData, State.Load);

            _machine = config.Build();
        }

        [TestMethod]
        public void Initial_State_Should_Be_Initial() {
            Assert.IsTrue(_machine.IsInState(State.Initial));
        }

        [TestMethod]
        public void Initial_State_Should_Not_Be_Running() {
            Assert.IsFalse(_machine.IsRunning());
        }
    }
}