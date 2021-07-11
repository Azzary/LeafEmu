namespace LeafEmu.World.Game.Item
{
    public class Effect
    {

        public int ID { get; set; }
        public int Fix { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int CurrentJet { get; set; }

        public override string ToString()
        {
            return ID.ToString("X") + $"#{CurrentJet.ToString("X")}###" + Min + "d" + Max + "+" + Fix;
        }

        public bool IsWeaponEffect()
        {
            switch (ID)
            {
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                    return true;
                default:
                    return false;
            }
        }

    }
}
