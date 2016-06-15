namespace AramBuddy.MainCore.Positioning
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Modes;

    using SharpDX;

    using Utility;

    internal class Pathing
    {
        /// <summary>
        ///     Bot movements position.
        /// </summary>
        public static Vector3 Position;

        /// <summary>
        ///     Picking best Position to move to.
        /// </summary>
        public static void BestPosition()
        {
            // Moves to HealthRelic if the bot needs heal.
            if ((Player.Instance.HealthPercent < 70 || Player.Instance.ManaPercent < 10) && ObjectsManager.HealthRelic != null)
            {
                Program.Moveto = "HealthRelic";
                Position = ObjectsManager.HealthRelic.Position.Random();
                return;
            }

            // Stays Under tower if the bot health under 15%.
            if (((Player.Instance.HealthPercent < 15 && Player.Instance.CountAlliesInRange(700) < 3) || ModesManager.Flee)
                && ObjectsManager.SafeAllyTurret != null)
            {
                Program.Moveto = "SafeAllyTurret";
                Position = ObjectsManager.SafeAllyTurret.ServerPosition.Random();
                return;
            }

            // Moves to AllyNexues if the bot is diving and it's not safe to dive.
            if (Player.Instance.IsUnderEnemyturret() && !Misc.SafeToDive && ObjectsManager.AllyNexues != null)
            {
                Program.Moveto = "AllyNexues1";
                Position = ObjectsManager.AllyNexues.Position.Random();
                return;
            }

            /*
            foreach (var point in (from circle in ObjectsManager.BestAlliesToFollow.Select(a => new Geometry.Polygon.Circle(a.ServerPosition, 400, 1)).ToList()
                                   from point in circle.Points
                                   where Misc.TeamTotal(point.To3D()) > Misc.TeamTotal(point.To3D(), true)
                                   select point).Where(point => point != null))
            {
                Program.Moveto = "AllyGroup";
                Position = point.To3D().Random();
                return;
            }
            */

            if (Player.Instance.IsMelee)
            {
                MeleeLogic();
            }
            else
            {
                RangedLogic();
            }
        }

        /// <summary>
        ///     Melee Champions Logic.
        /// </summary>
        public static void MeleeLogic()
        {
            // if SafestAllyToFollow not exsist picks other to follow.
            if (ObjectsManager.SafestAllyToFollow == null)
            {
                // if FarthestAllyToFollow exsists moves to FarthestAllyToFollow.
                if (ObjectsManager.FarthestAllyToFollow != null)
                {
                    Program.Moveto = "FarthestAllyToFollow";
                    Position = ObjectsManager.FarthestAllyToFollow.ServerPosition.Random();
                }
                else
                {
                    // if Minion exsists moves to Minion.
                    if (ObjectsManager.Minion != null)
                    {
                        Program.Moveto = "Minion";
                        Position = ObjectsManager.Minion.ServerPosition.Random();
                    }
                    else
                    {
                        // if SecondTurret exsists moves to SecondTurret.
                        if (ObjectsManager.SecondTurret != null)
                        {
                            Program.Moveto = "SecondTurret";
                            Position = ObjectsManager.SecondTurret.ServerPosition.Random();
                        }
                        else
                        {
                            // if SafeAllyTurret exsists moves to SafeAllyTurret.
                            if (ObjectsManager.SafeAllyTurret != null)
                            {
                                Program.Moveto = "SafeAllyTurret";
                                Position = ObjectsManager.SafeAllyTurret.ServerPosition.Random();
                            }
                            else
                            {
                                // if ClosesetAllyTurret exsists moves to ClosesetAllyTurret.
                                if (ObjectsManager.ClosesetAllyTurret != null)
                                {
                                    Program.Moveto = "ClosesetAllyTurret";
                                    Position = ObjectsManager.ClosesetAllyTurret.ServerPosition.Random();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // if SafestAllyToFollow exsist follow BestAllyToFollow.
                Program.Moveto = "SafestAllyToFollow";
                Position = ObjectsManager.SafestAllyToFollow.ServerPosition.Random();
            }
        }

        /// <summary>
        ///     Ranged Champions Logic.
        /// </summary>
        public static void RangedLogic()
        {
            // if SafestAllyToFollow2 not exsist picks other to follow.
            if (ObjectsManager.SafestAllyToFollow2 == null)
            {
                // if Minion exsists moves to Minion.
                if (ObjectsManager.Minion != null)
                {
                    Program.Moveto = "Minion";
                    Position = ObjectsManager.Minion.ServerPosition.Random();
                }
                else
                {
                    // if SecondTurret exsists moves to SecondTurret.
                    if (ObjectsManager.SecondTurret != null)
                    {
                        Program.Moveto = "SecondTurret";
                        Position = ObjectsManager.SecondTurret.ServerPosition.Random();
                    }
                    else
                    {
                        // if SafeAllyTurret exsists moves to SafeAllyTurret.
                        if (ObjectsManager.SafeAllyTurret != null)
                        {
                            Program.Moveto = "SafeAllyTurret";
                            Position = ObjectsManager.SafeAllyTurret.ServerPosition.Random();
                        }
                        else
                        {
                            // if ClosesetAllyTurret exsists moves to ClosesetAllyTurret.
                            if (ObjectsManager.ClosesetAllyTurret != null)
                            {
                                Program.Moveto = "ClosesetAllyTurret";
                                Position = ObjectsManager.ClosesetAllyTurret.ServerPosition.Random();
                            }
                        }
                    }
                }
            }
            else
            {
                // if SafestAllyToFollow exsist follow BestAllyToFollow.
                Program.Moveto = "SafestAllyToFollow";
                Position = ObjectsManager.SafestAllyToFollow2.ServerPosition.Random();
            }
        }

        /// <summary>
        ///     Returns last move time for the bot.
        /// </summary>
        private static float lastmove;

        /// <summary>
        ///     Sends movement commands.
        /// </summary>
        public static void MoveTo(Vector3 pos)
        {
            // This to prevent the bot from spamming unnecessary movements.
            if (!Player.Instance.Path.LastOrDefault().IsInRange(pos, 100) && !Player.Instance.IsInRange(pos, 100)
                && Core.GameTickCount - lastmove >= 666)
            {
                // This to prevent diving.
                if (pos.UnderEnemyTurret() && !Misc.SafeToDive)
                {
                    return;
                }

                // This to prevent Walking into walls / buildings.
                if (NavMesh.GetCollisionFlags(Position) == CollisionFlags.Wall || NavMesh.GetCollisionFlags(Position) == CollisionFlags.Building)
                {
                    return;
                }

                // If the bot alone uses IssueOrder.
                if (Orbwalker.DisableMovement)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, pos);
                }
                else
                {
                    Orbwalker.OrbwalkTo(pos);
                }

                // Returns last movement time.
                lastmove = Core.GameTickCount;
            }
        }
    }
}