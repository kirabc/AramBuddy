namespace AramBuddy.MainCore
{
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using Modes;
    using Positioning;
    using SharpDX;

    internal class Brain
    {
        public static void Decisions()
        {
            Pathing.BestPosition();
            ModesManager.OnTick();

            Orbwalker.DisableMovement = Alone();

            if (Pathing.Position != Vector3.Zero && Pathing.Position != null)
            {
                Pathing.MoveTo(Pathing.Position);
            }
        }

        public static bool Alone()
        {
            return Player.Instance.CountAlliesInRange(1000) < 2 || Player.Instance.Path.Any(p => p.IsInRange(Game.CursorPos, 50))
                   || EntityManager.Heroes.Allies.All(a => !a.IsMe && a.IsInShopRange());
        }
    }
}