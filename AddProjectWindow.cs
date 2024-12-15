using ProjektManager;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;

namespace ProjektManager {
    public partial class AddProjectWindow : Window {
        public  Project Proj;
        public  int     Result = 1;
        private bool    IsEditDialog;
        public ObservableCollection<Phase> Phases;

        public AddProjectWindow (bool _IsEditDialog, Project _Proj, ObservableCollection<Phase> _Phases) {
            InitializeComponent();
            IsEditDialog = _IsEditDialog;
            Proj         = _Proj;
            Result       = 0;
            Phases       = _Phases;
            if (IsEditDialog) {
                Title = "Edit Project";
                btnDel.Visibility = Visibility.Visible;
            } else {
                Title = "Add Project";
                btnDel.Visibility = Visibility.Hidden;
                Phases.Add(new Phase());
            }
            txtName.Text  = Proj.Name;
            txtID.Text    = Convert.ToString(Proj.ProjectID);
            txtEmpID.Text = Convert.ToString(Proj.EmpID);
            txtEnd.Text   = Convert.ToString(Proj.EndDate);
            txtStart.Text = Convert.ToString(Proj.StartDate);
            dataGridPhases.ItemsSource = Phases;
        }

        private void btnSave_Click (object sender, RoutedEventArgs e) {
            int _ProjectID = 0;
            int _EmpID     = 0;
            int Error      = 0;
            // Validate input
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtID.Text   ) ||
                string.IsNullOrWhiteSpace(txtEnd.Text  ) ||
                string.IsNullOrWhiteSpace(txtEmpID.Text) ||
                string.IsNullOrWhiteSpace(txtStart.Text)) {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try {
                _ProjectID = Convert.ToInt32(txtID.Text);
                _EmpID     = Convert.ToInt32(txtEmpID.Text);
            } catch (Exception) {
                MessageBox.Show("Please enter a valid ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Error = 1;
            }
            if (Error > 0) {
                return;
            }
            DateTime Start = DateTime.MinValue;
            DateTime End   = DateTime.MinValue;
            try {
                Start = Convert.ToDateTime(txtStart.Text);
                End   = Convert.ToDateTime(txtEnd.Text);
            } catch (Exception) {
                MessageBox.Show("Please enter a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Error = 1;
            }
            if (Error > 0) {
                return;
            }
            if (Start.CompareTo(End) >= 0) {
                MessageBox.Show("Start date must be earlier that the end date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Proj.Name      = txtName.Text;
            Proj.ProjectID = _ProjectID;
            Proj.EmpID     = _EmpID;
            Proj.StartDate = Start;
            Proj.EndDate   = End;
            Result = 1;
            Close();
        }
        private void btnDel_Click (object sender, RoutedEventArgs e) {
            Result = 2;
            Close();
        }
    }
}
