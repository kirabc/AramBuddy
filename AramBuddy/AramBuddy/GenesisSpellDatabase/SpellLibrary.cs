namespace GenesisSpellLibrary
{
    using System;

    using Spells;

    using EloBuddy;

    internal class SpellLibrary
    {
        public static SpellBase GetSpells(Champion heroChampion)
        {
            Type championType = Type.GetType("GenesisSpellLibrary.Spells." + heroChampion);
            if (championType != null)
            {
                return Activator.CreateInstance(championType) as SpellBase;
            }

            else
            {
                Chat.Print("<font color='#FF0000'><b>AramBuddy ERROR:</b></font> " + heroChampion + " is not supported.");
                return null;
            }
        }

        public static bool IsOnCooldown(AIHeroClient hero, SpellSlot slot)
        {
            if (!hero.Spellbook.GetSpell(slot).IsLearned)
            {
                return true;
            }

            float cooldown = hero.Spellbook.GetSpell(slot).CooldownExpires - Game.Time;
            return cooldown > 0;
        }

        public static void Initialize()
        {
        }
    }
}