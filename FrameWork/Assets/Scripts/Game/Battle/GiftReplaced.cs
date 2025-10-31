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

public class GiftReplaced : MonoBehaviour
{
	#region Inspector
		[SpineSkin]
		public string baseSkinName = "base";
		public Material sourceMaterial; // This will be used as the basis for shader and material property settings.

		[Header("GiftIcon")]
		public Sprite GiftIcon;
		[SpineSlot] public string GiftIconSlot;
		[SpineAttachment(slotField: "GiftIconSlot", skinField: "baseSkinName")] public string giftIconKey = "giftIconKey";
		[Header("GiftIcon2")]
		public Sprite GiftIcon2;
		[SpineSlot] public string GiftIconSlot2;
		[SpineAttachment(slotField: "GiftIconSlot2", skinField: "baseSkinName")] public string giftIconKey2 = "giftIconKey";
		

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
			
			
			int visorSlotIndex = skeleton.Data.FindSlot(GiftIconSlot).Index; 
			Attachment baseAttachment = baseSkin.GetAttachment(visorSlotIndex, giftIconKey); 
			Attachment newAttachment = baseAttachment.GetRemappedClone(GiftIcon, sourceMaterial); 
			customSkin.SetAttachment(visorSlotIndex, giftIconKey, newAttachment); 
			
			int visorSlotIndex2 = skeleton.Data.FindSlot(GiftIconSlot2).Index; 
			Attachment baseAttachment2 = baseSkin.GetAttachment(visorSlotIndex2, giftIconKey2); 
			Attachment newAttachment2 = baseAttachment2.GetRemappedClone(GiftIcon2, sourceMaterial); 
			customSkin.SetAttachment(visorSlotIndex2, giftIconKey2, newAttachment2); 
			
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

		public void Show(Sprite generalA)
		{
			if (generalA!=null)
			{
				GiftIcon = generalA;
				GiftIcon2 = generalA;
			}
			else
			{
				LogManager.LogError($"替换礼物传入的Sprite为Null");
			}
			Apply();
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
