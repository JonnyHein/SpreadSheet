//Jonathan hein
//11532242
//HW 9
//11/26/18
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using SpreadsheetEngine;
using CptS321;

namespace Spreadsheet
{
    public partial class Form1 : Form
    {
        private int colIndex, rowIndex;
        private SpreadSheet mySpreadSheet = new SpreadSheet(50, 26);
        public Form1()
        {
            InitializeComponent();
            string[] AZ = {"A","B","C","D","E","F",
                "G","H","I","J","K","L","M","N",
                "O","P","Q","R","S","T","U","V",
                "W","X","Y","Z"};

            dataGridView1.ColumnCount = 26;
            for (int i = 0; i < 26; i++)
            {
                dataGridView1.Columns[i].Name = AZ[i];
            }

            for (int j = 0; j < 50; j++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j].HeaderCell.Value = (j + 1).ToString();
            }
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellContentClick);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CellTyping);
            textBox1.KeyDown += new KeyEventHandler(textBox1_keyPressed);
            mySpreadSheet.CellPropertyChanged += OnCellPropertyChanged;
   
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //--------------------------- Update Cell ----------------------------
        public void OnCellPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            Cell ICell = sender as Cell;
            if (e.PropertyName == "Value")
            {
                dataGridView1.Rows[ICell._RowIndex].Cells[ICell._ColumnIndex].Value = ICell._Value;
            }
        }
        //--------------------------- Click on Cell --------------------------
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellCancelEventArgs e)
        {
            textBox1.Visible = true;
            textBox1.Text = mySpreadSheet.GetCell(e.RowIndex, e.ColumnIndex)._Text;
            rowIndex = e.RowIndex;
            colIndex = e.ColumnIndex; //to know what cell to update from textbox1
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = mySpreadSheet.GetCell(e.RowIndex, e.ColumnIndex)._Text;
        }
        //--------------------------- Cell Text Changing ---------------------
        private void dataGridView1_CellTextChanging(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
           
        }
        private void dataGridView1_CellTyping(object sender, EventArgs e)
        {
            DataGridView cell = sender as DataGridView;
            if (dataGridView1.IsCurrentCellDirty)
            {
                textBox1.Text = cell.EditingControl.Text;
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        //--------------------------- Exit Cell ------------------------------
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView mycell = sender as DataGridView;
                Cell cell = mySpreadSheet.GetCell(e.RowIndex, e.ColumnIndex);
                cell._Text = mycell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                mycell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cell._Value;
                textBox1.Visible = false;
            }
            catch
            {
                MessageBox.Show("ERROR");
                textBox1.Visible = false;
            }
        }
        //--------------------------- Edit topTextBox ------------------------

        private void textBox1_keyPressed(object sender, KeyEventArgs e)
        {

            if (e.KeyCode==Keys.Enter)
            {
                dataGridView1.Rows[rowIndex].Cells[colIndex].Value = textBox1.Text;
                textBox1.Visible = false;
            }
        }
        //--------------------------- Demo -----------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Random num = new Random();
            for(int i = 1; i<=50;i++)
            {
                int row = num.Next(0, 49);
                int col = num.Next(0, 25);
                mySpreadSheet.rowcol[row, col]._Text = "Hello World!";
            }
            for(int i = 0; i<50;i++)
            {
                mySpreadSheet.rowcol[i, 1]._Text = "This is Cell B" + (i + 1);
            }
            for(int i = 0; i<50;i++)
            {
                mySpreadSheet.rowcol[i, 0]._Text = "=B"+(i + 1);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // -------- below code from https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=netframework-4.7.2 ---------
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            Stream infile;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if((infile = openFileDialog.OpenFile()) != null)
                {
                    mySpreadSheet.Load(infile);
                }
         
            }
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ------ code taken from https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=netframework-4.7.2 ---------
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    mySpreadSheet.Save(myStream);
                }
                myStream.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //-------------------------------------------------------------------
    }
}
