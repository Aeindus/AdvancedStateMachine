using AsyncStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace TestStateMachine {
    [TestClass]
    public class ConstructionTest {
        private enum Trigger {
            LoadData
        }
        private enum State {
            Initial,
            Load,
            First
        }

        [TestMethod]
        public void Empty_Configuration_Constructor() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());
        }

        [TestMethod]
        public void Only_Permit_Configuration() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .Permit(Trigger.LoadData, State.Load);

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    return null;
                })
                .Permit(Trigger.LoadData, State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());
        }

        [TestMethod]
        public void Permit_And_Reconfiguration() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .Permit(Trigger.LoadData, State.Load);

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    return null;
                })
                .Permit(Trigger.LoadData, State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());
        }

        [TestMethod]
        public void One_State_Simple_Constructor() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    return null;
                });

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    return null;
                })
                .Permit(Trigger.LoadData, State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());
        }

        [TestMethod]
        public void One_State_Multiple_Declarations_Simple_Constructor() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .OnEntryAsync(async (transition, token) => {
                    return null;
                });

            config.Configure(State.Load)
              .OnEntryAsync(async (transition, token) => {
                  return null;
              });

            config.Configure(State.Initial)
              .OnEntryAsync(async (transition, token) => {
                  return null;
              });

            config.Configure(State.Initial)
              .OnEntryAsync(async (transition, token) => {
                  return null;
              })
              .Permit(Trigger.LoadData, State.Load);

            var machine = config.Build();

            Assert.IsTrue(machine.IsInState(State.Initial));
            Assert.IsFalse(machine.IsRunning());
        }
    }
}
