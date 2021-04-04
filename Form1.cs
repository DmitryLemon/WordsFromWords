using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordsFromWords
{
    public partial class Form1 : Form
    {
        Searcher src = new Searcher();

        private string wordspath = "";
        private int PageCount = 0;
        private int CurrentPage = 1;
        private int GlobalEntryNumber = 250;

        public Form1()
        {
            InitializeComponent();

            textBox2.KeyDown += textBox2_KeyDown;

            numericUpDown1.ValueChanged += (object sender, EventArgs e) => NumericUpDownPage_ValueChanged(sender, e, textBox3, numericUpDown1, labelFromPages1, textBox1.Text, 1);
            numericUpDown2.ValueChanged += (object sender, EventArgs e) => NumericUpDownPage_ValueChanged(sender, e, textBox4, numericUpDown2, labelFromPages2, wordspath, 2);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы txt|*.txt|Все файлы|*";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                wordspath = OPF.FileName;
            }
            showListOnScreen(textBox4, src.readWordList(wordspath), numericUpDown2, labelFromPages2);
            showListOnScreen(textBox3, src.search(textBox1.Text), numericUpDown1, labelFromPages1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            showListOnScreen(textBox3, src.search(textBox1.Text), numericUpDown1, labelFromPages1);;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox2.Text != "")
            {
                List<string> xList = new List<string>();
                xList = src.addWord(textBox2.Text, wordspath);
                textBox2.Text = "";
                showListOnScreen(textBox4, xList, numericUpDown2, labelFromPages2);
                showListOnScreen(textBox3, src.search(textBox1.Text), numericUpDown1, labelFromPages1);
            }
        }

        private void NumericUpDownPage_ValueChanged(object sender, EventArgs e, TextBox textBox, NumericUpDown numericUpDown, Label labelFromPages, string returnResultEntry, int choice)
        {
            List<string> xList = src.returnResult(returnResultEntry, choice);
            PageCount = xList.Count / GlobalEntryNumber;
            if (PageCount > 0 && (xList.Count % GlobalEntryNumber > 0)) PageCount += 1;
            showListOnScreen(textBox, xList, numericUpDown, labelFromPages);
        }


        private void showListOnScreen(TextBox textBox, List<string> showList, NumericUpDown numericUpDown, Label labelFromPages)
        {
            textBox.Text = "";
            int EntryNumber = GlobalEntryNumber;
            CurrentPage = (int)numericUpDown.Value;
            if (showList.Count < EntryNumber)
            {
                EntryNumber = showList.Count;
                PageCount = 0;
                numericUpDown.Enabled = false;
                labelFromPages.Text = "из 1";
            }
            else
            {
                PageCount = showList.Count / EntryNumber;
                if (PageCount > 0 && (showList.Count % EntryNumber > 0)) PageCount += 1;
                numericUpDown.Enabled = true;
                numericUpDown.Maximum = PageCount;
            }
            if (CurrentPage > PageCount) CurrentPage = 1;
            numericUpDown.Value = CurrentPage;
            if (EntryNumber != 0)
            {
                if ((CurrentPage == PageCount) && (showList.Count % EntryNumber > 0))
                {
                    if (PageCount == 0) PageCount = 1;
                    labelFromPages.Text = "из " + PageCount + " (" + showList[EntryNumber * (CurrentPage - 1)][0] + showList[EntryNumber * (CurrentPage - 1)][1] + "-" + showList[(EntryNumber * (CurrentPage - 1)) + (showList.Count % EntryNumber) - 1][0] + showList[(EntryNumber * (CurrentPage - 1)) + (showList.Count % EntryNumber) - 1][1] + ")";
                    for (int i = EntryNumber * (CurrentPage - 1); i < EntryNumber * (CurrentPage - 1) + (showList.Count % EntryNumber); i++)
                    {
                        textBox.Text += showList[i].ToString() + "\r\n";
                    }
                }
                else
                {
                    if (PageCount == 0) PageCount = 1;
                    labelFromPages.Text = "из " + PageCount + " (" + showList[EntryNumber * (CurrentPage - 1)][0] + showList[EntryNumber * (CurrentPage - 1)][1] + "-" + showList[EntryNumber * CurrentPage - 1][0] + showList[EntryNumber * CurrentPage - 1][1] + ")";
                    for (int i = EntryNumber * (CurrentPage - 1); i < EntryNumber * CurrentPage; i++)
                    {
                        textBox.Text += showList[i].ToString() + "\r\n";
                    }
                }
            }
        }
    }
}
