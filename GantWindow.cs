using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ProjektManager {
    public class PhaseNode { // Binary tree to save the structure of the Projekt Phases.

        public Phase Phase { get; set; }
        public PhaseNode Sibling { get; set; }
        public PhaseNode Child { get; set; }

        public PhaseNode(Phase phase) {
            Phase   = phase;
            Child   = this;
            Sibling = this;
        }
        public bool HasChild => Child != this;
        public bool HasSibling => Sibling != this;
    }

    public class PhaseTree {
        public PhaseNode RootNode;
        public PhaseTree(Phase phase) {
            RootNode = new PhaseNode(phase);
        }

        /*
        *  Description
        *    Searches the node of the precursing phase and adds the phase as its child.
        *  
        *  Return Value
        *    -1 Error: Phase not added.
        *     0 Success: Phase added.
        */
        public int Insert(Phase phase) {
            if(phase.Precursor == '-') {
                RootNode.Sibling = new PhaseNode(phase);
                return 0;
            } 
            return InsertRecursive(RootNode, phase);
        }

        private int InsertRecursive(PhaseNode Node, Phase phase) {
            PhaseNode NewNode;
            int r;

            if(Node.Phase.Number == phase.Precursor) {
                if (Node.HasChild) {                         // Precursing node already has a child? => Add the phase as sibling to the child node.
                    Node = Node.Child;
                    NewNode = new PhaseNode(phase);
                    Node.Sibling = NewNode;
                    return 0;
                } else {                                     // Precursing node not has a child? => Add the phase as child node.
                    NewNode = new PhaseNode(phase);
                    Node.Child = NewNode;
                    return 0;
                }
            }
            if (Node.HasSibling) {
               r = InsertRecursive(Node.Sibling, phase);
               if (r == 0) {
                   return 0;
               }
            }
            if (Node.HasChild) {
                r = InsertRecursive(Node.Child, phase);
                if (r == 0) {
                    return 0;
                }
            }
            return -1;
        }
    }

    public partial class GantWindow : Window {
        public ObservableCollection<Phase> Phases { get; set; }
        public PhaseTree? BinTree = null;
        public GantWindow (ObservableCollection<Phase> _Phases) {
            InitializeComponent();
            Phases = _Phases;
            CrateTree();
            DrawGanttChart(GanttCanvas);
        }

        private void CrateTree() {

            foreach (var phase in Phases) {
                if (phase.Precursor == '-') {
                    if (BinTree == null) {
                        BinTree = new PhaseTree(phase);
                    } else {
                        BinTree.Insert(phase);
                    }
                }
            }
            if (BinTree == null) {
                string caption = "Error";
                string messageBoxText = "First Phase not found!";
                MessageBox.Show(messageBoxText, caption);
                return;
            }
            int InsertCnt;
            do { 
                InsertCnt = 0;
                foreach (var phase in Phases) {
                    if(phase.Inserted == 0) {
                        int r = BinTree.Insert(phase);
                        if (r == 0) {
                            InsertCnt++;
                            phase.Inserted = 1;
                        }
                    }
                }
            } while (InsertCnt != 0);
            if (!AllInserted()) {
                string caption        = "Error";
                string messageBoxText = "Precursor of phase not found!";
                MessageBox.Show(messageBoxText, caption);
                return;
            }
            return;
        }

        private bool AllInserted() {
            foreach (var phase in Phases) {
                if(phase.Inserted == 0) {
                    return false;
                }
            }
            return true;
        }

        private void DrawGanttChart(Canvas canvas) {
            if (BinTree == null || BinTree.RootNode == null) {
                return;
            }
            canvas.Children.Clear(); // Clear existing content on the canvas
            double xOffset = 50; // Starting X position
            double yOffset = 50; // Starting Y position
            double barHeight = 20;
            double verticalSpacing = 30;
            double horizontalSpacing = 50;
            RenderPhaseNode(canvas, BinTree.RootNode, xOffset, yOffset, barHeight, horizontalSpacing, verticalSpacing);  // Render the Gantt Chart recursively
        }

        private double RenderPhaseNode(Canvas canvas, PhaseNode node, double xOffset, double yOffset, double barHeight, double horizontalSpacing, double verticalSpacing) {
            if (node == null) {
                return yOffset;
            }
            // Draw the current node
            double barWidth = node.Phase.Duration * 10; // Scale duration to pixel width
            Rectangle rect = new Rectangle {
                Width = barWidth,
                Height = barHeight,
                Fill = Brushes.Blue,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            TextBlock text = new TextBlock {
                Text = $"Phase {node.Phase.Number} ({node.Phase.Duration}d)",
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(rect, xOffset);
            Canvas.SetTop(rect, yOffset);
            Canvas.SetLeft(text, xOffset + barWidth + 5);
            Canvas.SetTop(text, yOffset);
            canvas.Children.Add(rect);
            canvas.Children.Add(text);
            double nextY = yOffset + verticalSpacing;
            // Render child phases
            if (node.HasChild) {
                nextY = RenderPhaseNode(canvas, node.Child, xOffset + barWidth, nextY, barHeight, horizontalSpacing, verticalSpacing);
            }
            // Render sibling phases
            if (node.HasSibling) {
                nextY = RenderPhaseNode(canvas, node.Sibling, xOffset, nextY, barHeight, horizontalSpacing, verticalSpacing);
            }
            return nextY;
        }
    }
}

