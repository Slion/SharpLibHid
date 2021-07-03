//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpDisplayManager.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Principal;
using Squirrel;
using System.Configuration;
using System.IO;

namespace HidDemo
{
    static class Program
    {
        //WARNING: This is assuming we have a single instance of our program.
        //That is what we want but we should enforce it somehow.
        public static MainForm iFormMain;

        public static string SettingsFilePath;
        public static string SettingsBackupFilePath;

        /// <summary>
        /// 
        /// </summary>
        public const string KSquirrelUpdateUrl = "http://publish.slions.net/HidDemo/Squirrel";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //
            SettingsFilePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            SettingsBackupFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";

#if !DEBUG
            // SL: Do Squirrel admin stuff.
            // NB: Note here that HandleEvents is being called as early in startup
            // as possible in the app. This is very important! Do _not_ call this
            // method as part of your app's "check for updates" code.
            using (var mgr = new UpdateManager(KSquirrelUpdateUrl))
            {
                // Note, in most of these scenarios, the app exits after this method
                // completes!
                SquirrelAwareApp.HandleEvents(
                  onInitialInstall: v =>
                  {
                      //MessageBox.Show("onInitialInstall " + v + " Current:" + mgr.CurrentlyInstalledVersion());
                      mgr.CreateShortcutForThisExe();
                  },
                  onAppUpdate: v =>
                  {
                      mgr.CreateShortcutForThisExe();
                      //Not a proper Click Once installation, assuming development build then
                      //var assembly = Assembly.GetExecutingAssembly();
                      //var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                      //string version = " - v" + versionInfo.ProductVersion;
                      //MessageBox.Show("onAppUpdate " + v + " Current:" + mgr.CurrentlyInstalledVersion() + version);
                  },
                  onAppObsoleted: v =>
                  {
                      //MessageBox.Show("onAppObsoleted " + v + " Current:" + mgr.CurrentlyInstalledVersion());
                  },
                  onAppUninstall: v =>
                  {
                      //MessageBox.Show("onAppUninstall " + v + " Current:" + mgr.CurrentlyInstalledVersion());
                      mgr.RemoveShortcutForThisExe();
                  },
                  onFirstRun: () =>
                  {
                      //MessageBox.Show("onFirstRun " + mgr.CurrentlyInstalledVersion());
                  });
            }
#endif
            RestoreSettings(SettingsBackupFilePath, SettingsFilePath);

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            //
            Application.EnableVisualStyles(); // Otherwise it looks like Windows 95
            //Application.SetCompatibleTextRenderingDefault(false);
            //
            iFormMain = new MainForm();
            Application.Run(iFormMain);
        }


        /// <summary>
        /// Make a backup of our settings.
        /// Used to persist settings across updates.
        /// </summary>
        public static void BackupSettings()
        {
            // Application Settings 
            try
            {
                File.Copy(SettingsFilePath, SettingsBackupFilePath, true);
            }
            catch
            {

            }

        }

        /// <summary>
        /// Restore our settings backup if any.
        /// Used to persist settings across updates.
        /// </summary>
        private static void RestoreSettings(string aSource, string aDestination)
        {
            //Restore settings after application update            
            // Check if we have settings that we need to restore
            if (!File.Exists(aSource))
            {
                // Nothing we need to do
                return;
            }

            //MessageBox.Show("Source: " + sourceFile);
            //MessageBox.Show("Dest: " + destFile);

            // Create directory as needed
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(aDestination));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

            // Copy our backup file in place 
            try
            {
                File.Copy(aSource, aDestination, true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

            // Delete backup file
            try
            {
                File.Delete(aSource);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

            //MessageBox.Show("After copy");
        }


        private static bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Occurs after form closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {

        }
    }
}
