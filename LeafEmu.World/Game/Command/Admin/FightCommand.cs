using LeafEmu.World.Game.Command.Gestion;
using LeafEmu.World.Game.Spells.SpellsEffect;
using System;

namespace LeafEmu.World.Game.Command.Admin
{
    class FightCommand
    {

        [CommandAttribute(3, "kill", 0, "Name entity if no entity kill all ennemy")]
        public void KillInFight(Network.listenClient Prmclient, string command)
        {
            if (Prmclient.account.character.CurrentFight == null || Prmclient.account.character.CurrentFight.FightStade == 0)
                return;

            string[] Info = command.Split(' ');
            if (Info.Length == 2)
            {
                foreach (var item in Prmclient.account.character.CurrentFight.Equipe[Prmclient.account.character.FightInfo.equipeID == 0 ? 1 : 0])
                {
                    if (item.speudo == Info[2])
                    {
                        Spells.SpellsEffects.Effect.Damage.DmgLife(Prmclient.account.character, item.Vie, item, false, false);
                        return;
                    }
                }

            }
            else
            {
                Prmclient.account.character.CurrentFight.Equipe[Prmclient.account.character.FightInfo.equipeID == 0 ? 1 : 0].ForEach
                    (x => Spells.SpellsEffects.Effect.Damage.DmgLife(Prmclient.account.character, x.Vie, x, false, false));
            }
            Prmclient.account.character.CurrentFight.CheckEnd();
        }

        [CommandAttribute(3, "send", 1, "{string} packet to send")]
        public void SendPacket(Network.listenClient Prmclient, string command)
        {
            if (Prmclient.account.character.FightInfo.InFight == 2)
            {
                Prmclient.account.character.CurrentFight.SendToAllFight(command.Substring(4));
            }
        }

        [CommandAttribute(3, "buff", 3, "{str} buff name, {Nb} Give pm, {Nb} nb turn")]
        public void AddBuff(Network.listenClient Prmclient, string command)
        {
            if (Prmclient.account.character.FightInfo.InFight == 2)
            {
                string[] Info = command.Split(' ');
                if (Int32.TryParse(Info[2], out int boost) && Int32.TryParse(Info[3], out int nbTurn))
                {
                    Spells.SpellsEffect.Spells spells = new Spells.SpellsEffect.Spells(nbTurn);
                    spells.effectID = getEffectBuffIdWithName(Info[1], boost);
                    if (!Buff.Buff.AddBuff(Prmclient.account.character, Prmclient.account.character, spells, boost, Info[1]))
                    {
                        Prmclient.SendMessageToPlayer($"carac {Info[1]} not exist");
                    }
                }
            }
        }

        private int getEffectBuffIdWithName(string name, int boost)
        {
            switch (name)
            {
                case "PM":
                    return boost > 0 ? 128 : 169;
                case "PA":
                    return boost > 0 ? 111 : 168;
                default:
                    break;
            }
            foreach (var item in Enum.GetValues(typeof(enumSpellsEffects)))
            {
                if (item.ToString().ToLower().Contains(name))
                    return (int)item;
            }
            return 0;
        }

    }
}
