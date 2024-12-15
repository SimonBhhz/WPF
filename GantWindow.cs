using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjektManager {

    public class PhasesStruct {
        private ObservableCollection<PhasesStruct> phases;
        public Phase Phase;

        public PhasesStruct (Phase phase) {
            phases = new ObservableCollection<PhasesStruct> ();
            Phase  = phase;
        }

        public void AddPhase (PhasesStruct cPhases) {
            phases.Add(cPhases);
        }
    }

    //public class GantDiagram { 
    //    private CPhases phases;
    //
    //    public GantDiagram(CPhases _phases) {
    //        phases = _phases;
    //    }
    //
    //}

    public partial class GantWindow : Window {
        public ObservableCollection<Phase> Phases { get; set; }
        public PhasesStruct phasesStruct;

        public GantWindow (ObservableCollection<Phase> _Phases) {
            InitializeComponent();
            Phases = _Phases;
            RenderGanttChart();
        }

        private void RenderGanttChart () {
            foreach (var phase in Phases) {
                if(phase.Precursor == '-') {
                    phasesStruct = new PhasesStruct(phase);
                }
            }

            foreach (var phase in Phases) {
                if(phase.Precursor == phasesStruct.Phase.Number) {
                    Phases.Add(phase);
                }
            }
        }
    }
}
