using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChordFinderWPF
{
    class GuitarStrings
    {
        //public int string1;
        //public int string2;
        //public int string3;
        //public int string4;
        //public int string5;
        //public int string6;

        public GuitarStrings()
        { }

        /// <summary>
        /// Returns the fret for a specific pitch on a specific string
        /// </summary>
        /// <param name="String"></param>
        /// <param name="Pitch"></param>
        /// <returns></returns>
        public int[] GetFingerPlace(string Pitch)
        {
            string[] Pitches = Pitch.Split(',');
            int[] FingerPlace = new int[6];

            FingerPlace[0] = (Pitches[0] != "-1") ? (Convert.ToInt32(Pitches[0]) + 8) % 12 : -1;
            FingerPlace[1] = (Pitches[1] != "-1") ? (Convert.ToInt32(Pitches[1]) + 1) % 12 : -1;
            FingerPlace[2] = (Pitches[2] != "-1") ? (Convert.ToInt32(Pitches[2]) + 5) % 12 : -1;
            FingerPlace[3] = (Pitches[3] != "-1") ? (Convert.ToInt32(Pitches[3]) + 10) % 12 : -1;
            FingerPlace[4] = (Pitches[4] != "-1") ? (Convert.ToInt32(Pitches[4]) + 3) % 12 : -1;
            FingerPlace[5] = (Pitches[5] != "-1") ? (Convert.ToInt32(Pitches[5]) + 8) % 12 : -1;

            return FingerPlace;
        }
    }
}
