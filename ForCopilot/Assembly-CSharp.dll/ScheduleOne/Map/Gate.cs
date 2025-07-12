using System;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C74 RID: 3188
	public class Gate : MonoBehaviour
	{
		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x0017A807 File Offset: 0x00178A07
		// (set) Token: 0x060059A0 RID: 22944 RVA: 0x0017A80F File Offset: 0x00178A0F
		public bool IsOpen { get; protected set; }

		// Token: 0x060059A1 RID: 22945 RVA: 0x0017A818 File Offset: 0x00178A18
		private void Update()
		{
			this.Momentum = Mathf.MoveTowards(this.Momentum, 1f, Time.deltaTime * this.Acceleration);
			if (this.IsOpen)
			{
				this.openDelta += Time.deltaTime * this.OpenSpeed * this.Momentum;
			}
			else
			{
				this.openDelta -= Time.deltaTime * this.OpenSpeed * this.Momentum;
			}
			this.openDelta = Mathf.Clamp01(this.openDelta);
			if (this.openDelta <= 0.01f || this.openDelta >= 0.99f)
			{
				if (this.LoopSounds[0].isPlaying)
				{
					AudioSourceController[] array = this.LoopSounds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Stop();
					}
					array = this.StopSounds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Play();
					}
				}
			}
			else if (!this.LoopSounds[0].isPlaying && this.StartSounds[0].AudioSource.time >= this.StartSounds[0].AudioSource.clip.length * 0.5f)
			{
				AudioSourceController[] array = this.LoopSounds;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Play();
				}
			}
			this.Gate1.localPosition = Vector3.Lerp(this.Gate1Closed, this.Gate1Open, this.openDelta);
			this.Gate2.localPosition = Vector3.Lerp(this.Gate2Closed, this.Gate2Open, this.openDelta);
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x0017A9AC File Offset: 0x00178BAC
		[Button]
		public void Open()
		{
			this.Momentum *= -1f;
			if (this.openDelta == 0f)
			{
				this.Momentum = 0f;
			}
			AudioSourceController[] startSounds = this.StartSounds;
			for (int i = 0; i < startSounds.Length; i++)
			{
				startSounds[i].Play();
			}
			this.IsOpen = true;
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x0017AA08 File Offset: 0x00178C08
		[Button]
		public void Close()
		{
			this.Momentum *= -1f;
			if (this.openDelta == 1f)
			{
				this.Momentum = 0f;
			}
			AudioSourceController[] startSounds = this.StartSounds;
			for (int i = 0; i < startSounds.Length; i++)
			{
				startSounds[i].Play();
			}
			this.IsOpen = false;
		}

		// Token: 0x040041BE RID: 16830
		public Transform Gate1;

		// Token: 0x040041BF RID: 16831
		public Vector3 Gate1Open;

		// Token: 0x040041C0 RID: 16832
		public Vector3 Gate1Closed;

		// Token: 0x040041C1 RID: 16833
		public Transform Gate2;

		// Token: 0x040041C2 RID: 16834
		public Vector3 Gate2Open;

		// Token: 0x040041C3 RID: 16835
		public Vector3 Gate2Closed;

		// Token: 0x040041C4 RID: 16836
		public float OpenSpeed;

		// Token: 0x040041C5 RID: 16837
		public float Acceleration = 2f;

		// Token: 0x040041C6 RID: 16838
		[Header("Sound")]
		public AudioSourceController[] StartSounds;

		// Token: 0x040041C7 RID: 16839
		public AudioSourceController[] LoopSounds;

		// Token: 0x040041C8 RID: 16840
		public AudioSourceController[] StopSounds;

		// Token: 0x040041C9 RID: 16841
		private float Momentum;

		// Token: 0x040041CA RID: 16842
		private float openDelta;
	}
}
