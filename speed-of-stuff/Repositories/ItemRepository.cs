using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using MySql.Data.MySqlClient;
using speed_of_stuff.Models;

namespace speed_of_stuff.Repositories
{
    public class ItemRepository
    {
        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
                                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                                    .AddJsonFile("appsettings.json")
                                                                    .AddEnvironmentVariables()
                                                                    .AddUserSecrets<Startup>()
                                                                    .Build();

        private static readonly string connectionString = Config["ConnectionStrings:MySql"];

        public static IDbConnection Connection
        {
            get => new MySqlConnection(connectionString);
        }

        public static void Add(Item item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"INSERT INTO items (id, name, maxSpeed, avgSpeed, source)
                                    VALUES(@id, @name, @maxSpeed, @avgSpeed, @source)";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }

        public static IEnumerable<Item> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Item>("SELECT * FROM items").ToList();
            }
        }

        public static Item GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"SELECT * FROM items
                                    WHERE id = @id";
                dbConnection.Open();
                return dbConnection.Query<Item>(query, new { id }).FirstOrDefault();
            }
        }

        public static void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"DELETE FROM items
                                    WHERE id = @id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }

        public static void Update(Item item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"UPDATE items SET 
                                    name = @name,
                                    maxSpeed = @maxSpeed, 
                                    avgSpeed= @avgSpeed,
                                    source = @source
                                    WHERE id = @id";
                dbConnection.Open();
                dbConnection.Query(query, item);
            }
        }

        public static Item[] GetClosestItems(float speed, string type)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"SELECT * FROM
                                  (SELECT *
                                      FROM items
                                      WHERE {0}speed < @speed
                                      ORDER BY maxspeed DESC LIMIT 1) low
                                  UNION
                                  (SELECT *
                                      FROM items
                                      WHERE {0}speed > @speed
                                      ORDER BY maxspeed ASC LIMIT 1)";
                dbConnection.Open();
                return dbConnection.Query<Item>(String.Format(query, type), new { speed }).ToArray();
            }
        }
    }
}
