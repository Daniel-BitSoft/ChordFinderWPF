using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChordFinderWPF
{
    class Node
    {
        public Node Parent;
        public ArrayList Children = new ArrayList();
        public int Fret;
        public string[] FingerPosition;
        public int finger;

    }
}
