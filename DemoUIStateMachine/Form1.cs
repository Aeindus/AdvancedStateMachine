using AdvancedStateMachine;
using System.ComponentModel;
using System.Security.Cryptography.Xml;

namespace DemoUIStateMachine {
    public partial class Form1 : Form {
        enum ViewState {
            Unlock,
            Lock
        }

        enum State {
            Initial,

            Loading,
            Loaded,

            Input1Changed
        }
        enum Trigger {
            Load,
            FinishLoad,

            ChangeInput1
        }

        class Model {
            public int Id { get; set; }
            public string Name { get; set; }
        }


        private StateMachine<Trigger, State> _machine;
        private BindingList<Model> _input1Source = new BindingList<Model>();
        private BindingList<Model> _input2Source = new BindingList<Model>();
        private BindingList<Model> _resultSource = new BindingList<Model>();


        public Form1() {
            InitializeComponent();
            BindControls();
        }

        private void BindControls() {
            cmbInput1.DataSource = _input1Source;
            cmbInput2.DataSource = _input2Source;
            cmbResult.DataSource = _resultSource;
        }

        private void CreateUIMachine() {
            var config = new MachineConfiguration<Trigger, State>(State.Initial);

            config.Configure(State.Initial)
                .Permit(Trigger.Load, State.Loading);

            config.Configure(State.Loading)
                .OnEntryAsync(StateLoading)
                .Permit(Trigger.FinishLoad, State.Loaded);

            config.Configure(State.Loaded)
                .Permit(Trigger.ChangeInput1, State.Input1Changed);

            config.Configure(State.Input1Changed)
                .OnEntryAsync(StateInput1Changed);

            _machine = config.Build();
        }

        private void UpdateView(State oldState, State newState, ViewState viewState) {
            switch (newState) {
                case State.Loading: {
                    if (viewState == ViewState.Lock) {
                        cmbInput1.Enabled = false;
                        cmbInput2.Enabled = false;
                        cmbResult.Enabled = false;
                        btnSearch.Enabled = false;
                    } else {
                        cmbInput1.Enabled = true;
                        cmbInput2.Enabled = true;
                        btnSearch.Enabled = true;
                    }
                    break;
                }

                case State.Input1Changed: {
                    if (viewState == ViewState.Lock) {
                        cmbInput1.Enabled = false;
                        cmbInput2.Enabled = false;
                        cmbResult.Enabled = false;
                        btnSearch.Enabled = false;
                    } else {
                        cmbInput1.Enabled = true;
                        cmbInput2.Enabled = true;
                        btnSearch.Enabled = true;
                    }
                    break;
                }
            }
        }

        private async Task<Trigger?> StateLoading(Transition<Trigger, State> transition, CancellationToken token) {
            UpdateView(transition.SourceState, transition.DestinationState, ViewState.Lock);

            var data = await GetInputData();

            foreach (var item in data)
                _input1Source.Add(item);

            UpdateView(transition.SourceState, transition.DestinationState, ViewState.Unlock);

            return Trigger.FinishLoad;
        }
        private async Task<Trigger?> StateInput1Changed(Transition<Trigger, State> transition, CancellationToken token) {
            UpdateView(transition.SourceState, transition.DestinationState, ViewState.Lock);

            _input2Source.Clear();

            var input1Item = (Model)cmbInput1.SelectedItem;

            if (input1Item != null) {
                var data = await GetInputData(input1Item);

                foreach (var item in data)
                    _input2Source.Add(item);
            }

            UpdateView(transition.SourceState, transition.DestinationState, ViewState.Unlock);

            return null;
        }

        private async Task<List<Model>> GetInputData() {
            await Task.Delay(1000);
            return new List<Model>() {
                new Model(){Id=1,Name="a" },
                new Model(){Id=2,Name="b" },
                new Model(){Id=3,Name="c" },
                new Model(){Id=4,Name="d" },
                new Model(){Id=5,Name="e" },
                new Model(){Id=6,Name="f" },
            };
        }
        private async Task<List<Model>> GetInputData(Model reference) {
            await Task.Delay(1000);
            return new List<Model>() {
                new Model(){Id=1, Name=1+"-"+reference.Name},
                new Model(){Id=2, Name=2+"-"+reference.Name},
                new Model(){Id=3, Name=3+"-"+reference.Name},
                new Model(){Id=4, Name=4+"-"+reference.Name},
                new Model(){Id=5, Name=5+"-"+reference.Name},
                new Model(){Id=6, Name=6+"-"+reference.Name}
            };
        }
        private async Task<List<Model>> SearchData(Model item1, Model item2) {
            await Task.Delay(1000);
            return new List<Model>() {
                new Model(){Id=1, Name=item1.Name },
                new Model(){Id=2, Name=item2.Name}
            };
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await _machine.Fire(Trigger.Load, CancellationToken.None);
        }

        private async void cmbInput1_SelectedIndexChanged(object sender, EventArgs e) {
            await _machine.Fire(Trigger.ChangeInput1, CancellationToken.None);
        }
    }
}