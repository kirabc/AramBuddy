#region

using AramBuddy.MainCore.Utility;

#endregion

namespace AramBuddy.MainCore.Positioning
{
    internal class Pathing
    {
        internal static Vector3 Position;

        public static float lastmove;

        public static void BestPosition()
        {
            if (((Player.Instance.HealthPercent < 70) || (Player.Instance.ManaPercent < 10)) &&
                (ObjectsManager.HealthRelic != null))
            {
                Program.Moveto = "HealthRelic";
                Position = ObjectsManager.HealthRelic.Position.Random();
                return;
            }
            if (Player.Instance.ServerPosition.UnderEnemyTurret() && !Misc.SafeToDive &&
                (ObjectsManager.AllyNexues != null))
            {
                Program.Moveto = "AllyNexues1";
                Position = ObjectsManager.AllyNexues.Position.Random();
                return;
            }
            if (ObjectsManager.BestAllyToFollow == null)
            {
                if (ObjectsManager.Minion != null)
                {
                    Program.Moveto = "Minion";
                    Position = ObjectsManager.Minion.ServerPosition.Random();
                }
                else
                {
                    if (ObjectsManager.SecondTurret != null)
                    {
                        Program.Moveto = "SecondTurret";
                        Position = ObjectsManager.SecondTurret.ServerPosition.Random();
                    }
                    else
                    {
                        if (ObjectsManager.SafeAllyTurret != null)
                        {
                            Program.Moveto = "SafeAllyTurret";
                            Position = ObjectsManager.SafeAllyTurret.ServerPosition.Random();
                        }
                        else
                        {
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
                Program.Moveto = "BestAllyToFollow";
                Position = ObjectsManager.BestAllyToFollow.ServerPosition.Random();
            }
            /*
            if (Player.Instance.CountEnemiesInRange(800) > 0)
            {
                if (ObjectsManager.NearestEnemy != null && ObjectsManager.AllyNexues != null)
                {
                    Program.Moveto = "AllyNexues2";
                    Position = ObjectsManager.AllyNexues.Position;
                }
            }
            */
            if ((Position != null) && (NavMesh.GetCollisionFlags(Position) != CollisionFlags.Wall))
            {
                MoveTo(Position);
            }
        }

        public static void MoveTo(Vector3 pos)
        {
            if (!Player.Instance.Path.LastOrDefault().IsInRange(pos, 100) && !Player.Instance.IsInRange(pos, 100) &&
                !Orbwalker.IsAutoAttacking && (Core.GameTickCount - lastmove > 500))
            {
                if (pos.UnderEnemyTurret() && !Misc.SafeToDive)
                {
                    return;
                }
                if (Brain.Alone())
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, pos);
                }
                else
                {
                    Orbwalker.OrbwalkTo(pos);
                }
                lastmove = Core.GameTickCount;
            }
        }

        public static void MoveTo(Obj_AI_Base target)
        {
            if (!Player.Instance.Path.LastOrDefault().IsInRange(target, 150) && !Player.Instance.IsInRange(target, 150) &&
                !Orbwalker.IsAutoAttacking && (Core.GameTickCount - lastmove > 500))
            {
                if (target.IsUnderEnemyturret() && !Misc.SafeToDive)
                {
                    return;
                }
                if (Brain.Alone())
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, target.ServerPosition.Random());
                }
                else
                {
                    Orbwalker.OrbwalkTo(target.ServerPosition.Random());
                }
                lastmove = Core.GameTickCount;
            }
        }
    }
}