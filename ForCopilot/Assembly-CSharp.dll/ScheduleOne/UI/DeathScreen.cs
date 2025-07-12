using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A62 RID: 2658
	public class DeathScreen : Singleton<DeathScreen>
	{
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x0012C98D File Offset: 0x0012AB8D
		// (set) Token: 0x06004783 RID: 18307 RVA: 0x0012C995 File Offset: 0x0012AB95
		public bool isOpen { get; protected set; }

		// Token: 0x06004784 RID: 18308 RVA: 0x0012C9A0 File Offset: 0x0012ABA0
		protected override void Awake()
		{
			base.Awake();
			this.respawnButton.onClick.AddListener(new UnityAction(this.RespawnClicked));
			this.loadSaveButton.onClick.AddListener(new UnityAction(this.LoadSaveClicked));
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.group.alpha = 0f;
			this.group.interactable = false;
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0012CA24 File Offset: 0x0012AC24
		private void RespawnClicked()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<RespawnClicked>g__Routine|13_0());
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0012CA43 File Offset: 0x0012AC43
		private void LoadSaveClicked()
		{
			this.Close();
			Singleton<LoadManager>.Instance.ExitToMenu(Singleton<LoadManager>.Instance.ActiveSaveInfo, null, false);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0012CA64 File Offset: 0x0012AC64
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			this.arrested = Player.Local.IsArrested;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Sound.Play();
			this.respawnButton.gameObject.SetActive(this.CanRespawn());
			this.loadSaveButton.gameObject.SetActive(!this.respawnButton.gameObject.activeSelf);
			base.StartCoroutine(this.<Open>g__Routine|15_0());
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0012CAF2 File Offset: 0x0012ACF2
		private bool CanRespawn()
		{
			return Player.PlayerList.Count > 1;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0012CB04 File Offset: 0x0012AD04
		public void Close()
		{
			this.isOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0012CB5C File Offset: 0x0012AD5C
		[CompilerGenerated]
		private IEnumerator <RespawnClicked>g__Routine|13_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			this.Close();
			Singleton<HospitalBillScreen>.Instance.Open();
			Transform transform = Singleton<Map>.Instance.MedicalCentre.RespawnPoint;
			if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive && (Player.Local.LastVisitedProperty != null || Property.OwnedProperties.Count > 0))
			{
				if (Player.Local.LastVisitedProperty != null)
				{
					transform = Player.Local.LastVisitedProperty.InteriorSpawnPoint;
				}
				else
				{
					transform = Property.OwnedProperties[0].InteriorSpawnPoint;
				}
			}
			Player.Local.Health.SendRevive(transform.position + Vector3.up * 1f, transform.rotation);
			if (this.arrested)
			{
				Singleton<ArrestNoticeScreen>.Instance.RecordCrimes();
				Player.Local.Free();
			}
			yield return new WaitForSeconds(2f);
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			yield break;
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0012CB6B File Offset: 0x0012AD6B
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|15_0()
		{
			yield return new WaitForSeconds(0.55f);
			this.Anim.Play();
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			float lerpTime = 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			this.group.interactable = true;
			yield break;
		}

		// Token: 0x04003454 RID: 13396
		[Header("References")]
		public Canvas canvas;

		// Token: 0x04003455 RID: 13397
		public RectTransform Container;

		// Token: 0x04003456 RID: 13398
		public CanvasGroup group;

		// Token: 0x04003457 RID: 13399
		public Button respawnButton;

		// Token: 0x04003458 RID: 13400
		public Button loadSaveButton;

		// Token: 0x04003459 RID: 13401
		public Animation Anim;

		// Token: 0x0400345A RID: 13402
		public AudioSourceController Sound;

		// Token: 0x0400345B RID: 13403
		private bool arrested;
	}
}
