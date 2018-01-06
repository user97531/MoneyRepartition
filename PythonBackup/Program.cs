using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonBackup
{
    public static class Program
    {
        public static Form mainForm;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new MainWindow();
            //Create an instance of the experiment
            Experiment exp = new Experiment("FeedBack");
            exp.RunExp();
            Application.Run(mainForm);
        }
    }
}
