using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRUI;

namespace CustomUI.BeatSaber
{
    public static class BeatSaberUIExtensions
    {
        #region Button Extensions
        public static void SetButtonText(this Button _button, string _text)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
                _button.GetComponentInChildren<TextMeshProUGUI>().text = _text;
        }

        public static void SetButtonTextSize(this Button _button, float _fontSize)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _button.GetComponentInChildren<TextMeshProUGUI>().fontSize = _fontSize;
            }
        }

        public static void ToggleWordWrapping(this Button _button, bool enableWordWrapping)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _button.GetComponentInChildren<TextMeshProUGUI>().enableWordWrapping = enableWordWrapping;
            }
        } 

        public static void SetButtonIcon(this Button _button, Sprite _icon)
        {
            if (_button.GetComponentsInChildren<Image>().Count() > 1)
                _button.GetComponentsInChildren<Image>().First(x => x.name == "Icon").sprite = _icon;
        }

        public static void SetButtonBackground(this Button _button, Sprite _background)
        {
            if (_button.GetComponentsInChildren<Image>().Count() > 0)
                _button.GetComponentsInChildren<Image>()[0].sprite = _background;
        }
        #endregion

        #region ViewController Extensions

        public static Button CreateUIButton(this VRUIViewController parent, string buttonTemplate)
        {
            Button btn = BeatSaberUI.CreateUIButton(parent.rectTransform, buttonTemplate);
            return btn;
        }

        public static Button CreateUIButton(this VRUIViewController parent, string buttonTemplate, Vector2 anchoredPosition, Vector2 sizeDelta, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = BeatSaberUI.CreateUIButton(parent.rectTransform, buttonTemplate, anchoredPosition, sizeDelta, onClick, buttonText, icon);
            return btn;
        }

        public static Button CreateUIButton(this VRUIViewController parent, string buttonTemplate, Vector2 anchoredPosition, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = BeatSaberUI.CreateUIButton(parent.rectTransform, buttonTemplate, anchoredPosition, onClick, buttonText, icon);
            return btn;
        }

        public static Button CreateUIButton(this VRUIViewController parent, string buttonTemplate, UnityAction onClick = null, string buttonText = "BUTTON", Sprite icon = null)
        {
            Button btn = BeatSaberUI.CreateUIButton(parent.rectTransform, buttonTemplate, onClick, buttonText, icon);
            return btn;
        }

        public static Button CreateBackButton(this VRUIViewController parent)
        {
            Button btn = BeatSaberUI.CreateBackButton(parent.rectTransform);
            return btn;
        }

        public static GameObject CreateLoadingSpinner(this VRUIViewController parent)
        {
            GameObject loadingSpinner = BeatSaberUI.CreateLoadingSpinner(parent.rectTransform);
            return loadingSpinner;
        }

        public static TextMeshProUGUI CreateText(this VRUIViewController parent, string text, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            TextMeshProUGUI textMesh = BeatSaberUI.CreateText(parent.rectTransform, text, anchoredPosition, sizeDelta);
            return textMesh;
        }

        public static TextMeshProUGUI CreateText(this VRUIViewController parent, string text, Vector2 anchoredPosition)
        {
            TextMeshProUGUI textMesh = BeatSaberUI.CreateText(parent.rectTransform, text, anchoredPosition);
            return textMesh;
        }
        #endregion

    }
}
