namespace LeafEmu.World.Game.Entity.Npc
{
    public class Npc
    {
        public int CellID { get; set; }
        public int Oriantation { get; set; }
        public int TempID { get; set; }
        public NpcTemplate Template { get; set; }

        public Npc(int cellid, int oriantation, int tempid, NpcTemplate template)
        {
            CellID = cellid;
            Oriantation = oriantation;
            TempID = tempid;
            Template = template;
        }


        public string DisplayOnMap
        {
            get
            {
                return "|+" + CellID + ";" + Oriantation + ";0;" + TempID + ";" + Template.id +
                    ";-4;" + Template.gfxID + "^" + Template.Scale + ";" + Template.sexe +
                    ";" + Template.couleur1.ToString("x") + ";" + Template.couleur2.ToString("x") + ";" + Template.couleur3.ToString("x") +
                    ";" + Template.Accessories + ";;" + Template.CustomArtWork;
            }
        }

    }
}
