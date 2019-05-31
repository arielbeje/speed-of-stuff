using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlConnector;
using speed_of_stuff.Models;
using speed_of_stuff.Repositories;

namespace speed_of_stuff.Controllers
{
    [Route("api")]
    [ApiController]
    public class SpeedController : ControllerBase
    {
        // Calculates the difference between two numbers arnd returns the result.
        /// <summary>
        /// Calculates the difference between the numbers given.
        /// </summary>
        /// <param name="num1"><c>float?</c> - the first number</param>
        /// <param name="num2"><c>float</c> - the second number</param>
        /// <returns>
        /// The difference between the numbers given.
        /// </returns>
        public float Difference(float? num1, float num2) => Math.Abs(num1.Value - num2);

        // Finds and returns the Item with the closest maximum speed to the speed given.
        /// <summary>
        /// Finds the <c>Item</c> with the closest maximum speed to the speed given.
        /// </summary>
        /// <param name="speed"><c>float</c> - the speed to use</param>
        /// <returns>
        /// The <c>Item</c> with the closest maximum speed to the speed given.
        /// </returns>
        public Item ClosestMaxSpeed(float speed)
        {
            var items = ItemRepository.GetClosestItems(speed, "max");

            if (items.Length == 1 || Difference(items[0].maxSpeed, speed) < Difference(items[1].maxSpeed, speed))
                return items[0];
            return items[1];
        }


        // Finds and returns the Item with the closest average speed to the one given.
        /// <summary>
        /// Finds the <c>Item</c> with the closest average speed to the speed given.
        /// </summary>
        /// <param name="speed"><c>float</c> - the speed to use</param>
        /// <returns>
        /// The <c>Item</c> with the closest average speed to the speed given.
        /// </returns>
        public Item ClosestAvgSpeed(float speed)
        {
            var items = ItemRepository.GetClosestItems(speed, "avg");

            if (items.Length == 1 || Difference(items[0].avgSpeed, speed) < Difference(items[1].avgSpeed, speed))
                return items[0];
            return items[1];
        }

        // Finds and returns the Item with the closest speed to the one given.
        /// <summary>
        /// Finds the <c>Item</c> with the closest speed to the speed given by comparing
        /// the closest average/maximum speeds.
        /// </summary>
        /// <param name="speed"><c>float</c> - the speed to use</param>
        /// <returns>
        /// The <c>Item</c> with the closest speed to the speed given.
        /// </returns>
        [HttpGet]
        public Item Get(float speed)
        {
            Item closestMax = ClosestMaxSpeed(speed);
            Item closestAvg = ClosestAvgSpeed(speed);

            if (!closestAvg.avgSpeed.HasValue || Difference(closestMax.maxSpeed, speed) < Difference(closestAvg.avgSpeed, speed))
                return closestMax;
            return closestAvg;
        }
    }
}