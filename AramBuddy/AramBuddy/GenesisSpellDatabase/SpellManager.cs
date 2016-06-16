namespace GenesisSpellLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AramBuddy.GenesisSpellDatabase;

    using Spells;

    using EloBuddy;
    using EloBuddy.SDK;

    public static class SpellManager
    {
        static SpellManager()
        {
            try
            {
                CurrentSpells = SpellLibrary.GetSpells(Player.Instance.Hero);
                SpellsDictionary = new Dictionary<AIHeroClient, SpellBase>();
            }
            catch (Exception ex)
            {
                // Exception has been cought; Notify the user of the error and print the exception to the console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Error] Exception occurred on initialization of Genesis SpellManager."
                    + Environment.NewLine);
                Console.ResetColor();
                Console.Write(ex);
            }
        }

        public static SpellBase CurrentSpells { get; set; }

        public static Dictionary<AIHeroClient, SpellBase> SpellsDictionary { get; set; }

        public static float GetRange(SpellSlot slot, AIHeroClient sender)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return SpellsDictionary.FirstOrDefault(x => x.Key == sender).Value.Q.Range;
                case SpellSlot.W:
                    return SpellsDictionary.FirstOrDefault(x => x.Key == sender).Value.W.Range;
                case SpellSlot.E:
                    return SpellsDictionary.FirstOrDefault(x => x.Key == sender).Value.E.Range;
                case SpellSlot.R:
                    return SpellsDictionary.FirstOrDefault(x => x.Key == sender).Value.R.Range;
                default:
                    return 0;
            }
        }

        public static bool IsCC(this Spell.SpellBase spell)
        {
            switch (spell.Slot)
            {
                case SpellSlot.Q:
                    return SpellsDictionary.FirstOrDefault().Value.QisCC;
                case SpellSlot.W:
                    return SpellsDictionary.FirstOrDefault().Value.WisCC;
                case SpellSlot.E:
                    return SpellsDictionary.FirstOrDefault().Value.EisCC;
                case SpellSlot.R:
                    return SpellsDictionary.FirstOrDefault().Value.RisCC;
                default:
                    return false;
            }
        }

        public static bool IsDash(this Spell.SpellBase spell)
        {
            switch (spell.Slot)
            {
                case SpellSlot.Q:
                    return SpellsDictionary.FirstOrDefault().Value.QisDash;
                case SpellSlot.W:
                    return SpellsDictionary.FirstOrDefault().Value.WisDash;
                case SpellSlot.E:
                    return SpellsDictionary.FirstOrDefault().Value.EisDash;
                case SpellSlot.R:
                    return SpellsDictionary.FirstOrDefault().Value.RisDash;
                default:
                    return false;
            }
        }

        public static bool IsToggle(this Spell.SpellBase spell)
        {
            switch (spell.Slot)
            {
                case SpellSlot.Q:
                    return SpellsDictionary.FirstOrDefault().Value.QisToggle;
                case SpellSlot.W:
                    return SpellsDictionary.FirstOrDefault().Value.WisToggle;
                case SpellSlot.E:
                    return SpellsDictionary.FirstOrDefault().Value.EisToggle;
                case SpellSlot.R:
                    return SpellsDictionary.FirstOrDefault().Value.RisToggle;
                default:
                    return false;
            }
        }

        public static void Initialize()
        {
            try
            {
                PrepareSpells(Player.Instance);
            }
            catch (Exception ex)
            {
                // Exception has been cought; Notify the user of the error and print the exception to the console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    DateTime.Now.ToString("[H:mm:ss - ") + "AramBuddy Error] Exception occurred on PrepareSpells of Genesis SpellManager."
                    + Environment.NewLine);
                Console.ResetColor();
                Console.Write(ex);
            }
        }

        public static void PrepareSpells(AIHeroClient hero)
        {
            SpellBase spells = SpellLibrary.GetSpells(hero.Hero);
            //This only needs to be called once per champion, anymore is a memory leak.
            if (spells != null)
            {
                SpellsDictionary.Add(hero, spells);
            }
        }
    }
}