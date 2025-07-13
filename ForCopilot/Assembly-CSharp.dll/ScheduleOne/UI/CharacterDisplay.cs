using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.UI
{
	// Token: 0x02000A07 RID: 2567
	public class CharacterDisplay : Singleton<CharacterDisplay>
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x00121A67 File Offset: 0x0011FC67
		// (set) Token: 0x06004507 RID: 17671 RVA: 0x00121A6F File Offset: 0x0011FC6F
		public bool IsOpen { get; private set; }

		// Token: 0x06004508 RID: 17672 RVA: 0x00121A78 File Offset: 0x0011FC78
		protected override void Awake()
		{
			base.Awake();
			this.SetOpen(false);
			if (this.ParentAvatar.CurrentSettings != null)
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			}
			this.ParentAvatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			});
			AudioSource[] componentsInChildren = this.Avatar.GetComponentsInChildren<AudioSource>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00121AF8 File Offset: 0x0011FCF8
		public void SetOpen(bool open)
		{
			this.IsOpen = open;
			this.Container.gameObject.SetActive(open);
			if (this.IsOpen)
			{
				LayerUtility.SetLayerRecursively(this.Container.gameObject, LayerMask.NameToLayer("Overlay"));
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), Player.Local.Clothing.ItemSlots);
			}
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00121B74 File Offset: 0x0011FD74
		private void Update()
		{
			if (this.IsOpen)
			{
				this.targetRotation = Mathf.Lerp(this.targetRotation, Mathf.Lerp(0f, 359f, Singleton<GameplayMenuInterface>.Instance.CharacterInterface.RotationSlider.value), Time.deltaTime * 5f);
				this.AvatarContainer.localEulerAngles = new Vector3(0f, this.targetRotation, 0f);
			}
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x00121BE8 File Offset: 0x0011FDE8
		public void SetAppearance(AvatarSettings settings)
		{
			AvatarSettings settings2 = UnityEngine.Object.Instantiate<AvatarSettings>(settings);
			this.Avatar.LoadAvatarSettings(settings2);
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Overlay"));
			foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					meshRenderer.enabled = false;
				}
				else
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				if (skinnedMeshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					skinnedMeshRenderer.enabled = false;
				}
				else
				{
					skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
		}

		// Token: 0x040031DF RID: 12767
		public CharacterDisplay.SlotAlignmentPoint[] AlignmentPoints;

		// Token: 0x040031E0 RID: 12768
		[Header("References")]
		public Transform Container;

		// Token: 0x040031E1 RID: 12769
		public Avatar ParentAvatar;

		// Token: 0x040031E2 RID: 12770
		public Avatar Avatar;

		// Token: 0x040031E3 RID: 12771
		public Transform AvatarContainer;

		// Token: 0x040031E4 RID: 12772
		private float targetRotation;

		// Token: 0x02000A08 RID: 2568
		[Serializable]
		public class SlotAlignmentPoint
		{
			// Token: 0x040031E5 RID: 12773
			public EClothingSlot SlotType;

			// Token: 0x040031E6 RID: 12774
			public Transform Point;
		}
	}
}
