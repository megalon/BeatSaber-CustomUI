using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;


namespace CustomUI.Settings
{
    public class SettingsUI : MonoBehaviour
    {
        private MainMenuViewController _mainMenuViewController = null;
        private SettingsNavigationController settingsMenu = null;
        private MainSettingsMenuViewController mainSettingsMenu = null;
        private MainSettingsTableView _mainSettingsTableView = null;
        private TableView subMenuTableView = null;
        private TableViewHelper subMenuTableViewHelper = null;
        private MainSettingsTableCell tableCell = null;
        private Transform othersSubmenu = null;
        private SimpleDialogPromptViewController prompt = null;

        private Button _pageUpButton = null;
        private Button _pageDownButton = null;
        private Vector2 buttonOffset = new Vector2(24, 0);
        private bool initialized = false;

        private static SettingsUI _instance = null;
        public static SettingsUI Instance
        {
            get
            {
                if (!_instance)
                    _instance = new GameObject("SettingsUI").AddComponent<SettingsUI>();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        
        public void SceneManagerOnActiveSceneChanged(Scene from, Scene to)
        {
            if (to.name == "EmptyTransition")
            {
                if (Instance)
                    Destroy(Instance.gameObject);
                initialized = false;
                Instance = null;
            }
        }

        private void SetupUI()
        {
            if (initialized) return;

            try
            {
                var _menuMasterViewController = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
                prompt = ReflectionUtil.GetPrivateField<SimpleDialogPromptViewController>(_menuMasterViewController, "_simpleDialogPromptViewController");

                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                settingsMenu = Resources.FindObjectsOfTypeAll<SettingsNavigationController>().FirstOrDefault();
                mainSettingsMenu = Resources.FindObjectsOfTypeAll<MainSettingsMenuViewController>().FirstOrDefault();
                _mainSettingsTableView = mainSettingsMenu.GetPrivateField<MainSettingsTableView>("_mainSettingsTableView");
                subMenuTableView = _mainSettingsTableView.GetComponentInChildren<TableView>();
                subMenuTableViewHelper = subMenuTableView.gameObject.AddComponent<TableViewHelper>();
                othersSubmenu = settingsMenu.transform.Find("OtherSettings");

                AddPageButtons();

                if (tableCell == null)
                {
                    tableCell = Resources.FindObjectsOfTypeAll<MainSettingsTableCell>().FirstOrDefault();
                    // Get a refence to the Settings Table cell text in case we want to change font size, etc
                    var text = tableCell.GetPrivateField<TextMeshProUGUI>("_settingsSubMenuText");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SettingsUI] Crash when trying to setup UI! Exception: {ex.ToString()}");
            }
        }

        private void AddPageButtons()
        {
            try
            {
                RectTransform viewport = _mainSettingsTableView.GetComponentsInChildren<RectTransform>().First(x => x.name == "Viewport");
                viewport.anchorMin = new Vector2(0f, 0.5f);
                viewport.anchorMax = new Vector2(1f, 0.5f);
                viewport.sizeDelta = new Vector2(0f, 48f);
                viewport.anchoredPosition = new Vector2(0f, 0f);

                RectTransform container = (RectTransform)_mainSettingsTableView.transform;

                if (_pageUpButton == null)
                {
                    _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), container);

                    _pageUpButton.transform.parent = container.parent;
                    _pageUpButton.transform.localScale = Vector3.one;
                    _pageUpButton.transform.localPosition -= new Vector3(0, 4.5f);
                    _pageUpButton.interactable = false;
                    _pageUpButton.onClick.AddListener(delegate ()
                    {
                        subMenuTableViewHelper.PageScrollUp();
                    });
                }

                if (_pageDownButton == null)
                {
                    _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), container);

                    _pageDownButton.transform.parent = container.parent;
                    _pageDownButton.transform.localScale = Vector3.one;
                    _pageDownButton.transform.localPosition -= new Vector3(0, 6.5f);
                    _pageDownButton.interactable = false;
                    _pageDownButton.onClick.AddListener(delegate ()
                    {
                        subMenuTableViewHelper.PageScrollDown();
                    });
                }

