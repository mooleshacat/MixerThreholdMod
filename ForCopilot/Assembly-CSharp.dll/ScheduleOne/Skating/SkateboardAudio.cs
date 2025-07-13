using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Skating
{
	// Token: 0x020002DD RID: 733
	public class SkateboardAudio : MonoBehaviour
	{
		// Token: 0x06000FDB RID: 4059 RVA: 0x00046073 File Offset: 0x00044273
		private void Awake()
		{
			this.Board.OnJump.AddListener(new UnityAction<float>(this.PlayJump));
			this.Board.OnLand.AddListener(new UnityAction(this.PlayLand));
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x000460B0 File Offset: 0x000442B0
		private void Start()
		{
			if (this.Board.IsGrounded())
			{
				this.PlayLand();
			}
			this.RollingAudio.VolumeMultiplier = 0f;
			this.RollingAudio.Play();
			this.DirtRollingAudio.VolumeMultiplier = 0f;
			this.DirtRollingAudio.Play();
			this.WindAudio.VolumeMultiplier = 0f;
			this.WindAudio.Play();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00046124 File Offset: 0x00044324
		private void Update()
		{
			float num = Mathf.Clamp(Mathf.Abs(this.Board.CurrentSpeed_Kmh) / this.Board.TopSpeed_Kmh, 0f, 1.5f);
			float volumeMultiplier = num;
			if (this.Board.AirTime > 0.2f)
			{
				volumeMultiplier = 0f;
			}
			this.DirtRollingAudio.VolumeMultiplier = 0f;
			this.RollingAudio.VolumeMultiplier = 0f;
			AudioSourceController audioSourceController = this.Board.IsOnTerrain() ? this.DirtRollingAudio : this.RollingAudio;
			audioSourceController.VolumeMultiplier = volumeMultiplier;
			audioSourceController.AudioSource.pitch = Mathf.Lerp(0.75f, 1f, num);
			if (this.Board.IsOwner)
			{
				this.WindAudio.VolumeMultiplier = num;
				this.WindAudio.AudioSource.pitch = Mathf.Lerp(1.2f, 1.5f, num);
				return;
			}
			this.WindAudio.VolumeMultiplier = 0f;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0004621D File Offset: 0x0004441D
		public void PlayJump(float force)
		{
			this.JumpAudio.VolumeMultiplier = Mathf.Lerp(0.5f, 1f, force);
			this.JumpAudio.Play();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00046245 File Offset: 0x00044445
		public void PlayLand()
		{
			this.LandAudio.Play();
		}

		// Token: 0x04001048 RID: 4168
		public Skateboard Board;

		// Token: 0x04001049 RID: 4169
		[Header("References")]
		public AudioSourceController JumpAudio;

		// Token: 0x0400104A RID: 4170
		public AudioSourceController LandAudio;

		// Token: 0x0400104B RID: 4171
		public AudioSourceController RollingAudio;

		// Token: 0x0400104C RID: 4172
		public AudioSourceController DirtRollingAudio;

		// Token: 0x0400104D RID: 4173
		public AudioSourceController WindAudio;
	}
}
