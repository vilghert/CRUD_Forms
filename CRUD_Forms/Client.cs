using Npgsql;

public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal Budget { get; set; }

    private static string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=221982;DataBase=realtor";

    private static NpgsqlConnection OpenConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public static List<Client> ReadClients()
    {
        var clients = new List<Client>();

        using (var connection = OpenConnection())
        using (var command = new NpgsqlCommand("SELECT * FROM clients", connection))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                clients.Add(new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString(reader.GetOrdinal("email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("phone")),
                    Budget = reader.IsDBNull(reader.GetOrdinal("budget")) ? 0 : reader.GetDecimal(reader.GetOrdinal("budget"))
                });
            }
        }
        return clients;
    }

    public static void CreateClient(string name, string email, string phone, decimal budget)
    {
        using (var connection = OpenConnection())
        using (var command = new NpgsqlCommand("INSERT INTO clients (name, email, phone, budget) VALUES (@Name, @Email, @Phone, @Budget)", connection))
        {
            command.Parameters.AddWithValue("Name", name);
            command.Parameters.AddWithValue("Email", email);
            command.Parameters.AddWithValue("Phone", phone);
            command.Parameters.AddWithValue("Budget", budget);
            command.ExecuteNonQuery();
        }
    }

    public static void UpdateClient(int id, string name, string email, string phone, decimal budget)
    {
        using (var connection = OpenConnection())
        using (var command = new NpgsqlCommand("UPDATE clients SET name = @Name, email = @Email, phone = @Phone, budget = @Budget WHERE id = @Id", connection))
        {
            command.Parameters.AddWithValue("Id", id);
            command.Parameters.AddWithValue("Name", name);
            command.Parameters.AddWithValue("Email", email);
            command.Parameters.AddWithValue("Phone", phone);
            command.Parameters.AddWithValue("Budget", budget);
            command.ExecuteNonQuery();
        }
    }

    public static void DeleteClient(int id)
    {
        using (var connection = OpenConnection())
        using (var command = new NpgsqlCommand("DELETE FROM clients WHERE id = @Id", connection))
        {
            command.Parameters.AddWithValue("Id", id);
            command.ExecuteNonQuery();
        }
    }
    public override string ToString()
    {
        return Name ?? "Unknown Client";
    }

}