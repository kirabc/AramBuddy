using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAram
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
            

            // test
            Game.OnNotify += Game_OnNotify;
        }

        private static void Game_OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId == GameEventId.OnGameStart)
                Console.WriteLine("start");
            if (args.EventId == GameEventId.OnEndGame)
                Console.WriteLine("end");
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Game.MapId != GameMapId.HowlingAbyss)
            {
                Console.WriteLine(Game.MapId + " Is Not Supported By AutoAram !");
                Chat.Print(Game.MapId + " Is Not Supported By AutoAram !");
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