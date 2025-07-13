using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000642 RID: 1602
	public class ViewmodelAvatar : Singleton<ViewmodelAvatar>
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x000AAB56 File Offset: 0x000A8D56
		// (set) Token: 0x06002964 RID: 10596 RVA: 0x000AAB5E File Offset: 0x000A8D5E
		public bool IsVisible { get; private set; }

		// Token: 0x06002965 RID: 10597 RVA: 0x000AAB68 File Offset: 0x000A8D68
		protected override void Awake()
		{
			base.Awake();
			this.baseOffset = base.transform.localPosition;
			this.SetVisibility(false);
			if (this.ParentAvatar.CurrentSettings != null)
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			}
			this.ParentAvatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			});
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000AABD3 File Offset: 0x000A8DD3
		public void SetVisibility(bool isVisible)
		{
			this.SetOffset(Vector3.zero);
			this.IsVisible = isVisible;
			base.gameObject.SetActive(isVisible);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000AABF4 File Offset: 0x000A8DF4
		public void SetAppearance(AvatarSettings settings)
		{
			AvatarSettings avatarSettings = UnityEngine.Object.Instantiate<AvatarSettings>(settings);
			avatarSettings.Height = 0.25f;
			this.Avatar.LoadAvatarSettings(avatarSettings);
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Viewmodel"));
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

		// Token: 0x06002968 RID: 10600 RVA: 0x000AAC9F File Offset: 0x000A8E9F
		public void SetAnimatorController(RuntimeAnimatorController controller)
		{
			this.Animator.runtimeAnimatorController = controller;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000AACAD File Offset: 0x000A8EAD
		public void SetOffset(Vector3 offset)
		{
			base.transform.localPosition = this.baseOffset + offset;
		}

		// Token: 0x04001DEB RID: 7659
		public Avatar ParentAvatar;

		// Token: 0x04001DEC RID: 7660
		public Animator Animator;

		// Token: 0x04001DED RID: 7661
		public Avatar Avatar;

		// Token: 0x04001DEE RID: 7662
		public Transform RightHandContainer;

		// Token: 0x04001DEF RID: 7663
		private Vector3 baseOffset;
	}
}
