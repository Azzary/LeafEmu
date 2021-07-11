using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.AI
{
    class FearfulAI : AI
    {
        public FearfulAI(Entity monster) : base(monster) { }

        public override void PlayAI()
        {
            try
            {
                List<int> NextMove = new List<int>();
                NextMove = MoveUntilCanHit();
                Move(NextMove);
                BestSpell();
                NextMove = MoveFar();
                Move(NextMove);
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
