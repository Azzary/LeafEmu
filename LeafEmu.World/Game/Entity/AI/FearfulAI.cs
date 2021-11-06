using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.AI
{
    class FearfulAI : AI
    {
        public FearfulAI(Entity monster) : base(monster) { }

        public async override Task PlayAI()
        {
            int timeWait = 0;
            try
            {
                await Move(MoveUntilCanHit());
                //timeWait += ;
                await BestSpell();
                await Move(MoveFar());
                //timeWait += ;
            }
            catch (Exception)
            {
            }
            return;
        }

        public async Task BestSpell()
        {
            if (FirstTurn)
            {
                FirstTurn = false;
                await BoostMe();
            }
            else
            {
                 await AttackNeightboor();
            }
        }



    }
}
