using LeafEmu.World.Game.Command.Gestion;
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

        [CommandAttribute(3, "pa", 1, "{Nb} Give pa")]
        public void AddPa(Network.listenClient Prmclient, string command)
        {
            string Info = command.Split(' ')[1];
            if (Int16.TryParse(Info, out short nbPa))
            {
                Prmclient.account.character.PA += nbPa;
                Spells.SpellsEffects.Effect.CaracEffect.PA(Prmclient.account.character, nbPa);
            }
        }

        [CommandAttribute(3, "pm", 1, "{Nb} Give pm")]
        public void AddPM(Network.listenClient Prmclient, string command)
        {
            string Info = command.Split(' ')[1];
            if (Int16.TryParse(Info, out short nbPm))
            {
                // Prmclient.account.character.PM += nbPm;
                //Spells.SpellsEffects.Effect.CaracEffect.PM(Prmclient.account.character, nbPm);
                Spells.SpellsEffect.Spells spells = new Spells.SpellsEffect.Spells(nbPm);
                Buff.Buff.AddBuff(Prmclient.account.character, Prmclient.account.character, spells, nbPm, "PM");
            }
        }

    }
}
