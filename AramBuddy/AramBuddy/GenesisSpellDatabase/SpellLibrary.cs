namespace AramBuddy.GenesisSpellDatabase
{
    using System;

    using EloBuddy;

    using GenesisSpellLibrary.Spells;

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
                // Exception has been cought; Notify the user of the error and print the exception to the console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Error] " + heroChampion + " is not supported Genesis Spell Library." + Environment.NewLine);
                Console.ResetColor();
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