                subMenuTableViewHelper._pageUpButton = _pageUpButton;
                subMenuTableViewHelper._pageDownButton = _pageDownButton;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SettingsUI] Crash when trying to add page buttons! Exception: {ex.ToString()}");
            }
        }

        public static SubMenu CreateSubMenu(string name)
        {
            lock(Instance) {
                Instance.SetupUI();

                var subMenuGameObject = Instantiate(Instance.othersSubmenu.gameObject, Instance.othersSubmenu.transform.parent);
                Transform mainContainer = CleanScreen(subMenuGameObject.transform);

                var newSubMenuInfo = new SettingsSubMenuInfo();
                newSubMenuInfo.SetPrivateField("_menuName", name);
                newSubMenuInfo.SetPrivateField("_viewController", subMenuGameObject.GetComponent<VRUIViewController>());

                var subMenuInfos = Instance.mainSettingsMenu.GetPrivateField<SettingsSubMenuInfo[]>("_settingsSubMenuInfos").ToList();
                subMenuInfos.Add(newSubMenuInfo);
                Instance.mainSettingsMenu.SetPrivateField("_settingsSubMenuInfos", subMenuInfos.ToArray());

                 SubMenu menu = new SubMenu(mainContainer);
                return menu;
            }
        }


        static Transform CleanScreen(Transform screen)
        {
            var container = screen.Find("Content").Find("SettingsContainer");
            var tempList = container.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
            return container;
        }
    }
    
    public class SubMenu
    {
        public Transform transform;

        public SubMenu(Transform transform)
        {
            this.transform = transform;
        }

        public BoolViewController AddBool(string name)
        {
            return AddToggleSetting<BoolViewController>(name);
        }

        public IntViewController AddInt(string name, int min, int max, int increment)
        {
            var view = AddIntSetting<IntViewController>(name);
            view.SetValues(min, max, increment);
            return view;
        }

        public ListViewController AddList(string name, float[] values)
        {
            var view = AddListSetting<ListViewController>(name);
            view.values = values.ToList();
            return view;
        }

        public T AddListSetting<T>(string name) where T : ListSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<VolumeSettingsController>().FirstOrDefault();
            GameObject newSettingsObject = MonoBehaviour.Instantiate(volumeSettings.gameObject, transform);
            newSettingsObject.name = name;

            VolumeSettingsController volume = newSettingsObject.GetComponent<VolumeSettingsController>();
            T newListSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(ListSettingsController), typeof(T), newSettingsObject);
            MonoBehaviour.DestroyImmediate(volume);

            newSettingsObject.GetComponentInChildren<TMP_Text>().text = name;

            return newListSettingsController;
        }

        public T AddToggleSetting<T>(string name) where T : SwitchSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<WindowModeSettingsController>().FirstOrDefault();
            GameObject newSettingsObject = MonoBehaviour.Instantiate(volumeSettings.gameObject, transform);
            newSettingsObject.name = name;

            WindowModeSettingsController volume = newSettingsObject.GetComponent<WindowModeSettingsController>();
            T newToggleSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(SwitchSettingsController), typeof(T), newSettingsObject);
            MonoBehaviour.DestroyImmediate(volume);

            newSettingsObject.GetComponentInChildren<TMP_Text>().text = name;

            return newToggleSettingsController;
        }

        public T AddIntSetting<T>(string name) where T : IntSettingsController
        {
            var volumeSettings = Resources.FindObjectsOfTypeAll<WindowModeSettingsController>().FirstOrDefault();
            GameObject newSettingsObject = MonoBehaviour.Instantiate(volumeSettings.gameObject, transform);
            newSettingsObject.name = name;

            WindowModeSettingsController volume = newSettingsObject.GetComponent<WindowModeSettingsController>();
            T newToggleSettingsController = (T)ReflectionUtil.CopyComponent(volume, typeof(IncDecSettingsController), typeof(T), newSettingsObject);
            MonoBehaviour.DestroyImmediate(volume);

            newSettingsObject.GetComponentInChildren<TMP_Text>().text = name;

            return newToggleSettingsController;
        }
    }
}
