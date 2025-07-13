using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Audio
{
	// Token: 0x020007E2 RID: 2018
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(EventTrigger))]
	[RequireComponent(typeof(AudioSourceController))]
	public class ButtonSound : MonoBehaviour
	{
		// Token: 0x060036A0 RID: 13984 RVA: 0x000E6218 File Offset: 0x000E4418
		public void Awake()
		{
			this.AddEventTrigger(this.EventTrigger, 0, new Action(this.Hovered));
			if (this.PlaySoundOnClickStart)
			{
				this.AddEventTrigger(this.EventTrigger, 2, new Action(this.Clicked));
			}
			else
			{
				this.AddEventTrigger(this.EventTrigger, 4, new Action(this.Clicked));
			}
			this.AudioSource.AudioSource.playOnAwake = false;
			this.Button = base.GetComponent<Button>();
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000E629A File Offset: 0x000E449A
		private void OnValidate()
		{
			if (this.AudioSource == null)
			{
				this.AudioSource = base.GetComponent<AudioSourceController>();
			}
			if (this.EventTrigger == null)
			{
				this.EventTrigger = base.GetComponent<EventTrigger>();
			}
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000E62D0 File Offset: 0x000E44D0
		public void AddEventTrigger(EventTrigger eventTrigger, EventTriggerType eventTriggerType, Action action)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = eventTriggerType;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				action();
			});
			eventTrigger.triggers.Add(entry);
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000E631C File Offset: 0x000E451C
		protected virtual void Hovered()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.AudioSource.VolumeMultiplier = this.HoverSoundVolume;
			this.AudioSource.AudioSource.clip = this.HoverClip;
			this.AudioSource.PitchMultiplier = 0.9f;
			this.AudioSource.Play();
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x000E637C File Offset: 0x000E457C
		protected virtual void Clicked()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.AudioSource.VolumeMultiplier = this.ClickSoundVolume;
			this.AudioSource.AudioSource.clip = this.ClickClip;
			this.AudioSource.Play();
		}

		// Token: 0x040026E1 RID: 9953
		public AudioSourceController AudioSource;

		// Token: 0x040026E2 RID: 9954
		public EventTrigger EventTrigger;

		// Token: 0x040026E3 RID: 9955
		public bool PlaySoundOnClickStart;

		// Token: 0x040026E4 RID: 9956
		[Header("Clips")]
		public AudioClip HoverClip;

		// Token: 0x040026E5 RID: 9957
		public float HoverSoundVolume = 1f;

		// Token: 0x040026E6 RID: 9958
		public AudioClip ClickClip;

		// Token: 0x040026E7 RID: 9959
		public float ClickSoundVolume = 1f;

		// Token: 0x040026E8 RID: 9960
		private Button Button;
	}
}
