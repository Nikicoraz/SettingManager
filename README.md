# Setting Manager
A simple C# setting manager library
## Install
Grab the dll from the releases, include it in the project, incude it in the `using` section.
## Usage
- To save just call the `SettingsManager.SaveSetting(<Setting Name>, <Setting Value>)` function
- To read the settings call the `SettingsManager.GetSetting(<Setting Name>)` function
- To remove a setting call the `SettingsManager.RemoveSetting(<Setting Name>)` function
- To change the the settings file path call the `SettingsManager.SetSettingsPath(<Path>)` function
