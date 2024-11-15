using System;
using System.Collections.Generic;
using Npgsql;

public class Property
{
    public int NewId { get; set; } 
    public string? Address { get; set; }
    public decimal Price { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }

    private static string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=221982;DataBase=realtor"; // Додайте свій рядок підключення

    public static List<Property> ReadProperties()
    {
        var properties = new List<Property>();
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM properties", connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    properties.Add(new Property
                    {
                        NewId = reader.IsDBNull(reader.GetOrdinal("new_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("new_id")), // Зчитування нового поля
                        Address = reader.IsDBNull(reader.GetOrdinal("address")) ? string.Empty : reader.GetString(reader.GetOrdinal("address")),
                        Price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("price")),
                        Type = reader.IsDBNull(reader.GetOrdinal("type")) ? string.Empty : reader.GetString(reader.GetOrdinal("type")),
                        Status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status"))
                    });
                }
            }
        }
        return properties;
    }

    public static void CreateProperty(string address, decimal price, string type, string status)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("INSERT INTO properties (address, price, type, status) VALUES (@Address, @Price, @Type, @Status)", connection))
            {
                command.Parameters.AddWithValue("Address", address);
                command.Parameters.AddWithValue("Price", price);
                command.Parameters.AddWithValue("Type", type);
                command.Parameters.AddWithValue("Status", status);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void UpdateProperty(int newId, string address, decimal price, string type, string status)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("UPDATE properties SET address = @Address, price = @Price, type = @Type, status = @Status WHERE new_id = @NewId", connection))
            {
                command.Parameters.AddWithValue("NewId", newId);
                command.Parameters.AddWithValue("Address", address);
                command.Parameters.AddWithValue("Price", price);
                command.Parameters.AddWithValue("Type", type);
                command.Parameters.AddWithValue("Status", status);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteProperty(int newId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("DELETE FROM properties WHERE new_id = @NewId", connection))
            {
                command.Parameters.AddWithValue("NewId", newId);
                command.ExecuteNonQuery();
            }
        }
    }
    public static void AddClient(string name, string phone, string email, decimal budget)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "CALL public.addclient(@Name, @Phone, @Email, @Budget)";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("Name", name);
                command.Parameters.AddWithValue("Phone", phone);
                command.Parameters.AddWithValue("Email", email);
                command.Parameters.AddWithValue("Budget", budget);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Client added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding client: " + ex.Message);
                }
            }
        }
    }
    public static void UpdateEmployeeSalary(int employeeId, decimal newSalary)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "CALL public.update_employee_salary(@EmployeeId, @NewSalary)";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("EmployeeId", employeeId);
                command.Parameters.AddWithValue("NewSalary", newSalary);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Employee salary updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating salary: " + ex.Message);
                }
            }
        }
    }
    public static void UpdatePropertyStatus(int propertyId, string newStatus)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "CALL public.update_property_status(@PropertyId, @NewStatus)";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("PropertyId", propertyId);
                command.Parameters.AddWithValue("NewStatus", newStatus);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Property status updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating property status: " + ex.Message);
                }
            }
        }
    }

    public static void UpdateClientBudget(int clientId, decimal newBudget)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("UPDATE clients SET budget = @Budget WHERE id = @ClientId", connection))
            {
                command.Parameters.AddWithValue("ClientId", clientId);
                command.Parameters.AddWithValue("Budget", newBudget);
                command.ExecuteNonQuery();
            }

        }
    }

}
