#region

using AramBuddy.MainCore.Positioning;
using AramBuddy.MainCore.Utility;

#endregion

namespace AramBuddy.MainCore.Modes
{
    internal class ModesManager
    {
        public static bool Combo
        {
            get
            {
                return (Misc.TeamTotal(Player.Instance) > Misc.TeamTotal(Player.Instance, true)) &&
                       (Player.Instance.CountAlliesInRange(800) >= Player.Instance.CountEnemiesInRange(800)) &&
                       (Player.Instance.CountEnemiesInRange(800) > 0);
            }
        }

        public static bool Harass
        {
            get
            {
                return (Misc.TeamTotal(Player.Instance) < Misc.TeamTotal(Player.Instance, true)) &&
                       (Player.Instance.CountEnemiesInRange(800) > 0);
            }
        }

        public static bool LaneClear
        {
            get
            {
                return (Player.Instance.CountEnemiesInRange(1000) == 0) &&
                       ((Player.Instance.CountAlliesInRange(800) > 1) || (Player.Instance.CountMinions() > 0)) &&
                       ((Player.Instance.CountMinions(true) > 1) || AttackObject);
            }
        }

        public static bool Flee
        {
            get
            {
                return ((Misc.TeamTotal(Player.Instance) < Misc.TeamTotal(Player.Instance, true)) &&
                        (Player.Instance.CountAlliesInRange(800) < 2)) ||
                       (Player.Instance.IsUnderEnemyturret() && !Misc.SafeToDive);
            }
        }

        public static bool None
        {
            get { return !Combo && !Harass && !LaneClear && !Flee; }
        }

        public static bool AttackObject
        {
            get
            {
                return ((ObjectsManager.EnemyNexues != null) &&
                        ObjectsManager.EnemyNexues.IsValidTarget(Player.Instance.GetAutoAttackRange() + 25))
                       ||
                       ((ObjectsManager.EnemyInhb != null) &&
                        ObjectsManager.EnemyInhb.IsValidTarget(Player.Instance.GetAutoAttackRange() + 25))
                       ||
                       ((ObjectsManager.EnemyTurret != null) &&
                        ObjectsManager.EnemyTurret.IsValidTarget(Player.Instance.GetAutoAttackRange() + 25));
            }
        }

        public static void OnTick()
        {
            if (Flee)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Flee;
                Pathing.MoveTo(Pathing.Position);
                return;
            }
            if (LaneClear)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.LaneClear;
                Pathing.MoveTo(Pathing.Position);
                return;
            }
            if (Harass)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Harass;
                Pathing.MoveTo(Pathing.Position);
                return;
            }
            if (Combo)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Combo;
                Pathing.MoveTo(Pathing.Position);
                return;
            }
            if (None)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.None;
                Pathing.MoveTo(Pathing.Position);
            }
        }
    }
}