using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjektManager {
    public partial class MainWindow : Window {
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        private bool IsEmpActiveList = false;

        public MainWindow () {
            InitializeComponent();
            Projects = new ObservableCollection<Project>();
            Employees = new ObservableCollection<Employee>();

            // Sample data for demonstration
            Projects.Add(new Project { ProjectID = 1, Name = "Analysis", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), EmpID = 1 });
            Projects.Add(new Project { ProjectID = 2, Name = "Design", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(2), EmpID = 2 });
            Projects[0].Phases.Add(new Phase());
            Projects[0].Phases.Add(new Phase());
            Projects[0].Phases.Add(new Phase());
            Projects[0].Phases.Add(new Phase());
            Projects[0].Phases[0] = new Phase();
            Projects[0].Phases[0].Duration  = 10;
            Projects[0].Phases[0].Name      = "Phase 1";
            Projects[0].Phases[0].Precursor = '-';
            Projects[0].Phases[0].Number    = 'A';

            Projects[0].Phases[1] = new Phase();
            Projects[0].Phases[1].Duration  = 15;
            Projects[0].Phases[1].Name      = "Phase 2";
            Projects[0].Phases[1].Precursor = 'A';
            Projects[0].Phases[1].Number    = 'B';

            Projects[0].Phases[2] = new Phase();
            Projects[0].Phases[2].Duration  = 20;
            Projects[0].Phases[2].Name      = "Phase 3";
            Projects[0].Phases[2].Precursor = 'B';
            Projects[0].Phases[2].Number    = 'C';

            Projects[0].Phases[2] = new Phase();
            Projects[0].Phases[2].Duration  = 5;
            Projects[0].Phases[2].Name      = "Phase 4";
            Projects[0].Phases[2].Precursor = 'B';
            Projects[0].Phases[2].Number    = 'D';

            Employees.Add(new Employee { EmployeeID = 1, Name = "Peter", FirstName = "Hans", Department = "HR", Phone = "0125642" });
            Employees.Add(new Employee { EmployeeID = 1, Name = "Frank", FirstName = "Heinz", Department = "Development", Phone = "0981237" });

            dataGridProjects.ItemsSource = Projects;
            dataGridEmployees.ItemsSource = Employees;
        }

        private void btnShowEmployees_Click (object sender, RoutedEventArgs e) {
            dataGridProjects.Visibility  = Visibility.Hidden;
            dataGridEmployees.Visibility = Visibility.Visible;
            listName.Text                = "Employee List";
            btnViewGanttChart.Visibility = Visibility.Hidden;
            IsEmpActiveList = true;
        }

        private void btnShowProjects_Click (object sender, RoutedEventArgs e) {
            dataGridEmployees.Visibility = Visibility.Hidden;
            dataGridProjects.Visibility  = Visibility.Visible;
            listName.Text                = "Project List";
            btnViewGanttChart.Visibility = Visibility.Visible;
            IsEmpActiveList = false;
        }

        private void btnAdd_Click (object sender, RoutedEventArgs e) {
            if (IsEmpActiveList) {
                Employee emp = new Employee();
                AddEmployeesWindow addEmp = new AddEmployeesWindow(false, emp);
                addEmp.ShowDialog();
                if (addEmp.Result == 1) {
                    Employees.Add(emp);
                }
            } else {
                Project proj = new Project();
                AddProjectWindow addProj = new AddProjectWindow(false, proj, proj.Phases);
                addProj.ShowDialog();
                if (addProj.Result == 1) {
                    Projects.Add(proj);
                    proj.Phases = addProj.Phases;
                }
            }
        }

        private void btnEdit_Click (object sender, RoutedEventArgs e) {
            if (IsEmpActiveList) {
                var selectedItems = dataGridEmployees.SelectedItems;
                if (selectedItems == null || selectedItems.Count == 0) {
                    return;
                }
                int rowIndex = dataGridEmployees.Items.IndexOf(dataGridEmployees.SelectedItem);
                if (rowIndex < 0 || rowIndex >= Employees.Count) {
                    return;
                }
                Employee selectedEmployee = Employees[rowIndex];
                AddEmployeesWindow addEmp = new AddEmployeesWindow(true, new Employee(selectedEmployee));
                addEmp.ShowDialog();
                if (addEmp.Result == 1) {
                    Employees[rowIndex] = addEmp.Emp;
                } else if(addEmp.Result == 2) {            // User clicked delete? => Remove employee.
                    Employees.RemoveAt(rowIndex);
                }
            } else {
                var selectedItems = dataGridProjects.SelectedItems;
                if (selectedItems == null || selectedItems.Count == 0) {
                    return;
                }
                int rowIndex = dataGridProjects.Items.IndexOf(dataGridProjects.SelectedItem);
                if (rowIndex < 0 || rowIndex >= Projects.Count) {
                    return;
                }
                Project                     selectedProject = Projects[rowIndex];
                ObservableCollection<Phase> copyPhases      = new ObservableCollection<Phase>(selectedProject.Phases.Select(phase => phase.Clone()));
                AddProjectWindow            addProj         = new AddProjectWindow(true, new Project(selectedProject), copyPhases);
                addProj.ShowDialog();
                if (addProj.Result == 1) {
                    addProj.Proj.Phases = addProj.Phases;
                    Projects[rowIndex]  = addProj.Proj;
                } else if (addProj.Result == 2) {            // User clicked delete? => Remove project.
                    Projects.RemoveAt(rowIndex);
                }
            }
        }

        private void btnViewGanttChart_Click (object sender, RoutedEventArgs e) {
            var selectedItems = dataGridProjects.SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0) {
                return;
            }
            int rowIndex = dataGridProjects.Items.IndexOf(dataGridProjects.SelectedItem);
            if (rowIndex < 0 || rowIndex >= Projects.Count) {
                return;
            }
            Project selectedProject = Projects[rowIndex];

            var window = new GantWindow(selectedProject.Phases);
            window.Show();
        }
    }

    public class Project {
        public int      ProjectID                 { get; set; }
        public string   Name                      { get; set; }
        public DateTime StartDate                 { get; set; }
        public DateTime EndDate                   { get; set; }
        public int      EmpID                     { get; set; }
        public ObservableCollection<Phase> Phases { get; set; }

        public Project () {
            ProjectID = 0;
            Name      = string.Empty;
            StartDate = DateTime.Now;
            EndDate   = DateTime.Now;
            EmpID     = 0;
            Phases    = new ObservableCollection<Phase>();
        }

        public Project (Project project) {
            ProjectID = project.ProjectID;
            Name      = project.Name;
            StartDate = project.StartDate;
            EndDate   = project.EndDate;
            EmpID     = project.EmpID;
            Phases    = new ObservableCollection<Phase>();
        }
    }

    public class Phase {
        private char _number;
        private char _precursor;
        public string Name       { get; set; }
        private int   _duration  { get; set; }

        public int Duration {
            get {
                return _duration;
            }
            set {
                if (value > 0 && value < 0xFFFF) {
                    _duration = value;
                } 
            }
        }

        public char Number {
            get {
                return _number;
            }
            set {
                if (value >= 'a' && value <= 'z' ) {
                    _number = Convert.ToChar(value.ToString().ToUpper());
                } else if (value >= 'A' && value <= 'Z') { 
                    _number = value;
                }
            }
        }

        public char Precursor {
            get {
                return _precursor;
            }
            set {
                if (value == '-') {
                    _precursor = value;
                    return;
                }
                if (value >= 'a' && value <= 'z') {
                    _precursor = Convert.ToChar(value.ToString().ToUpper());
                } else if (value >= 'A' && value <= 'Z') {
                    _precursor = value;
                }
            }
        }

        public Phase () {
            _number   = 'A';
            Name      = "Phase1";
            _duration  = 1;
            _precursor = '-';
        }

        public Phase (Phase phase) {
            _duration  = phase.Duration;
            _number    = phase.Number;
            Name       = phase.Name;
            _precursor = phase.Precursor;
        }

        public Phase Clone () {
            return new Phase {
                Precursor = this.Precursor,
                Name      = this.Name,
                Duration  = this.Duration,
                Number    = this.Number,
            };
        }
    }

    public class Employee {
        public int    EmployeeID { get; set; }
        public string Name       { get; set; }
        public string FirstName  { get; set; }
        public string Department { get; set; }
        public string Phone      { get; set; }

        public Employee () {
            EmployeeID = 0;
            Name       = string.Empty;
            FirstName  = string.Empty;
            Department = string.Empty;
            Phone      = string.Empty;
        }

        public Employee (Employee employee) {  // Copy constructor.
            EmployeeID = employee.EmployeeID;
            Name       = employee.Name;
            FirstName  = employee.FirstName;
            Department = employee.Department;
            Phone      = employee.Phone;
        }
    }
}
