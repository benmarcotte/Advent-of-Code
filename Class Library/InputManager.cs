using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class InputManager
    {
        /// <summary>
        /// Returns the file input for the session cookie set at user variable AOC_COOKIE.
        /// </summary>
        /// <param name="day">Desired day</param>
        /// <returns>The input for the user for the day</returns>
        public static async Task<Stream> GetInputAsync(DateTime day)
        {
            string cookie = Environment.GetEnvironmentVariable("AOC_COOKIE", EnvironmentVariableTarget.User);
            var uri = new Uri("https://adventofcode.com");
            var cookies = new CookieContainer();
            cookies.Add(uri, new Cookie("session", cookie));
            var handler = new HttpClientHandler() { CookieContainer = cookies };
            var client = new HttpClient(handler) { BaseAddress = uri };
            var response = await client.GetAsync($"/{day.Year}/day/{day.Day}/input");

            return await response.Content.ReadAsStreamAsync();
            
        }
    }
}
