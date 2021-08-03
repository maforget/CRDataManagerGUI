using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Xml.Linq;

namespace DataManagerGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DebugAppend("Checking for Running Instance");
            bool ok;
            Mutex m = new System.Threading.Mutex(true, "crdmcgui", out ok);

            if (!ok)
            {
                DebugAppend("Instance Found Execution will be aborted");
                MessageBox.Show("Another instance is already running.");
                return;
            }

            DebugAppend("Enabling Visual Styles...");
            Application.EnableVisualStyles();
            DebugAppend("Setting Compatible Text Rendering to false...");
            Application.SetCompatibleTextRenderingDefault(false);
            DebugAppend("Starting GUI...");
            Application.Run(new gui());
            DebugAppend("Ensuring only a single instance is allowed...");
            GC.KeepAlive(m);
        }
        public static void DebugAppend(string strDebugInfo)
        {
            string outFile = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "guidebug.log";
            using (System.IO.FileStream tmp = new System.IO.FileStream(outFile, System.IO.FileMode.Append))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(tmp);
                sw.WriteLine(string.Format("{0} ##{1}", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), strDebugInfo));
                sw.Close();
            }
        }

        public static void SendToClipboard(object item)
        {
            Type tmpType = item.GetType();

            if (tmpType == typeof(dmRuleset))
                Clipboard.SetData(typeof(dmRuleset).FullName, ((dmRuleset)item).ToXML("ruleset").ToString());
            else if (item.GetType() == typeof(dmGroup))
                Clipboard.SetData(typeof(dmGroup).FullName, ((dmGroup)item).ToXML("group").ToString());
            else if (item.GetType() == typeof(dmRule))
                Clipboard.SetData(typeof(dmRule).FullName, ((dmRule)item).ToXML("rule").ToString());
            else if (item.GetType() == typeof(dmAction))
                Clipboard.SetData(typeof(dmAction).FullName, ((dmAction)item).ToXML("action").ToString());
            else if (item.GetType() == typeof(List<dmRule>))
            {
                XElement xElement = new XElement("rules");
                foreach (dmRule item2 in (List<dmRule>)item)
                {
                    xElement.Add(item2.ToXML("rule"));
                }
                Clipboard.SetData(item.GetType().FullName, xElement.ToString());
            }
            else if (item.GetType() == typeof(List<dmAction>))
            {
                XElement xElement = new XElement("actions");
                foreach (dmAction item3 in (List<dmAction>)item)
                {
                    xElement.Add(item3.ToXML("action"));
                }
                Clipboard.SetData(item.GetType().FullName, xElement.ToString());
            }
        }
    }
}
