using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BeatSaberCustomUI
{
    public abstract class GameOption
    {
        public GameObject gameObject;
        public string optionName;
        public bool initialized;
        public abstract void Instantiate();
    }


    public class ToggleOption : GameOption
    {
        public event Action<bool> OnToggle;
        public bool GetValue = false;

        public ToggleOption(string optionName)
        {
            this.optionName = optionName;
        }

        public override void Instantiate()
        {
            if (initialized) return;

            //We have to find our own target
            //TODO: Clean up time complexity issue. This is called for each new option
            SoloFreePlayFlowCoordinator _sldvc = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().First();
            GameplaySetupViewController _govc = _sldvc.GetField<GameplaySetupViewController>("_gameplaySetupViewController");
            RectTransform container = (RectTransform)_govc.transform.Find("GameplayModifiers").Find("RightColumn");

            gameObject = UnityEngine.Object.Instantiate(container.Find("NoFail").gameObject, container);
            gameObject.name = optionName;
            gameObject.layer = container.gameObject.layer;
            gameObject.transform.parent = container;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(false); //All options start disabled

            var tog = gameObject.GetComponentInChildren<GameplayModifierToggle>();
            if (tog != null)
            {
                tog.toggle.isOn = GetValue;
                tog.toggle.onValueChanged.RemoveAllListeners();
                tog.toggle.onValueChanged.AddListener((bool e) => { OnToggle?.Invoke(e); });
            }

            SharedCoroutineStarter.instance.StartCoroutine(OnIsSet(tog, optionName));

            initialized = true;
        }

        private IEnumerator OnIsSet(GameplayModifierToggle t, string optionName)
        {
            while (t.GetPrivateField<TextMeshProUGUI>("_nameText").text == "!NOT SET!") yield return null;
            t.GetPrivateField<TextMeshProUGUI>("_nameText").text = optionName;
        }

    }

    public class MultiSelectOption : GameOption
    {
        private Dictionary<float, string> _options = new Dictionary<float, string>();
        public Func<float> GetValue;
        public event Action<float> OnChange;

        public MultiSelectOption(string optionName)
        {
            this.optionName = optionName;
        }

        public override void Instantiate()
        {
            if (initialized) return;

            //We have to find our own target
            //TODO: Clean up time complexity issue. This is called for each new option
            SoloFreePlayFlowCoordinator _sldvc = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().First();
            GameplaySetupViewController _govc = _sldvc.GetField<GameplaySetupViewController>("_gameplaySetupViewController");
            RectTransform container = (RectTransform)_govc.transform.Find("GameplayModifiers").Find("RightColumn");

            var volumeSettings = Resources.FindObjectsOfTypeAll<VolumeSettingsController>().FirstOrDefault();
            gameObject = UnityEngine.Object.Instantiate(volumeSettings.gameObject, container);
            gameObject.name = optionName;
            gameObject.GetComponentInChildren<TMP_Text>().text = optionName;

            //Slim down the toggle option so it fits in the space we have before the divider
            (gameObject.transform as RectTransform).sizeDelta = new Vector2(50, (gameObject.transform as RectTransform).sizeDelta.y);

            //This magical nonsense is courtesy of Taz and his SettingsUI class
            VolumeSettingsController volume = gameObject.GetComponent<VolumeSettingsController>();
            ListViewController newListSettingsController = (ListViewController)ReflectionUtil.CopyComponent(volume, typeof(ListSettingsController), typeof(ListViewController), gameObject);
            UnityEngine.Object.DestroyImmediate(volume);

            newListSettingsController.values = _options.Keys.ToList();
            newListSettingsController.SetValue = OnChange;
            newListSettingsController.GetValue = () =>
            {
                if (GetValue != null) return GetValue.Invoke();
                return _options.Keys.ElementAt(0);
            };
            newListSettingsController.GetTextForValue = (v) =>
            {
                if (_options.ContainsKey(v)) return _options[v];
                return "UNKNOWN";
            };

            //Initialize the controller, as if we had just opened the settings menu
            newListSettingsController.Init();
            gameObject.SetActive(false);
            initialized = true;
        }

        public void AddOption(float value)
        {
            _options.Add(value, Convert.ToString(value));
        }

        public void AddOption(float value, string option)
        {
            _options.Add(value, option);
        }
    }

}
