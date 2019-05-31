using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace speed_of_stuff.Models
{
    public class Item
    {
        [Key]
        public uint id { get; set; }
        public string name { get; set; }
        public float maxSpeed { get; set; }
        public float? avgSpeed { get; set; }
        public string source { get; set; }

        private static readonly Random random = new Random();

        // Assings a random ID
        /// <summary>
        /// Generates a random <c>uint</c> and assins it as the ID of the <c>Item</c>.
        /// </summary>
        private void RandomizeId()
        {
            uint thirtyBits = (uint)random.Next(1 << 30);
            uint twoBits = (uint)random.Next(1 << 2);
            id = (thirtyBits << 2) | twoBits;
        }

        /// <summary>
        /// Builds a new <c>Item</c>.
        /// </summary>
        /// <param name="id"><c>uint</c> - the new <c>Item</c>'s ID</param>
        /// <param name="name"><c>string</c> - the new <c>Item</c>'s name</param>
        /// <param name="maxSpeed"><c>float</c> - the new <c>Item</c>'s maximum speed</param>
        /// <param name="avgSpeed"><c>float?</c> - the new <c>Item</c>'s average speed</param>
        /// <param name="source"><c>string</c> - the new <c>Item</c>'s source</param>
        public Item(uint id, string name, float maxSpeed, float? avgSpeed, string source)
        {
            this.id = id;
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.avgSpeed = avgSpeed;
            this.source = source;
        }

        /// <summary>
        /// Builds a new <c>Item</c> with a random ID.
        /// </summary>
        /// <param name="name"><c>string</c> - the new <c>Item</c>'s name</param>
        /// <param name="maxSpeed"><c>float</c> - the new <c>Item</c>'s maximum speed</param>
        /// <param name="avgSpeed"><c>float?</c> - the new <c>Item</c>'s average speed</param>
        /// <param name="source"><c>string</c> - the new <c>Item</c>'s source</param>
        public Item(string name, float maxSpeed, float? avgSpeed, string source)
        {
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.avgSpeed = avgSpeed;
            this.source = source;
            RandomizeId();
        }

        /// <summary>
        /// Builds a new <c>Item</c> with a random ID and the average speed set to <c>null</c>.
        /// </summary>
        /// <param name="name"><c>string</c> - the new <c>Item</c>'s name</param>
        /// <param name="maxSpeed"><c>float</c> - the new <c>Item</c>'s maximum speed</param>
        /// <param name="source"><c>string</c> - the new <c>Item</c>'s source</param>
        public Item(string name, float maxSpeed, string source)
        {
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.avgSpeed = null;
            this.source = source;
            RandomizeId();
        }
    }
}
