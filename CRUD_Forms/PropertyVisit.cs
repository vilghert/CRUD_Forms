using Npgsql;

public class PropertyVisit
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int ClientId { get; set; }
    public DateTime VisitDate { get; set; }
    public string? Feedback { get; set; }
    public string? ClientName { get; set; }
    public string? PropertyAddress { get; set; }

    private static string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=221982;Database=realtor";

    public static List<PropertyVisit> ReadVisits()
    {
        var visits = new List<PropertyVisit>();
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var commandText = @"
                SELECT pv.id, pv.property_id, pv.client_id, pv.visit_date, pv.feedback, 
                       c.name AS client_name, p.address AS property_address
                FROM property_visits pv
                JOIN clients c ON pv.client_id = c.id
                JOIN properties p ON pv.property_id = p.new_id";
            using (var command = new NpgsqlCommand(commandText, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    visits.Add(new PropertyVisit
                    {
                        Id = reader.GetInt32(0),
                        PropertyId = reader.GetInt32(1),
                        ClientId = reader.GetInt32(2),
                        VisitDate = reader.GetDateTime(3),
                        Feedback = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ClientName = reader.GetString(5),
                        PropertyAddress = reader.GetString(6)
                    });
                }
            }
        }
        return visits;
    }

    public static void CreatePropertyVisit(PropertyVisit visit)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("INSERT INTO property_visits (property_id, client_id, visit_date, feedback) VALUES (@PropertyId, @ClientId, @VisitDate, @Feedback)", connection))
            {
                command.Parameters.AddWithValue("PropertyId", visit.PropertyId);
                command.Parameters.AddWithValue("ClientId", visit.ClientId);
                command.Parameters.AddWithValue("VisitDate", visit.VisitDate);
                command.Parameters.AddWithValue("Feedback", (object)visit.Feedback ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
    }
    public static void UpdatePropertyVisit(int id, PropertyVisit visit)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("UPDATE property_visits SET property_id = @PropertyId, client_id = @ClientId, visit_date = @VisitDate, feedback = @Feedback WHERE id = @Id", connection))
            {
                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("PropertyId", visit.PropertyId);
                command.Parameters.AddWithValue("ClientId", visit.ClientId);
                command.Parameters.AddWithValue("VisitDate", visit.VisitDate);
                command.Parameters.AddWithValue("Feedback", (object)visit.Feedback ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeletePropertyVisit(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("DELETE FROM property_visits WHERE id = @Id", connection))
            {
                command.Parameters.AddWithValue("Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
    public static decimal CalculateDiscount(int clientId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "SELECT public.calculate_discount(@ClientId)";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("ClientId", clientId);

                try
                {
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error calculating discount: " + ex.Message);
                    return 0;
                }
            }
        }
    }
    public static decimal CalculateAgentCommission(int agentId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "SELECT public.calculateagentcommission(@AgentId)";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("AgentId", agentId);

                try
                {
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error calculating agent commission: " + ex.Message);
                    return 0;
                }
            }
        }
    }
    public static int GetTotalTransactions()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            var commandText = "SELECT public.get_total_transactions()";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                try
                {
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting total transactions: " + ex.Message);
                    return 0;
                }
            }
        }
    }

}
