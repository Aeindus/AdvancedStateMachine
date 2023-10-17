using AsyncStateMachine;

namespace TestStateMachine {
    [TestClass]
    public class TriggeredMachineTest {
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


        private StateMachine<Trigger, State> _machine;

        [TestInitialize]
        public void Initialize() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .Permit(Trigger.LoadData, State.Load);

            config.Configure(State.Load)
                .OnEntryAsync(async (transition, token) => {
                    await Task.Delay(50);
                    token.ThrowIfCancellationRequested();
                    return null;
                })
                .Permit(Trigger.LoadFirst, State.First);

            config.Configure(State.First)
                .OnEntryAsync(async (transition, token) => {
                    await Task.Delay(50);
                    token.ThrowIfCancellationRequested();
                    return null;
                });

            _machine = config.Build();
        }




        [TestMethod]
        public async Task Fire_Trigger() {
            var result = await _machine.Fire(Trigger.LoadData, CancellationToken.None);

            Assert.IsTrue(result);
            Assert.IsTrue(_machine.IsInState(State.Load));
            Assert.IsTrue(_machine.IsRunning(), "Implementation detail but should stay constant: the machine still pumps because the continuation was executed immediatelly. " +
                "This happens when the trigger condition or entry function contain some async code");

            await Task.Delay(70);

            Assert.IsFalse(_machine.IsRunning(), "Implementation detail but should stay constant: the machine should have finished pumping by now");
        }

        [TestMethod]
        public async Task Fire_With_Cancelled_Token() {
            var cts = new CancellationTokenSource(5);
            var result = _machine.Fire(Trigger.LoadData, cts.Token);

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => result);

            Assert.IsTrue(_machine.IsInState(State.Initial));
            Assert.IsTrue(_machine.IsRunning(), "Implementation detail - in this case it should sill be pumping");
            await Task.Delay(2);
            Assert.IsFalse(_machine.IsRunning(), "Implementation detail - in this case it should not be pumping");
        }

        [TestMethod]
        public async Task Fire_With_Two_Triggers() {
            var result1 = await _machine.Fire(Trigger.LoadData, CancellationToken.None);

            Assert.IsTrue(result1);
            Assert.IsTrue(_machine.IsInState(State.Load));
            Assert.IsTrue(_machine.IsRunning(), "Implementation detail - in this case it should sill be pumping");

            await Task.Delay(70);
            Assert.IsFalse(_machine.IsRunning(), "Implementation detail");

            var result2 = await _machine.Fire(Trigger.LoadFirst, CancellationToken.None);

            Assert.IsTrue(result2);
            Assert.IsTrue(_machine.IsInState(State.First));
            Assert.IsTrue(_machine.IsRunning(), "Implementation detail - in this case it should sill be pumping");

            await Task.Delay(70);
            Assert.IsFalse(_machine.IsRunning(), "Implementation detail");
        }





        [TestMethod]
        public async Task Fire_Undefined_Trigger() {
            var result = await _machine.Fire(Trigger.UndefinedTrigger, CancellationToken.None);

            Assert.IsFalse(result);
            Assert.IsTrue(_machine.IsInState(State.Initial));
            Assert.IsFalse(_machine.IsRunning());
        }

        [TestMethod]
        public async Task Fire_Undefined_With_Cancelled_Token() {
            var cts = new CancellationTokenSource(5);
            var result = await _machine.Fire(Trigger.UndefinedTrigger, cts.Token);

            Assert.IsFalse(result);
            Assert.IsTrue(_machine.IsInState(State.Initial));
            Assert.IsFalse(_machine.IsRunning());
        }
    }
}