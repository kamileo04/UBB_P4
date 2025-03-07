using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

public class Customer
{
    public string CustomerID { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
}

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Customer> GetAllCustomers()
    {
        List<Customer> customers = new List<Customer>();
        string sql = "SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country, Phone, Fax FROM Customers";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var customer = new Customer
                        {
                            CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                            ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? null : reader.GetString(reader.GetOrdinal("ContactName")),
                            ContactTitle = reader.IsDBNull(reader.GetOrdinal("ContactTitle")) ? null : reader.GetString(reader.GetOrdinal("ContactTitle")),
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                            PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader.GetString(reader.GetOrdinal("PostalCode")),
                            Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                            Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax"))
                        };
                        customers.Add(customer);
                    }
                }
            }
        }
        return customers;
    }


    public string AddCustomer(Customer customer)
    {
        string newCustomerId = "";
        string query = @"
            INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country, Phone, Fax) 
            VALUES (@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @PostalCode, @Country, @Phone, @Fax);
        ";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                command.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PostalCode", customer.PostalCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Fax", customer.Fax ?? (object)DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
                newCustomerId = customer.CustomerID;
            }
        }
        return newCustomerId;
    }



    public void UpdateCustomer(Customer customer)
    {
        string query = @"
            UPDATE Customers 
            SET CompanyName = @CompanyName, 
                ContactName = @ContactName, 
                ContactTitle = @ContactTitle, 
                Address = @Address, 
                City = @City, 
                PostalCode = @PostalCode, 
                Country = @Country, 
                Phone = @Phone, 
                Fax = @Fax 
            WHERE CustomerID = @CustomerID";

        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                command.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PostalCode", customer.PostalCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Fax", customer.Fax ?? (object)DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


    public Customer GetCustomerById(string customerId)
    {
        Customer customer = null;
        string sql = @"SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country, Phone, Fax 
                       FROM Customers WHERE CustomerID = @CustomerID";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                            ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? null : reader.GetString(reader.GetOrdinal("ContactName")),
                            ContactTitle = reader.IsDBNull(reader.GetOrdinal("ContactTitle")) ? null : reader.GetString(reader.GetOrdinal("ContactTitle")),
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                            PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader.GetString(reader.GetOrdinal("PostalCode")),
                            Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                            Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax"))
                        };
                    }
                }
            }
        }
        return customer;
    }


    public void RemoveCustomerById(string customerId)
    {
        string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Data Source=DESKTOP-UGQIB7C;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=True";
        var dbHelper = new DatabaseHelper(connectionString);

        List<Customer> customers = dbHelper.GetAllCustomers();
        Console.WriteLine("Klienci przed zmianami:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"CustomerID: {customer.CustomerID}, CompanyName: {customer.CompanyName}");
        }

        var newCustomer = new Customer
        {
            CustomerID = "TEST6",
            CompanyName = "Firma Testowa",
            ContactName = "Jan Nowak",
            ContactTitle = "Manager",
            Address = "123 Test Street",
            City = "Testville",
            PostalCode = "12345",
            Country = "Testland",
            Phone = "123-456-7890",
            Fax = "098-765-4321"
        };
        string addedCustomerId = dbHelper.AddCustomer(newCustomer);
        Console.WriteLine($"\nDodano klienta z CustomerID: {addedCustomerId}");

        var retrievedCustomer = dbHelper.GetCustomerById(addedCustomerId);
        if (retrievedCustomer != null)
        {
            Console.WriteLine($"\nPobrano klienta: CompanyName: {retrievedCustomer.CompanyName}, ContactName: {retrievedCustomer.ContactName}");
        }

        retrievedCustomer.CompanyName = "Zaktualizowana Firma Testowa";
        dbHelper.UpdateCustomer(retrievedCustomer);
        Console.WriteLine($"\nZaktualizowano nazwę firmy dla klienta {retrievedCustomer.CustomerID}.");

        customers = dbHelper.GetAllCustomers();
        Console.WriteLine("\nKlienci po aktualizacji:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"CustomerID: {customer.CustomerID}, CompanyName: {customer.CompanyName}");
        }

      
        dbHelper.RemoveCustomerById(addedCustomerId);
        Console.WriteLine($"\nUsunięto klienta o CustomerID: {addedCustomerId}");

       
        customers = dbHelper.GetAllCustomers();
        Console.WriteLine("\nKlienci po usunięciu:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"CustomerID: {customer.CustomerID}, CompanyName: {customer.CompanyName}");
        }
    }
}
