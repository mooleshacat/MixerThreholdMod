using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000806 RID: 2054
	public class LoanSharkCarVisuals : MonoBehaviour
	{
		// Token: 0x06003803 RID: 14339 RVA: 0x000EC19D File Offset: 0x000EA39D
		private void Awake()
		{
			this.Note.gameObject.SetActive(false);
			this.BulletHoleDecals.gameObject.SetActive(false);
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000EC1C1 File Offset: 0x000EA3C1
		public void Configure(bool enabled, bool noteVisible)
		{
			this.Note.SetActive(noteVisible);
			this.BulletHoleDecals.SetActive(enabled);
		}

		// Token: 0x040027C9 RID: 10185
		public GameObject Note;

		// Token: 0x040027CA RID: 10186
		public GameObject BulletHoleDecals;
	}
}
