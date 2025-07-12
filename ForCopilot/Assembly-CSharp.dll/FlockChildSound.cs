using System;
using ScheduleOne.Audio;
using UnityEngine;

// Token: 0x02000050 RID: 80
[RequireComponent(typeof(AudioSource))]
public class FlockChildSound : MonoBehaviour
{
	// Token: 0x060001AA RID: 426 RVA: 0x00009814 File Offset: 0x00007A14
	public void Start()
	{
		this._flockChild = base.GetComponent<FlockChild>();
		this._audio = base.GetComponent<AudioSource>();
		base.InvokeRepeating("PlayRandomSound", UnityEngine.Random.value + 1f, 1f);
		if (this._scareSounds.Length != 0)
		{
			base.InvokeRepeating("ScareSound", 1f, 0.01f);
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00009874 File Offset: 0x00007A74
	public void PlayRandomSound()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (!this._audio.isPlaying && this._flightSounds.Length != 0 && this._flightSoundRandomChance > UnityEngine.Random.value && !this._flockChild._landing)
			{
				if (this.controller != null)
				{
					this.controller.Play();
					return;
				}
			}
			else if (!this._audio.isPlaying && this._idleSounds.Length != 0 && this._idleSoundRandomChance > UnityEngine.Random.value && this._flockChild._landing && this.controller != null)
			{
				this.controller.Play();
			}
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00009924 File Offset: 0x00007B24
	public void ScareSound()
	{
		if (base.gameObject.activeInHierarchy && this._hasLanded && !this._flockChild._landing && this._idleSoundRandomChance * 2f > UnityEngine.Random.value)
		{
			this._audio.clip = this._scareSounds[UnityEngine.Random.Range(0, this._scareSounds.Length)];
			this._audio.volume = UnityEngine.Random.Range(this._volumeMin, this._volumeMax);
			this._audio.PlayDelayed(UnityEngine.Random.value * 0.2f);
			this._hasLanded = false;
		}
	}

	// Token: 0x04000184 RID: 388
	public AudioSourceController controller;

	// Token: 0x04000185 RID: 389
	public AudioClip[] _idleSounds;

	// Token: 0x04000186 RID: 390
	public float _idleSoundRandomChance = 0.05f;

	// Token: 0x04000187 RID: 391
	public AudioClip[] _flightSounds;

	// Token: 0x04000188 RID: 392
	public float _flightSoundRandomChance = 0.05f;

	// Token: 0x04000189 RID: 393
	public AudioClip[] _scareSounds;

	// Token: 0x0400018A RID: 394
	public float _pitchMin = 0.85f;

	// Token: 0x0400018B RID: 395
	public float _pitchMax = 1f;

	// Token: 0x0400018C RID: 396
	public float _volumeMin = 0.6f;

	// Token: 0x0400018D RID: 397
	public float _volumeMax = 0.8f;

	// Token: 0x0400018E RID: 398
	private FlockChild _flockChild;

	// Token: 0x0400018F RID: 399
	private AudioSource _audio;

	// Token: 0x04000190 RID: 400
	private bool _hasLanded;
}
