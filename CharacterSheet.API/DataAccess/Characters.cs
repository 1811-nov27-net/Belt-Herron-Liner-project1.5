﻿using ClassLibrary;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Characters
    {
        public Characters()
        {
            Classes = new HashSet<Classes>();
            Feats = new HashSet<Feats>();
            Inventory = new HashSet<Inventory>();
            Skills = new HashSet<Skills>();
            SpellJunction = new HashSet<SpellJunction>();
            SpellSlots = new HashSet<SpellSlots>();
        }

        public int CharacterId { get; set; }
        public int GamerId { get; set; }
        public string CharacterName { get; set; }
        public int? CampaignId { get; set; }
        public string Race { get; set; }
        public string Sex { get; set; }
        public string Alignment { get; set; }
        public int MaxHP { get; set; }  
        public int Bab { get; set; }
        public int Ac { get; set; }
        public int TouchAc { get; set; }
        public int Ffac { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int BaseFortSave { get; set; }
        public int BaseReflexSave { get; set; }
        public int BaseWillSave { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Gamer Gamer { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
        public virtual ICollection<Feats> Feats { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Skills> Skills { get; set; }
        public virtual ICollection<SpellJunction> SpellJunction { get; set; }
        public virtual ICollection<SpellSlots> SpellSlots { get; set; }

        public static implicit operator Characters(Character character)  // implicit conversion, takes in library, returns data access
        {
            Characters ret = new Characters
            {
                CharacterId = character.CharID,
                GamerId = character.UserID,
                CampaignId = character.CampID,
                CharacterName = character.Name,
                Race = character.Race,
                Sex = character.Sex,
                Alignment = character.Alignment,
                MaxHP = character.MaxHitPoints,
                Bab = character.BaseAttackBonus,
                Ac = character.AC[0],
                TouchAc = character.AC[1],
                Ffac = character.AC[2],
                

            };

            if(character.Attributes.Count > 0)
            {
                ret.Strength = character.Attributes["Strength"];
                ret.Dexterity = character.Attributes["Dexterity"];
                ret.Stamina = character.Attributes["Stamina"];
                ret.Intelligence = character.Attributes["Intelligence"];
                ret.Charisma = character.Attributes["Charisma"];
                ret.Wisdom = character.Attributes["Wisdom"];

            }
            else
            {
                ret.Strength = 10;
                ret.Dexterity = 10;
                ret.Stamina = 10;
                ret.Intelligence = 10;
                ret.Charisma = 10;
                ret.Wisdom = 10;

            }

            if(character.BaseSavingThrows.Count > 0)
            {
                ret.BaseFortSave = character.BaseSavingThrows["Fort"];
                ret.BaseReflexSave = character.BaseSavingThrows["Reflex"];
                ret.BaseWillSave = character.BaseSavingThrows["Will"];

            }
            else
            {
                ret.BaseFortSave = 0;
                ret.BaseReflexSave = 0;
                ret.BaseWillSave = 0;

            }

            if (character.ClassLevels.Count > 0)
            {
                foreach (var ClassKVP in character.ClassLevels)
                {
                    ret.Classes.Add(new Classes()
                    {
                        ClassName = ClassKVP.Key,
                        Levels = ClassKVP.Value,
                        CharacterId = ret.CharacterId,
                        Character = ret
                    });
                }
            }

            if (character.FeatList.Count > 0)
            {
                foreach (var FeatKVP in character.FeatList)
                {
                    ret.Feats.Add(new Feats()
                    {
                        FeatName = FeatKVP.Key,
                        Copies = FeatKVP.Value,
                        CharacterId = ret.CharacterId,
                        Character = ret
                    });
                }
            }

            if (character.SkillList.Count > 0)
            {
                foreach (var SkillKVP in character.SkillList)
                {
                    ret.Skills.Add(new Skills()
                    {
                        SkillName = SkillKVP.Key,
                        Levels = SkillKVP.Value,
                        CharacterId = ret.CharacterId,
                        Character = ret
                    });

                }
            }

            if (character.Inventory.Count > 0)
            {
                foreach (var InvKVP in character.Inventory)
                {
                    ret.Inventory.Add(new Inventory()
                    {
                        ItemName = InvKVP.Key,
                        Quantity = InvKVP.Value,
                        CharacterId = ret.CharacterId,
                        Character = ret
                    });

                }
            }

            if(character.SpellSlots.Count > 0) {
                foreach (var SSKVP in character.SpellSlots)
                {
                    SpellSlots sstemp = new SpellSlots()
                    {
                        CharacterId = ret.CharacterId,
                        Character = ret,
                        ClassName = SSKVP.Key,

                    };

                    switch (SSKVP.Value.Length)
                    {
                        case 10:
                            sstemp.Level9Slots = SSKVP.Value[9];
                            goto case 9;
                        case 9:
                            sstemp.Level8Slots = SSKVP.Value[8];
                            goto case 8;
                        case 8:
                            sstemp.Level7Slots = SSKVP.Value[7];
                            goto case 7;
                        case 7:
                            sstemp.Level6Slots = SSKVP.Value[6];
                            goto case 6;
                        case 6:
                            sstemp.Level5Slots = SSKVP.Value[5];
                            goto case 5;
                        case 5:
                            sstemp.Level4Slots = SSKVP.Value[4];
                            goto case 4;
                        case 4:
                            sstemp.Level3Slots = SSKVP.Value[3];
                            goto case 3;
                        case 3:
                            sstemp.Level2Slots = SSKVP.Value[2];
                            goto case 2;
                        case 2:
                            sstemp.Level1Slots = SSKVP.Value[1];
                            goto case 1;
                        case 1:
                            sstemp.Level0Slots = SSKVP.Value[0];
                            break;

                        default:
                            break;
                    }


                    ret.SpellSlots.Add(sstemp);
                }

                if (character.SpellsKnown.Count > 0)
                {
                    foreach (var SpK in character.SpellsKnown)
                    {
                        Spells tempSpell = new Spells()
                        {
                            SpellId = SpK.SpellId,
                            SpellName = SpK.Name,
                            Class = SpK.Class,
                            SpellLevel = SpK.SpellLevel
                        };


                        ret.SpellJunction.Add(new SpellJunction()
                        {
                            CharacterId = ret.CharacterId,
                            SpellId = SpK.SpellId,
                            Character = ret,
                            Spell = tempSpell
                        });

                    }
                }
}

            return ret;

        }

        public static implicit operator Character(Characters character)  // implicit conversion, takes in data access, returns library
        {
            Character ret = new Character()
            {
                CharID = character.CharacterId,
                UserID = character.GamerId,
                Name = character.CharacterName,
                CampID = character.CampaignId ?? 0,
                Race = character.Race,
                Sex = character.Sex,
                Alignment = character.Alignment,
                MaxHitPoints = character.MaxHP,
                BaseAttackBonus = character.Bab,


            };
            ret.AC[0] = character.Ac;
            ret.Attributes["Strength"] = character.Strength;
            ret.Attributes["Dexterity"] = character.Dexterity;
            ret.Attributes["Stamina"] = character.Stamina;
            ret.Attributes["Intelligence"] = character.Intelligence;
            ret.Attributes["Charisma"] = character.Charisma;
            ret.Attributes["Wisdom"] = character.Wisdom;
            ret.BaseSavingThrows["Fort"] = character.BaseFortSave;
            ret.BaseSavingThrows["Reflex"] = character.BaseReflexSave;
            ret.BaseSavingThrows["Will"] = character.BaseWillSave;
            if (character.Classes.Count > 0)
            {
                foreach (var Klass in character.Classes)
                {
                    ret.ClassLevels.Add(Klass.ClassName, Klass.Levels);
                }
            }
            if (character.Skills.Count > 0)
            {
                foreach (var skillz in character.Skills)
                {
                    ret.SkillList.Add(skillz.SkillName, skillz.Levels);
                }
            }

            if (character.Feats.Count > 0)
            {
                foreach (var feat in character.Feats)
                {
                    ret.FeatList.Add(feat.FeatName, feat.Copies);
                }
            }

            if (character.Inventory.Count > 0)
            {
                foreach (var item in character.Inventory)
                {
                    ret.Inventory.Add(item.ItemName, item.Quantity);
                }
            }

            if (character.SpellJunction.Count > 0)
            {
                foreach (var spellKnown in character.SpellJunction)
                {
                    Spell tempSpell = new Spell()
                    {
                        SpellId = spellKnown.SpellId,
                        Name = spellKnown.Spell.SpellName,
                        Class = spellKnown.Spell.Class,
                        SpellLevel = spellKnown.Spell.SpellLevel
                    };

                    ret.SpellsKnown.Add(tempSpell);

                }
            }

            if (character.SpellSlots.Count > 0)
            {
                foreach (var SSlotsDB in character.SpellSlots)
                {
                    int[] intArray = new int[10];
                    intArray[9] = SSlotsDB.Level9Slots ?? 0;
                    intArray[8] = SSlotsDB.Level8Slots ?? 0;
                    intArray[7] = SSlotsDB.Level7Slots ?? 0;
                    intArray[6] = SSlotsDB.Level6Slots ?? 0;
                    intArray[5] = SSlotsDB.Level5Slots ?? 0;
                    intArray[4] = SSlotsDB.Level4Slots ?? 0;
                    intArray[3] = SSlotsDB.Level3Slots ?? 0;
                    intArray[2] = SSlotsDB.Level2Slots ?? 0;
                    intArray[1] = SSlotsDB.Level1Slots ?? 0;
                    intArray[0] = SSlotsDB.Level0Slots ?? 0;

                    ret.SpellSlots.Add(SSlotsDB.ClassName, intArray);
                }
            }
            ret.CalculateBonusesAndSaves();

            return ret;
        }
    }
}
