using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DisconnectedMode
{public partial class MainWindow : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
        }
        DataSet set = new DataSet();
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            string authorName = txtAuthorName.Text;
            if (!string.IsNullOrEmpty(authorName))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("INSERT INTO Authors (AuthorName) VALUES (@AuthorName)", connection);
                        command.Parameters.AddWithValue("@AuthorName", authorName);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Author inserted successfully!");
                            txtAuthorName.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter an author name.");
            }
        }


        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Authors", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(set, "authors");
                    lstAuthors.ItemsSource = set.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedAuthor = lstAuthors.SelectedItem as DataRowView;
            if (selectedAuthor != null)
            {
                int authorId = (int)selectedAuthor["Id"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("DELETE FROM Authors WHERE Id = @Id", connection);
                        command.Parameters.AddWithValue("@Id", authorId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Author deleted successfully!");
                            lstAuthors.ItemsSource = null;
                            lstAuthors.Items.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an author to delete.");
            }
        }
    }
}
