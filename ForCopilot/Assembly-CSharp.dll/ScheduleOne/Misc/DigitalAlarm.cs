using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C5E RID: 3166
	public class DigitalAlarm : MonoBehaviour
	{
		// Token: 0x06005923 RID: 22819 RVA: 0x0017896C File Offset: 0x00176B6C
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x001789C5 File Offset: 0x00176BC5
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.Instance != null)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x001789FC File Offset: 0x00176BFC
		public void SetScreenLit(bool lit)
		{
			Material[] materials = this.ScreenMesh.materials;
			materials[this.ScreenMeshMaterialIndex] = (lit ? this.ScreenOnMat : this.ScreenOffMat);
			this.ScreenMesh.materials = materials;
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x00178A3A File Offset: 0x00176C3A
		public void DisplayText(string text)
		{
			this.ScreenText.text = text;
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x00178A48 File Offset: 0x00176C48
		public void DisplayMinutes(int mins)
		{
			int num = mins / 60;
			mins %= 60;
			this.DisplayText(string.Format("{0:D2}:{1:D2}", num, mins));
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x00178A7C File Offset: 0x00176C7C
		private void MinPass()
		{
			if (this.DisplayCurrentTime)
			{
				this.DisplayText(TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, false));
			}
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x00178AA0 File Offset: 0x00176CA0
		private void FixedUpdate()
		{
			if (this.FlashScreen)
			{
				float num = Mathf.Sin(Time.timeSinceLevelLoad * 4f);
				this.SetScreenLit(num > 0f);
			}
		}

		// Token: 0x04004149 RID: 16713
		public const float FLASH_FREQUENCY = 4f;

		// Token: 0x0400414A RID: 16714
		public MeshRenderer ScreenMesh;

		// Token: 0x0400414B RID: 16715
		public int ScreenMeshMaterialIndex;

		// Token: 0x0400414C RID: 16716
		public TextMeshPro ScreenText;

		// Token: 0x0400414D RID: 16717
		public bool FlashScreen;

		// Token: 0x0400414E RID: 16718
		[Header("Settings")]
		public bool DisplayCurrentTime;

		// Token: 0x0400414F RID: 16719
		public Material ScreenOffMat;

		// Token: 0x04004150 RID: 16720
		public Material ScreenOnMat;
	}
}
