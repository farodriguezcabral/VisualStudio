using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Backup
{
    public partial class Form1 : Form
    {
        DirectorySearch dir;//New DirectorySearch object

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();//close form
        }

        private void button3_Click(object sender, EventArgs e)//source search button
        {
            using(var browseDir = new FolderBrowserDialog())//show a dialog box for users to be able to search for a directory
            {
                browseDir.ShowDialog();
                textBox1.Text = String.Format("{0}",browseDir.SelectedPath);
            }
        }

        private void button4_Click(object sender, EventArgs e)//backup search button
        {
            using (var browseDir = new FolderBrowserDialog())
            {
                browseDir.ShowDialog();
                textBox2.Text = String.Format("{0}", browseDir.SelectedPath);
            }
        }

        private void button1_Click(object sender, EventArgs e)//process button
        {
            try
            {
                dir = new DirectorySearch();
                dir.Source = textBox1.Text;//set Source property to the text entered on source textbox
                dir.Backup = textBox2.Text;//set Backup property to the text entered on the backup textbox
                dir.FileExtension = comboBox1.SelectedItem.ToString().ToLower();//get the type of file selected by user on combo box
                DirectoryInfo d = null;//intialize DirectoryInfo object to set up the search directory 
                try
                {
                    d = new DirectoryInfo(textBox2.Text);
                    if (d.Exists && !Directory.EnumerateFileSystemEntries(textBox2.Text).Any())//check that the destination directory exists and is empty
                    {
                        dir.Backup = textBox2.Text;//set Backup property to destination directory
                        dir.getDirectories(dir.Source);//get all subdirectories on root directory 
                        progressBar1.Minimum = 0;//set the minimum value for the progress bar
                        progressBar1.Maximum = dir.NumberOfFiles;//set the maximum value for the progress bar            
                        dir.copyFiles(Increment);//copy the files
                    }
                    else
                        MessageBox.Show("Back up folder is not empty!\nPlease enter an existing empty folder");
                }
                catch (System.ArgumentException)//to prevent program from crashing if a show dialog to select directory is opened but no directory is selected
                {
                    d = null;
                }
            }
            catch (System.NullReferenceException)//In case there is a field that has not been filled out
            {
                MessageBox.Show("One or more fields are empty.\nPlease select a source path, backup path and type of file to be copied.");
            }
        }
        /* Increment function updates the progress bar as documents are copied
         * @progress the amount of steps is going to take to update the progress bar
         * @return the current progress
         */
        public int Increment(int progress)
        {
            progressBar1.Increment(progress);//increment progress bar
            if (progressBar1.Value == progressBar1.Maximum)//check if task has been completed
            {
                MessageBox.Show("Copying files completed");
                progressBar1.Value = 0;//reset progress bar
                textBox1.Text = "";
                textBox2.Text = "";
            }
            return progress;
        }     
    }
}
