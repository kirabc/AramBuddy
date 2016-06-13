namespace AramBuddy.MainCore.Utility
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    internal static class Misc
    {
        public static float TeamTotal(Obj_AI_Base target, bool Enemy = false, int range = 1250)
        {
            float enemyteamTotal = 0;
            float allyteamTotal = 0;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget() && e.IsInRange(target, range)))
            {
                enemyteamTotal += enemy.TotalShieldHealth();
                enemyteamTotal += enemy.Mana;
                enemyteamTotal += enemy.Armor;
                enemyteamTotal += enemy.SpellBlock;
                enemyteamTotal += enemy.TotalMagicalDamage;
                enemyteamTotal += enemy.TotalAttackDamage;
            }

            foreach (var ally in EntityManager.Heroes.Allies.Where(e => e.IsValidTarget() && e.IsInRange(target, range)))
            {
                allyteamTotal += ally.TotalShieldHealth();
                allyteamTotal += ally.Mana;
                allyteamTotal += ally.Armor;
                allyteamTotal += ally.SpellBlock;
                allyteamTotal += ally.TotalMagicalDamage;
                allyteamTotal += ally.TotalAttackDamage;
            }
            return Enemy ? enemyteamTotal : allyteamTotal;
        }

        public static bool SafeToDive
        {
            get
            {
                return ObjectsManager.EnemyTurret != null && (ObjectsManager.EnemyTurret.CountMinions() > 2 || ObjectsManager.EnemyTurret.CountAlliesInRange(800) > 1);
            }
        }

        public static bool UnderEnemyTurret(this Vector3 pos)
        {
            return EntityManager.Turrets.Enemies.Any(t => !t.IsDead && t.IsValidTarget() && t.IsInRange(pos, t.GetAutoAttackRange() + 20));
        }

        public static float CountMinions(this Obj_AI_Base target, bool EnemyMinions = false, int range = 800)
        {
            return EnemyMinions
                       ? EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => m.IsValidTarget() && m.IsInRange(target, range))
                       : EntityManager.MinionsAndMonsters.AlliedMinions.Count(m => m.IsValidTarget() && m.IsInRange(target, range));
        }

        public static Vector3 Random(this Vector3 pos)
        {
            var rnd = new Random();
            var X = rnd.Next((int)(pos.X - 200), (int)(pos.X + 200));
            var Y = rnd.Next((int)(pos.Y - 200), (int)(pos.Y + 200));

            return new Vector3(X, Y, pos.Z);
        }
    }
}