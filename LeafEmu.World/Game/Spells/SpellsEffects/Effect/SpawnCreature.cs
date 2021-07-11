using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System.Linq;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class SpawnCreature
    {
        [EffectAttribute(new int[1] { 181 })]
        public void Invocation(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            if (laucher.FightInfo.NbInvo > laucher.invo
               || !laucher.CurrentFight.map.Cells[TargetCell].IsWalkable()
               || laucher.CurrentFight.AllEntityInFight.Exists(x => !x.FightInfo.isDead && x.FightInfo.FightCell == TargetCell))
            {
                return;
            }
            laucher.FightInfo.NbInvo++;
            string[] infoInvo = spells.args.Split(';');
            var AllEntityInFight = laucher.CurrentFight.AllEntityInFight;
            if (Database.table.Monster.Mobs.ContainsKey(int.Parse(infoInvo[0])))
            {
                int id = -10;       
                for (int i = 0; i < AllEntityInFight.Count; i++)
                {
                    if (id == AllEntityInFight[i].id)
                    {
                        i = -1;
                        id--;
                    }
                }
                Entity.Mob invo = Entity.Mob.Copy(Database.table.Monster.Mobs[int.Parse(infoInvo[0])][int.Parse(infoInvo[1]) - 1], id);
                invo.FightInfo = new Fight.FightEntityInfo(id, laucher.FightInfo.equipeID, false);
                invo.FightInfo.InFight = 2;
                invo.AI = invo.AI = invo.TypeAI == Entity.EnumTypeAI.Cac ? new Entity.AI.CacAI(invo) : new Entity.AI.FearfulAI(invo);
                invo.FightInfo.FightCell = TargetCell;
                invo.CurrentFight = laucher.CurrentFight;
                laucher.FightInfo.EntityInvocation.Add(invo);

                foreach (var item in AllEntityInFight
                      .Select((o, i) => new { Value = o, Index = i })
                      .Where(p => p.Value == laucher)
                      .OrderByDescending(p => p.Index))
                {
                    if (item.Index + 1 == AllEntityInFight.Count) AllEntityInFight.Add(invo);
                    else AllEntityInFight.Insert(item.Index + 1, invo);
                }
                laucher.CurrentFight.CreateOrderPacket();
                laucher.CurrentFight.SendToAllFight(laucher.CurrentFight.GTLpacket + $"\0GM{Map.MapGestion.GmPacket(invo)}");
            }

        }


        [EffectAttribute(new int[1] { 182 })]
        public void AddInvocation(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => !x.FightInfo.isDead && x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                Buff.Buff.AddBuff(laucher, target, spells, Util.rng.Next(spells.JetMin, spells.JetMax + 1), "invo");
            }

        }

    }
}
