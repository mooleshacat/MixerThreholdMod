using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C7 RID: 1735
	public class Peephole : MonoBehaviour
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000C8FC1 File Offset: 0x000C71C1
		public void Open()
		{
			this.DoorAnim.Play("Peephole open");
			this.OpenSound.Play();
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000C8FDF File Offset: 0x000C71DF
		public void Close()
		{
			this.DoorAnim.Play("Peephole close");
			this.CloseSound.Play();
		}

		// Token: 0x0400219C RID: 8604
		public Animation DoorAnim;

		// Token: 0x0400219D RID: 8605
		public AudioSourceController OpenSound;

		// Token: 0x0400219E RID: 8606
		public AudioSourceController CloseSound;
	}
}
