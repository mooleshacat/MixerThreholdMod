using System;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008F8 RID: 2296
	public class BoilingFlask : Fillable
	{
		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00106815 File Offset: 0x00104A15
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x0010681D File Offset: 0x00104A1D
		public float CurrentTemperature { get; private set; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x00106826 File Offset: 0x00104A26
		// (set) Token: 0x06003E51 RID: 15953 RVA: 0x0010682E File Offset: 0x00104A2E
		public float CurrentTemperatureVelocity { get; private set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x00106837 File Offset: 0x00104A37
		public bool IsTemperatureInRange
		{
			get
			{
				return this.Recipe != null && this.CurrentTemperature >= this.Recipe.CookTemperatureLowerBound && this.CurrentTemperature <= this.Recipe.CookTemperatureUpperBound;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x00106874 File Offset: 0x00104A74
		// (set) Token: 0x06003E54 RID: 15956 RVA: 0x0010687C File Offset: 0x00104A7C
		public float OverheatScale { get; private set; }

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003E55 RID: 15957 RVA: 0x00106885 File Offset: 0x00104A85
		// (set) Token: 0x06003E56 RID: 15958 RVA: 0x0010688D File Offset: 0x00104A8D
		public StationRecipe Recipe { get; private set; }

		// Token: 0x06003E57 RID: 15959 RVA: 0x00106898 File Offset: 0x00104A98
		public void Update()
		{
			if (this.Burner == null)
			{
				return;
			}
			if (!this.LockTemperature)
			{
				float num = this.Burner.CurrentHeat - this.CurrentTemperature / 500f;
				this.CurrentTemperatureVelocity = Mathf.MoveTowards(this.CurrentTemperatureVelocity, num * this.TEMPERATURE_MAX_VELOCITY, this.TEMPERATURE_ACCELERATION * Time.deltaTime);
				this.CurrentTemperature = Mathf.Clamp(this.CurrentTemperature + this.CurrentTemperatureVelocity * Time.deltaTime, 0f, 500f);
			}
			if (this.CurrentTemperature > 0f)
			{
				this.BoilSound.VolumeMultiplier = Mathf.Clamp01(this.CurrentTemperature / 500f);
				this.BoilSound.AudioSource.pitch = this.BoilSoundPitchCurve.Evaluate(Mathf.Clamp01(this.CurrentTemperature / 500f));
				this.BoilSound.ApplyVolume();
				this.BoilSound.ApplyPitch();
				if (!this.BoilSound.AudioSource.isPlaying)
				{
					this.BoilSound.AudioSource.Play();
				}
			}
			else
			{
				this.BoilSound.AudioSource.Stop();
			}
			if (this.Recipe != null && this.CurrentTemperature >= this.Recipe.CookTemperatureUpperBound)
			{
				float num2 = Mathf.Clamp((this.CurrentTemperature - this.Recipe.CookTemperatureUpperBound) / (500f - this.Recipe.CookTemperatureUpperBound), 0.25f, 1f);
				this.OverheatScale += num2 * Time.deltaTime / 1.25f;
			}
			else
			{
				this.OverheatScale = Mathf.MoveTowards(this.OverheatScale, 0f, Time.deltaTime / 1.25f);
			}
			if (this.OverheatScale > 0f)
			{
				this.OverheatMesh.material.color = new Color(1f, 1f, 1f, Mathf.Pow(this.OverheatScale, 2f));
				this.OverheatMesh.enabled = true;
				return;
			}
			this.OverheatMesh.enabled = false;
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00106AB0 File Offset: 0x00104CB0
		private void FixedUpdate()
		{
			this.UpdateCanvas();
			this.UpdateSmoke();
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00106AC0 File Offset: 0x00104CC0
		private void UpdateCanvas()
		{
			if (this.TemperatureCanvas.gameObject.activeSelf)
			{
				this.TemperatureLabel.text = Mathf.RoundToInt(this.CurrentTemperature).ToString() + "°C";
				if (this.CurrentTemperature < this.Recipe.CookTemperatureLowerBound)
				{
					this.TemperatureLabel.color = Color.white;
				}
				else if (this.CurrentTemperature > this.Recipe.CookTemperatureUpperBound)
				{
					this.TemperatureLabel.color = new Color32(byte.MaxValue, 90, 90, byte.MaxValue);
				}
				else
				{
					this.TemperatureLabel.color = Color.green;
				}
				this.TemperatureSlider.value = this.CurrentTemperature / 500f;
				if (this.OverheatScale > 0f)
				{
					this.TemperatureLabel.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f) * Mathf.Clamp(this.OverheatScale, 0.3f, 1f) * this.LabelJitterScale;
					return;
				}
				this.TemperatureLabel.transform.localPosition = Vector3.zero;
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00106C10 File Offset: 0x00104E10
		private void UpdateSmoke()
		{
			if (this.CurrentTemperature < 1f)
			{
				if (this.SmokeParticles.isPlaying)
				{
					this.SmokeParticles.Stop();
				}
				return;
			}
			ParticleSystem.MainModule main = this.SmokeParticles.main;
			main.simulationSpeed = Mathf.Lerp(1f, 3f, this.CurrentTemperature / 500f);
			main.startColor = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, this.CurrentTemperature / 500f));
			if (!this.SmokeParticles.isPlaying)
			{
				this.SmokeParticles.Play();
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00106CC4 File Offset: 0x00104EC4
		public void SetCanvasVisible(bool visible)
		{
			this.TemperatureCanvas.gameObject.SetActive(visible);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00106CD7 File Offset: 0x00104ED7
		public void SetTemperature(float temp)
		{
			this.CurrentTemperature = temp;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00106CE0 File Offset: 0x00104EE0
		public void SetRecipe(StationRecipe recipe)
		{
			this.Recipe = recipe;
			if (recipe == null)
			{
				return;
			}
			float x = this.Recipe.CookTemperatureLowerBound / 500f;
			float x2 = this.Recipe.CookTemperatureUpperBound / 500f;
			this.TemperatureRangeIndicator.anchorMin = new Vector2(x, this.TemperatureRangeIndicator.anchorMin.y);
			this.TemperatureRangeIndicator.anchorMax = new Vector2(x2, this.TemperatureRangeIndicator.anchorMax.y);
		}

		// Token: 0x04002C5D RID: 11357
		public const float TEMPERATURE_MAX = 500f;

		// Token: 0x04002C5E RID: 11358
		public float TEMPERATURE_MAX_VELOCITY = 200f;

		// Token: 0x04002C5F RID: 11359
		public float TEMPERATURE_ACCELERATION = 50f;

		// Token: 0x04002C60 RID: 11360
		public const float OVERHEAT_TIME = 1.25f;

		// Token: 0x04002C65 RID: 11365
		public bool LockTemperature;

		// Token: 0x04002C66 RID: 11366
		public AnimationCurve BoilSoundPitchCurve;

		// Token: 0x04002C67 RID: 11367
		public float LabelJitterScale = 1f;

		// Token: 0x04002C68 RID: 11368
		[Header("References")]
		public BunsenBurner Burner;

		// Token: 0x04002C69 RID: 11369
		public Canvas TemperatureCanvas;

		// Token: 0x04002C6A RID: 11370
		public TextMeshProUGUI TemperatureLabel;

		// Token: 0x04002C6B RID: 11371
		public Slider TemperatureSlider;

		// Token: 0x04002C6C RID: 11372
		public RectTransform TemperatureRangeIndicator;

		// Token: 0x04002C6D RID: 11373
		public ParticleSystem SmokeParticles;

		// Token: 0x04002C6E RID: 11374
		public AudioSourceController BoilSound;

		// Token: 0x04002C6F RID: 11375
		public MeshRenderer OverheatMesh;
	}
}
