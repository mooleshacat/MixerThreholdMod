using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.Noise;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000961 RID: 2401
	public class Equippable_RangedWeapon : Equippable_AvatarViewmodel
	{
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x00111B01 File Offset: 0x0010FD01
		// (set) Token: 0x060040BB RID: 16571 RVA: 0x00111B09 File Offset: 0x0010FD09
		public float Aim { get; private set; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x00111B12 File Offset: 0x0010FD12
		// (set) Token: 0x060040BD RID: 16573 RVA: 0x00111B1A File Offset: 0x0010FD1A
		public float Accuracy { get; private set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x00111B23 File Offset: 0x0010FD23
		// (set) Token: 0x060040BF RID: 16575 RVA: 0x00111B2B File Offset: 0x0010FD2B
		public float TimeSinceFire { get; set; } = 1000f;

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00111B34 File Offset: 0x0010FD34
		// (set) Token: 0x060040C1 RID: 16577 RVA: 0x00111B3C File Offset: 0x0010FD3C
		public bool IsReloading { get; private set; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x00111B45 File Offset: 0x0010FD45
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x00111B4D File Offset: 0x0010FD4D
		public bool IsCocked { get; private set; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x00111B56 File Offset: 0x0010FD56
		// (set) Token: 0x060040C5 RID: 16581 RVA: 0x00111B5E File Offset: 0x0010FD5E
		public bool IsCocking { get; private set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x00111B67 File Offset: 0x0010FD67
		public int Ammo
		{
			get
			{
				if (this.weaponItem == null)
				{
					return 0;
				}
				return this.weaponItem.Value;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x00111B7E File Offset: 0x0010FD7E
		private float aimFov
		{
			get
			{
				return Singleton<Settings>.Instance.CameraFOV - this.AimFOVReduction;
			}
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x00111B94 File Offset: 0x0010FD94
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("gun");
			this.weaponItem = (item as IntegerItemInstance);
			base.InvokeRepeating("CheckAimingAtNPC", 0f, 0.5f);
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x00111BE4 File Offset: 0x0010FDE4
		public override void Unequip()
		{
			base.Unequip();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.fovOverridden)
			{
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(this.FOVChangeDuration);
				PlayerSingleton<PlayerMovement>.Instance.RemoveSprintBlocker("Aiming");
				this.fovOverridden = false;
			}
			if (this.reloadRoutine != null)
			{
				base.StopCoroutine(this.reloadRoutine);
			}
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00111C4E File Offset: 0x0010FE4E
		protected override void Update()
		{
			base.Update();
			this.UpdateInput();
			this.UpdateAnim();
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			this.TimeSinceFire += Time.deltaTime;
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00111C80 File Offset: 0x0010FE80
		private void UpdateInput()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if ((GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) || this.timeSincePrimaryClick < 0.5f || this.IsCocking) && this.CanAim())
			{
				this.Aim = Mathf.SmoothDamp(this.Aim, 1f, ref this.aimVelocity, this.AimDuration);
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, 1f, Time.deltaTime / this.AccuracyChangeDuration);
				if (!this.fovOverridden)
				{
					PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.aimFov, this.FOVChangeDuration);
					PlayerSingleton<PlayerMovement>.Instance.AddSprintBlocker("Aiming");
					this.fovOverridden = true;
					Player.Local.SendEquippableMessage_Networked("Raise", UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			else
			{
				if (this.TimeSinceFire > this.FireCooldown)
				{
					this.Aim = Mathf.SmoothDamp(this.Aim, 0f, ref this.aimVelocity, this.AimDuration);
				}
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, 0f, Time.deltaTime / this.AccuracyChangeDuration * 2f);
				if (this.fovOverridden)
				{
					PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(this.FOVChangeDuration);
					PlayerSingleton<PlayerMovement>.Instance.RemoveSprintBlocker("Aiming");
					this.fovOverridden = false;
					Player.Local.SendEquippableMessage_Networked("Lower", UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			float t = Mathf.Clamp01(PlayerSingleton<PlayerMovement>.Instance.Controller.velocity.magnitude / PlayerMovement.WalkSpeed);
			float num = Mathf.Lerp(1f, 0f, t);
			if (this.Accuracy > num)
			{
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, num, Time.deltaTime / this.AccuracyChangeDuration * 2f);
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.timeSincePrimaryClick = 0f;
			}
			else
			{
				this.timeSincePrimaryClick += Time.deltaTime;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || this.shotQueued)
			{
				if (this.CanFire(false))
				{
					if (this.Ammo > 0)
					{
						if (!this.MustBeCocked || this.IsCocked)
						{
							this.Fire();
						}
						else
						{
							this.Cock();
						}
					}
					else if (this.EmptySound != null)
					{
						this.EmptySound.Play();
						this.shotQueued = false;
						if (this.IsReloadReady(false))
						{
							this.Reload();
						}
					}
				}
				else if (this.TimeSinceFire < this.FireCooldown || this.IsCocking)
				{
					this.shotQueued = true;
				}
			}
			if (this.reloadQueued || GameInput.GetButtonDown(GameInput.ButtonCode.Reload))
			{
				if (this.IsReloadReady(false))
				{
					this.Reload();
					return;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Reload) && this.IsReloadReady(true) && this.TimeSinceFire > this.FireCooldown * 0.5f)
				{
					Console.Log("Reload qeueued", null);
					this.reloadQueued = true;
				}
			}
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00111F88 File Offset: 0x00110188
		private void UpdateAnim()
		{
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Aim", this.Aim);
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x000022C9 File Offset: 0x000004C9
		private bool CanAim()
		{
			return true;
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x00111FA4 File Offset: 0x001101A4
		public virtual void Fire()
		{
			this.IsCocked = false;
			this.shotQueued = false;
			this.TimeSinceFire = 0f;
			Vector3 data = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 50f;
			Player.Local.SendEquippableMessage_Networked_Vector("Shoot", UnityEngine.Random.Range(int.MinValue, int.MaxValue), data);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.FireAnimTriggers[UnityEngine.Random.Range(0, this.FireAnimTriggers.Length)]);
			PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
			this.FireSound.Play();
			this.weaponItem.ChangeValue(-1);
			float spread = this.GetSpread();
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			vector = Quaternion.Euler(UnityEngine.Random.insideUnitCircle * spread) * vector;
			Vector3 vector2 = PlayerSingleton<PlayerCamera>.Instance.transform.position;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.forward * 0.4f;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.right * 0.1f;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.up * -0.03f;
			Singleton<FXManager>.Instance.CreateBulletTrail(vector2, vector, this.TracerSpeed, this.Range, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			NoiseUtility.EmitNoise(base.transform.position, ENoiseType.Gunshot, 25f, Player.Local.gameObject);
			if (Player.Local.CurrentProperty == null)
			{
				Player.Local.VisualState.ApplyState("shooting", PlayerVisualState.EVisualState.DischargingWeapon, 4f);
			}
			RaycastHit[] array = Physics.SphereCastAll(vector2, this.RayRadius, vector, this.Range, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit hit = array2[i];
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent == null || componentInParent != Player.Local)
				{
					if (componentInParent != null)
					{
						Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, this.ImpactForce, this.Damage, EImpactType.Bullet, Player.Local, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
						componentInParent.SendImpact(impact);
						Singleton<FXManager>.Instance.CreateImpactFX(impact);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			this.Accuracy = 0f;
			if (this.onFire != null)
			{
				this.onFire.Invoke();
			}
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0011226F File Offset: 0x0011046F
		public virtual void Reload()
		{
			this.reloadQueued = false;
			this.IsReloading = true;
			Console.Log("Reloading...", null);
			this.reloadRoutine = base.StartCoroutine(this.<Reload>g__ReloadRoutine|77_0());
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void NotifyIncrementalReload()
		{
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x0011229C File Offset: 0x0011049C
		private bool IsReloadReady(bool ignoreTiming)
		{
			StorableItemInstance storableItemInstance;
			return this.CanReload && !this.IsReloading && this.GetMagazine(out storableItemInstance) && this.weaponItem.Value < this.MagazineSize && (this.TimeSinceFire >= this.FireCooldown || ignoreTiming) && (base.equipAnimDone || ignoreTiming) && !this.IsCocking;
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x0011230C File Offset: 0x0011050C
		protected virtual bool GetMagazine(out StorableItemInstance mag)
		{
			mag = null;
			for (int i = 0; i < PlayerSingleton<PlayerInventory>.Instance.hotbarSlots.Count; i++)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].Quantity != 0 && PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].ItemInstance.ID == this.Magazine.ID)
				{
					mag = (PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].ItemInstance as StorableItemInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00112398 File Offset: 0x00110598
		private bool CanFire(bool checkAmmo = true)
		{
			return this.TimeSinceFire >= this.FireCooldown && this.Aim >= 0.1f && base.equipAnimDone && (!checkAmmo || this.Ammo > 0) && !this.IsReloading && !this.IsCocking;
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x001123F4 File Offset: 0x001105F4
		private bool CanCock()
		{
			return !this.IsCocked && !this.IsCocking && this.weaponItem.Value > 0 && base.equipAnimDone && !this.IsReloading && this.TimeSinceFire >= this.FireCooldown;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x0011244A File Offset: 0x0011064A
		private void Cock()
		{
			Console.Log("Cocking", null);
			this.shotQueued = false;
			this.IsCocking = true;
			base.StartCoroutine(this.<Cock>g__CockRoutine|83_0());
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00112472 File Offset: 0x00110672
		private float GetSpread()
		{
			return Mathf.Lerp(this.MaxSpread, this.MinSpread, this.Accuracy);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x0011248C File Offset: 0x0011068C
		private void CheckAimingAtNPC()
		{
			if (this.Aim < 0.5f)
			{
				return;
			}
			RaycastHit[] array = Physics.SphereCastAll(new Ray(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.forward), 0.5f, 10f, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			List<NPC> list = new List<NPC>();
			foreach (RaycastHit raycastHit in array)
			{
				NPC componentInParent = raycastHit.collider.GetComponentInParent<NPC>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
					if (componentInParent.awareness.VisionCone.IsPlayerVisible(Player.Local))
					{
						componentInParent.responses.RespondToAimedAt(Player.Local);
					}
				}
			}
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00112655 File Offset: 0x00110855
		[CompilerGenerated]
		private IEnumerator <Reload>g__ReloadRoutine|77_0()
		{
			if (this.onReloadStart != null)
			{
				this.onReloadStart.Invoke();
			}
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadStartAnimTrigger);
			yield return new WaitForSeconds(this.ReloadStartTime);
			StorableItemInstance storableItemInstance;
			if (this.IncrementalReload)
			{
				StorableItemInstance mag;
				while (this.weaponItem.Value < this.MagazineSize && this.GetMagazine(out mag))
				{
					if (this.onReloadIndividual != null)
					{
						this.onReloadIndividual.Invoke();
					}
					Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadIndividualAnimTrigger);
					yield return new WaitForSeconds(this.ReloadIndividalTime);
					this.weaponItem.ChangeValue(1);
					IntegerItemInstance integerItemInstance = mag as IntegerItemInstance;
					integerItemInstance.ChangeValue(-1);
					this.NotifyIncrementalReload();
					if (integerItemInstance.Value <= 0)
					{
						mag.ChangeQuantity(-1);
						if (this.ReloadTrash != null)
						{
							Vector3 posiiton = PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.4f;
							NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.ReloadTrash.ID, posiiton, UnityEngine.Random.rotation, default(Vector3), "", false);
						}
					}
				}
				yield return new WaitForSeconds(0.05f);
				if (this.onReloadEnd != null)
				{
					this.onReloadEnd.Invoke();
				}
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadEndAnimTrigger);
				yield return new WaitForSeconds(this.ReloadEndTime);
			}
			else if (this.GetMagazine(out storableItemInstance))
			{
				IntegerItemInstance integerItemInstance2 = storableItemInstance as IntegerItemInstance;
				integerItemInstance2.ChangeValue(-(this.MagazineSize - this.weaponItem.Value));
				if (integerItemInstance2.Value <= 0)
				{
					storableItemInstance.ChangeQuantity(-1);
					if (this.ReloadTrash != null)
					{
						Vector3 posiiton2 = PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.4f;
						NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.ReloadTrash.ID, posiiton2, UnityEngine.Random.rotation, default(Vector3), "", false);
					}
				}
				this.weaponItem.SetValue(this.MagazineSize);
			}
			Console.Log("Reloading done!", null);
			this.IsReloading = false;
			this.reloadRoutine = null;
			yield break;
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00112664 File Offset: 0x00110864
		[CompilerGenerated]
		private IEnumerator <Cock>g__CockRoutine|83_0()
		{
			if (this.onCockStart != null)
			{
				this.onCockStart.Invoke();
			}
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.CockAnimTrigger);
			yield return new WaitForSeconds(this.CockTime);
			this.IsCocked = true;
			this.IsCocking = false;
			yield break;
		}

		// Token: 0x04002E24 RID: 11812
		public const float NPC_AIM_DETECTION_RANGE = 10f;

		// Token: 0x04002E2B RID: 11819
		public int MagazineSize = 7;

		// Token: 0x04002E2C RID: 11820
		[Header("Aim Settings")]
		public float AimDuration = 0.2f;

		// Token: 0x04002E2D RID: 11821
		public float AimFOVReduction = 10f;

		// Token: 0x04002E2E RID: 11822
		public float FOVChangeDuration = 0.3f;

		// Token: 0x04002E2F RID: 11823
		[Header("Firing")]
		public AudioSourceController FireSound;

		// Token: 0x04002E30 RID: 11824
		public AudioSourceController EmptySound;

		// Token: 0x04002E31 RID: 11825
		public float FireCooldown = 0.3f;

		// Token: 0x04002E32 RID: 11826
		public string[] FireAnimTriggers;

		// Token: 0x04002E33 RID: 11827
		public float AccuracyChangeDuration = 0.6f;

		// Token: 0x04002E34 RID: 11828
		[Header("Raycasting")]
		public float Range = 40f;

		// Token: 0x04002E35 RID: 11829
		public float RayRadius = 0.05f;

		// Token: 0x04002E36 RID: 11830
		[Header("Spread")]
		public float MinSpread = 5f;

		// Token: 0x04002E37 RID: 11831
		public float MaxSpread = 15f;

		// Token: 0x04002E38 RID: 11832
		[Header("Damage")]
		public float Damage = 60f;

		// Token: 0x04002E39 RID: 11833
		public float ImpactForce = 300f;

		// Token: 0x04002E3A RID: 11834
		[Header("Reloading")]
		public bool CanReload = true;

		// Token: 0x04002E3B RID: 11835
		public bool IncrementalReload;

		// Token: 0x04002E3C RID: 11836
		public StorableItemDefinition Magazine;

		// Token: 0x04002E3D RID: 11837
		public float ReloadStartTime = 1.5f;

		// Token: 0x04002E3E RID: 11838
		public float ReloadIndividalTime;

		// Token: 0x04002E3F RID: 11839
		public float ReloadEndTime;

		// Token: 0x04002E40 RID: 11840
		public string ReloadStartAnimTrigger = "MagazineReload";

		// Token: 0x04002E41 RID: 11841
		public string ReloadIndividualAnimTrigger = string.Empty;

		// Token: 0x04002E42 RID: 11842
		public string ReloadEndAnimTrigger = string.Empty;

		// Token: 0x04002E43 RID: 11843
		public TrashItem ReloadTrash;

		// Token: 0x04002E44 RID: 11844
		[Header("Cocking")]
		public bool MustBeCocked;

		// Token: 0x04002E45 RID: 11845
		public float CockTime = 0.5f;

		// Token: 0x04002E46 RID: 11846
		public string CockAnimTrigger = "MagazineReload";

		// Token: 0x04002E47 RID: 11847
		[Header("Effects")]
		public float TracerSpeed = 50f;

		// Token: 0x04002E48 RID: 11848
		public UnityEvent onFire;

		// Token: 0x04002E49 RID: 11849
		public UnityEvent onReloadStart;

		// Token: 0x04002E4A RID: 11850
		public UnityEvent onReloadIndividual;

		// Token: 0x04002E4B RID: 11851
		public UnityEvent onReloadEnd;

		// Token: 0x04002E4C RID: 11852
		public UnityEvent onCockStart;

		// Token: 0x04002E4D RID: 11853
		protected IntegerItemInstance weaponItem;

		// Token: 0x04002E4E RID: 11854
		private bool fovOverridden;

		// Token: 0x04002E4F RID: 11855
		private float aimVelocity;

		// Token: 0x04002E50 RID: 11856
		private Coroutine reloadRoutine;

		// Token: 0x04002E51 RID: 11857
		private bool shotQueued;

		// Token: 0x04002E52 RID: 11858
		private bool reloadQueued;

		// Token: 0x04002E53 RID: 11859
		private float timeSincePrimaryClick = 100f;
	}
}
