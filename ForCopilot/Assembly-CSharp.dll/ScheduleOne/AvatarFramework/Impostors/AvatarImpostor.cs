using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Impostors
{
	// Token: 0x020009B3 RID: 2483
	public class AvatarImpostor : MonoBehaviour
	{
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06004358 RID: 17240 RVA: 0x0011B1C4 File Offset: 0x001193C4
		// (set) Token: 0x06004359 RID: 17241 RVA: 0x0011B1CC File Offset: 0x001193CC
		public bool HasTexture { get; private set; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600435A RID: 17242 RVA: 0x0011B1D5 File Offset: 0x001193D5
		private Transform Camera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					PlayerCamera instance = PlayerSingleton<PlayerCamera>.Instance;
					this.cachedCamera = ((instance != null) ? instance.transform : null);
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0011B204 File Offset: 0x00119404
		public void SetAvatarSettings(AvatarSettings settings)
		{
			Texture2D impostorTexture = settings.ImpostorTexture;
			if (impostorTexture != null)
			{
				this.meshRenderer.material.mainTexture = impostorTexture;
				this.HasTexture = true;
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0011B239 File Offset: 0x00119439
		private void LateUpdate()
		{
			this.Realign();
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0011B244 File Offset: 0x00119444
		private void Realign()
		{
			if (this.Camera != null)
			{
				Vector3 position = this.Camera.position;
				position.y = base.transform.position.y;
				Vector3 forward = base.transform.position - position;
				base.transform.rotation = Quaternion.LookRotation(forward);
			}
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0011B2A5 File Offset: 0x001194A5
		public void EnableImpostor()
		{
			base.gameObject.SetActive(true);
			this.Realign();
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void DisableImpostor()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003018 RID: 12312
		public MeshRenderer meshRenderer;

		// Token: 0x04003019 RID: 12313
		private Transform cachedCamera;
	}
}
