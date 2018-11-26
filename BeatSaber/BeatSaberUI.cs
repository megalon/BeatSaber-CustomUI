using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;
using Image = UnityEngine.UI.Image;

namespace CustomUI.BeatSaber
{
    public class BeatSaberUI : MonoBehaviour
    {
        private static Button _backButtonInstance;
        private static GameObject _loadingIndicatorInstance;

        public static bool initialized = false;

        public static Button CreateUIButton(RectTransform parent, string buttonTemplate, Vector2 anchoredPosition, Vector2 sizeDelta, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = Instantiate(Resources.FindObjectsOfTypeAll<Button>().Last(x => (x.name == buttonTemplate)), parent, false);
            DestroyImmediate(btn.GetComponent<SignalOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();
            if (onClick != null)
                btn.onClick.AddListener(onClick);
            btn.name = "CustomUIButton";

            (btn.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (btn.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
            (btn.transform as RectTransform).anchoredPosition = anchoredPosition;
            (btn.transform as RectTransform).sizeDelta = sizeDelta;

            btn.SetButtonText(buttonText);
            if (icon != null)
                btn.SetButtonIcon(icon);

            return btn;
        }

        public static Button CreateUIButton(RectTransform parent, string buttonTemplate, Vector2 anchoredPosition, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = Instantiate(Resources.FindObjectsOfTypeAll<Button>().Last(x => (x.name == buttonTemplate)), parent, false);
            DestroyImmediate(btn.GetComponent<SignalOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();
            if (onClick != null)
                btn.onClick.AddListener(onClick);
            btn.name = "CustomUIButton";

            (btn.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (btn.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
            (btn.transform as RectTransform).anchoredPosition = anchoredPosition;

            btn.SetButtonText(buttonText);
            if (icon != null)
                btn.SetButtonIcon(icon);

            return btn;
        }
        
        public static Button CreateUIButton(RectTransform parent, string buttonTemplate, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = Instantiate(Resources.FindObjectsOfTypeAll<Button>().Last(x => (x.name == buttonTemplate)), parent, false);
            DestroyImmediate(btn.GetComponent<SignalOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();
            if (onClick != null)
                btn.onClick.AddListener(onClick);
            btn.name = "CustomUIButton";

            (btn.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (btn.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
            btn.SetButtonText(buttonText);
            if (icon != null)
                btn.SetButtonIcon(icon);

            return btn;
        }

        public static Button CreateBackButton(RectTransform parent, UnityAction onClick = null)
        {
            if (_backButtonInstance == null)
            {
                try
                {
                    _backButtonInstance = Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "BackArrowButton"));
                }
                catch
                {
                    return null;
                }
            }
            
            Button btn = Instantiate(_backButtonInstance, parent, false);
            DestroyImmediate(btn.GetComponent<SignalOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();
            if (onClick != null)
                btn.onClick.AddListener(onClick);
            btn.name = "CustomUIButton";
            
            return btn;
        }

        public static T CreateViewController<T>() where T : VRUIViewController
        {
            T vc = new GameObject("CustomViewController").AddComponent<T>();

            vc.rectTransform.anchorMin = new Vector2(0f, 0f);
            vc.rectTransform.anchorMax = new Vector2(1f, 1f);
            vc.rectTransform.sizeDelta = new Vector2(0f, 0f);
            vc.rectTransform.anchoredPosition = new Vector2(0f, 0f);
            
            return vc;
        }

        public static GameObject CreateLoadingSpinner(Transform parent)
        {
            if (_loadingIndicatorInstance == null)
            {
                try
                {
                    _loadingIndicatorInstance = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "LoadingIndicator").First();
                }
                catch
                {
                    return null;
                }
            }

            GameObject loadingSpinner = Instantiate(_loadingIndicatorInstance, parent, false);
            loadingSpinner.name = "CustomUILoadingSpinner";

            return loadingSpinner;
        }

        public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition)
        {
            TextMeshProUGUI textMesh = new GameObject("CustomUIText").AddComponent<TextMeshProUGUI>();
            textMesh.rectTransform.SetParent(parent, false);
            textMesh.text = text;
            textMesh.fontSize = 4;
            textMesh.color = Color.white;
            textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            textMesh.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textMesh.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textMesh.rectTransform.sizeDelta = new Vector2(60f, 10f);
            textMesh.rectTransform.anchoredPosition = anchoredPosition;

            return textMesh;
        }

        public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            TextMeshProUGUI textMesh = new GameObject("CustomUIText").AddComponent<TextMeshProUGUI>();
            textMesh.rectTransform.SetParent(parent, false);
            textMesh.text = text;
            textMesh.fontSize = 4;
            textMesh.color = Color.white;
            textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            textMesh.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textMesh.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textMesh.rectTransform.sizeDelta = sizeDelta;
            textMesh.rectTransform.anchoredPosition = anchoredPosition;

            return textMesh;
        }
    }
}
