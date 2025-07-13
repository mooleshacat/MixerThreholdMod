using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200099C RID: 2460
	public class AvatarEffects : MonoBehaviour
	{
		// Token: 0x06004293 RID: 17043 RVA: 0x0011840C File Offset: 0x0011660C
		private void Start()
		{
			this.AdditionalWeightController.Initialize();
			this.AdditionalWeightController.SetDefault(0f);
			this.AdditionalGenderController.Initialize();
			this.AdditionalGenderController.SetDefault(0f);
			this.HeadSizeBoost.Initialize();
			this.HeadSizeBoost.SetDefault(0f);
			this.NeckSizeBoost.Initialize();
			this.NeckSizeBoost.SetDefault(0f);
			this.SkinColorSmoother.Initialize();
			if (this.Avatar.CurrentSettings != null)
			{
				this.SetDefaultSkinColor(true);
			}
			this.ZapLoopSound.VolumeMultiplier = 0f;
			this.Avatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetDefaultSkinColor(true);
			});
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x001184D8 File Offset: 0x001166D8
		public void FixedUpdate()
		{
			this.SetEffectsCulled(this.Avatar.Anim.IsAvatarCulled);
			if (!this.Avatar.Anim.enabled)
			{
				return;
			}
			if (this.Avatar.Anim.IsAvatarCulled)
			{
				return;
			}
			this.Avatar.SetAdditionalWeight(this.AdditionalWeightController.CurrentValue);
			this.Avatar.SetAdditionalGender(this.AdditionalGenderController.CurrentValue);
			this.Avatar.SetSkinColor(this.SkinColorSmoother.CurrentValue);
			this.currentEmission = Color.Lerp(this.currentEmission, this.targetEmission, Time.deltaTime * 0.5f);
			this.Avatar.SetEmission(this.currentEmission);
			if (this.DisableHead)
			{
				this.HeadBone.transform.localScale = Vector3.zero;
			}
			else
			{
				this.HeadBone.transform.localScale = Vector3.one * (1f + this.HeadSizeBoost.CurrentValue);
			}
			this.NeckBone.transform.localScale = Vector3.one * (1f + this.NeckSizeBoost.CurrentValue);
			if (this.FireParticles.isPlaying)
			{
				this.FireSound.VolumeMultiplier = Mathf.MoveTowards(this.FireSound.VolumeMultiplier, 1f, Time.deltaTime);
				if (!this.FireSound.isPlaying)
				{
					this.FireSound.Play();
				}
			}
			else
			{
				this.FireSound.VolumeMultiplier = Mathf.MoveTowards(this.FireSound.VolumeMultiplier, 0f, Time.deltaTime);
				if (this.FireSound.VolumeMultiplier <= 0f)
				{
					this.FireSound.Stop();
				}
			}
			if (this.ZapParticles.isPlaying)
			{
				this.ZapLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.ZapLoopSound.VolumeMultiplier, 1f, Time.deltaTime * 2f);
				if (!this.ZapLoopSound.isPlaying)
				{
					this.ZapLoopSound.Play();
					return;
				}
			}
			else
			{
				this.ZapLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.ZapLoopSound.VolumeMultiplier, 0f, Time.deltaTime * 2f);
				if (this.ZapLoopSound.VolumeMultiplier <= 0f)
				{
					this.ZapLoopSound.Stop();
				}
			}
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00118734 File Offset: 0x00116934
		private void SetEffectsCulled(bool culled)
		{
			if (this.isCulled == culled)
			{
				return;
			}
			this.isCulled = culled;
			GameObject[] objectsToCull = this.ObjectsToCull;
			for (int i = 0; i < objectsToCull.Length; i++)
			{
				objectsToCull[i].SetActive(!culled);
			}
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x00118774 File Offset: 0x00116974
		public void SetStinkParticlesActive(bool active, bool mirror = true)
		{
			foreach (ParticleSystem particleSystem in this.StinkParticles)
			{
				if (active)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Stop();
				}
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetStinkParticlesActive(active, false);
				}
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x001187D0 File Offset: 0x001169D0
		public void TriggerSick(bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].TriggerSick(false);
				}
			}
			base.StartCoroutine(this.<TriggerSick>g__Routine|36_0());
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0011880C File Offset: 0x00116A0C
		public void SetAntiGrav(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetAntiGrav(active, false);
				}
			}
			if (active)
			{
				this.AntiGravParticles.Play();
				return;
			}
			this.AntiGravParticles.Stop();
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x00118858 File Offset: 0x00116A58
		public void SetFoggy(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetFoggy(active, false);
				}
			}
			if (active)
			{
				this.FoggyEffects.Play();
				return;
			}
			this.FoggyEffects.Stop();
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x001188A4 File Offset: 0x00116AA4
		public void VanishHair(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.SetHairVisible(false);
			this.Avatar.EyeBrows.leftBrow.gameObject.SetActive(false);
			this.Avatar.EyeBrows.rightBrow.gameObject.SetActive(false);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].VanishHair(false);
				}
			}
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x0011892C File Offset: 0x00116B2C
		public void SetZapped(bool zapped, bool mirror = true)
		{
			if (zapped)
			{
				LayerUtility.SetLayerRecursively(this.ZapParticles.gameObject, LayerMask.NameToLayer("Default"));
				this.ZapParticles.Play();
				this.ZapSound.Play();
			}
			else
			{
				this.ZapParticles.Stop();
				this.ZapSound.Stop();
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetZapped(zapped, false);
				}
			}
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x001189A8 File Offset: 0x00116BA8
		public void ReturnHair(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.SetHairVisible(true);
			this.Avatar.EyeBrows.leftBrow.gameObject.SetActive(true);
			this.Avatar.EyeBrows.rightBrow.gameObject.SetActive(true);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ReturnHair(false);
				}
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x00118A30 File Offset: 0x00116C30
		public void OverrideHairColor(Color color, bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.OverrideHairColor(color);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].OverrideHairColor(color, false);
				}
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x00118A84 File Offset: 0x00116C84
		public void ResetHairColor(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.ResetHairColor();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ResetHairColor(false);
				}
			}
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00118AD4 File Offset: 0x00116CD4
		public void OverrideEyeColor(Color color, float emission = 0.115f, bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.SetEyeballColor(color, emission, false);
			this.Avatar.Eyes.leftEye.SetEyeballColor(color, emission, false);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].OverrideEyeColor(color, emission, false);
				}
			}
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00118B34 File Offset: 0x00116D34
		public void ResetEyeColor(bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.ResetEyeballColor();
			this.Avatar.Eyes.leftEye.ResetEyeballColor();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ResetEyeColor(false);
				}
			}
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x00118B8C File Offset: 0x00116D8C
		public void SetEyeLightEmission(float intensity, Color color, bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.ConfigureEyeLight(color, intensity);
			this.Avatar.Eyes.leftEye.ConfigureEyeLight(color, intensity);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetEyeLightEmission(intensity, color, false);
				}
			}
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x00118BEC File Offset: 0x00116DEC
		public void EnableLaxative(bool mirror = true)
		{
			this.laxativeEnabled = true;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].EnableLaxative(false);
				}
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableLaxative>g__Routine|47_0());
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x00118C34 File Offset: 0x00116E34
		public void DisableLaxative(bool mirror = true)
		{
			this.laxativeEnabled = false;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].DisableLaxative(false);
				}
			}
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x00118C6C File Offset: 0x00116E6C
		public void SetFireActive(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetFireActive(active, false);
				}
			}
			this.FireLight.Enabled = active;
			if (active)
			{
				this.FireParticles.Play();
				return;
			}
			this.FireParticles.Stop();
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x00118CC4 File Offset: 0x00116EC4
		public void SetBigHeadActive(bool active, bool mirror = true)
		{
			if (active)
			{
				this.HeadSizeBoost.AddOverride(0.4f, 7, "big head");
			}
			else
			{
				this.HeadSizeBoost.RemoveOverride("big head");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetBigHeadActive(active, false);
				}
			}
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x00118D20 File Offset: 0x00116F20
		public void SetGiraffeActive(bool active, bool mirror = true)
		{
			if (active)
			{
				this.HeadSizeBoost.AddOverride(-0.5f, 8, "giraffe");
				this.NeckSizeBoost.AddOverride(1f, 8, "giraffe");
			}
			else
			{
				this.HeadSizeBoost.RemoveOverride("giraffe");
				this.NeckSizeBoost.RemoveOverride("giraffe");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGiraffeActive(active, false);
				}
			}
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00118DA0 File Offset: 0x00116FA0
		public void SetSkinColorInverted(bool inverted, bool mirror = true)
		{
			if (inverted)
			{
				if (this.Avatar.IsWhite())
				{
					this.SkinColorSmoother.AddOverride(new Color32(58, 49, 42, byte.MaxValue), 7, "inverted");
				}
				else
				{
					this.SkinColorSmoother.AddOverride(new Color32(223, 189, 161, byte.MaxValue), 7, "inverted");
				}
			}
			else
			{
				this.SkinColorSmoother.RemoveOverride("inverted");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetSkinColorInverted(inverted, false);
				}
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x00118E48 File Offset: 0x00117048
		public void SetSicklySkinColor(bool mirror = true)
		{
			Color skinColor = this.Avatar.CurrentSettings.SkinColor;
			float num = 0.5f;
			float num2 = 0.3f * skinColor.r + 0.59f * skinColor.g + 0.11f * skinColor.b;
			Color color = Color.white;
			color.r = skinColor.r + (num2 - skinColor.r) * num;
			color.g = skinColor.g + (num2 - skinColor.g) * num;
			color.b = skinColor.b + (num2 - skinColor.b) * num;
			color *= 1.1f;
			string str = "Sickly Color: ";
			Color color2 = color;
			Console.Log(str + color2.ToString(), null);
			this.SkinColorSmoother.AddOverride(color, 6, "sickly");
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetSicklySkinColor(false);
				}
			}
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x00118F48 File Offset: 0x00117148
		private void SetDefaultSkinColor(bool mirror = true)
		{
			if (this.Avatar.CurrentSettings == null)
			{
				return;
			}
			this.SkinColorSmoother.SetDefault(this.Avatar.CurrentSettings.SkinColor);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetDefaultSkinColor(false);
				}
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00118FA8 File Offset: 0x001171A8
		public void SetGenderInverted(bool inverted, bool mirror = true)
		{
			if (inverted)
			{
				if (this.Avatar.IsMale())
				{
					this.AdditionalGenderController.AddOverride(1f, 7, "jennerising");
				}
				else
				{
					this.AdditionalGenderController.AddOverride(-1f, 7, "jennerising");
				}
			}
			else
			{
				this.AdditionalGenderController.RemoveOverride("jennerising");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGenderInverted(inverted, false);
				}
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00119028 File Offset: 0x00117228
		public void AddAdditionalWeightOverride(float value, int priority, string label, bool mirror = true)
		{
			this.AdditionalWeightController.AddOverride(value, priority, label);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].AddAdditionalWeightOverride(value, priority, label, false);
				}
			}
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00119068 File Offset: 0x00117268
		public void RemoveAdditionalWeightOverride(string label, bool mirror = true)
		{
			this.AdditionalWeightController.RemoveOverride(label);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].RemoveAdditionalWeightOverride(label, false);
				}
			}
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x001190A4 File Offset: 0x001172A4
		public void SetGlowingOn(Color color, bool mirror = true)
		{
			this.targetEmission = color;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGlowingOn(color, false);
				}
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x001190DC File Offset: 0x001172DC
		public void SetGlowingOff(bool mirror = true)
		{
			this.targetEmission = Color.black;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGlowingOff(false);
				}
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x00119118 File Offset: 0x00117318
		public void TriggerCountdownExplosion(bool mirror = true)
		{
			this.CountdownExplosion.Trigger();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].TriggerCountdownExplosion(false);
				}
			}
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x00119154 File Offset: 0x00117354
		public void StopCountdownExplosion(bool mirror = true)
		{
			this.CountdownExplosion.StopCountdown();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].StopCountdownExplosion(false);
				}
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00119190 File Offset: 0x00117390
		public void SetCyclopean(bool enabled, bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			if (enabled)
			{
				this.Avatar.Eyes.rightEye.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.Avatar.Eyes.rightEye.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
				this.Avatar.Eyes.leftEye.gameObject.SetActive(false);
				this.Avatar.SetBlockEyeFaceLayers(true);
			}
			else
			{
				this.Avatar.Eyes.rightEye.transform.localRotation = Quaternion.Euler(0f, 22f, 0f);
				this.Avatar.Eyes.rightEye.transform.localScale = new Vector3(1f, 1f, 1f);
				this.Avatar.Eyes.leftEye.gameObject.SetActive(true);
				this.Avatar.SetBlockEyeFaceLayers(false);
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetCyclopean(enabled, false);
				}
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x001192E8 File Offset: 0x001174E8
		public void SetZombified(bool zombified, bool mirror = true)
		{
			if (zombified)
			{
				this.SkinColorSmoother.AddOverride(new Color32(117, 122, 92, byte.MaxValue), 10, "Zombified");
				this.Avatar.Eyes.leftEye.PupilContainer.gameObject.SetActive(!zombified);
				this.Avatar.Eyes.rightEye.PupilContainer.gameObject.SetActive(!zombified);
				this.OverrideEyeColor(new Color32(159, 129, 129, byte.MaxValue), 0.115f, false);
				this.Avatar.EmotionManager.AddEmotionOverride("Zombie", "Zombified", 0f, 10);
			}
			else
			{
				this.SkinColorSmoother.RemoveOverride("Zombified");
				this.Avatar.Eyes.leftEye.PupilContainer.gameObject.SetActive(true);
				this.Avatar.Eyes.rightEye.PupilContainer.gameObject.SetActive(true);
				this.ResetEyeColor(false);
				this.Avatar.EmotionManager.RemoveEmotionOverride("Zombified");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetZombified(zombified, false);
				}
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0011946A File Offset: 0x0011766A
		[CompilerGenerated]
		private IEnumerator <TriggerSick>g__Routine|36_0()
		{
			this.GurgleSound.Play();
			yield return new WaitForSeconds(4.5f);
			this.VomitSound.Play();
			this.VomitParticles.gameObject.layer = LayerMask.NameToLayer("Default");
			this.VomitParticles.Play();
			yield break;
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00119479 File Offset: 0x00117679
		[CompilerGenerated]
		private IEnumerator <EnableLaxative>g__Routine|47_0()
		{
			do
			{
				this.FartParticles.Play();
				this.FartSound.Play();
				yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 20f));
			}
			while (this.laxativeEnabled);
			yield break;
		}

		// Token: 0x04002F67 RID: 12135
		[Header("References")]
		public Avatar Avatar;

		// Token: 0x04002F68 RID: 12136
		public ParticleSystem[] StinkParticles;

		// Token: 0x04002F69 RID: 12137
		public ParticleSystem VomitParticles;

		// Token: 0x04002F6A RID: 12138
		public ParticleSystem HeadPoofParticles;

		// Token: 0x04002F6B RID: 12139
		public ParticleSystem FartParticles;

		// Token: 0x04002F6C RID: 12140
		public ParticleSystem AntiGravParticles;

		// Token: 0x04002F6D RID: 12141
		public ParticleSystem FireParticles;

		// Token: 0x04002F6E RID: 12142
		public OptimizedLight FireLight;

		// Token: 0x04002F6F RID: 12143
		public ParticleSystem FoggyEffects;

		// Token: 0x04002F70 RID: 12144
		public Transform HeadBone;

		// Token: 0x04002F71 RID: 12145
		public Transform NeckBone;

		// Token: 0x04002F72 RID: 12146
		public AvatarEffects[] MirrorEffectsTo;

		// Token: 0x04002F73 RID: 12147
		public ParticleSystem ZapParticles;

		// Token: 0x04002F74 RID: 12148
		public CountdownExplosion CountdownExplosion;

		// Token: 0x04002F75 RID: 12149
		public GameObject[] ObjectsToCull;

		// Token: 0x04002F76 RID: 12150
		[Header("Settings")]
		public bool DisableHead;

		// Token: 0x04002F77 RID: 12151
		[Header("Sounds")]
		public AudioSourceController GurgleSound;

		// Token: 0x04002F78 RID: 12152
		public AudioSourceController VomitSound;

		// Token: 0x04002F79 RID: 12153
		public AudioSourceController PoofSound;

		// Token: 0x04002F7A RID: 12154
		public AudioSourceController FartSound;

		// Token: 0x04002F7B RID: 12155
		public AudioSourceController FireSound;

		// Token: 0x04002F7C RID: 12156
		public AudioSourceController ZapSound;

		// Token: 0x04002F7D RID: 12157
		public AudioSourceController ZapLoopSound;

		// Token: 0x04002F7E RID: 12158
		[Header("Smoothers")]
		[SerializeField]
		private FloatSmoother AdditionalWeightController;

		// Token: 0x04002F7F RID: 12159
		[SerializeField]
		private FloatSmoother AdditionalGenderController;

		// Token: 0x04002F80 RID: 12160
		[SerializeField]
		private FloatSmoother HeadSizeBoost;

		// Token: 0x04002F81 RID: 12161
		[SerializeField]
		private FloatSmoother NeckSizeBoost;

		// Token: 0x04002F82 RID: 12162
		[SerializeField]
		private ColorSmoother SkinColorSmoother;

		// Token: 0x04002F83 RID: 12163
		private bool laxativeEnabled;

		// Token: 0x04002F84 RID: 12164
		private Color currentEmission = Color.black;

		// Token: 0x04002F85 RID: 12165
		private Color targetEmission = Color.black;

		// Token: 0x04002F86 RID: 12166
		private bool isCulled;
	}
}
