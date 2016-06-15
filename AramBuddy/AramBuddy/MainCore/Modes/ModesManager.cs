namespace AramBuddy.MainCore.Modes
{
    using AramBuddy.MainCore.Utility;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class ModesManager
    {
        public static void OnTick()
        {
            // Activate Flee mode
            if (Flee)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Flee;
                return;
            }

            // Activate LaneClear mode
            if (LaneClear)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.LaneClear;
                return;
            }

            // Activate Harass mode
            if (Harass)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Harass;
                return;
            }

            // Activate Combo mode
            if (Combo)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.Combo;
                return;
            }

            // Activate None mode
            if (None)
            {
                Orbwalker.ActiveModesFlags = Orbwalker.ActiveModes.None;
            }
        }

        /// <summary>
        ///     Returns True if combo needs to be active.
        /// </summary>
        public static bool Combo
        {
            get
            {
                return Misc.TeamTotal(Player.Instance.ServerPosition) > Misc.TeamTotal(Player.Instance.ServerPosition, true)
                       && Player.Instance.CountAlliesInRange(800) >= Player.Instance.CountEnemiesInRange(800)
                       && Player.Instance.CountEnemiesInRange(800) > 0 && !Player.Instance.ServerPosition.UnderEnemyTurret();
            }
        }

        /// <summary>
        ///     Returns True if Harass needs to be active.
        /// </summary>
        public static bool Harass
        {
            get
            {
                return (Misc.TeamTotal(Player.Instance.ServerPosition) < Misc.TeamTotal(Player.Instance.ServerPosition, true)
                        || Player.Instance.IsUnderHisturret()) && Player.Instance.CountEnemiesInRange(800) > 0
                       && !Player.Instance.ServerPosition.UnderEnemyTurret() && !Flee;
            }
        }

        /// <summary>
        ///     Returns True if LaneClear needs to be active.
        /// </summary>
        public static bool LaneClear
        {
            get
            {
                return Player.Instance.CountEnemiesInRange(1000) < 1 && !Flee
                       && (Player.Instance.CountAlliesInRange(800) > 1 || Player.Instance.CountMinions() > 0)
                       && (Player.Instance.CountMinions(true) > 0 || AttackObject);
            }
        }

        /// <summary>
        ///     Returns True if Flee needs to be active.
        /// </summary>
        public static bool Flee
        {
            get
            {
                return !Player.Instance.IsUnderHisturret()
                       && ((Misc.TeamTotal(Player.Instance.ServerPosition) < Misc.TeamTotal(Player.Instance.ServerPosition, true)
                            && Player.Instance.CountAlliesInRange(800) < 2) || (Player.Instance.IsUnderEnemyturret() && !Misc.SafeToDive)
                           || (Player.Instance.CountEnemiesInRange(800) > Player.Instance.CountAlliesInRange(800)));
            }
        }

        /// <summary>
        ///     Returns True if No modes are active.
        /// </summary>
        public static bool None
        {
            get
            {
                return !Combo && !Harass && !LaneClear && !Flee;
            }
        }

        /// <summary>
        ///     Returns True if Can attack objects.
        /// </summary>
        public static bool AttackObject
        {
            get
            {
                return (ObjectsManager.EnemyNexues != null
                        && ObjectsManager.EnemyNexues.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange() + 30))
                       || (ObjectsManager.EnemyInhb != null
                           && ObjectsManager.EnemyInhb.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange() + 30))
                       || (ObjectsManager.EnemyTurret != null
                           && ObjectsManager.EnemyTurret.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange() + 30));
            }
        }
    }
}