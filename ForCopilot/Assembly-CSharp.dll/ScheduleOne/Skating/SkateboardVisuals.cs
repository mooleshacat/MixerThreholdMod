using System;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002E0 RID: 736
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardVisuals : MonoBehaviour
	{
		// Token: 0x06000FF5 RID: 4085 RVA: 0x00046AE4 File Offset: 0x00044CE4
		private void Awake()
		{
			this.skateboard = base.GetComponent<Skateboard>();
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00046AF4 File Offset: 0x00044CF4
		private void LateUpdate()
		{
			Vector3 euler = new Vector3(0f, 0f, this.skateboard.CurrentSteerInput * -this.MaxBoardLean);
			this.Board.localRotation = Quaternion.Lerp(this.Board.localRotation, Quaternion.Euler(euler), Time.deltaTime * this.BoardLeanRate);
		}

		// Token: 0x0400106E RID: 4206
		[Header("Settings")]
		public float MaxBoardLean = 8f;

		// Token: 0x0400106F RID: 4207
		public float BoardLeanRate = 2f;

		// Token: 0x04001070 RID: 4208
		[Header("References")]
		public Transform Board;

		// Token: 0x04001071 RID: 4209
		private Skateboard skateboard;
	}
}
