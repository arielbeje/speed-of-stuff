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

        private static Random random = new Random();

        private void RandomizeId()
        {
            uint thirtyBits = (uint)random.Next(1 << 30);
            uint twoBits = (uint)random.Next(1 << 2);
            id = (thirtyBits << 2) | twoBits;
        }
        
        public Item(uint id, string name, float maxSpeed, float? avgSpeed, string source)
        {
            this.id = id;
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.avgSpeed = avgSpeed;
            this.source = source;
        }

        public Item(string name, float maxSpeed, float? avgSpeed, string source)
        {
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.avgSpeed = avgSpeed;
            this.source = source;
            RandomizeId();
        }

        public Item(string name, float maxSpeed, string source)
        {
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.source = source;
            RandomizeId();
        }
    }
}
