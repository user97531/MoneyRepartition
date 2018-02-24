using System.Threading;
using PythonBackup.Settings;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Constants.ExpLength = 23;
            Constants.AdditionalParameters["min age of entry"] = "12";
            Constants.AdditionalParameters["myUselessParameter"] = "A random value";
            Constants.Save();
            //GaussianDistrTest.Test();
        }
    }
}