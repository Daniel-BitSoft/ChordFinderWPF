using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChordFinderWPF
{
    class Finger
    {
        private int FingerIndex;
        private int[] FingerPosition;

        public Finger(int index, int[] Position)
        {
            FingerIndex = index;
            FingerPosition = Position;
        }

        public int GetIndex()
        {
            return FingerIndex;
        }

        public int[] GetPosition()
        {
            return FingerPosition;
        }
    }
}
