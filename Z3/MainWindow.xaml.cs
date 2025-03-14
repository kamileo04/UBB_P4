using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private DataSet dataSet;
        private string tableName = "Employees";
        private string connectionString = "Data Source=KAMIL_REDMIBOOK;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=True";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeData()
        {
            conn = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);


            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);

            dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);
        }
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            InitializeData();
            dataGrid.ItemsSource = dataSet.Tables[tableName].DefaultView;
        }
        private void InsertData(string firstname, string lastname)
        {
            DataRow newRow = dataSet.Tables[tableName].NewRow();
            newRow["FirstName"] = firstname;
            newRow["Lastname"] = lastname;

            dataSet.Tables[tableName].Rows.Add(newRow);
            adapter.Update(dataSet, tableName);

            LoadData_Click(null, null);
        }
        private void InsertData_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Wprowadź imię:", "Wprowadź nazwisko:");
            if (inputDialog.ShowDialog() == true)
            {
                InsertData(inputDialog.FirstName, inputDialog.LastName);
            }
        }

        private void UpdateData(int id, string newfirstName, string newlastName)
        {
            DataRow[] rows = dataSet.Tables[tableName].Select($"EmployeeID = {id}");
            if (rows.Length > 0)
            {
                rows[0]["FirstName"] = newfirstName;
                rows[0]["LastName"] = newlastName;
                adapter.Update(dataSet, tableName);

                LoadData_Click(null, null);
            }
        }
        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Proszę zaznaczyć wiersz do aktualizacji.");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            int employeeId = Convert.ToInt32(selectedRow["EmployeeID"]);

            var inputDialog = new InputDialog("Wprowadź nowe imię:", "Wprowadź nowe nazwisko:");
            if (inputDialog.ShowDialog() == true)
            {
                UpdateData(employeeId, inputDialog.FirstName, inputDialog.LastName);
            }
        }

        private void DeleteData(int id)
        {
            DataRow[] rows = dataSet.Tables[tableName].Select($"EmployeeID = {id}");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                adapter.Update(dataSet, tableName);

                LoadData_Click(null, null);
            }
        }
        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Proszę zaznaczyć wiersz do usunięcia.");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            int employeeId = Convert.ToInt32(selectedRow["EmployeeID"]);

            MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć wiersz o ID: {employeeId}?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                DeleteData(employeeId);
            }
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            adapter.Update(dataSet, tableName);
            MessageBox.Show("Changes saved!");
        }
    }



    public class InputDialog : Window
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private TextBox firstNameTextBox;
        private TextBox lastNameTextBox;

        public InputDialog(string prompt1, string prompt2)
        {
            Title = "Wprowadź dane";
            Width = 300;
            Height = 200;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            StackPanel mainPanel = new StackPanel();
            mainPanel.Margin = new Thickness(10);

            Label firstNameLabel = new Label() { Content = prompt1 };
            firstNameTextBox = new TextBox();
            Label lastNameLabel = new Label() { Content = prompt2 };
            lastNameTextBox = new TextBox();


            Button okButton = new Button() { Content = "OK", Margin = new Thickness(0, 10, 0, 0) };
            okButton.Click += OkButton_Click;

            mainPanel.Children.Add(firstNameLabel);
            mainPanel.Children.Add(firstNameTextBox);
            mainPanel.Children.Add(lastNameLabel);
            mainPanel.Children.Add(lastNameTextBox);
            mainPanel.Children.Add(okButton);

            Content = mainPanel;
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            FirstName = firstNameTextBox.Text;
            LastName = lastNameTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
