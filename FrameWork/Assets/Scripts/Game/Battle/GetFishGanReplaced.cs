using System;
using Spine.Unity.AttachmentTools;
using System.Collections;
using Game;
using Knight.Core;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GetFishGanReplaced : MonoBehaviour
{
	#region Inspector
		[SpineSkin]
		public string baseSkinName = "base";
		public Material sourceMaterial; // This will be used as the basis for shader and material property settings.

		[Header("Di")]
		public Sprite generalASprite;
		[SpineSlot] public string generalASlot;
		[SpineAttachment(slotField: "generalASlot", skinField: "baseSkinName")] public string generalAKey = "general A";

		[Header("Xiao")]
		public Sprite generalBSprite;
		[SpineSlot] public string generalBSlot;
		[SpineAttachment(slotField: "generalBSlot", skinField: "baseSkinName")] public string generalBKey = "general B";
		
		[Header("Zhong")]
		public Sprite YuXianSprite;
		[SpineSlot] public string YuxianSlot;
		[SpineAttachment(slotField: "YuxianSlot", skinField: "baseSkinName")] public string yuxianKey = "general C";
		
		[Header("Da")]
		public Sprite ZiSprite;
		[SpineSlot] public string Zi4Slot;
		[SpineAttachment(slotField: "Zi4Slot", skinField: "baseSkinName")] public string Zi4Key = "general D";

		[Header("Runtime Repack Required!!")]
		public bool repack = true;

		[Header("Do not assign")]
		public Texture2D runtimeAtlas;
		public Material runtimeMaterial;
		#endregion

		Skin customSkin;
		private SkeletonAnimation m_skeletonGraphic;
		
		private float passTime,showTime = 2f;
		private bool showIng;

		private Action finishCall;

		private void Awake()
		{
			Init();
		}

		void Init () {
			if (m_skeletonGraphic == null)
			{
				m_skeletonGraphic = GetComponent<SkeletonAnimation>();
			}
			if (sourceMaterial == null) {
				if (m_skeletonGraphic != null)
					sourceMaterial = m_skeletonGraphic.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial;
			}
			Apply();
		}

		void Apply () {
			SkeletonAnimation skeletonGraphic = GetComponent<SkeletonAnimation>();
			Skeleton skeleton = skeletonGraphic.Skeleton;

			
			customSkin = customSkin ?? new Skin("custom skin"); 

			
			Skin baseSkin = skeleton.Data.FindSkin(baseSkinName);

			

			
			int visorSlotIndex = skeleton.Data.FindSlot(generalASlot).Index; 
			Attachment baseAttachment = baseSkin.GetAttachment(visorSlotIndex, generalAKey); 
			Attachment newAttachment = baseAttachment.GetRemappedClone(generalASprite, sourceMaterial); 
			customSkin.SetAttachment(visorSlotIndex, generalAKey, newAttachment); 

			// And now for the gun.
			int gunSlotIndex = skeleton.Data.FindSlot(generalBSlot).Index;
			Attachment baseGeneral = baseSkin.GetAttachment(gunSlotIndex, generalBKey);
			Attachment newSkin = baseGeneral.GetRemappedClone(generalBSprite, sourceMaterial); 
			/*if (newSkin != null) */customSkin.SetAttachment(gunSlotIndex, generalBKey, newSkin); 
			
			int xianSlotIndex = skeleton.Data.FindSlot(YuxianSlot).Index;
			Attachment baseXian = baseSkin.GetAttachment(xianSlotIndex, yuxianKey);
			Attachment xianSkin = baseXian.GetRemappedClone(YuXianSprite, sourceMaterial); 
			/*if (xianSkin != null) */customSkin.SetAttachment(xianSlotIndex, yuxianKey, xianSkin); 
			
			int Zi4SlotIndex = skeleton.Data.FindSlot(Zi4Slot).Index;
			Attachment baseZi = baseSkin.GetAttachment(Zi4SlotIndex, Zi4Key);
			Attachment ZiSkin = baseZi.GetRemappedClone(ZiSprite, sourceMaterial); 
			/*if (xianSkin != null) */customSkin.SetAttachment(Zi4SlotIndex, Zi4Key, ZiSkin); 
			
			if (repack) {
				Skin repackedSkin = new Skin("repacked skin");
				repackedSkin.AddSkin(skeleton.Data.DefaultSkin);
				repackedSkin.AddSkin(customSkin);
				
				if (runtimeMaterial)
					Destroy(runtimeMaterial);
				if (runtimeAtlas)
					Destroy(runtimeAtlas);
				repackedSkin = repackedSkin.GetRepackedSkin("repacked skin", sourceMaterial, out runtimeMaterial, out runtimeAtlas);
				skeleton.SetSkin(repackedSkin);
			} else {
				skeleton.SetSkin(customSkin);
			}

			//skeleton.SetSlotsToSetupPose();
			skeleton.SetToSetupPose();
			skeletonGraphic.Update(0);
			//skeletonGraphic.OverrideTexture = runtimeAtlas;
			
			AtlasUtilities.ClearCache();
			Resources.UnloadUnusedAssets();
		}

		public void Show(/*string generalNameA,string generalNameB,string playerName,string playerIcon,*/Sprite generalA,Sprite generalB,Sprite xian,Sprite da,Action call)
		{
			finishCall = call;
			if (generalA!=null)
			{
				generalASprite = generalA;
			}
			/*else
			{
				LogManager.LogError($"传入的Sprite为Null");
			}
			*/

			if (generalB != null)
			{
				generalBSprite = generalB;
			}
			/*else
			{
				LogManager.LogError($"传入的generalBSprite为Null");
			}*/
			
			if (xian != null)
			{
				YuXianSprite = xian;
			}

			if (da!=null)
			{
				ZiSprite = da;
			}
			/*else
			{
				LogManager.LogError($"传入的xianSprite为Null");
			}*/
			Apply();
			//Active(true);
			//m_skeletonGraphic.AnimationState.SetAnimation(0, "idle", false);
		}
		private void Update()
		{
			/*if (showIng == false)
			{
				passTime = 0;
				return;
			}
			passTime += Time.deltaTime;
			if (passTime >= showTime)
			{
				passTime = 0;
           
				Active(false);
			}*/
			
		}
		public void Active(bool active)
		{
			gameObject.SetActive(active);
			showIng = active;

			if (active == false)
			{
				passTime = 0;
				finishCall?.Invoke();
				finishCall = null;
			}

		}
}
