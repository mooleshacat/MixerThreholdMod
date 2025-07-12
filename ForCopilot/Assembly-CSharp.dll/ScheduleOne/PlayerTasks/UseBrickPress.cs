using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200036B RID: 875
	public class UseBrickPress : Task
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00056BFF File Offset: 0x00054DFF
		// (set) Token: 0x060013B8 RID: 5048 RVA: 0x00056C07 File Offset: 0x00054E07
		public override string TaskName { get; protected set; } = "Use brick press";

		// Token: 0x060013B9 RID: 5049 RVA: 0x00056C10 File Offset: 0x00054E10
		public UseBrickPress(BrickPress _press, ProductItemInstance _product)
		{
			if (_press == null)
			{
				Console.LogError("Press is null!", null);
				return;
			}
			if (_press.GetState() != PackagingStation.EState.CanBegin)
			{
				Console.LogError("Press not ready to begin packaging!", null);
				return;
			}
			this.press = _press;
			this.product = _product;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition_Pouring.position, this.press.CameraPosition_Pouring.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			this.press.Container1.gameObject.SetActive(false);
			this.container = this.press.CreateFunctionalContainer(this.product, 0.75f, out this.products);
			base.CurrentInstruction = "Pour product into mould (0/20)";
			this.press.StartCoroutine(this.<.ctor>g__CheckMould|11_0());
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00056D23 File Offset: 0x00054F23
		public override void Update()
		{
			base.Update();
			if (this.currentStep == UseBrickPress.EStep.Pressing && this.press.Handle.CurrentPosition >= 1f)
			{
				this.FinishPress();
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00056D54 File Offset: 0x00054F54
		public override void StopTask()
		{
			base.StopTask();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.container != null)
			{
				UnityEngine.Object.Destroy(this.container.gameObject);
			}
			for (int i = 0; i < this.products.Count; i++)
			{
				UnityEngine.Object.Destroy(this.products[i].gameObject);
			}
			this.press.Container1.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition.position, this.press.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(this.press, true, true);
			this.press.Handle.Locked = false;
			this.press.Handle.SetInteractable(false);
			if (this.currentStep == UseBrickPress.EStep.Complete)
			{
				this.press.CompletePress(this.product);
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00056E64 File Offset: 0x00055064
		private void CheckMould()
		{
			if (this.currentStep != UseBrickPress.EStep.Pouring)
			{
				return;
			}
			List<FunctionalProduct> productInMould = this.press.GetProductInMould();
			base.CurrentInstruction = "Pour product into mould (" + productInMould.Count.ToString() + "/20)";
			if (productInMould.Count >= 20)
			{
				this.BeginPress();
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00056EBC File Offset: 0x000550BC
		private void BeginPress()
		{
			this.currentStep = UseBrickPress.EStep.Pressing;
			this.press.Handle.SetInteractable(true);
			this.container.ClickableEnabled = false;
			this.container.Rb.AddForce((this.press.transform.right + this.press.transform.up) * 2f, 2);
			base.CurrentInstruction = "Rotate handle quickly to press product";
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.3f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition_Raising.position, this.press.CameraPosition_Raising.rotation, 0.3f, false);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00056F7C File Offset: 0x0005517C
		private void FinishPress()
		{
			this.press.SlamSound.Play();
			this.currentStep = UseBrickPress.EStep.Complete;
			this.press.Handle.Locked = true;
			this.press.Handle.SetInteractable(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.1f);
			PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(0.25f, 0.2f, true);
			this.press.StartCoroutine(this.<FinishPress>g__Wait|16_0());
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00056FFD File Offset: 0x000551FD
		[CompilerGenerated]
		private IEnumerator <.ctor>g__CheckMould|11_0()
		{
			while (base.TaskActive)
			{
				this.CheckMould();
				yield return new WaitForSeconds(0.2f);
			}
			yield break;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005700C File Offset: 0x0005520C
		[CompilerGenerated]
		private IEnumerator <FinishPress>g__Wait|16_0()
		{
			yield return new WaitForSeconds(0.8f);
			this.StopTask();
			yield break;
		}

		// Token: 0x040012C8 RID: 4808
		public const float PRODUCT_SCALE = 0.75f;

		// Token: 0x040012CA RID: 4810
		protected UseBrickPress.EStep currentStep;

		// Token: 0x040012CB RID: 4811
		protected BrickPress press;

		// Token: 0x040012CC RID: 4812
		protected ProductItemInstance product;

		// Token: 0x040012CD RID: 4813
		protected List<FunctionalProduct> products = new List<FunctionalProduct>();

		// Token: 0x040012CE RID: 4814
		protected Draggable container;

		// Token: 0x0200036C RID: 876
		public enum EStep
		{
			// Token: 0x040012D0 RID: 4816
			Pouring,
			// Token: 0x040012D1 RID: 4817
			Pressing,
			// Token: 0x040012D2 RID: 4818
			Complete
		}
	}
}
