using System;
using ScheduleOne.Audio;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C1D RID: 3101
	public class BunsenBurner : MonoBehaviour
	{
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06005453 RID: 21587 RVA: 0x00164730 File Offset: 0x00162930
		// (set) Token: 0x06005454 RID: 21588 RVA: 0x00164738 File Offset: 0x00162938
		public bool Interactable { get; private set; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06005455 RID: 21589 RVA: 0x00164741 File Offset: 0x00162941
		// (set) Token: 0x06005456 RID: 21590 RVA: 0x00164749 File Offset: 0x00162949
		public bool IsDialHeld { get; private set; }

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06005457 RID: 21591 RVA: 0x00164752 File Offset: 0x00162952
		// (set) Token: 0x06005458 RID: 21592 RVA: 0x0016475A File Offset: 0x0016295A
		public float CurrentDialValue { get; private set; }

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06005459 RID: 21593 RVA: 0x00164763 File Offset: 0x00162963
		// (set) Token: 0x0600545A RID: 21594 RVA: 0x0016476B File Offset: 0x0016296B
		public float CurrentHeat { get; private set; }

		// Token: 0x0600545B RID: 21595 RVA: 0x00164774 File Offset: 0x00162974
		private void Start()
		{
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001647C0 File Offset: 0x001629C0
		private void Update()
		{
			if (!this.LockDial)
			{
				if (this.IsDialHeld)
				{
					this.CurrentDialValue = Mathf.Clamp01(this.CurrentDialValue + this.HandleRotationSpeed * Time.deltaTime);
				}
				else
				{
					this.CurrentDialValue = Mathf.Clamp01(this.CurrentDialValue - this.HandleRotationSpeed * Time.deltaTime);
				}
				this.Handle.localRotation = Quaternion.Lerp(this.Handle_Min.localRotation, this.Handle_Max.localRotation, this.CurrentDialValue);
			}
			this.CurrentHeat = this.CurrentDialValue;
			this.Highlight.gameObject.SetActive(this.Interactable && !this.IsDialHeld);
			if (this.CurrentHeat > 0f)
			{
				this.FlameSound.VolumeMultiplier = this.CurrentHeat;
				this.FlameSound.AudioSource.pitch = this.FlamePitch.Evaluate(this.CurrentHeat);
				if (!this.FlameSound.AudioSource.isPlaying)
				{
					this.FlameSound.Play();
				}
			}
			else if (this.FlameSound.AudioSource.isPlaying)
			{
				this.FlameSound.Stop();
			}
			this.UpdateEffects();
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x001648F8 File Offset: 0x00162AF8
		private void UpdateEffects()
		{
			if (this.CurrentHeat > 0f)
			{
				if (!this.Flame.isPlaying)
				{
					this.Flame.Play();
				}
				this.Light.gameObject.SetActive(true);
				this.Flame.startColor = this.FlameColor.Evaluate(this.CurrentHeat);
				this.Light.color = this.Flame.startColor;
				this.Light.intensity = this.LightIntensity.Evaluate(this.CurrentHeat);
				return;
			}
			if (this.Flame.isPlaying)
			{
				this.Flame.Stop();
			}
			this.Light.gameObject.SetActive(false);
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x001649B3 File Offset: 0x00162BB3
		public void SetDialPosition(float pos)
		{
			this.CurrentDialValue = Mathf.Clamp01(pos);
			this.Handle.localRotation = Quaternion.Lerp(this.Handle_Min.localRotation, this.Handle_Max.localRotation, this.CurrentDialValue);
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x001649F0 File Offset: 0x00162BF0
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
			if (!this.Interactable)
			{
				this.IsDialHeld = false;
			}
			if (this.Interactable)
			{
				this.Anim.Play();
				return;
			}
			this.Anim.Stop();
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x00164A3F File Offset: 0x00162C3F
		public void ClickStart(RaycastHit hit)
		{
			this.IsDialHeld = true;
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x00164A48 File Offset: 0x00162C48
		public void ClickEnd()
		{
			this.IsDialHeld = false;
		}

		// Token: 0x04003ECC RID: 16076
		public bool LockDial;

		// Token: 0x04003ECD RID: 16077
		[Header("Settings")]
		public Gradient FlameColor;

		// Token: 0x04003ECE RID: 16078
		public AnimationCurve LightIntensity;

		// Token: 0x04003ECF RID: 16079
		public float HandleRotationSpeed = 1f;

		// Token: 0x04003ED0 RID: 16080
		public AnimationCurve FlamePitch;

		// Token: 0x04003ED1 RID: 16081
		[Header("References")]
		public ParticleSystem Flame;

		// Token: 0x04003ED2 RID: 16082
		public Light Light;

		// Token: 0x04003ED3 RID: 16083
		public Transform Handle;

		// Token: 0x04003ED4 RID: 16084
		public Clickable HandleClickable;

		// Token: 0x04003ED5 RID: 16085
		public Transform Handle_Min;

		// Token: 0x04003ED6 RID: 16086
		public Transform Handle_Max;

		// Token: 0x04003ED7 RID: 16087
		public Transform Highlight;

		// Token: 0x04003ED8 RID: 16088
		public Animation Anim;

		// Token: 0x04003ED9 RID: 16089
		public AudioSourceController FlameSound;
	}
}
