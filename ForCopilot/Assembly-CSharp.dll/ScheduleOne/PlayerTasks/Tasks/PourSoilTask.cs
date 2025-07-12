using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.Soil;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000372 RID: 882
	public class PourSoilTask : PourIntoPotTask
	{
		// Token: 0x060013F0 RID: 5104 RVA: 0x0005828C File Offset: 0x0005648C
		public PourSoilTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			base.CurrentInstruction = "Click and drag to cut soil bag";
			this.soil = (this.pourable as PourableSoil);
			this.soil.onOpened.AddListener(new UnityAction(base.RemoveItem));
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000582DC File Offset: 0x000564DC
		public override void Update()
		{
			base.Update();
			if (this.soil.IsOpen)
			{
				base.CurrentInstruction = "Pour soil into pot (" + Mathf.FloorToInt(this.pot.SoilLevel / this.pot.SoilCapacity * 100f).ToString() + "%)";
			}
			this.UpdateHover();
			this.UpdateCursor();
			if (this.HoveredTopCollider != null && GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.soil.TopColliders.IndexOf(this.HoveredTopCollider) == this.soil.currentCut)
			{
				this.soil.Cut();
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0005838B File Offset: 0x0005658B
		public override void StopTask()
		{
			this.pot.PushSoilDataToServer();
			base.StopTask();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000583A0 File Offset: 0x000565A0
		protected override void UpdateCursor()
		{
			if (this.soil.IsOpen)
			{
				base.UpdateCursor();
				return;
			}
			if (this.HoveredTopCollider != null && this.soil.TopColliders.IndexOf(this.HoveredTopCollider) == this.soil.currentCut)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Scissors);
				return;
			}
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00058409 File Offset: 0x00056609
		private void UpdateHover()
		{
			this.HoveredTopCollider = this.GetHoveredTopCollider();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00058418 File Offset: 0x00056618
		private Collider GetHoveredTopCollider()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(3f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f) && this.soil.TopColliders.Contains(raycastHit.collider))
			{
				return raycastHit.collider;
			}
			return null;
		}

		// Token: 0x040012F1 RID: 4849
		private PourableSoil soil;

		// Token: 0x040012F2 RID: 4850
		private Collider HoveredTopCollider;
	}
}
