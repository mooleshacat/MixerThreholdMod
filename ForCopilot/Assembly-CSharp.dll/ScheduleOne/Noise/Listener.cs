using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x02000571 RID: 1393
	public class Listener : MonoBehaviour
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x0008ACEB File Offset: 0x00088EEB
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x0008ACF3 File Offset: 0x00088EF3
		public float SquaredHearingRange { get; protected set; }

		// Token: 0x060021A3 RID: 8611 RVA: 0x0008ACFC File Offset: 0x00088EFC
		public void Awake()
		{
			this.SquaredHearingRange = Mathf.Pow(this.Sensitivity, 2f);
			if (this.HearingOrigin == null)
			{
				this.HearingOrigin = base.transform;
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x0008AD2E File Offset: 0x00088F2E
		public void OnEnable()
		{
			if (!Listener.listeners.Contains(this))
			{
				Listener.listeners.Add(this);
			}
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0008AD48 File Offset: 0x00088F48
		public void OnDisable()
		{
			Listener.listeners.Remove(this);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0008AD56 File Offset: 0x00088F56
		public void Notify(NoiseEvent nEvent)
		{
			if (this.onNoiseHeard != null)
			{
				this.onNoiseHeard(nEvent);
			}
		}

		// Token: 0x040019BA RID: 6586
		public static List<Listener> listeners = new List<Listener>();

		// Token: 0x040019BB RID: 6587
		[Header("Settings")]
		[Range(0.1f, 5f)]
		public float Sensitivity = 1f;

		// Token: 0x040019BC RID: 6588
		public Transform HearingOrigin;

		// Token: 0x040019BE RID: 6590
		public Listener.HearingEvent onNoiseHeard;

		// Token: 0x02000572 RID: 1394
		// (Invoke) Token: 0x060021AA RID: 8618
		public delegate void HearingEvent(NoiseEvent nEvent);
	}
}
