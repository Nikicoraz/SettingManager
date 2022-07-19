
namespace SettingsManager
{
    public static class SettingsManager
    {
        /// <summary>
        /// Throws when you try to set a setting with a type different from the type that initialized it
        /// </summary>
        private class TypeErrorException : Exception
        {
            public TypeErrorException(string? message) : base(message){}
        }

        /// <summary>
        /// Throws when the setting file isn't found or when you haven't set a single setting
        /// </summary>
        private class SettingFileNotFound : Exception
        {
            public SettingFileNotFound(string? message) : base(message) { }
        }

        /// <summary>
        /// Throws when you try to get a setting that hasn't been declared yet
        /// </summary>
        private class SettingNotFound : Exception
        {
            public SettingNotFound(string? message) : base(message) { }
        }

        /// <summary>
        /// Path to the settings directory
        /// </summary>
        private static string SettingsPath = ".";

        private const char delimitor = (char)28;
        private const char groupDelimitor = (char)30;

        /// <summary>
        /// Save a setting in the settings file.
        /// If the setting already exists it will be overritten.
        /// </summary>
        /// <param name="settingName">The name of the setting</param>
        /// <param name="settingValue">The value of the setting</param>
        /// <exception cref="TypeErrorException">If you try to set a setting to a different type</exception>
        public static void SaveSetting(string settingName, dynamic settingValue)
        {
            int status = 0;

            string filePath = SettingsPath + "/.settings";
            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }
            string oldSettings = "";
            if (File.Exists(filePath))
            {
                StreamReader sr = File.OpenText(filePath);
                oldSettings = sr.ReadToEnd();
                sr.Close();
            }
            string newSettings = "";
            if (!string.IsNullOrEmpty(oldSettings))
            {
                foreach(string setting in oldSettings.Split(groupDelimitor))
                {
                    string[] splitSetting = setting.Split(delimitor);
                    string type = splitSetting[0];
                    string name = splitSetting[1];
                    if(name == settingName)
                    {
                        if(type != settingValue.GetType().ToString())
                        {
                            throw new TypeErrorException("This setting is saved with another type!");
                        }
                        status = 1;
                        splitSetting[2] = settingValue.ToString();
                        newSettings += string.Join(delimitor, splitSetting) + groupDelimitor;
                        continue;
                    }
                    newSettings += setting + groupDelimitor;
                }
            }
            if (status == 0)
            {
                newSettings += settingValue.GetType().ToString() + delimitor + settingName + delimitor + settingValue.ToString() + groupDelimitor;
            }
            newSettings = newSettings.Remove(newSettings.Length - 1, 1);
            StreamWriter sw = File.CreateText(filePath);
            sw.Write(newSettings);
            sw.Close();
        }
        
        /// <summary>
        /// Gets a setting value in it's type from the settings file.
        /// If the type isn't supported returns a string
        /// </summary>
        /// <param name="settingName">The settings name</param>
        /// <returns>The setting value</returns>
        /// <exception cref="SettingFileNotFound"></exception>
        /// <exception cref="SettingNotFound"></exception>
        public static dynamic GetSetting(string settingName)
        {
            string filePath = SettingsPath + "/.settings";
            if (!File.Exists(filePath))
            {
                throw new SettingFileNotFound("The settings file was not found!");
            }

            StreamReader sr = File.OpenText(filePath);
            string settings = sr.ReadToEnd();
            sr.Close();

            foreach (string setting in settings.Split(groupDelimitor))
            {
                string[] splitSettings = setting.Split(delimitor);
                if (splitSettings[1] == settingName)
                {
                    if (splitSettings[0] == typeof(int).ToString())
                    {
                        return Convert.ToInt32(splitSettings[2]);
                    } else if (splitSettings[0] == typeof(float).ToString())
                    {
                        return float.Parse(splitSettings[2]);
                    } else if (splitSettings[0] == typeof(double).ToString())
                    {
                        return Convert.ToDouble(splitSettings[2]);
                    }
                    else if (splitSettings[0] == typeof(string).ToString())
                    {
                        return splitSettings[2];
                    } else if (splitSettings[0] == typeof(char).ToString())
                    {
                        return Convert.ToChar(splitSettings[2]);
                    }
                    else
                    {
                        return splitSettings[2];
                    }
                }
            }

            throw new SettingNotFound("The requested setting was not found!");
            
        }
        
        /// <summary>
        /// Removes the setting from the settings file
        /// </summary>
        /// <param name="settingName">The settings name</param>
        /// <exception cref="SettingFileNotFound"></exception>
        public static void RemoveSetting(string settingName)
        {
            string filePath = SettingsPath + "/.settings";
            if (!File.Exists(filePath))
            {
                throw new SettingFileNotFound("The settings file was not found!");
            }

            StreamReader sr = File.OpenText(filePath);
            string settings = sr.ReadToEnd();
            sr.Close();

            string newSettings = "";

            foreach(string setting in settings.Split(groupDelimitor))
            {
                if (settingName == setting.Split(delimitor)[1])
                {
                    continue;
                }
                newSettings += setting + groupDelimitor;
            }

            newSettings = newSettings.Remove(newSettings.Length - 1, 1);
            StreamWriter sw = File.CreateText(filePath);
            sw.Write(newSettings);
            sw.Close();
        }

        /// <summary>
        /// Set where the settings are saved
        /// </summary>
        /// <param name="path">The path where the .settings file is</param>
        public static void SetSettingsPath(string path)
        {
            SettingsPath = path;
        }
    }
}