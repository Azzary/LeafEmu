using System;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Entity
{
    public class MobGroup
    {
        public List<Mob> Mobs { get; set; }
        public int cellID { get; set; }
        public int ID { get; set; }
        public string GroupData { get; set; }
        public int Stars { get; set; }

        public static int GetRngCellWalkable(List<Map.Cell.Cell> Cells)
        {
            int MobCellID = -1;
            for (int id = 0; id < Cells.Count; id++)
            {
                MobCellID = Util.rng.Next(0, Cells.Count);
                if (Cells[MobCellID].IsWalkable)
                    break;
            }
            return MobCellID;
        }
        public static MobGroup CreateMobGroupe(int idGroupe, string groupDate, int cellID, char CharGroupDataSplit, short GroupSize = 8, bool FixeGroupe = true)
        {
            MobGroup newGroupe = new MobGroup(idGroupe, new List<Mob>(), cellID, groupDate);
            List<int[]> mobsIDs = new List<int[]>();
            foreach (var item in groupDate.Split(CharGroupDataSplit))
            {
                if (item == string.Empty)
                    continue;
                mobsIDs.Add(Array.ConvertAll(item.Split(','), int.Parse));
            }
            for (int x = 0; x < GroupSize; x++)
            {
                int rngMob = FixeGroupe ? x : Util.rng.Next(Util.rng.Next(0, mobsIDs.Count));
                if (rngMob >= mobsIDs.Count)
                {
                    break;
                }
                if (Database.table.Monster.Mobs.ContainsKey(mobsIDs[rngMob][0]))
                {
                    int grade = Util.rng.Next(0, Database.table.Monster.Mobs[mobsIDs[rngMob][0]].Count);
                    newGroupe.Mobs.Add(Mob.Copy(Database.table.Monster.Mobs[mobsIDs[rngMob][0]][grade], -x));
                }
                if (Math.Abs(x) > GroupSize)
                    break;
            }
            return newGroupe;
        }

        public MobGroup(int _id, List<Mob> mobs, int _cellid, string groupData)
        {
            GroupData = groupData;
            ID = _id;
            cellID = _cellid;
            Mobs = mobs;
            Stars = Util.getStarAlea();
        }

        public string parseGM()
        {
            StringBuilder mobIDs = new StringBuilder();
            StringBuilder mobGFX = new StringBuilder();
            StringBuilder mobLevels = new StringBuilder();
            StringBuilder colors = new StringBuilder();
            StringBuilder toreturn = new StringBuilder();

            bool isFirst = true;
            if (Mobs.Count == 0)
                return string.Empty;

            foreach (Mob entry in Mobs)
            {
                if (!isFirst)
                {
                    mobIDs.Append(",");
                    mobGFX.Append(",");
                    mobLevels.Append(",");
                    colors.Append(";");
                }
                mobIDs.Append(entry.id);
                mobGFX.Append(entry.gfxID).Append("^").Append(entry.Scale);
                mobLevels.Append(entry.level);
                var color1 = entry.couleur1 == -1 ? "-1" : entry.couleur1.ToString("X");
                var color2 = entry.couleur2 == -1 ? "-1" : entry.couleur2.ToString("X");
                var color3 = entry.couleur3 == -1 ? "-1" : entry.couleur3.ToString("X");
                colors.Append($"{color1};{color2};{color3}");

                isFirst = false;
            }
            toreturn.Append("+").Append(cellID).Append(";").Append("7").Append(";");
            toreturn.Append(Stars);// bonus en pourcentage (�toile/20%) 
            toreturn.Append(";").Append(ID).Append(";").Append(mobIDs).Append(";-3;").Append(mobGFX).Append(";").Append(mobLevels).Append(";").Append(colors);
            return toreturn.ToString();
        }

    }
}
