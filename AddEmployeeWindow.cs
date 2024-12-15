using ProjektManager;
using System.Windows;

namespace ProjektManager {
    public partial class AddEmployeesWindow : Window {
        public  Employee Emp;
        public  int      Result = 1;
        private bool     IsEditDialog;

        public AddEmployeesWindow (bool _IsEditDialog, Employee _Emp) {
            InitializeComponent();
            IsEditDialog = _IsEditDialog;
            Emp          = _Emp;
            Result       = 0;
            if (IsEditDialog) {
                Title = "Edit Employee";
                btnDel.Visibility = Visibility.Visible;
            } else {
                Title = "Add Employee";
                btnDel.Visibility = Visibility.Hidden;
            }
            txtName.Text       = Emp.Name;
            txtFirstname.Text  = Emp.FirstName;
            txtDepartment.Text = Emp.Department;
            txtID.Text         = Convert.ToString(Emp.EmployeeID);
            txtPhone.Text      = Emp.Phone;
        }

        private void btnSave_Click (object sender, RoutedEventArgs e) {
            int _EmployeeID = 0;
            int Error       = 0;
            // Validate input
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstname.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                string.IsNullOrWhiteSpace(txtID.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text)) {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try {
                _EmployeeID = Convert.ToInt32(txtID.Text);
            } catch (Exception) {
                MessageBox.Show("Please enter a valid ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Error = 1;
            }
            if (Error > 0) {
                return;
            }
            Emp.Name       = txtName.Text;
            Emp.FirstName  = txtFirstname.Text;
            Emp.Department = txtDepartment.Text;
            Emp.EmployeeID = _EmployeeID;
            Emp.Phone      = txtPhone.Text;
            Result = 1;
            Close();
        }
        private void btnDel_Click (object sender, RoutedEventArgs e) {
            Result = 2;
            Close();
        }
    }
}
