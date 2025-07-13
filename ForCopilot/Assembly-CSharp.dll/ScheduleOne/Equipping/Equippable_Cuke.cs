using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000951 RID: 2385
	public class Equippable_Cuke : Equippable_Viewmodel
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x00110451 File Offset: 0x0010E651
		// (set) Token: 0x06004062 RID: 16482 RVA: 0x00110459 File Offset: 0x0010E659
		public bool IsDrinking { get; protected set; }

		// Token: 0x06004063 RID: 16483 RVA: 0x00110462 File Offset: 0x0010E662
		protected override void Update()
		{
			base.Update();
			if (this.IsDrinking)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && !GameInput.IsTyping && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				this.Drink();
			}
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x00110494 File Offset: 0x0010E694
		public void Drink()
		{
			this.IsDrinking = true;
			base.StartCoroutine(this.<Drink>g__DrinkRoutine|17_0());
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x001104AC File Offset: 0x0010E6AC
		public void ApplyEffects()
		{
			float num = Mathf.Pow(this.ConsecutiveReduction, (float)Player.Local.Energy.EnergyDrinksConsumed);
			float num2 = Mathf.Clamp(this.BaseEnergyGain * num, this.MinEnergyGain, this.BaseEnergyGain);
			Player.Local.Energy.SetEnergy(Player.Local.Energy.CurrentEnergy + num2);
			PlayerSingleton<PlayerMovement>.Instance.SetStamina(PlayerMovement.StaminaReserveMax, true);
			if (this.HealthGain > 0f)
			{
				Player.Local.Health.RecoverHealth(this.HealthGain);
			}
			Player.Local.Energy.IncrementEnergyDrinks();
			if (this.ClearDrugEffects && Player.Local.ConsumedProduct != null && Player.Local.ConsumedProduct.Definition != this.PseudoProduct)
			{
				Player.Local.ClearProduct();
				return;
			}
			if (this.PseudoProduct != null)
			{
				ProductItemInstance product = this.PseudoProduct.GetDefaultInstance(1) as ProductItemInstance;
				Player.Local.ConsumeProduct(product);
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x001105EA File Offset: 0x0010E7EA
		[CompilerGenerated]
		private IEnumerator <Drink>g__DrinkRoutine|17_0()
		{
			this.OpenAnim.Play();
			this.DrinkAnim.Play();
			this.OpenSound.Play();
			this.SlurpSound.Play();
			yield return new WaitForSeconds(this.AnimationDuration);
			this.ApplyEffects();
			this.TrashPrefab = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.TrashPrefab.ID, PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.3f, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, PlayerSingleton<PlayerMovement>.Instance.Controller.velocity + (PlayerSingleton<PlayerCamera>.Instance.transform.forward + PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.25f) * 4f, "", false);
			this.itemInstance.ChangeQuantity(-1);
			if (this.itemInstance.Quantity > 0)
			{
				PlayerSingleton<PlayerInventory>.Instance.Reequip();
			}
			yield break;
		}

		// Token: 0x04002DD1 RID: 11729
		[Header("Settings")]
		public float BaseEnergyGain = 100f;

		// Token: 0x04002DD2 RID: 11730
		public float MinEnergyGain = 2.5f;

		// Token: 0x04002DD3 RID: 11731
		public float ConsecutiveReduction = 0.5f;

		// Token: 0x04002DD4 RID: 11732
		public float HealthGain;

		// Token: 0x04002DD5 RID: 11733
		public float AnimationDuration = 2f;

		// Token: 0x04002DD6 RID: 11734
		public bool ClearDrugEffects;

		// Token: 0x04002DD7 RID: 11735
		public ProductDefinition PseudoProduct;

		// Token: 0x04002DD8 RID: 11736
		[Header("References")]
		public Animation OpenAnim;

		// Token: 0x04002DD9 RID: 11737
		public Animation DrinkAnim;

		// Token: 0x04002DDA RID: 11738
		public AudioSourceController OpenSound;

		// Token: 0x04002DDB RID: 11739
		public AudioSourceController SlurpSound;

		// Token: 0x04002DDC RID: 11740
		public TrashItem TrashPrefab;
	}
}
