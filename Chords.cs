using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChordFinderWPF
{
    class Chords
    {
        Dictionary<int, string> AllChords = new Dictionary<int, string>();
        public int[] SetOfPitches = new int[3];

        public Chords()
        {
            AllChords.Add(0, "C");
            AllChords.Add(1, "C#");
            AllChords.Add(2, "D");
            AllChords.Add(3, "D#");
            AllChords.Add(4, "E");
            AllChords.Add(5, "F");
            AllChords.Add(6, "F#");
            AllChords.Add(7, "G");
            AllChords.Add(8, "G#");
            AllChords.Add(9, "A");
            AllChords.Add(10, "A#");
            AllChords.Add(11, "B");
        }

        /// <summary>
        /// Returns set of pitches for a chord
        /// </summary>
        /// <param name="Major">true for major chords and false for minor chords</param>
        /// <param name="chord">chord name</param>
        public int[] CreateSetOfPitches(bool Major, string chord)
        {
            int root = AllChords.FirstOrDefault(x => x.Value == chord).Key;

            if (Major)
            {
                SetOfPitches[0] = root;
                SetOfPitches[1] = (root + 4) % 12;
                SetOfPitches[2] = (root + 7) % 12;
            }
            else
            {
                SetOfPitches[0] = root;
                SetOfPitches[1] = (root + 3) % 12;
                SetOfPitches[2] = (root + 7) % 12;
            }

            return SetOfPitches;
        }
    }
}
 