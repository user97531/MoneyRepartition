using System.Collections.Generic;
using System.IO;

namespace PythonBackup.Settings
{
    public static class Constants
    {
        private static string SettingsDirectory = Path.GetFullPath("./");
        private const string SettingsFile = "Settings.xml";
        private static string SettingsPath = Path.Combine(SettingsDirectory, SettingsFile);
        private static ParameterSet currentSetting;
        private static FileSystemWatcher fsw;
        private static bool listenToChanges = false;

        // =============================================================================
        // MONEYWISE
        // =============================================================================
        public static int StartCurrency
        {
            get
            {
                return currentSetting.StartCurrency;
            }
            set
            {
                currentSetting.StartCurrency = value;
            }
        }

        /* =============================================================================
		MEMBER NUMBERS
		=============================================================================*/
        public static int NStart
        {
            get
            {
                return currentSetting.NStart;
            }
            set
            {
                currentSetting.NStart = value;
            }
        }

        public static int SteadyMembers
        {
            get
            {
                return currentSetting.SteadyMembers;
            }
            set
            {
                currentSetting.NStart = SteadyMembers;
            }
        }

        public static int MaxMembers
        {
            get
            {
                return currentSetting.MaxMembers;
            }
            set
            {
                currentSetting.MaxMembers = value;
            }
        }

        public static int MinMembers
        {
            get
            {
                return currentSetting.MinMembers;
            }
            set
            {
                currentSetting.MinMembers = value;
            }
        }

        /// <summary>
        /// Dictates the growth
        /// </summary>
        public static float NewProp
        {
            get
            {
                return currentSetting.NewProp;
            }
            set
            {
                currentSetting.NewProp = value;
            }
        }

        /// <summary>
        /// Dictates the decline
        /// </summary>
        public static float LeavingProp
        {
            get
            {
                return currentSetting.LeavingProp;
            }
            set
            {
                currentSetting.LeavingProp = value;
            }
        }

        /*=============================================================================
		TIMEWISE
		=============================================================================*/
        //As 1 year has not a fixed length we use days. The Timespan class allow to store durations not specific to any unit. You can manipulate them more easily than values stored in different units.
        //Like adding/substracting (negative values allowed) several of them, multiplying them by a number or adding them to a date to get another date.
        /// <summary>
        /// Average life expectancy in year
        /// </summary>
        public static int LifeExpectancy
        {
            get
            {
                return currentSetting.LifeExpectancy;
            }
            set
            {
                currentSetting.LifeExpectancy = value;
            }
        }

        /// <summary>
        /// Max longevity of a human
        /// </summary>
        public static float MaxLongevity
        {
            get
            {
                return currentSetting.MaxLongevity;
            }
            set
            {
                currentSetting.MaxLongevity = value;
            }
        }

        /// <summary>
        /// Deviation around the average life expectancy in years
        /// </summary>
        public static float Sigma
        {
            get
            {
                return currentSetting.Sigma;
            }
            set
            {
                currentSetting.Sigma = value;
            }
        }

        /// <summary>
        /// Experiment length in month
        /// </summary>
        public static int ExpLength
        {
            get
            {
                return currentSetting.ExpLength;
            }
            set
            {
                currentSetting.ExpLength = value;
            }
        }

        /// <summary>
        /// Potential additional parameters
        /// </summary>
        public static Dictionary<string, string> AdditionalParameters
        {
            get
            {
                return currentSetting.AdditionalParameters;
            }
            set
            {
                currentSetting.AdditionalParameters = value;
            }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static Constants()
        {
            if (File.Exists(SettingsPath))
            {
                ReadSettings();
            }
            else
            {
                SetDefaultValues();
            }
            fsw = new FileSystemWatcher(SettingsDirectory);
            fsw.Filter = SettingsFile;
            fsw.IncludeSubdirectories = false;
            ListenToChanges(true);
            fsw.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Save settings on the disk
        /// </summary>
        public static void Save()
        {
            ListenToChanges(false);
            currentSetting.Serialize(SettingsPath);
            ListenToChanges(true);
        }

        /// <summary>
        /// Enable or disable watch on settings file changes
        /// </summary>
        /// <param name="enabled">True to enable, else false</param>
        private static void ListenToChanges(bool enabled)
        {
            if (enabled && !listenToChanges)
            {
                listenToChanges = true;
                fsw.Changed += UpdateSettings;
                fsw.Created += UpdateSettings;
            }
            if (!enabled && listenToChanges)
            {
                listenToChanges = true;
                fsw.Changed -= UpdateSettings;
                fsw.Created -= UpdateSettings;
            }
        }

        /// <summary>
        /// Called when settings file is modified
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="args">Parameter of the event</param>
        private static void UpdateSettings(object sender, FileSystemEventArgs args)
        {
            ReadSettings();
        }

        /// <summary>
        /// Reads the settings from the file
        /// </summary>
        private static void ReadSettings()
        {
            currentSetting = ParameterSet.Deserialize(SettingsPath);
        }

        /// <summary>
        /// Gets the default settings values
        /// </summary>
        private static void SetDefaultValues()
        {
            currentSetting = new ParameterSet();
        }
    }
}