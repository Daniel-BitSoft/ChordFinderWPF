using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChordFinderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] Pitches;
        List<int[]> FinalFingering = new List<int[]>();
        List<string> Solutions = new List<string>();
        int Min, Max;
        int[] Strings = new int[6];
        string[] Chrd;
        int[] c;

        public MainWindow()
        {
            InitializeComponent();
            ChordsComboBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetAllSets();
        }

        private void GetAllSets()
        {
            List<int[]> Sets = new List<int[]>();

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    if (i + j >= 6)
                        break;

                    for (int k = 1; k <= 4; k++)
                    {
                        if (i + j + k > 6)
                            break;

                        for (int l = 1; l <= 3; l++)
                        {
                            if (i + j + k + l > 6)
                                break;

                            int[] values = new int[6];
                            int x = 0;
                            for (x = 0; x < i; x++)
                                values[x] = Pitches[0];
                            for (x = i; x < i + j; x++)
                                values[x] = Pitches[1];
                            for (x = i + j; x < i + j + k; x++)
                                values[x] = Pitches[2];
                            for (x = i + j + k; x < i + j + k + l; x++)
                                values[x] = -1;
                            Sets.Add(values);
                        }
                    }
                }
            }

            string Output = string.Empty;
            string FingerPositions = string.Empty;

            

            foreach (int[] a in Sets)
            {
                string[] permutations = GetPermutation(a);
                Output += permutations[0];

                FingerPositions += permutations[1];
            }

            textBox2.Text = Output;
            textBox3.Text = FingerPositions;

        }

        private string[] GetPermutation(int[] inputSet)
        {
            string[] output = new string[2];
            Permutation rec = new Permutation();
            rec.Pitches = Pitches;
            rec.InputSet = inputSet;
            rec.Output = string.Empty;
            rec.CalcPermutation(0);
            output[0] = rec.Output;
            output[1] = rec.Output2;
            return output;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string[] Fingering = textBox3.Text.Substring(0, textBox3.Text.LastIndexOf('\t')).Split('\t');
            List<int[]> Fingerings = new List<int[]>();

            for (int i = 0; i < Fingering.Length; i++)
            {
                string[] a = Fingering[i].Split(',');
                int[] b = new int[6];
                b[0] = Convert.ToInt32(a[0]);
                b[1] = Convert.ToInt32(a[1]);
                b[2] = Convert.ToInt32(a[2]);
                b[3] = Convert.ToInt32(a[3]);
                b[4] = Convert.ToInt32(a[4]);
                b[5] = Convert.ToInt32(a[5]);

                Fingerings.Add(b);
            }

            for (int i = 0; i < Fingerings.Count; i++)
            {
                if (Fingerings[i].Count(b => b == 0) == 2 && Fingerings[i].Contains(5) && Fingerings[i].Contains(3) && Fingerings[i].Contains(2) && Fingerings[i].Contains(0) && Fingerings[i].Contains(-1))
                {
                    int[] val = Fingerings[i];
                }
                List<int> SortedList = Fingerings[i].Where(a => a != -1 && a != 0).OrderBy(a => a).ToList();
                if (SortedList[SortedList.Count - 1] - SortedList[0] <= 3)
                    FinalFingering.Add(Fingerings[i]);
            }

            //=============================================
            //TreeNode Root = new TreeNode(null, 0, 0);
            //Root.Values = Fingerings;
            //CreateTree(0, 0, Root);

            string fingerings = string.Empty;
            foreach (int[] c in FinalFingering)
            {
                // if (!fingerings.Contains(c[0] + "," + c[1] + "," + c[2] + "," + c[3] + "," + c[4] + "," + c[5]))
                fingerings += c[0] + "," + c[1] + "," + c[2] + "," + c[3] + "," + c[4] + "," + c[5] + "\t";
            }

            string[] fings = fingerings.Substring(0, fingerings.LastIndexOf('\t')).Split('\t');
            List<string> final = fings.Distinct().ToList();
            foreach (string s in final)
                textBox4.Text += s + '\t';

            for (int j = 0; j < FinalFingering.Count; j++)
            {
                c = FinalFingering[j];

                //if (c[0] == 0 && c[1] == 5 && c[2] == 0 && c[3] == 2 && c[4] == 3 && c[5] == -1)
                //{ }
                //if (c.Count(b => b == 0) == 2 && c.Contains(5) && c.Contains(3) && c.Contains(2) && c.Contains(0) && c.Contains(-1))
                //{ }

                Chrd = new string[] { c[0].ToString(), c[1].ToString(), c[2].ToString(), c[3].ToString(), c[4].ToString(), c[5].ToString() };
                // Getting min which is not -1 or 0
                int index = 0;
                Min = c.OrderBy(a => a).ToList()[index];
                while (Min <= 0)
                    Min = c.OrderBy(a => a).ToList()[index++];
                Max = c.OrderBy(a => a).ToList()[5];
                int PitchNeedFinger = c.Count(a => a > 0);
                Strings[0] = Strings[1] = Strings[2] = Strings[3] = Strings[4] = Strings[5] = 0;
                int MinCount = c.Count(a => a == Min);
                if (MinCount > 1)   // There are two or more positions on same fret - Using one finger to hold all positions
                {
                    Node root = new Node();
                    root.Parent = null;
                    root.Fret = Min + 1;
                    root.finger = 1;
                    root.FingerPosition = new string[MinCount];
                    int indx = MinCount - 1;

                    for (int i = 5; i >= 0; i--)
                    {
                        if (c[i] == Min)
                        {
                            root.FingerPosition[indx] = c[i] + "," + i.ToString();
                            Strings[i] = 1;
                            indx--;
                        }
                        else if (c[i] == -1 || c[i] == 0)
                            Strings[i] = 1;
                    }

                    bool IsMuteBetweenBarre = false;
                    int MinBarre = Convert.ToInt32(root.FingerPosition[0].Split(',')[1]);
                    int MaxBarre = Convert.ToInt32(root.FingerPosition[root.FingerPosition.Length - 1].Split(',')[1]);

                    for (int i = 5; i >= 0; i--)
                    {
                        if (c[i] == -1 || c[i] == 0)
                        {
                            if (i >= MinBarre && i <= MaxBarre)
                                IsMuteBetweenBarre = true; // Not Good
                        }
                    }

                    if (!IsMuteBetweenBarre)
                        CreateSmallTree(root, 3, PitchNeedFinger - MinCount, 5);
                    else
                    {
                    }
                }

                //===============================================================
                // You need to change all variables to their default values because the following is a new tree
                Strings[0] = Strings[1] = Strings[2] = Strings[3] = Strings[4] = Strings[5] = 0;
                Node root2 = new Node();
                root2.Fret = Min;

                for (int i = 5; i >= 0; i--)
                {
                    if (c[i] == -1 || c[i] == 0)
                        Strings[i] = 1;
                }

                for (int i = 5; i >= 0; i--)
                {
                    if (c[i] == Min)
                    {
                        root2.FingerPosition = new string[] { c[i] + "," + i.ToString() };
                        Strings[i] = 1;
                        root2.Parent = null;
                        root2.finger = 1;
                        CreateSmallTree(root2, 3, PitchNeedFinger - 1, i - 1);
                    }
                }

            }

            foreach (string b in Solutions)
            {
                textBox5.Text += b + '\t';
            }

            var OrderedSolutions = Solutions.OrderBy(a => Convert.ToInt32(a.Split('/').Where(b => b.Split(',')[0] != "-1" && b.Split(',')[0] != "0").Last().Split(',')[0])).ToList();

            VisualFingering vf = new VisualFingering(OrderedSolutions);
            vf.ShowDialog();

            //foreach (string b in Solutions)
            //{
            //    string[] finger = b.Split('/');
                
            //}

            //for (int k = 0; k < (Solutions.Count / 10) + 1; k++)
            //{
            //    for (int M = 0; M < 10; M++)
            //    {
            //        DataGridColumn col = new DataGridTemplateColumn();
            //        vf.MainGrid.Columns.Add(col);
            //    }
            //    //vf.MainGrid.Items.Add();
            //}
        }

        private void CreateSmallTree(Node TreeNode, int RemainingFingers, int PitchNeedFinger, int stringNo)
        {
            List<string> positions = new List<string>();
            int fret = TreeNode.Fret;

            while (positions.Count == 0 && fret <= Max)
            {
                for (int i = stringNo; i >= 0; i--)
                {
                    if (Chrd[i] == (fret).ToString())
                    {
                        positions.Add(Chrd[i] + "," + i.ToString());   // Like x,y in cartesian
                        Node node = new Node();
                        node.Fret = fret;
                        node.finger = TreeNode.finger + 1;
                        node.FingerPosition = new string[] { Chrd[i] + "," + i.ToString() };
                        TreeNode.Children.Add(node);
                        node.Parent = TreeNode;

                        if (((RemainingFingers - 1) >= 0) && (Strings[i] != 1)) // Same string is not held by a finger
                        {
                            Strings[i] = 1;

                            if (PitchNeedFinger - 1 == 0) // Check to see if all pitches has fingers on them
                            {
                                AddToSolution(node);
                                return;
                            }

                            CreateSmallTree(node, RemainingFingers - 1, PitchNeedFinger - 1, i - 1);
                        }
                    }
                }

                if (positions.Count > 1)
                {
                    Node node = new Node();
                    node.Fret = fret + 1;
                    node.finger = TreeNode.finger + 1;
                    node.FingerPosition = new string[positions.Count];
                    for (int i = 0; i < positions.Count; i++)
                        node.FingerPosition[i] = positions[i];

                    ////////////////////////////////////////
                    bool IsMuteBetweenBarre = false;
                    int MaxBarre = Convert.ToInt32(node.FingerPosition[0].Split(',')[1]);
                    int MinBarre = Convert.ToInt32(node.FingerPosition[node.FingerPosition.Length - 1].Split(',')[1]);

                    for (int i = 5; i >= 0; i--)
                    {
                        if (c[i] == -1 || c[i] == 0)
                        {
                            if (i >= MinBarre && i <= MaxBarre)
                                IsMuteBetweenBarre = true; // Not Good
                        }
                    }

                    if (!IsMuteBetweenBarre)
                    {
                        TreeNode.Children.Add(node);
                        node.Parent = TreeNode;

                        if (!CheckBarreIsLegal(positions, fret))
                            return;

                        if ((RemainingFingers - 1) != 0)
                        {
                            if (PitchNeedFinger - 1 == 0) // Check to see if all pitches has fingers on them
                            {
                                AddToSolution(node);
                                return;
                            }

                            CreateSmallTree(node, RemainingFingers - 1, PitchNeedFinger - positions.Count, 5);
                        }
                    }
                }

                fret++;
                stringNo = 5;
            }

            // It needs to check if this is the last node and it gives us the solution
            // Or needs to check if conditions are not satisfied and stop going deep
        }

        private void AddToSolution(Node TreeNode)
        {
            Node parent = TreeNode;
            string solution = string.Empty;
            while (parent != null)
            {
                foreach (string s in parent.FingerPosition)
                    solution += s + "," + parent.finger + "/";
                parent = parent.Parent;
            }

            for (int i = 0; i < 6; i++)
            {
                if (c[i] == 0 || c[i] == -1)
                    solution += c[i] + "," + i + "/";
            }

            solution = solution.Substring(0, solution.LastIndexOf("/"));
            if (!Solutions.Contains(solution))
                Solutions.Add(solution);
        }

        private bool CheckBarreIsLegal(List<string> positions, int fret)
        {
            bool IsLegal = true;

            int min = 10;
            int max = -2;
            foreach (string s in positions)
            {
                int index = Convert.ToInt32(s.Split(',')[1]);
                if (index < min)
                    min = index;
                if (index > max)
                    max = index;
            }
            fret--;
            while (fret >= Min)
            {
                int min2 = 10;
                int max2 = -2;

                for (int i = 0; i < 6; i++)
                {
                    if (Chrd[i] == fret.ToString())
                    {
                        if (i < min)
                            min2 = i;
                        if (i > max)
                            max2 = i;
                    }
                }

                if (min2 >= min && max2 <= max)
                    IsLegal = false;

                fret--;
            }
            return IsLegal;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSetOfPitches();
        }

        private void GetSetOfPitches()
        {
            Chords chords = new Chords();
            bool major = true;

            if (Convert.ToBoolean(MajorRadio.IsChecked))
                major = true;
            else if (Convert.ToBoolean(MinorRadio.IsChecked))
                major = false;

            Pitches = chords.CreateSetOfPitches(major, ChordsComboBox.Text);
            textBox1.Text = Pitches[0] + "-" + Pitches[1] + "-" + Pitches[2];
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Untitled"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box
            if (dlg.ShowDialog() == true)
            {
                System.IO.TextReader reader = new System.IO.StreamReader(dlg.FileName);
                string[] text = reader.ReadToEnd().Split('\t');

                string found = string.Empty;
                foreach (string txt in text)
                {
                    string[] a = txt.Split(',');
                    if (a.Count(b => b == "0") == 2 && a.Contains("5") && a.Contains("3") && a.Contains("2") && a.Contains("0") && a.Contains("-1"))
                    {
                        found = txt;
                        MessageBox.Show(found);
                    }
                }

                
                //5,1,3/3,4,2/2,3,1/0,0/0,2/-1,5

            }
        }

    }
}
