//Jonathan hein
//11532242
//HW 9
//11/26/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        private readonly int RowIndex;
        private readonly int ColumnIndex;
        protected string Text = "";
        protected string Value = "";
        public event PropertyChangedEventHandler PropertyChanged;



        protected void OnPropertyChange(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler!=null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public Cell(int Rowindex, int Columnindex) // cell constructor
        {
            /*● Add a RowIndex property that is read only
                ○ (set in constructor and returned through the get)
              ● Add a ColumnIndex property that is read only
                ○ (set in constructor and returned through the get)*/
    
            RowIndex = Rowindex;  //set in constructor (A-Z)
            ColumnIndex = Columnindex; //set in constructor (1-50)
        }
        
        public string _Text
        {
            get { return this.Text; }
            set
            {
                if (value != this.Text)
                {
                    this.Text = value;
                    OnPropertyChange("Text");
                }
                else
                {
                    return;
                }
            }
        }
       /*○ This represents the “evaluated” value of the cell. It will just be the Text​ property if
        the text doesn’t start with the ‘=’ character.Otherwise it will represent the
       evaluation of the formula that’s type in the cell.
       ○ Since many formulas in spreadsheet cells reference other cells we need for the
       actual spreadsheet class to set this value.
       ○ However, we don’t want the “outside world” (outside of the spreadsheet class) to
       set this value so you must design it so that only the spreadsheet class can set it,
       but anything can get it.
       ○ The big hint for this: It’s a protected property which means inheriting classes can
       see it.Inheriting classes should NOT be publicly exposed to code outside the
       class library.
       ○ So to summarize, this Value​ property is a getter only and you’ll have to
       implement a way to allow the spreadsheet class to set the value, but no other
       class can.*/
       public string _Value
        {
            get { return this.Value; }
        }
        public int _RowIndex
        {
            get { return this.RowIndex; }
        }
        public int _ColumnIndex
        {
            get { return this.ColumnIndex; }
        }
    }
}
