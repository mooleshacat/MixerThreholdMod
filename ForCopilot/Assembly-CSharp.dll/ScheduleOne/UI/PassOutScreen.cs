using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A6A RID: 2666
	public class PassOutScreen : Singleton<PassOutScreen>
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x0012D4F6 File Offset: 0x0012B6F6
		// (set) Token: 0x060047B3 RID: 18355 RVA: 0x0012D4FE File Offset: 0x0012B6FE
		public bool isOpen { get; protected set; }

		// Token: 0x060047B4 RID: 18356 RVA: 0x0012D507 File Offset: 0x0012B707
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Group.alpha = 0f;
			this.Group.interactable = false;
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0012D537 File Offset: 0x0012B737
		private void Continue()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<Continue>g__Routine|14_0());
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0012D556 File Offset: 0x0012B756
		private void LoadSaveClicked()
		{
			this.Close();
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0012D560 File Offset: 0x0012B760
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			Singleton<EyelidOverlay>.Instance.Canvas.sortingOrder = 5;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.cashLoss = Mathf.Min(UnityEngine.Random.Range(50f, 500f), NetworkSingleton<MoneyManager>.Instance.cashBalance);
			base.StartCoroutine(this.<Open>g__Routine|16_0());
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0012D5D0 File Offset: 0x0012B7D0
		public void Close()
		{
			this.isOpen = false;
			this.Canvas.enabled = false;
			Singleton<EyelidOverlay>.Instance.Canvas.sortingOrder = -1;
			Singleton<EyelidOverlay>.Instance.AutoUpdate = true;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (!Singleton<ArrestNoticeScreen>.Instance.isOpen)
			{
				Player.Activate();
			}
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x0012D634 File Offset: 0x0012B834
		[CompilerGenerated]
		private IEnumerator <Continue>g__Routine|14_0()
		{
			float fadeTime = 1f;
			for (float i = 0f; i < fadeTime; i += Time.deltaTime)
			{
				this.Group.alpha = Mathf.Lerp(1f, 0f, i / fadeTime);
				yield return new WaitForEndOfFrame();
			}
			this.MainLabel.gameObject.SetActive(false);
			Player.Local.SendPassOutRecovery();
			Player.Local.Health.RecoverHealth(100f);
			Transform child = this.RecoveryPointsContainer.GetChild(UnityEngine.Random.Range(0, this.RecoveryPointsContainer.childCount));
			PlayerSingleton<PlayerMovement>.Instance.Teleport(child.position);
			Player.Local.transform.forward = child.forward;
			yield return new WaitForSeconds(0.5f);
			bool fadeBlur = false;
			if (Player.Local.IsArrested)
			{
				Singleton<ArrestNoticeScreen>.Instance.RecordCrimes();
				Player.Local.Free();
				Singleton<ArrestNoticeScreen>.Instance.Open();
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				yield return new WaitForSeconds(1f);
			}
			else
			{
				this.ContextLabel.text = "You awaken in a new location, unsure of how you got there.";
				if (this.cashLoss > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.cashLoss, true, false);
					this.ContextLabel.text = this.ContextLabel.text + "\n\n<color=#54E717>" + MoneyManager.FormatAmount(this.cashLoss, false, false) + "</color> is missing from your wallet.";
				}
				this.ContextLabel.gameObject.SetActive(true);
				for (float i = 0f; i < fadeTime; i += Time.deltaTime)
				{
					this.Group.alpha = Mathf.Lerp(0f, 1f, i / fadeTime);
					yield return new WaitForEndOfFrame();
				}
				fadeBlur = true;
				yield return new WaitForSeconds(4f);
				for (float i = 0f; i < fadeTime; i += Time.deltaTime)
				{
					this.Group.alpha = Mathf.Lerp(1f, 0f, i / fadeTime);
					yield return new WaitForEndOfFrame();
				}
				this.Group.alpha = 0f;
			}
			yield return new WaitForSeconds(1f);
			float lerpTime = 2f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<EyelidOverlay>.Instance.SetOpen(Mathf.Lerp(0f, 1f, i / lerpTime));
				if (fadeBlur)
				{
					Singleton<PostProcessingManager>.Instance.SetBlur(1f - i / lerpTime);
				}
				yield return new WaitForEndOfFrame();
			}
			Singleton<EyelidOverlay>.Instance.SetOpen(1f);
			if (fadeBlur)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			}
			this.Close();
			yield break;
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x0012D643 File Offset: 0x0012B843
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|16_0()
		{
			this.MainLabel.gameObject.SetActive(true);
			this.ContextLabel.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.5f);
			Singleton<EyelidOverlay>.Instance.AutoUpdate = false;
			float lerpTime = 2f;
			float startOpenness = Singleton<EyelidOverlay>.Instance.CurrentOpen;
			float endOpenness = 0f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<EyelidOverlay>.Instance.SetOpen(Mathf.Lerp(startOpenness, endOpenness, i / lerpTime));
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<EyelidOverlay>.Instance.SetOpen(0f);
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			yield return new WaitForSeconds(0.5f);
			this.Anim.Play();
			this.Canvas.enabled = true;
			yield return new WaitForSeconds(3f);
			this.Continue();
			yield break;
		}

		// Token: 0x0400347F RID: 13439
		public const float CASH_LOSS_MIN = 50f;

		// Token: 0x04003480 RID: 13440
		public const float CASH_LOSS_MAX = 500f;

		// Token: 0x04003481 RID: 13441
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003482 RID: 13442
		public CanvasGroup Group;

		// Token: 0x04003483 RID: 13443
		public Transform RecoveryPointsContainer;

		// Token: 0x04003484 RID: 13444
		public TextMeshProUGUI MainLabel;

		// Token: 0x04003485 RID: 13445
		public TextMeshProUGUI ContextLabel;

		// Token: 0x04003486 RID: 13446
		public Animation Anim;

		// Token: 0x04003487 RID: 13447
		private float cashLoss;
	}
}
