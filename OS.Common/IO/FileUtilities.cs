using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace OS.IO
{
    public class FileUtilities
    {

        #region Properties

        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;

        #endregion

        #region Static Methods

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Execution)]
        static public bool RunExecutable(String path, String Arg, bool wait)
        {
            object RunExecutable = new object();
            lock (RunExecutable)
            {
                try
                {
                    Process pgm = new Process();
                    pgm.StartInfo.FileName = path;
                    pgm.StartInfo.Arguments = Arg;
                    //		MessageBox.Show(path + " " + Arg);
                    if (wait == true)
                    {
                        //    OnInstStarted(EventArgs.Empty); // Tell top window sub executable has started
                        //					this.Hide();
                        //		this.Enabled = false;
                        pgm.WaitForExit();
                        //		this.Enabled = true;
                        //					this.Show();
                        //      OnInstDone(EventArgs.Empty);	// Tell top window sub executable has finished
                        //      this.Focus();
                    }
                    pgm.Start();
                }

                catch (Win32Exception exc)
                {
                    string txt;
                    switch (exc.NativeErrorCode)
                    {
                        case ERROR_FILE_NOT_FOUND:
                            txt = string.Format("{0} \n\n\rCannot be found. {1} Check the path.", path, exc.Message);
                            break;
                        case ERROR_ACCESS_DENIED:
                            txt = string.Format("{0} - Access denied.", exc.Message);
                            break;
                        default:
                            txt = string.Format("Error running executable. - {0}", exc.Message);
                            break;
                    }
                    //MessageBox.Show(null, txt, "Execute Error", 0, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Writes a new binary file from the byte array passed in
        /// </summary>
        /// <param name="pathname"></param>
        /// <param name="data"></param>
        public static void WriteBinaryFile(string pathname, byte[] data)
        {
            if (data == null) return;
            if (pathname.Length == 0)
            {
                // MessageBox.Show("The file path is invalid", "Text File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            object writeLock = new object();
            lock (writeLock)
            {

                string folderPath = Path.GetDirectoryName(Uri.UnescapeDataString(pathname));
                if (!Directory.Exists(folderPath))
                {
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }

                BinaryWriter bw = new BinaryWriter(File.Open(pathname, FileMode.Create));
                bw.Write(data);
                bw.Flush();
                bw.Close();
                //FileStream fs;

                //fs = File.Create(Uri.UnescapeDataString(pathname));//,System.IO.FileMode.Create,System.IO.FileAccess.Write, System.IO.FileShare.None);
                //fs.Write(data,0, data.Length);
                //fs.Close();
            }
        }
        /// <summary>
        /// Writes a string of data to a text file. The function can append or overwrite from the beginnng
        /// </summary>
        /// <param name="pathname"></param>
        /// <param name="data"></param>
        /// <param name="append"></param>
        public static void WriteTextFile(string pathname, string data, bool append)
        {
            if (pathname.Length == 0)
            {
                // MessageBox.Show("The file path is invalid", "Text File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            object WriteTextFile = new object();
            lock (WriteTextFile)
            {
                StreamWriter sr;
                if (append == true)
                {
                    sr = File.AppendText(pathname);//,System.IO.FileMode.Append,System.IO.FileAccess.Write, System.IO.FileShare.None);
                }
                else
                {
                    string folderPath = Path.GetDirectoryName(Uri.UnescapeDataString(pathname));
                    if (!Directory.Exists(folderPath))
                    {
                        if (!string.IsNullOrEmpty(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                    }

                    sr = File.CreateText(Uri.UnescapeDataString(pathname));//,System.IO.FileMode.Create,System.IO.FileAccess.Write, System.IO.FileShare.None);
                }
                sr.Write(data);
                sr.Close();
            }
        }






        //static public string BrowseForFile(bool toOpen, string name, string heading, string extention)
        //{
        //    if (toOpen == true)
        //    {
        //        OpenFileDialog fdlg = new OpenFileDialog();
        //        fdlg.FileName = name;
        //        fdlg.Title = heading;
        //        fdlg.DefaultExt = extention;
        //        fdlg.Filter = extention + " files (*." + extention + ")|*." + extention + "|All files (*.*)|*.*";
        //        fdlg.RestoreDirectory = true;
        //        if (fdlg.ShowDialog() == DialogResult.Cancel) return null;
        //        return fdlg.FileName;
        //    }
        //    else
        //    {
        //        SaveFileDialog fdlg = new SaveFileDialog();
        //        fdlg.FileName = name;
        //        fdlg.Title = heading;
        //        fdlg.DefaultExt = extention;
        //        fdlg.Filter = extention + " files (*." + extention + ")|*." + extention + "|All files (*.*)|*.*";
        //        fdlg.RestoreDirectory = true;
        //        if (fdlg.ShowDialog() == DialogResult.Cancel) return null;
        //        return fdlg.FileName;
        //    }
        //}
        ///// <summary>
        ///// This is a static utility function that you can pass multiple extentions to for file browsing
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="heading"></param>
        ///// <param name="extentions"></param>
        ///// <returns></returns>
        //static public string BrowseForFile(bool toOpen, string name, string heading, string[] extentions)
        //{
        //    if (extentions.Length == 0) return null;

        //    string txt;
        //    DialogResult dlgRes;
        //    string luk;
        //    try
        //    {
        //        if (toOpen == true)
        //        {
        //            using (OpenFileDialog fdlg = new OpenFileDialog())
        //            {
        //                fdlg.FileName = name;
        //                fdlg.Title = heading;
        //                fdlg.DefaultExt = extentions[0];
        //                fdlg.Filter = "";
        //                foreach (string st in extentions)
        //                {
        //                    luk = ((fdlg.Filter.Length != 0) ? "|" : "") + st + " files (*." + st + ")|*." + st;
        //                    fdlg.Filter += luk;// st + " files (*." + st + ")|*." + st + "|";
        //                }
        //                fdlg.Filter += "|All files (*.*)|*.*";
        //                fdlg.RestoreDirectory = true;
        //                dlgRes = fdlg.ShowDialog();
        //                txt = fdlg.FileName;
        //                if (dlgRes == DialogResult.Cancel) return null;
        //                else return txt;
        //            }
        //        }
        //        else
        //        {
        //            using (SaveFileDialog fdlg = new SaveFileDialog())
        //            {
        //                fdlg.FileName = name;
        //                fdlg.Title = heading;
        //                fdlg.DefaultExt = extentions[0];
        //                fdlg.Filter = "";
        //                foreach (string st in extentions)
        //                {
        //                    luk = ((fdlg.Filter.Length != 0) ? "|" : "") + st + " files (*." + st + ")|*." + st;
        //                    fdlg.Filter += luk;// st + " files (*." + st + ")|*." + st + "|";
        //                }
        //                fdlg.Filter += "|All files (*.*)|*.*";
        //                fdlg.RestoreDirectory = true;
        //                dlgRes = fdlg.ShowDialog();
        //                txt = fdlg.FileName;
        //                if (dlgRes == DialogResult.Cancel) return null;
        //                else return txt;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("There was a browsing error: " + ex.Message, "Browse For File", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}
        //static public string BrowseForFolder(string startFolder, string heading)
        //{
        //    string txt;
        //    DialogResult dlgRes;
        //    try
        //    {
        //        using (FolderBrowserDialog fdlg = new FolderBrowserDialog())
        //        {
        //            //fdlg.RootFolder = Environment.SpecialFolder.;
        //            fdlg.SelectedPath = startFolder;
        //            fdlg.Description = heading;
        //            dlgRes = fdlg.ShowDialog();
        //            txt = fdlg.SelectedPath;
        //            if (dlgRes == DialogResult.Cancel) return null;
        //            else return txt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("There was a browsing error: " + ex.Message, "Browse For Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}

        #endregion

    }

}
