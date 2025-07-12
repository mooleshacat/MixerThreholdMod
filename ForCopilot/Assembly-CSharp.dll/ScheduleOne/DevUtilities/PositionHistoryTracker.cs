using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000734 RID: 1844
	public class PositionHistoryTracker : MonoBehaviour
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060031D5 RID: 12757 RVA: 0x000D0157 File Offset: 0x000CE357
		public float RecordedTime
		{
			get
			{
				return (float)this.positionHistory.Count * this.recordingFrequency;
			}
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000D016C File Offset: 0x000CE36C
		private void Start()
		{
			this.lastRecordTime = Time.time;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000D0179 File Offset: 0x000CE379
		private void Update()
		{
			if (Time.time - this.lastRecordTime >= this.recordingFrequency)
			{
				this.RecordPosition();
				this.lastRecordTime = Time.time;
			}
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000D01A0 File Offset: 0x000CE3A0
		private void RecordPosition()
		{
			this.positionHistory.Add(base.transform.position);
			if ((float)this.positionHistory.Count * this.recordingFrequency > this.historyDuration)
			{
				this.positionHistory.RemoveAt(0);
			}
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000D01E0 File Offset: 0x000CE3E0
		public Vector3 GetPositionXSecondsAgo(float secondsAgo)
		{
			int num = (int)(secondsAgo / this.recordingFrequency);
			num = Mathf.Clamp(num, 0, this.positionHistory.Count - 1);
			return this.positionHistory[num];
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x000D0218 File Offset: 0x000CE418
		public void ClearHistory()
		{
			this.positionHistory.Clear();
		}

		// Token: 0x04002317 RID: 8983
		[Tooltip("Frequency (in seconds) to record the position.")]
		public float recordingFrequency = 1f;

		// Token: 0x04002318 RID: 8984
		[Tooltip("Duration (in seconds) to store the position history.")]
		public float historyDuration = 10f;

		// Token: 0x04002319 RID: 8985
		public List<Vector3> positionHistory = new List<Vector3>();

		// Token: 0x0400231A RID: 8986
		private float lastRecordTime;
	}
}
