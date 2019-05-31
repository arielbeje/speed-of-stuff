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
    /*
     * Contains all methods for interacting with the Item database.
     */
    /// <summary>
    /// Contains all methods for interacting with the <c>Item</c> database.
    /// </summary>
    public class ItemRepository
    {
        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
                                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                                    .AddJsonFile("appsettings.json")
                                                                    .AddEnvironmentVariables()
                                                                    .AddUserSecrets<Startup>()
                                                                    .Build();

        private static readonly string connectionString = Config["ConnectionStrings:MySql"];

        // Creates and returns a connection to the database.
        /// <summary>
        /// Makes a connection to the database using the connection string.
        /// </summary>
        /// <returns>
        /// A connection to the database.
        /// </returns>
        public static IDbConnection Connection
        {
            get => new MySqlConnection(connectionString);
        }

        // Adds an Item to the database.
        /// <summary>
        /// Adds an Item to the database.
        /// </summary>
        /// <param name="item"><c>Item</c> - the <c>Item</c> to add</param>
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

        // Gets all the items from the database.
        /// <summary>
        /// Gets all the <c>Item</c>s from the database.
        /// </summary>
        /// <returns>
        /// An <c>IEnumerable</c> contatining all the items in the database.
        /// </returns>
        public static IEnumerable<Item> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Item>("SELECT * FROM items").ToList();
            }
        }

        // Gets the Item with the specified ID
        /// <summary>
        /// Find the <c>Item</c> with the speicified ID in the database.
        /// </summary>
        /// <param name="id"><c>uint</c> - the ID to search for</param>
        /// <returns>
        /// The <c>Item</c> with the specified ID.
        /// </returns>
        public static Item GetByID(uint id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"SELECT * FROM items
                                    WHERE id = @id";
                dbConnection.Open();
                return dbConnection.Query<Item>(query, new { id }).FirstOrDefault();
            }
        }

        // Deletes the Item with the specified ID from the database
        /// <summary>
        /// Deletes the <c>Item</c> with the specified ID from the database.
        /// </summary>
        /// <param name="id"><c>uint</c> - the ID of the <c>Item</c></param>
        public static void Delete(uint id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = @"DELETE FROM items
                                    WHERE id = @id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }

        // Updates the Item in the database
        /// <summary>
        /// Updates the <c>Item</c> in the database.
        /// </summary>
        /// <param name="item"><c>Item</c> - the <c>Item</c> to update</param>
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

        // Gets the two items with the speed closest to the one specified
        /// <summary>
        /// Finds the two items with the closest speeds to the speed given within the type specified.
        /// </summary>
        /// <param name="speed"><c>float</c> - the speed to use</param>
        /// <param name="type"><c>string</c> - the type to use (can be either <c>max</c> or <c>avg</c>)</param>
        /// <returns>
        /// A <c>Item[]</c> with the two <c>Item</c>s.
        /// </returns>
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
