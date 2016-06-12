using System;
using AutoAram.AutoShop;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace AutoAram
{
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
                Console.WriteLine(Game.MapId + " Is Not Supported By AutoAram !");
                Chat.Print(Game.MapId + " Is Not Supported By AutoAram !");
                return;
            }

            // Initialize the AutoShop
            Setup.Init();

            Game.OnTick += Game_OnTick;
            Events.OnGameEnd += Events_OnGameEnd;
        }

        private static void Events_OnGameEnd(EventArgs args)
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