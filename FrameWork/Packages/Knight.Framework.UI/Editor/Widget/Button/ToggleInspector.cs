using NaughtyAttributes.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

namespace UnityEditor.UI
{
	[CustomEditor(typeof(ToggleAssist),true)]
	public class ToggleInspector : InspectorEditor
	{
		private string mUISoundPath = "Assets/Game/GameAsset/Sound/UI";
		private ToggleAssist mTarget;
		protected override void OnEnable()
		{
			base.OnEnable();
			this.mTarget = this.target as ToggleAssist;
		}

		public override void OnInspectorGUI()
		{
			var rSoundAssets = new List<string>();
			var rGUIDS = AssetDatabase.FindAssets("t:AudioClip",new string[] { this.mUISoundPath});
			for (int i = 0; i < rGUIDS.Length; i++)
			{
				var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDS[i]);
				rSoundAssets.Add(Path.GetFileNameWithoutExtension(rAssetPath));
			}
			this.mTarget.SetUIAudioClips(rSoundAssets);
			base.OnInspectorGUI();
		}
	} 
}
