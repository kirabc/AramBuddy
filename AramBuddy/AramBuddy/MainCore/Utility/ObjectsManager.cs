namespace AramBuddy.MainCore.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class ObjectsManager
    {
        public static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name.ToLower().Contains("healthrelic"))
            {
                HealthRelics.Add(sender);
                Chat.Print("create");
            }
        }

        public static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name.ToLower().Contains("healthrelic"))
            {
                HealthRelics.Remove(sender);
                Chat.Print("delete");
            }
        }

        public static List<GameObject> HealthRelics = new List<GameObject>();

        public static AIHeroClient NearestEnemy
        {
            get
            {
                return
                    EntityManager.Heroes.Enemies.OrderBy(e => e.Distance(Player.Instance))
                        .FirstOrDefault(e => e.IsValidTarget() && !e.IsDead && !e.IsZombie);
            }
        }

        public static AIHeroClient BestAllyToFollow
        {
            get
            {
                return
                    EntityManager.Heroes.Allies.OrderByDescending(a => Misc.TeamTotal(a) - Misc.TeamTotal(a, true))
                        .ThenByDescending(a => a.Distance(AllyNexues)).ThenBy(a => a.Distance(Player.Instance))
                        .FirstOrDefault(a => a.IsValidTarget() && a.HealthPercent > 10 && !a.IsInShopRange() && !a.IsDead && !a.IsZombie && !a.IsMe);
            }
        }

        public static GameObject HealthRelic
        {
            get
            {
                return
                    HealthRelics.OrderBy(e => e.Distance(Player.Instance))
                        .FirstOrDefault(e => e.IsValid && e.Distance(Player.Instance) < 800 && e.CountEnemiesInRange(1000) < 1);
            }
        }

        public static Obj_AI_Minion Minion
        {
            get
            {
                return 
                    EntityManager.MinionsAndMonsters.AlliedMinions.OrderByDescending(a => a.Distance(AllyNexues))
                        .FirstOrDefault(
                            m =>
                            m.IsValidTarget() && m.IsValid && m.IsHPBarRendered && !m.IsDead && m.Health > 65
                            && m.CountAlliesInRange(800) >= m.CountEnemiesInRange(800));
            }
        }

        public static Obj_AI_Turret SecondTurret
        {
            get
            {
                return Player.Instance.Team == GameObjectTeam.Order
                           ? EntityManager.Turrets.Allies.FirstOrDefault(
                               t => t.IsValidTarget() && !t.IsDead && t.BaseSkinName.ToLower().Equals("ha_ap_orderturret"))
                           : EntityManager.Turrets.Allies.FirstOrDefault(
                               t => t.IsValidTarget() && !t.IsDead && t.BaseSkinName.ToLower().Equals("ha_ap_chaosturret"));
            }
        }

        public static Obj_AI_Turret ClosesetAllyTurret
        {
            get
            {
                return EntityManager.Turrets.Allies.OrderBy(t => t.Distance(Player.Instance)).FirstOrDefault(t => t.IsValidTarget() && !t.IsDead);
            }
        }

        public static Obj_AI_Turret SafeAllyTurret
        {
            get
            {
                return
                    EntityManager.Turrets.Allies.OrderBy(t => t.Distance(Player.Instance))
                        .FirstOrDefault(
                            t =>
                            t.IsValidTarget() && !t.IsDead
                            && t.CountAlliesInRange(t.GetAutoAttackRange()) > t.CountEnemiesInRange(t.GetAutoAttackRange()));
            }
        }

        public static Obj_AI_Turret EnemyTurret
        {
            get
            {
                return EntityManager.Turrets.Enemies.OrderBy(t => t.Distance(Player.Instance)).FirstOrDefault(t => t.IsValidTarget() && !t.IsDead);
            }
        }

        public static Obj_BarracksDampener EnemyInhb
        {
            get
            {
                return ObjectManager.Get<Obj_BarracksDampener>().FirstOrDefault(i => i.IsEnemy && i.IsValidTarget() && !i.IsDead);
            }
        }

        public static Obj_HQ EnemyNexues
        {
            get
            {
                return ObjectManager.Get<Obj_HQ>().FirstOrDefault(i => i.IsEnemy);
            }
        }

        public static Obj_HQ AllyNexues
        {
            get
            {
                return ObjectManager.Get<Obj_HQ>().FirstOrDefault(i => i.IsAlly);
            }
        }
    }
}