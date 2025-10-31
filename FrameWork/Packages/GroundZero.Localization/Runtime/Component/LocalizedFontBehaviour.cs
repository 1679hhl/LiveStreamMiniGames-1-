using System;
using UnityEngine;
using UnityEngine.UI;
using Knight.Core;

namespace GroundZero.Localization
{
    [DisallowMultipleComponent]
    [AddComponentMenu(ComponentMenuRoot + "Localized Font")]
    public class LocalizedFontBehaviour : LocalizedGenericAssetBehaviour<string>
    {
        private void Reset()
        {
            this.TrySetComponentAndPropertyIfNotSet<Text>("font");
            this.TrySetComponentAndPropertyIfNotSet<TextMesh>("font");
        }

        protected override object GetLocalizedValue()
        {
            if (LocalizationManager.Instance == null)
                return default(Font);
            return LocalizationManager.Instance.GetMultiLanFont(this.m_LocalizedAsset);
        }

        protected override void OnComponentLocalizationChanged()
        {
        }

        protected override Type GetValueType()
        {
            return typeof(Font);
        }
    }
}
