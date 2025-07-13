using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x0200093B RID: 2363
	public class Product_Equippable : Equippable_Viewmodel
	{
		// Token: 0x0600400C RID: 16396 RVA: 0x0010EE08 File Offset: 0x0010D008
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			ProductItemInstance productItemInstance = item as ProductItemInstance;
			this.productAmount = productItemInstance.Amount;
			if (this.Consumable && this.productAmount == 1)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("consumable");
				Singleton<InputPromptsCanvas>.Instance.currentModule.gameObject.GetComponentsInChildren<Transform>().FirstOrDefault((Transform c) => c.gameObject.name == "Label").GetComponent<TextMeshProUGUI>().text = "(Hold) " + this.ConsumeDescription;
			}
			productItemInstance.SetupPackagingVisuals(this.Visuals);
			if (this.ModelContainer == null)
			{
				Console.LogWarning("Model container not set for equippable product: " + item.Name, null);
				this.ModelContainer = base.transform.GetChild(0);
			}
			this.defaultModelPosition = this.ModelContainer.localPosition;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0010EEF4 File Offset: 0x0010D0F4
		public override void Unequip()
		{
			if (this.Consumable)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			if (base.transform.IsChildOf(Player.Local.transform))
			{
				if (!string.IsNullOrEmpty(this.ConsumeAnimationTrigger))
				{
					Player.Local.SendAnimationTrigger(this.ConsumeAnimationTrigger);
				}
				else if (!string.IsNullOrEmpty(this.ConsumeAnimationBool))
				{
					Player.Local.SendAnimationBool(this.ConsumeAnimationBool, false);
				}
			}
			if (this.consumingInProgress)
			{
				base.StopCoroutine(this.consumeRoutine);
			}
			base.Unequip();
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0010EF84 File Offset: 0x0010D184
		protected override void Update()
		{
			base.Update();
			Vector3 b = this.defaultModelPosition;
			if (this.Consumable && !this.consumingInProgress && GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.productAmount == 1 && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				if (this.consumeTime == 0f && this.onConsumeInputStart != null)
				{
					this.onConsumeInputStart.Invoke();
				}
				this.consumeTime += Time.deltaTime;
				Singleton<HUD>.Instance.ShowRadialIndicator(this.consumeTime / this.ConsumeTime);
				if (this.consumeTime >= this.ConsumeTime)
				{
					this.Consume();
					if (this.onConsumeInputComplete != null)
					{
						this.onConsumeInputComplete.Invoke();
					}
				}
			}
			else
			{
				if (this.consumeTime > 0f && this.onConsumeInputCancel != null && !this.consumingInProgress)
				{
					this.onConsumeInputCancel.Invoke();
					if (base.transform.IsChildOf(Player.Local.transform) && !string.IsNullOrEmpty(this.ConsumeAnimationBool))
					{
						Player.Local.SendAnimationBool(this.ConsumeAnimationBool, false);
					}
				}
				this.consumeTime = 0f;
			}
			if (this.consumeTime > 0f || this.consumingInProgress)
			{
				b = this.defaultModelPosition - this.ModelContainer.transform.parent.InverseTransformDirection(PlayerSingleton<PlayerCamera>.Instance.transform.up) * 0.25f;
			}
			this.ModelContainer.transform.localPosition = Vector3.Lerp(this.ModelContainer.transform.localPosition, b, Time.deltaTime * 6f);
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0010F134 File Offset: 0x0010D334
		protected virtual void Consume()
		{
			this.consumingInProgress = true;
			if (base.transform.IsChildOf(Player.Local.transform))
			{
				if (!string.IsNullOrEmpty(this.ConsumeAnimationTrigger))
				{
					Player.Local.SendAnimationTrigger(this.ConsumeAnimationTrigger);
				}
				else if (!string.IsNullOrEmpty(this.ConsumeAnimationBool))
				{
					Player.Local.SendAnimationBool(this.ConsumeAnimationBool, true);
				}
				if (this.ConsumeEquippableAssetPath != string.Empty)
				{
					Player.Local.SendEquippable_Networked(this.ConsumeEquippableAssetPath);
				}
			}
			this.consumeRoutine = base.StartCoroutine(this.<Consume>g__ConsumeRoutine|20_0());
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0010F1D0 File Offset: 0x0010D3D0
		protected virtual void ApplyEffects()
		{
			Player.Local.ConsumeProduct(this.itemInstance as ProductItemInstance);
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0010F256 File Offset: 0x0010D456
		[CompilerGenerated]
		private IEnumerator <Consume>g__ConsumeRoutine|20_0()
		{
			yield return new WaitForSeconds(this.EffectsApplyDelay);
			this.consumingInProgress = false;
			this.ApplyEffects();
			this.itemInstance.ChangeQuantity(-1);
			yield break;
		}

		// Token: 0x04002D7D RID: 11645
		[Header("References")]
		public FilledPackagingVisuals Visuals;

		// Token: 0x04002D7E RID: 11646
		public Transform ModelContainer;

		// Token: 0x04002D7F RID: 11647
		[Header("Settings")]
		public bool Consumable = true;

		// Token: 0x04002D80 RID: 11648
		public string ConsumeDescription = "Smoke";

		// Token: 0x04002D81 RID: 11649
		public float ConsumeTime = 1.5f;

		// Token: 0x04002D82 RID: 11650
		public float EffectsApplyDelay = 2f;

		// Token: 0x04002D83 RID: 11651
		public string ConsumeAnimationBool = string.Empty;

		// Token: 0x04002D84 RID: 11652
		public string ConsumeAnimationTrigger = string.Empty;

		// Token: 0x04002D85 RID: 11653
		public string ConsumeEquippableAssetPath = string.Empty;

		// Token: 0x04002D86 RID: 11654
		[Header("Events")]
		public UnityEvent onConsumeInputStart;

		// Token: 0x04002D87 RID: 11655
		public UnityEvent onConsumeInputComplete;

		// Token: 0x04002D88 RID: 11656
		public UnityEvent onConsumeInputCancel;

		// Token: 0x04002D89 RID: 11657
		private float consumeTime;

		// Token: 0x04002D8A RID: 11658
		private bool consumingInProgress;

		// Token: 0x04002D8B RID: 11659
		private Vector3 defaultModelPosition = Vector3.zero;

		// Token: 0x04002D8C RID: 11660
		private int productAmount = 1;

		// Token: 0x04002D8D RID: 11661
		private Coroutine consumeRoutine;
	}
}
