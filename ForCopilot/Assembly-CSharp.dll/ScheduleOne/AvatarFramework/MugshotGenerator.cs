using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009B0 RID: 2480
	public class MugshotGenerator : Singleton<MugshotGenerator>
	{
		// Token: 0x06004349 RID: 17225 RVA: 0x0011AF5B File Offset: 0x0011915B
		protected override void Awake()
		{
			base.Awake();
			this.MugshotRig.gameObject.SetActive(false);
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x0011AF74 File Offset: 0x00119174
		private void LateUpdate()
		{
			if (this.generate)
			{
				this.generate = false;
				this.FinalizeMugshot();
			}
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x0011AF8B File Offset: 0x0011918B
		private void FinalizeMugshot()
		{
			this.finalTexture = this.Generator.GetTexture(this.MugshotRig.transform);
			Debug.Log("Mugshot capture");
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x0011AFB3 File Offset: 0x001191B3
		[Button]
		public void GenerateMugshot()
		{
			this.GenerateMugshot(this.Settings, true, null);
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x0011AFC4 File Offset: 0x001191C4
		public void GenerateMugshot(AvatarSettings settings, bool fileToFile, Action<Texture2D> callback)
		{
			MugshotGenerator.<>c__DisplayClass12_0 CS$<>8__locals1 = new MugshotGenerator.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fileToFile = fileToFile;
			CS$<>8__locals1.settings = settings;
			CS$<>8__locals1.callback = callback;
			this.finalTexture = null;
			Debug.Log("Mugshot start");
			AvatarSettings avatarSettings = UnityEngine.Object.Instantiate<AvatarSettings>(CS$<>8__locals1.settings);
			avatarSettings.Height = 1f;
			this.MugshotRig.gameObject.SetActive(true);
			this.MugshotRig.LoadAvatarSettings(avatarSettings);
			LayerUtility.SetLayerRecursively(this.MugshotRig.gameObject, LayerMask.NameToLayer("IconGeneration"));
			SkinnedMeshRenderer[] componentsInChildren = this.MugshotRig.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].updateWhenOffscreen = true;
			}
			this.generate = true;
			base.StartCoroutine(CS$<>8__locals1.<GenerateMugshot>g__Routine|0());
		}

		// Token: 0x04003008 RID: 12296
		public string OutputPath;

		// Token: 0x04003009 RID: 12297
		public AvatarSettings Settings;

		// Token: 0x0400300A RID: 12298
		[Header("References")]
		public Avatar MugshotRig;

		// Token: 0x0400300B RID: 12299
		public IconGenerator Generator;

		// Token: 0x0400300C RID: 12300
		public AvatarSettings DefaultSettings;

		// Token: 0x0400300D RID: 12301
		public Transform LookAtPosition;

		// Token: 0x0400300E RID: 12302
		private Texture2D finalTexture;

		// Token: 0x0400300F RID: 12303
		private bool generate;
	}
}
