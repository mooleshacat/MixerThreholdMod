using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A8 RID: 2216
	public class PlayAnimation : MonoBehaviour
	{
		// Token: 0x06003C24 RID: 15396 RVA: 0x000FD780 File Offset: 0x000FB980
		[Button]
		public void Play()
		{
			base.GetComponent<Animation>().Play();
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000FD78E File Offset: 0x000FB98E
		public void Play(string animationName)
		{
			base.GetComponent<Animation>().Play(animationName);
		}
	}
}
