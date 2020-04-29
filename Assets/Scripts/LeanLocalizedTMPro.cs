using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lean.Localization
{
    /// <summary>This component will update a TMPro UGUI component with localized text, or use a fallback if none is found.</summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLocalizedTMPro")]
    [AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized Text TMPro")]
    public class LeanLocalizedTMPro : LeanLocalizedBehaviour
    {
		[Tooltip("If PhraseName couldn't be found, this text will be used")]
		public string FallbackText = "- Translation Not Found -";

		// This gets called every time the translation needs updating
		public override void UpdateTranslation(LeanTranslation translation)
		{
			// Get the Text component attached to this GameObject
			var text = GetComponent<TextMeshProUGUI>();

			// Use translation?
			if (translation != null && translation.Data is string)
			{
				text.text = LeanTranslation.FormatText((string)translation.Data, text.text, this);
			}
			// Use fallback?
			else
			{
				text.text = LeanTranslation.FormatText(FallbackText, text.text, this);
			}
		}

		protected virtual void Awake()
		{
			// Should we set FallbackText?
			if (string.IsNullOrEmpty(FallbackText) == true)
			{
				// Get the Text component attached to this GameObject
				var text = GetComponent<TextMeshProUGUI>();

				// Copy current text to fallback
				FallbackText = text.text;
			}
		}
	}
}
