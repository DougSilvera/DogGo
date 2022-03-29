using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _configuration;

        public DogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id, d.[Name], d.OwnerId, d.Breed, d.Notes, d.ImageUrl, o.[Name] as OwnerName FROM Dog d 
                                        LEFT JOIN Owner o ON d.OwnerId = o.Id";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Dog> list = new List<Dog>();
                        while (reader.Read())
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")) ,
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                }
                            };
                            if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                            {
                                dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            if (reader.IsDBNull (reader.GetOrdinal("ImageUrl")) ==false)
                            {
                                dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                            }
                            list.Add(dog);
                        }
                        return list;
                    }
                }
            }
        }
       public Dog GetDogById(int id)
        {
            throw new NotImplementedException();
        }
        public void AddDog(Dog dog)
        {
            throw new NotImplementedException();
        }
        public void UpdateDog(Dog dog)
        {
            throw new NotImplementedException();
        }
        public void DeleteDog(int dogId)
        {
            throw new NotImplementedException();
        }

    }
}
