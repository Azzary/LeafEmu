using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System.Linq;
using System.Text;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class SpawnCreature
    {
        [EffectAttribute(new int[1] { 181 })]
        public void Invocation(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            if (laucher.FightInfo.NbInvo > laucher.invo
               || !laucher.CurrentFight.map.Cells[TargetCell].IsWalkable
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
                    if (id == AllEntityInFight[i].ID_InFight)
                    {
                        i = -1;
                        id--;
                    }
                }
                int mobID = int.Parse(infoInvo[0]);
                int grade = int.Parse(infoInvo[1]) - 1;
                if (!Database.table.Monster.Mobs.ContainsKey(mobID))
                    return;
                grade = Database.table.Monster.Mobs[mobID].Count > grade ? grade : Database.table.Monster.Mobs[mobID].Count -1;
                Entity.Mob invo = Entity.Mob.Copy(Database.table.Monster.Mobs[mobID][grade], id);
                invo.FightInfo = new Fight.FightEntityInfo(id, laucher.FightInfo.equipeID, false);
                invo.FightInfo.InFight = 2;
                invo.AI = invo.AI = invo.TypeAI == Entity.EnumTypeAI.Cac ? new Entity.AI.CacAI(invo) : new Entity.AI.FearfulAI(invo);
                invo.FightInfo.FightCell = TargetCell;
                invo.CurrentFight = laucher.CurrentFight;
                invo.IsInvocation = true;
                laucher.FightInfo.EntityInvocation.Add(invo);
                
                foreach (var item in AllEntityInFight
                      .Select((o, i) => new { Value = o, Index = i })
                      .Where(p => p.Value == laucher)
                      .OrderByDescending(p => p.Index))
                {
                    if (item.Index + 1 == AllEntityInFight.Count) AllEntityInFight.Add(invo);
                    else AllEntityInFight.Insert(item.Index + 1, invo);
                }
                laucher.CurrentFight.CreateOrderPacketGTL();
                laucher.CurrentFight.SendToAllFight(packetSpawnInvo(invo, mobID, laucher));
            }

        }


        private static string packetSpawnInvo(Entity.Entity invo, int mobID, Entity.Entity laucher)
        {
            StringBuilder packet = new StringBuilder("GA;181");
            packet.Append(";").Append(laucher.ID_InFight)
            .Append(";+").Append(invo.FightInfo.FightCell)
            .Append(";").Append("1").Append(";0;").Append(invo.ID_InFight)
            .Append(";").Append(mobID)
            .Append(";-1")
            .Append(";").Append(invo.gfxID).Append("^").Append(invo.Scale)
            .Append(";").Append("1")
            .Append(";").Append("-1;-1;-1")
            .Append(";").Append("0,0,0,0")
            .Append(";").Append(invo.Vie)
            .Append(";").Append(invo.PA)
            .Append(";").Append(invo.PM)
            .Append(";").Append(invo.P_TotalResNeutre)
            .Append(";").Append(invo.P_TotalResForce)
            .Append(";").Append(invo.P_TotalResIntell)
            .Append(";").Append(invo.P_TotalResEau)
            .Append(";").Append(invo.P_TotalResAgi)
            .Append(";11;11;").Append(invo.FightInfo.equipeID);
            packet.Append($"\0GA;999;{laucher.ID_InFight};{invo.CurrentFight.GTLpacket}\0");
            return packet.ToString();
        }

        [EffectAttribute(new int[1] { 182 })]
        public void AddNbInvocation(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => !x.FightInfo.isDead && x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                Buff.Buff.AddBuff(laucher, target, spells, Util.rng.Next(spells.JetMin, spells.JetMax + 1), "invo");
            }

        }

    }
}
