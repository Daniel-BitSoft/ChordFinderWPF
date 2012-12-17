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
        List<string> Solutions = new List<string>();
        List<string> FinalSolutions = new List<string>();
        int Min, Max;
        int[] Strings = new int[6];
        //string[] Chrd;
        int[] c;
        List<int[]> FingerPositions;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetAllSets()
        {
            List<int[]> Sets = new List<int[]>();
            FingerPositions = new List<int[]>();

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

            foreach (int[] a in Sets)
            {
                List<int[]> permutations = GetPermutation(a);
                FingerPositions.AddRange(permutations.Select(b => b));
            }
        }

        private List<int[]> GetPermutation(int[] inputSet)
        {
            Permutation rec = new Permutation();
            rec.Pitches = Pitches;
            rec.InputSet = inputSet;
            rec.Output2 = new List<int[]>();
            rec.CalcPermutation(0);
            return rec.Output2;
        }

        private void TreeFiltering()
        {
            if (FingerPositions.Count == 0)
                return;

            Solutions.Clear();
            List<int[]> FinalFingering = new List<int[]>();

            for (int i = 0; i < FingerPositions.Count; i++)
            {
                var list = FingerPositions[i].Where(a => a != -1 && a != 0).ToList();
                if (list.Count > 0)
                {
                    var SortedList = list.OrderBy(a => a).ToList();

                    // To filter those fingerings where the distance of first frett and last frett are less and equal to selected distance using ComboBox
                    if ((SortedList[SortedList.Count - 1] - SortedList[0]) <= (Convert.ToInt32(FingerDistanceComboBx.Text) - 1) && (FingerPositions[i].Count(a => a == -1) <= 2))
                        FinalFingering.Add(FingerPositions[i]);
                }
            }

            //=============================================

            string fingerings = string.Empty;
            foreach (int[] c in FinalFingering)
            {
                fingerings += c[0] + "," + c[1] + "," + c[2] + "," + c[3] + "," + c[4] + "," + c[5] + "\t";
            }

            string[] fings = fingerings.Substring(0, fingerings.LastIndexOf('\t')).Split('\t');
            List<string> final = fings.Distinct().ToList();

            for (int j = 0; j < FinalFingering.Count; j++)
            {
                c = FinalFingering[j];

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
                // Changing all variables to their default values because the following is a new tree
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

            DrawTab();
        }

        private void CreateSmallTree(Node TreeNode, int RemainingFingers, int PitchNeedFinger, int stringNo)
        {
            List<string> positions = new List<string>();
            int fret = TreeNode.Fret;

            while (positions.Count == 0 && fret <= Max)
            {
                for (int i = stringNo; i >= 0; i--)
                {
                    if (c[i] == fret)
                    {
                        positions.Add(c[i] + "," + i.ToString());   // Like x,y in cartesian
                        Node node = new Node();
                        node.Fret = fret;
                        node.finger = TreeNode.finger + 1;
                        node.FingerPosition = new string[] { c[i] + "," + i.ToString() };
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
            {
                // if rich filter selected
                if (RichCombo.SelectedIndex == 1 && Solutions.Count > 0)
                {
                    // List<string> DeleteList = new List<string>();
                    //for (int i = 0; i < Solutions.Count; i++)
                    //{
                    var a = solution.Split('/').Where(b => !b.Contains("-1")).ToList();
                    //bool IsContained = false;
                    List<string> same = Solutions.Where(c => c.Contains(a[0])).ToList();

                    for (int j = 1; j < a.Count; j++)
                    {
                        same = same.Where(c => c.Split('/').Contains(a[j])).ToList();
                    }

                    //int maxIndex = -1;
                    if (same.Count > 1)
                    {
                        foreach (string s in same)
                        {
                            if (s.Split('/').Where(c => !c.Contains("-1")).Count() < a.Count)
                            {
                                if (!Solutions.Contains(solution)) // To prevent from adding multiple times in foreach
                                {
                                    Solutions.Add(solution);
                                }
                                Solutions.Remove(s);
                            }
                        }

                    }

                    //for (int index = 0; index < same.Count; index++)
                    ////{
                    else if (same.Count == 1)
                    {
                        var selected = same[0].Split('/').Where(b => !b.Contains("-1")).ToList();
                        if (selected.Count < a.Count)
                        {
                            Solutions.Add(solution);
                            Solutions.Remove(same[0]);
                        }
                    }
                    else
                        Solutions.Add(solution);
                    //}
                    // }
                    //foreach (string s in DeleteList)
                    //    Solutions.Remove(s);
                }
                else
                    Solutions.Add(solution);
            }
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
                    if (c[i] == fret)
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
        }

        private void GetSetOfPitches()
        {
            Chords chords = new Chords();
            bool major = true;

            if (Convert.ToBoolean(MajorRadio.IsChecked))
                major = true;
            else if (Convert.ToBoolean(MinorRadio.IsChecked))
                major = false;

            Pitches = chords.CreateSetOfPitches(major, (ChordsComboBox.SelectedItem as ComboBoxItem).Content.ToString());
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
            }
        }

        private void DrawTab()
        {
            FlowDocument doc1 = new FlowDocument();
            doc1.ColumnWidth = 180;
            FlowDocReader.Document = null;
            FlowDocReader.Document = doc1;
            FlowDocReader.Zoom = 40;
            
            Chords chrd = new Chords();

            TotalLbl.Content = "Results Found: " + Solutions.Count + "  -  Set of Notes: " + chrd.AllChords[Pitches[0]] + "-" + chrd.AllChords[Pitches[1]] + "-" + chrd.AllChords[Pitches[2]];

            List<Paragraph> content = new List<Paragraph>();

            // if re-order based on difficulty
            if (OrderCombo.SelectedIndex == 1)
                ReOrder();
            else
                FinalSolutions = Solutions.OrderBy(a => Convert.ToInt32(a.Split('/').Where(b => b.Split(',')[0] != "-1" && b.Split(',')[0] != "0").Last().Split(',')[0])).ToList();


            for (int k = 0; k < FinalSolutions.Count; k++)
            {
                Figure fig = new Figure();
                fig.CanDelayPlacement = false;
                // Create Table ...
                Table tbl = new Table();
                tbl.FontSize = 14;
                tbl.Background = new ImageBrush
                {
                    Stretch = Stretch.Fill,
                    ImageSource =
                      new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/ChordBG.png", UriKind.RelativeOrAbsolute)
                      )
                };

                tbl.CellSpacing = 2;

                tbl.Margin = new Thickness(0, 0, 0, 0);
                // tbl.Background = new Brush("Images/ChordBG.png");
                for (int x = 0; x < 7; x++)
                {
                    tbl.Columns.Add(new TableColumn());
                    tbl.Columns[tbl.Columns.Count - 1].Width = new GridLength(23);
                }
                tbl.Columns[0].Width = new GridLength(18);
                // Create and add an empty TableRowGroup to hold the table's Rows.
                tbl.RowGroups.Add(new TableRowGroup());
                // Add the first (title) row.
                for (int i = 0; i < 7; i++)
                {
                    TableRow currentRow = new TableRow();
                    tbl.RowGroups[0].Rows.Add(currentRow);

                    for (int j = 0; j < 7; j++)
                    {
                        TableCell cell = new TableCell();
                        currentRow.Cells.Add(cell);
                        cell.BorderThickness = new Thickness(0);
                        cell.Padding = new Thickness(0, 0, 0, 0);
                    }
                }

                // Fill table with fingering values
                string[] vals = FinalSolutions[k].Split('/');
                string b = vals.Where(a => !a.Contains("-1") && a.Split(',')[0] != "0").Last();
                int MinFrett = Convert.ToInt32(b.Split(',')[0]);

                // Showing the starting Frett
                Paragraph paragraph2 = new Paragraph();
                paragraph2.Inlines.Add(new Run(MinFrett.ToString()));
                tbl.RowGroups[0].Rows[1].Cells[0].Blocks.Add(paragraph2);

                // Showing finger positions
                for (int i = 0; i < vals.Length; i++)
                {
                    string[] points = vals[i].Split(',');
                    if (points.Length == 2)
                    {
                        int ColumnIndex = Math.Abs(Convert.ToInt32(points[1]) - 6);
                        if (points[0] == "-1")
                        {
                            Paragraph paragraph = new Paragraph();
                            paragraph.Inlines.Add(new Run("X"));
                            tbl.RowGroups[0].Rows[0].Cells[ColumnIndex].Blocks.Add(paragraph);
                        }
                        else
                        {
                            Paragraph paragraph = new Paragraph();
                            paragraph.Inlines.Add(new Run("O"));
                            tbl.RowGroups[0].Rows[0].Cells[ColumnIndex].Blocks.Add(paragraph);
                        }
                    }
                    else
                    {
                        int RowIndex = Convert.ToInt32(points[0]) - MinFrett + 1;
                        int ColumnIndex = Math.Abs(Convert.ToInt32(points[1]) - 6);

                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run(points[2]));
                        tbl.RowGroups[0].Rows[RowIndex].Cells[ColumnIndex].Blocks.Add(paragraph);

                    }
                }

                fig.Blocks.Add(tbl);
                Paragraph MainParagh = new Paragraph(fig);
                doc1.Blocks.Add(MainParagh);
                //content.Add(MainParagh);

            }

            //doc1.Blocks.AddRange(content);
        }

        private void ReOrder()
        {
            Dictionary<string, int> OrderedSolutions = new Dictionary<string, int>();

            foreach (string Sol in Solutions)
            {
                var a = Sol.Split('/').Where(b => b.Split(',').Length > 2).ToList();
                int FrettCount = a.Select(c => c.Split(',')[0]).Distinct().Count();
                int StringCount = a.Select(c => c.Split(',')[1]).Distinct().Count();
                int FingerCount = a.Select(c => c.Split(',')[2]).Distinct().Count();

                int Difficulty = FrettCount + StringCount + FingerCount;
                OrderedSolutions.Add(Sol, Difficulty);
            }

            FinalSolutions = OrderedSolutions.OrderBy(c => c.Value).ThenBy(d => Convert.ToInt32(d.Key.Split('/').Where(b => b.Split(',')[0] != "-1" && b.Split(',')[0] != "0").Last().Split(',')[0])).Select(a => a.Key).ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MinorRadio_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MajorRadio_Click(object sender, RoutedEventArgs e)
        {
        }

        private void FingerDistanceComboBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (ChordsComboBox.SelectedItem != null && FingerDistanceComboBx.SelectedItem != null)
            {
                GetSetOfPitches();
                GetAllSets();
                TreeFiltering();
            }
        }
    }
}
