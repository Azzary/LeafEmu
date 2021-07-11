using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class GlyphAndTrap
    {
        public List<int> ListCell = new List<int>();
        public int BaseCell;
        private Entity.Entity Laucher;
        public int LifeTime;
        private SpellsStats spellsStat;
        public bool IsTrap;
        private int id;
        private int LenTrap;
        public GlyphAndTrap(int _id, Entity.Entity _Laucher, int _baseCell,   int CurrentCell , int _LenTrap, int _LifeTime, SpellsStats _spellsStat, bool _IsTrap)
        {
            LenTrap = _LenTrap;
            id = _id;
            ListCell.Add(CurrentCell);
            IsTrap = _IsTrap;
            spellsStat = _spellsStat;
            LifeTime = _LifeTime;
            Laucher = _Laucher;
            BaseCell = _baseCell;
        }

        public void ActionGliphEntity(Entity.Entity entity)
        {
            foreach (SpellsEffect.Spells Spell in spellsStat.effects)
            {
                    EffectGestion.Gestion(entity, Spell, entity.FightInfo.FightCell,
                    new List<Map.Cell.Cell>() { entity.CurrentFight.map.Cells[entity.FightInfo.FightCell] });
            }
        }

        public void Remove()
        {
            Laucher.CurrentFight.glyphAndTrapsOnMap.Remove(this);
            Laucher.CurrentFight.SendToAllFight("GA;999;" + Laucher.ID_InFight + ";GDZ-" + BaseCell + ";" + LenTrap + ";7");
            Laucher.CurrentFight.SendToAllFight("GA;999;" + Laucher.ID_InFight + ";GDC" + BaseCell);
        }

        /// <summary>
        /// Don't Remove is for refection
        /// </summary>
        public GlyphAndTrap()
        {
            ListCell = null;
        }

        [EffectAttribute(new int[2] { 400, 401 })]
        public static byte SetGlypheAndTrap(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            if (isFristTime)
            {
                if (laucher.CurrentFight.glyphAndTrapsOnMap.Exists(x => x.BaseCell == BaseCellTarget))
                    return 0;
                int LenTrap = 2;
                bool IsTrap = spells.effectID == 400;
                int Color = Convert.ToInt32(spells.args.Split(';')[2]);
                int id = laucher.ID_InFight + Util.rng.Next(2000, 5000);
                switch (spells.SpellsID)
                {
                    //Repul
                    case 73:
                        LenTrap = 1;
                        break;
                    //Mortel
                    //Sournoi
                    case 80:
                    case 65:
                        LenTrap = 0;
                        break;
                    //De masse
                    case 79:
                        LenTrap = 2;
                        break;
                    //Aveuglement
                    case 12:
                        if (spells.SpellsLvl > 5)
                            LenTrap++;
                        break;
                    //Agressif
                    case 17:
                        LenTrap = 2;
                        break;
                    //Emflammé
                    case 10:
                        LenTrap = 1 + (int)MathF.Floor(spells.SpellsLvl / 4);
                        if (spells.SpellsLvl >= 5)
                            LenTrap++;
                        break;
                    //Immobilisation
                    case 15:
                        LenTrap = 2 + (int)MathF.Floor(spells.SpellsLvl / 5);
                        if (spells.SpellsLvl >= 3)
                            LenTrap++;
                        break;
                    //Silence
                    case 13:
                        LenTrap = 1 + (int)MathF.Floor(spells.SpellsLvl / 5);
                        if (spells.SpellsLvl >= 2)
                            LenTrap++;
                        break;

                    default:
                        Logger.Logger.Warning($"Glyph or Trap {spells.SpellsID} is not set");
                        return 0;
                }
                laucher.CurrentFight.glyphAndTrapsOnMap.Add(new GlyphAndTrap(id, laucher, BaseCellTarget, TargetCell, LenTrap, laucher.CurrentFight.nbTour + spells.duration,
                    Database.table.Spells.SpellsList[(int)spells.value].SpellsStats[spells.SpellsLvl - 1], IsTrap));

                laucher.CurrentFight.SendToAllFight($"GA;300;{laucher.id};{spells.SpellsID},{BaseCellTarget},0,{spells.SpellsLvl},1\0" +
                                                $"GA;999;{id};GDZ+{BaseCellTarget};{LenTrap};{Color}\0" +
                                                $"GA;999;{id};GDC{BaseCellTarget};Haaaaaaaaa3005;\0",
                                                IsTrap ? laucher.FightInfo.equipeID : -1);

            }
            else
                laucher.CurrentFight.glyphAndTrapsOnMap.Find(x => BaseCellTarget == x.BaseCell).ListCell.Add(TargetCell);
            return 1;
        }

    }
}