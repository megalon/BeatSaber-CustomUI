# Example Usage
*It's important that you setup your settings options in the SceneManager_sceneLoaded event when the "Menu" scene is loaded! It can't be the SceneManager_activeSceneChanged event!*
```cs
bool toggleValue = false;
private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) 
{
  if (arg0.name == "Menu")
  {
    var toggle = GameplaySettingsUI.CreateToggleOption("Test Option", "This is a short description of the option, which will be displayed as a tooltip when you hover over it");
    toggle.GetValue = toggleValue;
    toggle.OnToggle += ((bool e) =>
    {
      toggleValue = e;
    });

    var settingsSubmenu = SettingsUI.CreateSubMenu("Test Submenu");
    var testInt = settingsSubmenu.AddInt("Test Int", 0, 100, 1);
    testInt.GetValue += delegate { return ModPrefs.GetInt(Plugin.Name, "Test Int", 0, true); };
    testInt.SetValue += delegate (int value) { ModPrefs.SetInt(Plugin.Name, "Test Int", value); };
  }
}
