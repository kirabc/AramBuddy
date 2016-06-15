namespace AramBuddy.MainCore
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Modes;

    using Positioning;

    using SharpDX;

    using Utility;

    internal class Brain
    {
        /// <summary>
        ///     Decisions picking for the bot.
        /// </summary>
        public static void Decisions()
        {
            // Picks best position for the bot.
            Pathing.BestPosition();

            // Ticks for the modes manager.
            ModesManager.OnTick();

            // Disables Orbwalker Movements when the bot has Autsim.
            Orbwalker.DisableMovement = Alone();

            // Moves to the Bot selected Position.
            if (Pathing.Position != Vector3.Zero && Pathing.Position != null)
            {
                Pathing.MoveTo(Pathing.Position);
            }

            // Removes HealthRelics if a hero is in range with them.
            var HR = ObjectsManager.HealthRelics.FirstOrDefault(h => EntityManager.Heroes.AllHeroes.Any(a => !a.IsDead && a.IsInRange(h, 150)));
            if (HR != null)
            {
                ObjectsManager.HealthRelics.Remove(HR);
                Chat.Print("Removed");
            }
        }

        /// <summary>
        ///     Bool returns true if the bot is alone.
        /// </summary>
        public static bool Alone()
        {
            return Player.Instance.CountAlliesInRange(1000) < 2 || Player.Instance.Path.Any(p => p.IsInRange(Game.CursorPos, 50))
                   || EntityManager.Heroes.Allies.All(a => !a.IsMe && a.IsInShopRange());
        }

        /// <summary>
        ///     Last Turret Attack Time.
        /// </summary>
        public static float LastTurretAttack;

        /// <summary>
        ///     Checks Turret Attacks.
        /// </summary>
        public static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Turret && args.Target.IsMe)
            {
                LastTurretAttack = Core.GameTickCount;
            }
        }
    }
}