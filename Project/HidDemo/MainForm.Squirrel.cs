
using Squirrel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HidDemo
{
    public partial class MainForm
    {


        /// <summary>
        /// Check for application update and ask the user to proceed if any.
        /// </summary>
        async void TrySquirrelUpdate(bool aAutoCheck = false)
        {
            // Check for Squirrel application update
#if !DEBUG
            // Prevent user from starting an update while one is already running
            updateToolStripMenuItem.Enabled = false; 

            ReleaseEntry release = null;
            using (var mgr = new UpdateManager(Program.KSquirrelUpdateUrl))
            {
                
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                //
                UpdateInfo updateInfo = null;
                try
                {
                    updateInfo = await mgr.CheckForUpdate();
                }
                //catch (WebException ex)
                //{
                //    MessageBox.Show("Update error!\n\n" + ex.ToString(), fvi.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
                catch (Exception ex)
                {
                    if (!aAutoCheck)
                    {
                        MessageBox.Show("Update error!\nCheck that you are online and try again.\nIt could also be a data corruption issue on the server side.\n\n" + ex.ToString(), fvi.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }

                if (updateInfo!=null && updateInfo.ReleasesToApply.Any()) // Check if we have any update
                {
                    // We have an update ask our user if he wants it
                    string msg = "New version available!" +
                                    "\n\nCurrent version: " + updateInfo.CurrentlyInstalledVersion.Version +
                                    "\nNew version: " + updateInfo.FutureReleaseEntry.Version +
                                    "\n\nUpdate application now?";
                    DialogResult dialogResult = MessageBox.Show(msg, fvi.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // User wants it, do the update
                        release = await mgr.UpdateApp();
                        // Backup our users settings
                        Program.BackupSettings();
                    }
                    else
                    {
                        // User cancel an update enable manual update option
                        //iToolStripMenuItemUpdate.Visible = true;
                    }
                }
                else if (updateInfo != null)
                {
                    // Don't display intrusive message for auto checks
                    if (!aAutoCheck)
                    {
                        MessageBox.Show("You are already running the latest version.", fvi.ProductName);
                    }

                }
            }

            // Restart the app
            if (release != null)
            {
                UpdateManager.RestartApp();
            }

            // Our update is completed re-enable the update button then
            updateToolStripMenuItem.Enabled = true;
#endif
        }
    }
}