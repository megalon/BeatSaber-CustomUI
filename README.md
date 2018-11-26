# Example Usage

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
}```
