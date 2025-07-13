using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200088B RID: 2187
	public class CountdownText : MonoBehaviour
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x000FCB28 File Offset: 0x000FAD28
		private void Start()
		{
			TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
			DateTime dateTime = new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, DateTimeKind.Unspecified);
			this.targetPDTDate = TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000FCB79 File Offset: 0x000FAD79
		private void Update()
		{
			this.UpdateCountdown();
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x000FCB84 File Offset: 0x000FAD84
		private void UpdateCountdown()
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = this.targetPDTDate - utcNow;
			if (timeSpan.TotalSeconds > 0.0)
			{
				this.TimeLabel.text = this.FormatTime(timeSpan);
				return;
			}
			this.TimeLabel.text = "Now available!";
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000FCBDC File Offset: 0x000FADDC
		private string FormatTime(TimeSpan timeSpan)
		{
			return string.Concat(new string[]
			{
				timeSpan.Days.ToString(),
				" days, ",
				timeSpan.Hours.ToString(),
				" hours, ",
				timeSpan.Minutes.ToString(),
				" minutes"
			});
		}

		// Token: 0x04002AA8 RID: 10920
		public TextMeshProUGUI TimeLabel;

		// Token: 0x04002AA9 RID: 10921
		[Header("Date Setting")]
		public int Year = 2025;

		// Token: 0x04002AAA RID: 10922
		public int Month = 3;

		// Token: 0x04002AAB RID: 10923
		public int Day = 24;

		// Token: 0x04002AAC RID: 10924
		public int Hour = 16;

		// Token: 0x04002AAD RID: 10925
		public int Minute;

		// Token: 0x04002AAE RID: 10926
		public int Second;

		// Token: 0x04002AAF RID: 10927
		private DateTime targetPDTDate;
	}
}
