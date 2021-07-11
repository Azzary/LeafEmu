using LeafEmu.World.PacketGestion;

namespace LeafEmu.World.Game.Character
{
    class Stat
    {
        [PacketAttribute("AB")]
        public void AddStats(Network.listenClient prmClient, string prmPacket)
        {
            string action = prmPacket.Substring(2, 2);
            //string stats = prmPacket.Substring(5).Split("\0")[0];
            int res = 5;
            //if (int.TryParse(stats, out int res))
            //{
            if (prmClient.account.character.capital >= res)
            {
                prmClient.account.character.capital -= res;
            }
            else
                return;

            if (action == "10")//force
            {
                prmClient.account.character.CaracForce += res;
            }
            else if (action == "11")//vita
            {
                prmClient.account.character.CaracVie += res;
            }
            else if (action == "12")//sagesse
            {
                prmClient.account.character.CaracSagesse += res;
            }
            else if (action == "13")//chance
            {
                prmClient.account.character.CaracChance += res;
            }
            else if (action == "14")//agi
            {
                prmClient.account.character.CaracAgi += res;
            }
            else if (action == "15")//intell
            {
                prmClient.account.character.CaracIntell += res;
            }
            prmClient.account.character.resCaract();
            prmClient.send("ILF0");
            prmClient.send("ILF0\0ILS2000");
            prmClient.send(GestionCharacter.createAsPacket(prmClient));
            //}
            //else
            //return;

        }

    }
}
