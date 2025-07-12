using System;
using UnityEngine;

namespace ScheduleOne.Building.Doors
{
	// Token: 0x020007D5 RID: 2005
	public class DoorKnocker : MonoBehaviour
	{
		// Token: 0x06003643 RID: 13891 RVA: 0x000E4A61 File Offset: 0x000E2C61
		public void Knock()
		{
			if (this.Anim.isPlaying)
			{
				this.Anim.Stop();
			}
			this.Anim.Play(this.KnockingSoundClipName);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000E4A8D File Offset: 0x000E2C8D
		public void PlayKnockingSound()
		{
			this.KnockingSound.Play();
		}

		// Token: 0x04002677 RID: 9847
		[Header("References")]
		public Animation Anim;

		// Token: 0x04002678 RID: 9848
		public string KnockingSoundClipName;

		// Token: 0x04002679 RID: 9849
		public AudioSource KnockingSound;
	}
}
