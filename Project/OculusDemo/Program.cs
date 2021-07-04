using SharpLib.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hid = SharpLib.Hid;
using System.Windows.Forms;

namespace OculusDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Program prog = new Program();
            prog.Execute();
        }


        void Execute()
        {
            //CreateHandler();

            FormMain form = new FormMain();
            Application.Run(form);

            bool runForever = true;
            while (runForever)
            {
                Console.Write("Command [? for help]: ");
                string userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(userInput)) continue;
                string[] splitInput = userInput.Split(new string[] { " " }, 2, StringSplitOptions.None);

                switch (splitInput[0])
                {
                    case "?":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("  ?                            help (this menu)");
                        Console.WriteLine("  q                            quit");

                        break;

                    case "q":
                        runForever = false;
                        break;
                }
            }

        }


    }
}
