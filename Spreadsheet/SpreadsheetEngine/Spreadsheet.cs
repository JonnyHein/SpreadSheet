//Jonathan hein
//11532242
//HW 9
//11/26/18
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using CptS321;
using System.ComponentModel;
using System.IO;

namespace SpreadsheetEngine
{
    public class SpreadSheet
    {
        private Dictionary<string,List<ICell>> depend = new Dictionary<string,List<ICell>>();
        
        public Cell[,] rowcol;
        char[] AZ = {'A','B','C','D','E','F',
                'G','H','I','J','K','L','M','N',
                'O','P','Q','R','S','T','U','V',
                'W','X','Y','Z'};
        public event PropertyChangedEventHandler CellPropertyChanged;
        
        public class ICell : Cell
        {
            public List<ICell> dependList;
            public ICell(int row, int col) 
                : base(row, col)
            {
            }   
            public void setVal(string val)
            {
                Value = val;
            }
        }
        public SpreadSheet(int Row, int Col)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays
            rowcol = new Cell[Row, Col];

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    rowcol[i, j] = new ICell(i, j);
                    rowcol[i, j].PropertyChanged += new PropertyChangedEventHandler(OnCellPropertyChanged);
                }
            }
        }
        /*Make a CellPropertyChanged event in the spreadsheet class. This will serve as a way
        for the outside world (like UI stuff) to subscribe to a single event that lets them know
        when any property for any cell in the worksheet has changed.
        ○ The spreadsheet class has to subscribe to all the PropertyChanged​ events for
        every cell in order to allow this to happen.
        ○  This is where the spreadsheet will set the value for a particular cell if its text has
        just changed. The implementation of this is discussed more in Step 6.
        ○ When a cell triggers the event the spreadsheet will “route” it by calling its
        CellPropertyChanged event.*/
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Text")
            {
                ICell i = sender as ICell;

                if (i._Text[0] != '=')
                {
                    i.setVal(i._Text);
                    CellPropertyChanged(sender as Cell, new PropertyChangedEventArgs("Value"));
                }
                else
                {
                    string Exp = i._Text.TrimStart('=');

                    ExpTree myTree = new ExpTree(Exp);
                    
                    List<string> varList = myTree.varList();
                    foreach(string Var in varList)
                    {
                        if (AZ.Contains(Var[0]) && Var[1] >= '1' && Var[1] <= '9' && Var.Length <= 2)
                        {
                            int col = Var[0] - 65; //convert ascii value to index
                            int row = Var[1] - 49; //""
                            try
                            {
                                double val = Convert.ToDouble(rowcol[row, col]._Value);
                                rowcol[row, col].PropertyChanged += new PropertyChangedEventHandler(DependencyCell);
                                if (depend.ContainsKey(Var))
                                {
                                    depend[Var].Add(i);
                                }
                                else
                                {
                                    depend.Add(Var, new List<ICell>());
                                    depend[Var].Add(i);
                                }
                                myTree.SetVar(Var, val);
                            }
                            catch
                            {
                                i.setVal(null);
                                break;
                            }
                            
                        }
                        else
                        {
                            i.setVal(null);
                            break;
                        }
                    }
                    if (i._Value != null)
                    {
                        i.setVal(myTree.Eval().ToString());
                        CellPropertyChanged(i, new PropertyChangedEventArgs("Value"));
                    }
                    else
                    {
                        i.setVal("#REF");
                    }
                    
                    //for (int j=0;j<26;j++)
                    //{
                    //    if(rowloc[0] == AZ[j])
                    //    {
                    //        string temp = rowloc;
                    //        string col = temp.TrimStart(rowloc[0]);
                    //        int colloc = Convert.ToInt32(col);
                    //        setVal(sender as Cell, rowcol[(colloc-1), j]);
                    //        CellPropertyChanged(sender as Cell, new PropertyChangedEventArgs("Value"));
                    //    }
                    //}
                }
            }
        }
        private void DependencyCell(object sender, EventArgs e)
        {
            ICell cell = sender as ICell;
            char row = Convert.ToChar(cell._RowIndex + 49);
            char col = Convert.ToChar(cell._ColumnIndex + 65);
            string TCell = (""+col + row);
            List<ICell> cells = depend[TCell];
            foreach (ICell dcell in cells)
            {
                string Exp = dcell._Text.TrimStart('=');
                ExpTree myTree = new ExpTree(Exp);
                double val = Convert.ToDouble(rowcol[cell._RowIndex, cell._ColumnIndex]._Value);
                myTree.SetVar(TCell, val);
                dcell.setVal(myTree.Eval().ToString());
                CellPropertyChanged(dcell, new PropertyChangedEventArgs("Value"));
            }

        }
        public void setVal(Cell tobeChanged, Cell changeTo)
        {
            ICell cell = tobeChanged as ICell;
            cell.setVal(changeTo._Text);
            
        }
        /*Make a GetCell​ function that takes a row and column index and returns the cell at that
        location or null if there is no such cell. The return type for this method should be the
        abstract cell class declared in step 4.*/

        public Cell GetCell(int rowi,int coli)
        {
            if(rowcol[rowi,coli] != null)
            {
                return rowcol[rowi, coli];
            }
            else
            {
                return null;
            }
        }
        /*Add properties ColumnCount​ and RowCount​ that return the number of columns and
        rows in the spreadsheet, respectively.*/
        public int ColumnCount
        {
            get { return rowcol.GetLength(0); }
        }
        public int RowCount
        {
            get { return rowcol.GetLength(1); }
        }
        public void Save(Stream outfile)
        {
            XmlWriter xml = XmlWriter.Create(outfile);
            xml.WriteStartDocument();
            xml.WriteStartElement("spreadsheet");
            for (int i=0;i<50;i++)
            {
                for (int j=0; j<26;j++)
                {
                    if(rowcol[i,j]._Text != "" && rowcol[i,j] != null)
                    {
                        xml.WriteStartElement("cell");
                        xml.WriteAttributeString("name", "" + (Convert.ToChar(rowcol[i, j]._ColumnIndex + 65)) + (Convert.ToString(rowcol[i, j]._RowIndex+1)));
                        xml.WriteStartElement("text");
                        xml.WriteString(rowcol[i, j]._Text);
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                    }
                }
            }
            xml.WriteEndElement();
            xml.WriteEndDocument();
        }
        public void Load(Stream infile)
        {
            XmlReader xml = XmlReader.Create(infile);
            int row = 0;
            int col = 0;
            string name, text;
            while(xml.Read())
            {
                if(xml.IsStartElement("cell"))
                {
                    name = xml.GetAttribute("name");
                    if(name!=null)
                    {
                        row = Convert.ToChar(name.Substring(0, 1)) - 65;
                        col = Convert.ToInt32(name.Substring(1))-1;
                    }
                }
                if(xml.IsStartElement("text"))
                {
                    try
                    {
                        text = xml.ReadElementContentAsString();
                        rowcol[col, row]._Text = text;
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
