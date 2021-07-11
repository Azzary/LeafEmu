using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.AI
{
    class CacAI : AI
    {

        public CacAI(Entity _monster) : base(_monster)
        {
        }

        public override void PlayAI()
        {
            try
            {
                List<int> NextMove = new List<int>();
                BestSpell();
                NextMove = MoveNear();
                Move(NextMove);
                BestSpell();
            }
            catch (Exception)
            {
            }
        }

        public void BestSpell()
        {
            if (FirstTurn)
            {
                BoostMe();
                FirstTurn = false;
            }
            else
            {
                AttackNeightboor();
            }
        }

    }
}
