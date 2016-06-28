// <summary>
//   The class containing the BuildData used by the interpreter to buy items in order
// </summary>
namespace AramBuddy.AutoShop
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;

    using EloBuddy;

    /// <summary>
    ///     The class containing the BuildData used by the interpreter to buy items in order
    /// </summary>
    public class Build
    {
        /// <summary>
        ///     An array of the item names
        /// </summary>
        public string[] BuildData { get; set; }

        /// <summary>
        /// returns The build name.
        /// </summary>
        public static string BuildName()
        {
            var ChampionName = Player.Instance.ChampionName;

            if (ADC.Contains(ChampionName))
            {
                return "ADC";
            }

            if (AD.Contains(ChampionName))
            {
                return "AD";
            }

            if (AP.Contains(ChampionName))
            {
                return "AP";
            }

            if (ManaAP.Contains(ChampionName))
            {
                return "ManaAP";
            }

            if (Tank.Contains(ChampionName))
            {
                return "Tank";
            }

            Console.WriteLine("Failed To Detect " + ChampionName);
            Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Info] Using Default Build !");
            return "Default";
        }
        

        /// <summary>
        ///     Creates Builds
        /// </summary>
        public static void Create()
        {
            try
            {
                GetResponse(WebRequest.Create("https://raw.githubusercontent.com/plsfixrito/AramBuddy/master/DefaultBuilds/" + BuildName() + ".json"), 
                    response =>
                        {
                            var data = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
                            if (data.Contains("data"))
                            {
                                File.WriteAllText(Setup.BuildPath + "\\" + BuildName() + ".json", data);
                                Setup.Builds.Add(BuildName(), File.ReadAllText(Setup.BuildPath + "\\" + BuildName() + ".json"));

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Info] " + BuildName() + " Build Created for " + Player.Instance.ChampionName + " - " + BuildName());
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Warning] Wrong Response, No Champion Build Created");
                                Console.WriteLine(data);
                                Console.ResetColor();
                            }
                    });
            }
            catch (Exception ex)
            {
                // if faild to create build terminate the AutoShop
                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Warning] Failed to create default build for " + Player.Instance.ChampionName);
                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Warning] No build is currently used!");
                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + ex);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Sends and get response from web
        /// </summary>
        private static void GetResponse(WebRequest Request, Action<HttpWebResponse> ResponseAction)
        {
            try
            {
                Action wrapperAction = () =>
                {
                    Request.BeginGetResponse(
                        iar =>
                        {
                            var Response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                            ResponseAction(Response);
                        }, 
                        Request);
                };
                wrapperAction.BeginInvoke(
                    iar =>
                    {
                        var Action = (Action)iar.AsyncState;
                        Action.EndInvoke(iar);
                    }, 
                    wrapperAction);
            }
            catch (Exception ex)
            {

                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Warning] Failed to create default build, No Response.");
                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + ex);
                Console.ResetColor();
            }
        }

        /// <summary>
        ///  ADC Champions.
        /// </summary>
        public static readonly string[] ADC =
            {
                "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Jhin", "Jinx", "Kalista", "Kindred", "KogMaw", 
                "Lucian", "MissFortune", "Sivir", "Quinn", "Tristana", "Twitch", "Urgot", "Varus", "Vayne"
            };

        /// <summary>
        ///  Mana AP Champions.
        /// </summary>
        public static readonly string[] ManaAP =
            {
                "Ahri", "Anivia", "Annie", "AurelioSol", "Azir", "Brand", "Cassiopeia", "Diana", "Elise", 
                "Fiddlesticks", "Fizz", "Galio", "Gragas", "Heimerdinger", "Janna", "Karma", "Karthus", 
                "Kassadin", "Kayle", "LeBlanc", "Lissandra", "Lulu", "Lux", "Malzahar", "Morgana", "Nami", 
                "Nidalee", "Ryze", "Orianna", "Sona", "Soraka", "Swain", "Syndra", "Taliyah", "Teemo", 
                "TwistedFate", "Veigar", "Viktor", "VelKoz", "Xerath", "Ziggs", "Zilean", "Zyra"
            };

        /// <summary>
        ///  AP no Mana Champions.
        /// </summary>
        public static readonly string[] AP = { "Akali", "Katarina", "Kennen", "Mordekaiser", "Rumble", "Vladimir" };

        /// <summary>
        ///  AD Champions.
        /// </summary>
        public static readonly string[] AD =
            {
                 "Vaca"
            };

        /// <summary>
        ///  Tank Champions.
        /// </summary>
        public static readonly string[] Tank =
            {
                "Alistar", "Amumu", "Blitzcrank", "Bard", "Braum", "ChoGath", "DrMundo", "Garen", "Gnar", "Hecarim", 
                "Illaoi", "Irelia", "JarvanIV", "Leona", "Malphite", "Maokai", "Nasus", "Nautilus", "Nunu", "Poppy", 
                "Rammus", "Sejuani", "Shaco", "Shen", "Shyvana", "Singed", "Sion", "Skarner", "TahmKench", "Taric", 
                "Thresh", "Trundle", "Udyr", "Vi", "Volibear", "Warwick", "Yorick", "Zac", "Ekko", "Evelynn", "Aatrox",
                "Darius", "Fiora", "Gangplank", "Jax", "Jayce", "KhaZix", "LeeSin", "MasterYi", "Nocturne", "Olaf", 
                "Pantheon", "RekSai", "Renekton", "Rengar", "Riven", "Talon", "Tryndamere", "Wukong", "XinZhao", "Yasuo",
                "Zed"
            };
    }
}
