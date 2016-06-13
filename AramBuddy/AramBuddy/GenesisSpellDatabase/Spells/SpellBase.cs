#region

using System;
using System.Collections.Generic;

#endregion

namespace GenesisSpellLibrary.Spells
{
    public abstract class SpellBase
    {
        public bool EisCC = false;

        public bool EisDash = false;

        public bool EisToggle = false;

        public Dictionary<string, Func<AIHeroClient, Obj_AI_Base, bool>> LogicDictionary;

        public Dictionary<string, object> Options;

        public bool QisCC = false;

        public bool QisDash = false;

        public bool QisToggle = false;

        public bool RisCC = false;

        public bool RisDash = false;

        public bool RisToggle = false;

        public bool WisCC = false;

        public bool WisDash = false;

        public bool WisToggle = false;
        public abstract Spell.SpellBase Q { get; set; }

        public abstract Spell.SpellBase W { get; set; }

        public abstract Spell.SpellBase E { get; set; }

        public abstract Spell.SpellBase R { get; set; }
    }
}