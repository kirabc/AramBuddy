namespace AramBuddy
{
    using System;
    using System.Linq;

    using AramBuddy.MainCore;
    using AramBuddy.MainCore.Positioning;
    using AramBuddy.MainCore.Utility;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using GenesisSpellLibrary;

    using SharpDX;

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

                    // Clears HealthRelics.
                    ObjectsManager.HealthRelics.Clear();

                    // Initialize The SpellsLibrary.
                    SpellManager.Initialize();
                    SpellLibrary.Initialize();

                    // Overrides Orbwalker Movements
                    Orbwalker.OverrideOrbwalkPosition = OverrideOrbwalkPosition;
                    Obj_AI_Base.OnBasicAttack += Brain.Obj_AI_Base_OnBasicAttack;
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
        }

        private static Vector3? OverrideOrbwalkPosition()
        {
            return Pathing.Position;
        }
    }
}