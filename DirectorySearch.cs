using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace Backup
{
    class DirectorySearch
    {
       
        private string sourceDir;//directory documents will be copied from
        private string backupDir;//directory documents will be copied to
        private string extension;//type of documents to be copied (i.e. .tif, .bin, .dat, .pdf)
        private Dictionary<string, string> directories = new Dictionary<string,string>();//Dictionary that stores source path and destination path that corresponds
     
        public string Source//Source property to set and get the source directory
        {
            get { return sourceDir; }
            set { sourceDir = value; }
        }

        public int NumberOfFiles//NumberOfFiles property to get the number of files that will be copied for progress calculation purposes
        {
            get { return directories.Count; }
        }

        public String Backup//Backup property to set the directory in which files will be backed up
        {
            set { backupDir = value; }
        }

        public string FileExtension//FileExtension property to set the type of files to be searched and copied
        {
            set { extension = "*."+ value; }
        }

        /*copyFiles function copies the files from the source directory to the back up destination
         * @param callback a call to the Increment function in Form1.cs to update the progress bar as files are copied
         */
        public void copyFiles(Func<int,int> callback)
        {
            foreach (KeyValuePair<string,string> file in directories)//iterate through each source,destination pair to copy files
            {
                 System.IO.File.Copy(file.Key, file.Value, true);//copy the current file
                 callback(1);//make a call to Increment function in Form1.cs to show the progress on progress bar
            }
        }
        /*getDirectories function populates a dictionary storing the source path of the file and the destination path where it will be copied to
         * @param path the current path where subdirectories will be searched for
         */
        public void getDirectories(string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);//get the subdirectories in the current path
                var files = Directory.EnumerateFiles(path, extension);//get a list of the files that were found in the directory
                if (dirs.Length > 0 || files != null)
                {
                    if (!path.Equals(backupDir))//avoid program to try to copy the backup directory to itself
                    {
                        foreach (string curr in files)//Iterate through each file found
                        {
                            string fileName = curr.Substring(path.Length + 1);//get the name of the file without its path
                            string source = System.IO.Path.Combine(path, fileName); //setup the source path (including the name of the file)
                            string dest = backupDir + "\\" + curr.Substring(sourceDir.Length + 1);//set up the destination directory path that keeps the folder structure of the source
                            string destStruct = dest.Replace(fileName, "");//set up folder structure
                            directories.Add(source, dest);//add both the source destination and its corresponding destination directory 
                            Directory.CreateDirectory(destStruct);//create the destination directory for the current file
                        }
                    }
                    foreach (string s in dirs)//iterate through each subdirectory to get all the files starting from the root directory (i.e. sourceDir)
                    {
                        getDirectories(s);//recursive call to search for files on every subfolder
                    }
                }
            }
            catch (System.UnauthorizedAccessException)//catch exception in case user does not have proper access to documents
            {
                MessageBox.Show("Some files could not be accessed because user does not have the proper permissions to copy them!");
            }
        }
    }
}
