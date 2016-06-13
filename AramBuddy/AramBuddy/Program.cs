#region

using System;
using System.Drawing;
using System.Linq;
using AramBuddy.AutoShop;
using AramBuddy.MainCore;
using AramBuddy.MainCore.Positioning;
using AramBuddy.MainCore.Utility;
using GenesisSpellLibrary;

#endregion

namespace AramBuddy
{
    internal class Program
    {
        public static bool Loaded;

        public static float Timer;

        public static string Activemode;

        public static string Moveto;

        public static bool Moving;

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

            Timer = Game.Time;
            Game.OnTick += Game_OnTick;
            Events.OnGameEnd += Events_OnGameEnd;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Events_OnGameEnd(EventArgs args)
        {
            Core.DelayAction(() => Game.QuitGame(), 250);
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            Drawing.DrawText(
                Drawing.Width * 0.01f,
                Drawing.Height * 0.025f,
                Color.White,
                "AllyTeamTotal: " + (int) Misc.TeamTotal(Player.Instance) + " | EnemyTeamTotal: " +
                (int) Misc.TeamTotal(Player.Instance, true)
                + " | MoveTo: " + Moveto + " | ActiveMode: " + Orbwalker.ActiveModesFlags + " | Alone: " + Brain.Alone());
            if ((Pathing.Position != null) && (Pathing.Position != Vector3.Zero))
            {
                Circle.Draw(Color.White, 100, Pathing.Position);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (!Loaded)
            {
                if (Game.Time - Timer >= 5)
                {
                    Loaded = true;
                    // Initialize the AutoShop
                    Setup.Init();

                    ObjectsManager.HealthRelics.Clear();
                    SpellManager.Initialize();
                    SpellLibrary.Initialize();

                    Orbwalker.OverrideOrbwalkPosition = OverrideOrbwalkPosition;
                    GameObject.OnCreate += ObjectsManager.GameObject_OnCreate;
                    GameObject.OnDelete += ObjectsManager.GameObject_OnDelete;

                    Chat.Print("AramBuddy Loaded !");
                }
            }
            else
            {
                if (Player.Instance.IsDead)
                {
                    return;
                }
                Brain.Decisions();
            }

            var HR =
                ObjectsManager.HealthRelics.FirstOrDefault(
                    h => EntityManager.Heroes.AllHeroes.Any(a => !a.IsDead && a.IsInRange(h, 150)));
            if (HR != null)
            {
                ObjectsManager.HealthRelics.Remove(HR);
                Chat.Print("Removed");
            }
        }

        private static Vector3? OverrideOrbwalkPosition()
        {
            return Pathing.Position;
        }
    }
}