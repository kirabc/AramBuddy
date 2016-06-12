using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramBuddy
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    internal class Program
    {
        public static bool Loaded;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Game.MapId != GameMapId.HowlingAbyss)
            {
                Console.WriteLine(Game.MapId + " Is Not Supported By AramBuddy !");
                Chat.Print(Game.MapId + " Is Not Supported By AramBuddy !");
                return;
            }
            
            // Initialize the AutoShop
            AutoShop.Setup.Init();

            Game.OnTick += Game_OnTick;
            Game.OnEnd += Game_OnEnd;

        }

        private static void Game_OnEnd(GameEndEventArgs args)
        {
            Core.DelayAction(() => Game.QuitGame(), 250);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (!Loaded)
            {
                if (Game.Time >= 15)
                {
                    Loaded = true;
                    Chat.Print("Loaded");
                }
            }
        }
    }
}