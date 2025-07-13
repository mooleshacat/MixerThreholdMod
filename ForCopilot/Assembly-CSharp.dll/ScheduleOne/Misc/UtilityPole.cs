using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Power;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C63 RID: 3171
	public class UtilityPole : MonoBehaviour
	{
		// Token: 0x0600594A RID: 22858 RVA: 0x00179210 File Offset: 0x00177410
		private void Awake()
		{
			if (this.Cable1Container.gameObject.activeSelf)
			{
				this.cableStart = this.cable1Connection.position;
				this.cableEnd = this.cable1Segments[this.cable1Segments.Count - 1].position;
				this.cableMid = (this.cableStart + this.cableEnd) / 2f;
				return;
			}
			this.cableStart = this.cable2Connection.position;
			this.cableEnd = this.cable2Segments[this.cable2Segments.Count - 1].position;
			this.cableMid = (this.cableStart + this.cableEnd) / 2f;
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x001792DA File Offset: 0x001774DA
		private void Start()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.<Start>g__Register|17_0();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|17_0));
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x0017930C File Offset: 0x0017750C
		private void UpdateCulling()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float sqrMagnitude = (this.cableStart - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float sqrMagnitude2 = (this.cableEnd - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float sqrMagnitude3 = (this.cableMid - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float num = Mathf.Min(new float[]
			{
				sqrMagnitude,
				sqrMagnitude2,
				sqrMagnitude3
			}) * QualitySettings.lodBias;
			this.Cable1Container.gameObject.SetActive(num < 10000f && this.Connection1Enabled);
			this.Cable2Container.gameObject.SetActive(num < 10000f && this.Connection2Enabled);
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x001793F0 File Offset: 0x001775F0
		[Button]
		public void Orient()
		{
			if (this.previousPole == null && this.nextPole == null)
			{
				Console.LogWarning("No neighbour poles!", null);
				return;
			}
			if (this.nextPole != null && this.previousPole != null)
			{
				Vector3 normalized = (base.transform.position - this.previousPole.transform.position).normalized;
				Vector3 normalized2 = (this.nextPole.transform.position - base.transform.position).normalized;
				Vector3 normalized3 = (normalized + normalized2).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized3, Vector3.up);
				return;
			}
			if (this.previousPole != null)
			{
				Vector3 normalized4 = (base.transform.position - this.previousPole.transform.position).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized4, Vector3.up);
				return;
			}
			if (this.nextPole != null)
			{
				Vector3 normalized5 = (this.nextPole.transform.position - base.transform.position).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized5, Vector3.up);
			}
		}

		// Token: 0x0600594E RID: 22862 RVA: 0x00179558 File Offset: 0x00177758
		[Button]
		public void DrawLines()
		{
			if (this.previousPole == null)
			{
				if (this.Connection1Enabled)
				{
					foreach (Transform transform in this.cable1Segments)
					{
						transform.gameObject.SetActive(false);
					}
				}
				if (this.Connection2Enabled)
				{
					foreach (Transform transform2 in this.cable2Segments)
					{
						transform2.gameObject.SetActive(false);
					}
				}
				return;
			}
			if (this.Connection1Enabled)
			{
				PowerLine.DrawPowerLine(this.cable1Connection.position, this.previousPole.cable1Connection.position, this.cable1Segments, this.LengthFactor);
				foreach (Transform transform3 in this.cable1Segments)
				{
					transform3.gameObject.SetActive(true);
				}
			}
			if (this.Connection2Enabled)
			{
				PowerLine.DrawPowerLine(this.cable2Connection.position, this.previousPole.cable2Connection.position, this.cable2Segments, this.LengthFactor);
				foreach (Transform transform4 in this.cable2Segments)
				{
					transform4.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x0017976B File Offset: 0x0017796B
		[CompilerGenerated]
		private void <Start>g__Register|17_0()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|17_0));
			PlayerSingleton<PlayerCamera>.Instance.RegisterMovementEvent(2, new Action(this.UpdateCulling));
		}

		// Token: 0x04004163 RID: 16739
		public const float CABLE_CULL_DISTANCE = 100f;

		// Token: 0x04004164 RID: 16740
		public const float CABLE_CULL_DISTANCE_SQR = 10000f;

		// Token: 0x04004165 RID: 16741
		public UtilityPole previousPole;

		// Token: 0x04004166 RID: 16742
		public UtilityPole nextPole;

		// Token: 0x04004167 RID: 16743
		public bool Connection1Enabled = true;

		// Token: 0x04004168 RID: 16744
		public bool Connection2Enabled = true;

		// Token: 0x04004169 RID: 16745
		public float LengthFactor = 1.002f;

		// Token: 0x0400416A RID: 16746
		[Header("References")]
		public Transform cable1Connection;

		// Token: 0x0400416B RID: 16747
		public Transform cable2Connection;

		// Token: 0x0400416C RID: 16748
		public List<Transform> cable1Segments = new List<Transform>();

		// Token: 0x0400416D RID: 16749
		public List<Transform> cable2Segments = new List<Transform>();

		// Token: 0x0400416E RID: 16750
		public Transform Cable1Container;

		// Token: 0x0400416F RID: 16751
		public Transform Cable2Container;

		// Token: 0x04004170 RID: 16752
		private Vector3 cableStart = Vector3.zero;

		// Token: 0x04004171 RID: 16753
		private Vector3 cableEnd = Vector3.zero;

		// Token: 0x04004172 RID: 16754
		private Vector3 cableMid = Vector3.zero;
	}
}
