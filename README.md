# Example Usage
*It's important that you setup your settings options in the SceneManager_sceneLoaded event! It can't be the SceneManager_activeSceneChanged event!*
```cs
bool toggleValue = false;
private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) 
{
  if (arg0.name == "Menu")
  {
    var toggle = GameplaySettingsUI.CreateToggleOption("Test Option");
    toggle.GetValue = toggleValue;
    toggle.OnToggle += ((bool e) =>
    {
      toggleValue = e;
    });

    var settingsSubmenu = SettingsUI.CreateSubMenu("Test Submenu 1");
    settingsSubmenu.AddInt("Test Int", 0, 100, 1);
  }
}
