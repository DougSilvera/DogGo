using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using DogGo.Interfaces;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _configuration;
        public WalksRepository(IConfiguration config)
        {
            _configuration = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Walk> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT w.Id, w.[Date], w.Duration, o.[Name] as OwnerName, d.[Name] as DogName FROM Walks w LEFT JOIN Dog d on w.DogId = d.Id LEFT JOIN[Owner] o on d.OwnerId = o.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Owner = new Owner
                                {
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                },
                                Dog = new Dog
                                {
                                    Name = reader.GetString(reader.GetOrdinal("DogName"))
                                }
                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
	                                        w.Id, 
	                                        w.[Date], 
	                                        w.Duration, 
	                                        o.[Name] as OwnerName, 
	                                        d.[Name] as DogName 
	                                    FROM Walks w 
	                                    LEFT JOIN Dog d on w.DogId = d.Id 
	                                    LEFT JOIN [Owner] o on d.OwnerId = o.Id 
	                                    WHERE w.WalkerId = @id;";
                    cmd.Parameters.AddWithValue("@id", walkerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Owner = new Owner
                                {
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                },
                                Dog = new Dog
                                {
                                    Name = reader.GetString(reader.GetOrdinal("DogName"))
                                }

                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
        public void CreateWalk(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO Walks
                                 (Date, Duration, WalkerId, DogId)
                                 OUTPUT INSERTED.ID
                                 VALUES (@date, @duration, @walkerId, @dogId)";
                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration * 60);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);

                    int id= (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }
        public void DeleteWalk(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Walks WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
