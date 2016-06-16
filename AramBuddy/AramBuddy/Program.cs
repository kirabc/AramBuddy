﻿namespace AramBuddy
{
    using System;
    using System.Linq;

    using AramBuddy.MainCore;
    using AramBuddy.MainCore.Positioning;
    using AramBuddy.MainCore.Utility;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Program
    {
        public static bool Loaded;

        public static float Timer;

        public static string Activemode;

        public static string Moveto;

        public static bool Moving;

        public static Menu MenuIni;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            /*
            if (Game.MapId != GameMapId.HowlingAbyss)
            {
                Console.WriteLine(Game.MapId + " Is Not Supported By AramBuddy !");
                Chat.Print(Game.MapId + " Is Not Supported By AramBuddy !");
                return;
            }
            */

            Timer = Game.Time;
            Game.OnTick += Game_OnTick;
            Events.OnGameEnd += Events_OnGameEnd;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Events_OnGameEnd(EventArgs args)
        {
            Core.DelayAction(() => Game.QuitGame(), 250);
        }

        private static void Init()
        {
            MenuIni = MainMenu.AddMenu("AramBuddy", "AramBuddy");
            MenuIni.AddGroupLabel("AramBuddy Settings");
            MenuIni.Add("DisableSpells", new CheckBox("Disable Built-in Casting Logic", false));
            MenuIni.Add("Safe", new Slider("Safe Slider (Recommended 1250)", 1250, 0, 2500));
            MenuIni.AddLabel("More Value = more defensive playstyle");
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            Drawing.DrawText(
                Drawing.Width * 0.01f,
                Drawing.Height * 0.025f,
                System.Drawing.Color.White,
                "AllyTeamTotal: " + (int)Misc.TeamTotal(Player.Instance.ServerPosition) + " | EnemyTeamTotal: "
                + (int)Misc.TeamTotal(Player.Instance.ServerPosition, true) + " | MoveTo: " + Moveto + " | ActiveMode: " + Orbwalker.ActiveModesFlags
                + " | Alone: " + Brain.Alone() + " | AttackObject: " + MainCore.Modes.ModesManager.AttackObject + " | LastTurretAttack: "
                + (Core.GameTickCount - Brain.LastTurretAttack) + " | SafeToDive: " + Misc.SafeToDive);

            Drawing.DrawText(
                Game.CursorPos.WorldToScreen().X + 50,
                Game.CursorPos.WorldToScreen().Y,
                System.Drawing.Color.Goldenrod,
                (Misc.TeamTotal(Game.CursorPos) - Misc.TeamTotal(Game.CursorPos, true)).ToString(),
                5);

            foreach (var hr in ObjectsManager.HealthRelics.Where(h => h.IsValid))
            {
                Circle.Draw(Color.White, 125, hr.Position);
            }

            foreach (var trap in ObjectsManager.EnemyTraps)
            {
                Circle.Draw(Color.White, trap.BoundingRadius * 4, trap.Position);
            }

            if (Pathing.Position != null && Pathing.Position != Vector3.Zero)
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

                    // Initialize the AutoShop.
                    AutoShop.Setup.Init();

                    // Initialize The Bot.
                    Brain.Init();
                    Init();
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
        }
    }
}