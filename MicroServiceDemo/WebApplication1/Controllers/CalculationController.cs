﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        [HttpGet("info")]
        public ActionResult<string> Info()
            => Environment.GetEnvironmentVariable("machineName") ?? Environment.MachineName;

        [HttpGet("add/{x}/{y}")]
        public int Add(int x, int y) => x + y;

        [HttpGet("sub/{x}/{y}")]
        public int Sub(int x, int y) => x - y;

        [HttpPost("stddev")]
        public double StdDev([FromBody]float[] numbers)
        => Math.Sqrt(numbers.Select(x => Math.Pow(x - numbers.Average(), 2)).Sum() / (numbers.Length - 1));
    }
}
