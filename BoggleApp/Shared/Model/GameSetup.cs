using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Model
{
    public class GameSetup
    {
        private Dice[] dices;

        public GameSetup()
        {
            dices = new Dice[]
            {
                new Dice(1, Letter.S, Letter.B, Letter.J, Letter.U, Letter.I, Letter.L),
                new Dice(2, Letter.K, Letter.V, Letter.I, Letter.T, Letter.E, Letter.I),
                new Dice(3, Letter.P, Letter.L, Letter.N, Letter.U, Letter.A, Letter.J),  
                new Dice(4, Letter.LJ, Letter.E, Letter.E, Letter.O, Letter.I, Letter.G),
                new Dice(5, Letter.O, Letter.A, Letter.D, Letter.S, Letter.V, Letter.DŽ),
                new Dice(6, Letter.S, Letter.T, Letter.A, Letter.E, Letter.Š, Letter.A),
                new Dice(7, Letter.NJ, Letter.M, Letter.O, Letter.A, Letter.N, Letter.O),
                new Dice(8, Letter.I, Letter.N, Letter.D, Letter.Ž, Letter.R, Letter.A),
                new Dice(9, Letter.E, Letter.E, Letter.F, Letter.R, Letter.I, Letter.J),
                new Dice(10, Letter.K, Letter.E, Letter.N, Letter.I, Letter.C, Letter.T),
                new Dice(11, Letter.A, Letter.M, Letter.S, Letter.Č, Letter.A, Letter.E),
                new Dice(12, Letter.A, Letter.J, Letter.M, Letter.R, Letter.A, Letter.O),
                new Dice(13, Letter.G, Letter.P, Letter.O, Letter.V, Letter.U, Letter.Đ),
                new Dice(14, Letter.Z, Letter.E, Letter.N, Letter.O, Letter.O, Letter.S),
                new Dice(15, Letter.D, Letter.Ć, Letter.T, Letter.I, Letter.A, Letter.M),
                new Dice(16, Letter.N, Letter.U, Letter.R, Letter.K, Letter.H, Letter.I),
            };
        }



        public List<Dice> Shuffle()
        {
            var random = new Random();
            var shuffled = dices.OrderBy(x => random.Next(1,17)).ToList();
            //shuffled.Select(i => i.Id).ToList().ForEach(_ => System.Diagnostics.Debug.WriteLine(_.ToString()));
            return shuffled;
        }
    }


}
