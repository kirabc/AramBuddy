using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAram.MainCore
{
    using EloBuddy;
    using EloBuddy.SDK;

    class ObjectsManager
    {
        public static AIHeroClient BestAllyToFollow
        {
            get
            {
                return
                    EntityManager.Heroes.Allies.OrderByDescending(a => a.CountAlliesInRange(800) - a.CountEnemiesInRange(800))
                        .ThenBy(a => a.Distance(Player.Instance))
                        .FirstOrDefault(a => a.IsValidTarget() && !a.IsUnderEnemyturret() && !a.IsInShopRange() && !a.IsDead && !a.IsZombie && !a.IsMe);
            }
        }

        public static Obj_AI_Minion Minion
        {
            get
            {
                return
                    EntityManager.MinionsAndMonsters.AlliedMinions.OrderBy(m => m.Distance(Player.Instance))
                        .FirstOrDefault(m => m.IsValidTarget() && m.CountAlliesInRange(750) >= m.CountEnemiesInRange(750));
            }
        }

        public static Obj_AI_Turret SecondTurret
        {
            get
            {
                return Player.Instance.Team == GameObjectTeam.Order ? EntityManager.Turrets.Allies.FirstOrDefault(t => t.IsValidTarget() && !t.IsDead && t.BaseSkinName.ToLower().Equals("ha_ap_orderturret")) : EntityManager.Turrets.Allies.FirstOrDefault(t => t.IsValidTarget() && !t.IsDead && t.BaseSkinName.ToLower().Equals("ha_ap_chaosturret"));
            }
        }

        public static Obj_AI_Turret ClosesetAllyTurret
        {
            get
            {
                return EntityManager.Turrets.Allies.FirstOrDefault(t => t.IsValidTarget() && !t.IsDead);
            }
        }

        public static Obj_AI_Turret SafeAllyTurret
        {
            get
            {
                return EntityManager.Turrets.Allies.FirstOrDefault(t => t.IsValidTarget() && !t.IsDead && t.CountAlliesInRange(t.GetAutoAttackRange()) >= t.CountEnemiesInRange(t.GetAutoAttackRange()));
            }
        }

        public static Obj_AI_Minion HealthRelic
        {
            get
            {
                return ObjectManager.Get<Obj_AI_Minion>().OrderBy(hr => hr.Distance(Player.Instance)).FirstOrDefault(hr => hr.BaseSkinName.ToLower().Equals("ha_ap_healthrelic") && hr.IsValid && hr.CountEnemiesInRange(900) < 2 && !hr.IsUnderEnemyturret());
            }
        }
    }
}
