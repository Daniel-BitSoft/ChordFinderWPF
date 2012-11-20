using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChordFinderWPF
{
    class Permutation
    {
        private int elementLevel = -1;
        private int numberOfElements = 6;
        private int[] permutationValue = new int[6];
        public int[] Pitches = new int[3];
        //public List<string> Output = new List<string>();
        public string Output, Output2;

        private int[] inputSet;
        public int[] InputSet
        {
            get { return inputSet; }
            set { inputSet = value; }
        }

        private int permutationCount = 0;
        public int PermutationCount
        {
            get { return permutationCount; }
            set { permutationCount = value; }
        }

        public void CalcPermutation(int k)
        {
            elementLevel++;
            permutationValue.SetValue(elementLevel, k);

            if (elementLevel == numberOfElements)
            {
                OutputPermutation(permutationValue);
            }
            else
            {
                for (int i = 0; i < numberOfElements; i++)
                {
                    if (permutationValue[i] == 0)
                    {
                        CalcPermutation(i);
                    }
                }
            }
            elementLevel--;
            permutationValue.SetValue(0, k);
        }

        private void OutputPermutation(int[] value)
        {
            string a = string.Empty;
            GuitarStrings GS = new GuitarStrings();

            foreach (int i in value)
            {
                a += inputSet.GetValue(i - 1).ToString() + ',';
            }
            a = a.Substring(0, a.Length - 1);
            if (a.Contains(Pitches[0].ToString()) && a.Contains(Pitches[1].ToString()) && a.Contains(Pitches[2].ToString()))
                if (!Output.Contains(a))
                {
                    int[] FingerPlace = GS.GetFingerPlace(a);
                    Output2 += FingerPlace[0] + "," + FingerPlace[1] + "," + FingerPlace[2] + "," + FingerPlace[3] + "," +
                        FingerPlace[4] + "," + FingerPlace[5] + "\t";

                    Output += a + "\t";
                }

            PermutationCount++;
        }
    }
}
