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
using System.Windows.Shapes;

namespace ChordFinderWPF
{
    /// <summary>
    /// Interaction logic for VisualFingering.xaml
    /// </summary>
    public partial class VisualFingering : Window
    {
        List<string> Fingerings;

        public VisualFingering(List<string> fingerings)
        {
            InitializeComponent();
            Fingerings = fingerings;
            DrawTab();
        }

        private void DrawTab()
        {
            ImageBrush One = new ImageBrush
                {
                    ImageSource =
                      new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/One.png", UriKind.RelativeOrAbsolute)
                      )
                };

            TotalLbl.Content = "Results Found: " + Fingerings.Count;

            foreach (string Fingering in Fingerings)
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
                
                tbl.Margin = new Thickness(0, 10, 0, 0);
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
                string[] vals = Fingering.Split('/');
                string b = vals.Where(a => !a.Contains("-1") && a.Split(',')[0] != "0").Last();
                int MinFrett = Convert.ToInt32(b.Split(',')[0]);

                // Showing the starting Frett
                Paragraph paragraph2 = new Paragraph();
                paragraph2.Inlines.Add(new Run(MinFrett.ToString()));
                tbl.RowGroups[0].Rows[1].Cells[0].Blocks.Add(paragraph2);

                // Showing finger positions
                foreach (string val in vals)
                {
                    string[] points = val.Split(',');
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
                Paragraph MainParag = new Paragraph(fig);
                doc1.Blocks.Add(MainParag);
                
            }
            
        }
    }
}
