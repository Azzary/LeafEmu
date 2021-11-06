using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.AI
{
    class CacAI : AI
    {

        public CacAI(Entity _monster) : base(_monster)
        {
        }

        public async override Task PlayAI()
        {
            try
            {
                List<int> NextMove = new List<int>();
                await BestSpell();
                await Move(MoveNear()); //timeWait += 
                await BestSpell();
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
