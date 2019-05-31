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

        public float Difference(float? num1, float num2)
        {
            return Math.Abs(num1.Value - num2);
        }

        public Item ClosestMaxSpeed(float speed)
        {
            var items = ItemRepository.GetClosestItems(speed, "max");

            if (items.Length == 1 || Difference(items[0].maxSpeed, speed) < Difference(items[1].maxSpeed, speed))
                return items[0];
            return items[1];
        }

        public Item ClosestAvgSpeed(float speed)
        {
            var items = ItemRepository.GetClosestItems(speed, "avg");

            if (items.Length == 1 || Difference(items[0].avgSpeed, speed) < Difference(items[1].avgSpeed, speed))
                return items[0];
            return items[1];
        }

        // GET api?speed=5
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