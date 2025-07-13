using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Impostors
{
	// Token: 0x020009B4 RID: 2484
	public class ImpostorGenerator : MonoBehaviour
	{
		// Token: 0x0400301A RID: 12314
		[Header("References")]
		public Camera ImpostorCamera;

		// Token: 0x0400301B RID: 12315
		public Avatar Avatar;

		// Token: 0x0400301C RID: 12316
		[Header("Settings")]
		public List<AvatarSettings> GenerationQueue = new List<AvatarSettings>();

		// Token: 0x0400301D RID: 12317
		[SerializeField]
		private Texture2D output;
	}
}
