﻿using Microsoft.Data.SqlClient;
using Test1.Models;

namespace Test1.Services;

public class VisitsService : IVisitsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;" +
                                                "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                                                "Trust Server Certificate=False;Application Intent=ReadWrite;" +
                                                "Multi Subnet Failover=False";

    public async Task<Visit> GetVisitByIdAsync(int visit_id)
    {
        Visit visit = null;

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            string command = @"SELECT v.date, c.first_name, c.last_name, m.mechanic_id, m.licence_number
FROM Visit v
JOIN Client c ON v.client_id = c.client_id
JOIN Mechanic m ON v.mechanic_id = m.mechanic_id
WHERE v.visit_id = @id_visit;";

            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id_visit", visit_id);
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    visit = new Visit
                    {
                        date = reader.GetDateTime(0),
                        client = new Client
                        {
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                        },
                        mechanic = new Mechanic
                        {
                            mechanic_id = reader.GetInt32(3),
                            licence_number = reader.GetString(4)
                        },
                        visit_services = new List<Visit_Service>()
                    };
                }
                else
                {
                    return null;
                }
            }
            
            command = @"SELECT s.name, sv.service_fee FROM Visit_Service sv 
JOIN Service s ON sv.service_id = s.service_id
WHERE sv.visit_id = @id_visit;";

            using (var cmd  = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id_visit", visit_id);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    visit.visit_services.Add(new Visit_Service
                    {
                        name = reader.GetString(0),
                        service_fee = reader.GetDecimal(1)
                    });
                }
            }
        }
        return visit;
    }

    public async Task<int> AddVisitAsync(Visit visit)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            string command = @"SELECT 1 FROM Visit WHERE visit_id = @id_visit";
            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id_visit", visit.visit_id);
                var existingVisit = await cmd.ExecuteScalarAsync();
                if (existingVisit != null) return -1;
            }
            
            command = @"SELECT 1 FROM Client WHERE client_id = @id_client";
            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("id_client", visit.client.client_id);
                var existingClient = await cmd.ExecuteScalarAsync();
                if (existingClient == null) return -2;
            }
            
            command = @"SELECT mechanic_id FROM Mechanic WHERE licence_number = @licence_number";
            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@licence_number", visit.mechanic.licence_number);
                var existingMechanic = await cmd.ExecuteScalarAsync();
                if (existingMechanic == null) return -3;
            }
            
            command = @"INSERT INTO Visit (visit_id, client_id, mechanic_id) 
VALUES (@visit_id, @client_id, @mechanic_id)";
            return 1;
        }
    }
}