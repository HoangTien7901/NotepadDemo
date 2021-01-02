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

namespace NotepadDemo
{
    public partial class Form1 : Form
    {
        string fileName = "Untitled";
        Stack<string> undoList = new Stack<string>();
        Stack<string> redoList = new Stack<string>();
        public Form1()
        {
            InitializeComponent();
        }
        //load Data
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = fileName + "- Notepad";
            LoadSetting();
        }
        //save setting location,width,height
        private void SaveSetting()
        {
            Properties.Settings.Default.Location = this.Location;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.Height = this.Height;
            Properties.Settings.Default.Font = this.Font;
            Properties.Settings.Default.WordWrap = textBoxContent.WordWrap;
            Properties.Settings.Default.Save();

        }
        //load setting location,width,height when loadForm
        private void LoadSetting()
        {
            this.Location = Properties.Settings.Default.Location;
            this.Width = Properties.Settings.Default.Width;
            this.Height = Properties.Settings.Default.Height;
            this.Font = Properties.Settings.Default.Font;
            textBoxContent.WordWrap = Properties.Settings.Default.WordWrap;
            WordWrapToolStripMenuItem.Checked = textBoxContent.WordWrap;
        }

        //start Close form
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WarningDialog();
            SaveSetting();
        }
        //end Close form
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                /*fontDialog.ShowColor = true;
                fontDialog.ShowApply = true;
                fontDialog.ShowEffects = true;
                fontDialog.ShowHelp = true;*/
                textBoxContent.Font = fontDialog.Font;


            }
        }
        //Start Menu save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save as";
            saveFileDialog.InitialDirectory = @"D:\tien\Windowform\data";
            saveFileDialog.Filter = "Text Documents|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                File.WriteAllText(saveFileDialog.FileName, textBoxContent.Text);
                this.Text = fileName;
            }
        }
        //End Menu Save as
        //Start Menu Save
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (fileName.Equals("Untitled"))
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
            else
            {
                File.WriteAllText(fileName, textBoxContent.Text);

            }

        }
        //End Menu Save
        //End Text Change Event
        private void textBoxContent_TextChanged(object sender, EventArgs e)
        {
            if (textBoxContent.Modified)
            {
                this.Text = "*" + fileName + " - Notepad";
            }
            undoList.Push(textBoxContent.Text);
            
        }
        //End Text Change Event

        //Start Menu New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WarningDialog() != DialogResult.Cancel)
            {
                fileName = "Untitled";
                this.Text = fileName + " - Notepad";
                textBoxContent.Text = null;
            }
        }
        //End Menu New
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WarningDialog() != DialogResult.Cancel)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open File";
                openFileDialog.Multiselect = false;
                openFileDialog.InitialDirectory = @"D:\tien\Windowform\data";
                openFileDialog.Filter = "Text Documents|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                    this.Text = fileName + " - Notepad";
                    textBoxContent.Text = File.ReadAllText(openFileDialog.FileName);
                }
            }

        }
        private DialogResult WarningDialog()
        {
            var result = DialogResult.OK;
            if (textBoxContent.Modified)
            {
                result = MessageBox.Show("Do you Want to save changes to \n "
                    + fileName, "Notepad", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click_1(null, null);
                }
            }
            return result;
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form1 = new Form1();
            form1.Show();
        }

        private void WordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WordWrapToolStripMenuItem.Checked)
            {
                textBoxContent.WordWrap = true;
            }
            else
            {
                textBoxContent.WordWrap = false;
            }
        }

        //Start Menu cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxContent.SelectedText = string.Empty;
        }
        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(null, null);
        }
        //end Menu cut
        //Start Menu Copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBoxContent.SelectedText);
            }
            catch
            {
               
            }
        }
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(null, null);
        }
        //end Menu Copy
        //Start Menu Paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pasteText = Clipboard.GetText();
            textBoxContent.Text = textBoxContent.Text.Insert(textBoxContent.SelectionStart, pasteText);
        }
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(null, null);
        }
        //end Menu Paste
        //Start Menu SelectAll
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxContent.SelectAll();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectAllToolStripMenuItem_Click(null, null);
        }
        //end Menu SelectAll

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectedText.Length > 0)
            {
                cutToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
            }
            else
            {
                cutToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
            }
            if (string.IsNullOrEmpty(Clipboard.GetText()))
            {
                pasteToolStripMenuItem.Enabled = false;
            }
        }
        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBoxContent.SelectedText.Length > 0)
            {
                cutToolStripMenuItem1.Enabled = true;
                copyToolStripMenuItem1.Enabled = true;
            }
            else
            {
                cutToolStripMenuItem1.Enabled = false;
                copyToolStripMenuItem1.Enabled = false;
            }
            if (string.IsNullOrEmpty(Clipboard.GetText()))
            {
                pasteToolStripMenuItem1.Enabled = false;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxContent.Text = undoList.Pop();
                redoList.Push(undoList.Pop());
            }
            catch
            {

            }
        }

        private void textBoxContent_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Z && e.Control)
                {
                    textBoxContent.Text = undoList.Pop();
                    redoList.Push(undoList.Pop());
                }
                if(e.KeyCode == Keys.Y && e.Control)
                {
                    textBoxContent.Text = redoList.Pop();
                    undoList.Push(redoList.Pop());
                }
            }
            catch
            {
               
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxContent.Text = redoList.Pop();
                undoList.Push(redoList.Pop());
            }
            catch
            {

            }
        }
    }
}
