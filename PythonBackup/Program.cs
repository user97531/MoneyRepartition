using System;
using System.Windows.Forms;

namespace PythonBackup
{
    public static class Program
    {
        public static MainWindow MainForm;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainWindow();
            //Create an instance of the experiment
            Experiment exp = new Experiment("FeedBack");
            exp.RunExp();
            Application.Run(MainForm);
        }
    }
}