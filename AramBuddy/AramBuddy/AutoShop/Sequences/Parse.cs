using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AramBuddy.AutoShop.Sequences
{
    /// <summary>
    ///     A class containing all the methods needed for parsing JSON
    /// </summary>
    internal static class Parse
    {
        /// <summary>
        ///     Tries to parse data into a build.
        /// </summary>
        /// <param name="data">The build JSON</param>
        /// <param name="build">
        ///     The Build class object that will be outputted
        ///     validly if the parse is successful, or null if it is not.
        /// </param>
        /// <returns>True on success, false on failiure</returns>
        public static bool TryParseData(this string data, out Build build)
        {
            try
            {
                // First stage in parsing - getting a dynamic object
                // that we can use to get subobjects
                dynamic parsed = JObject.Parse(data);

                // Get the build array from from the dynamic
                string[] arr = parsed.data.ToObject<string[]>();

                // Create a new build object
                build = new Build {BuildData = arr};
                return true;
            }
            catch (JsonSerializationException ex)
            {
                // Exception has been cought; Notify the user of the error and print the exception to the console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("[hh:mm:ss - ") +
                                  "AramBuddy Error] Exception occurred in AutoShop on JSON parse:" + Environment.NewLine);
                Console.ResetColor();
                Console.Write(ex);

                // Warn the user that AutoShop may not be functioning correctly
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(
                    DateTime.Now.ToString("[hh:mm:ss - ") +
                    "AramBuddy Warning] Exception occurred during AutoShop JSON parse. AutoShop will most likely NOT work properly!");
                Console.ResetColor();
                build = null;
                return false;
            }
        }
    }
}