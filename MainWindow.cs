using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ProjektManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Projects = new ObservableCollection<Project>();
            Employees = new ObservableCollection<Employee>();

            // Sample data for demonstration
            Projects.Add(new Project { ProjectID = 1, Name = "Analysis", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), ResponsibleEmployee = "John Doe" });
            Projects.Add(new Project { ProjectID = 2, Name = "Design", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(2), ResponsibleEmployee = "Jane Smith" });

            dataGridProjects.ItemsSource = Projects;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Employee clicked");
            // Code to open add employee window/dialog
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit Employee clicked");
            // Code to open edit employee window/dialog
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Project clicked");
            // Code to open add project window/dialog
        }

        private void btnEditProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit Project clicked");
            // Code to open edit project window/dialog
        }

        private void btnViewGanttChart_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Gantt Chart clicked");
            // Code to generate and display Gantt chart
        }
    }

    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ResponsibleEmployee { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }
    }
}
