using Microsoft.AspNetCore.Mvc;
using System;

namespace Qoniac.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QoniacController : ControllerBase
    {
        private static string[] digitWords  = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private static string[] teenWords  = { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private static string[] tensWords  = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        public QoniacController()
        {

        }

        [HttpGet("numbertowords")]
        public IActionResult Get(string number)
        {
            try
            {
                long dollars = 0;
                int cents = 0;


                if (!string.IsNullOrEmpty(number))
                {
                    string[] parts = number.Split(',');
                    parts[0] = parts[0].Replace(" ", "");
                    if ((parts.Length > 0 && !long.TryParse(parts[0], out dollars)) || parts.Length > 2)
                    {
                        return BadRequest("Invalid dollar amount");
                    }
                    if (!parts[0].All(char.IsDigit))
                    {
                        return BadRequest("Input must contain only digits");
                    }
                    if (parts[0].Length > 9) 
                    {
                        return BadRequest("Total amount exceeds the maximum limit set by Qoniac");
                    }

                    if (parts.Length > 1 && parts[1].Length > 0 && !int.TryParse(parts[1], out cents))
                    {
                        return BadRequest("Invalid cent amount");
                    }
                }

                if (dollars < 0)
                {
                    return BadRequest("Dollars must be non-negative");
                }

                if (cents < 0 || cents > 99)
                {
                    return BadRequest("Cents must be from 0 to 99");
                }

            
                return Ok(ConvertToWords(dollars, cents)+".");

            }
            catch (Exception ex)
            {
                return BadRequest("Invalid input: " + ex.Message);
            }

        }

        public static string ConvertToWords(long dollars, int cents)
        {
            if (dollars == 0 && cents == 0)
            {
                return "zero dollars";
            }

            string words = Convert(dollars) + " dollars";

            if (cents > 0)
            {
                words += " and " + Convert(cents) + " cents";
            }

            return words;
        }

        private static string Convert(long num)
        {
            if (num < 10)
            {
                return digitWords [num];
            }
            else if (num < 20)
            {
                return teenWords [num - 10];
            }
            else if (num < 100)
            {
                return tensWords [num / 10] + (num % 10 != 0 ? "-" + digitWords [num % 10] : "");
            }
            else if (num < 1000)
            {
                return digitWords [num / 100] + " hundred" + (num % 100 != 0 ? " and " + Convert(num % 100) : "");
            }
            else if (num < 1000000)
            {
                return Convert(num / 1000) + " thousand" + (num % 1000 != 0 ? " " + Convert(num % 1000) : "");
            }
            else if (num < 1000000000)
            {
                return Convert(num / 1000000) + " million" + (num % 1000000 != 0 ? " " + Convert(num % 1000000) : "");
            }
            else
            {
                throw new ArgumentOutOfRangeException("Number out of range");
            }
        }
    }
}
