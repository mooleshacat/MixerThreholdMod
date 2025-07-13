using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.Interaction;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000362 RID: 866
	public class HarvestPlant : Task
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00054C9D File Offset: 0x00052E9D
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x00054CA5 File Offset: 0x00052EA5
		public override string TaskName { get; protected set; } = "Harvest plant";

		// Token: 0x06001378 RID: 4984 RVA: 0x00054CB0 File Offset: 0x00052EB0
		public HarvestPlant(Pot _pot, bool canDrag, AudioSourceController soundLoopPrefab)
		{
			if (_pot == null)
			{
				Console.LogWarning("HarvestPlant: pot null", null);
				this.StopTask();
				return;
			}
			if (_pot.Plant == null)
			{
				Console.LogWarning("HarvestPlant: pot has no plant in it", null);
			}
			this.ClickDetectionEnabled = true;
			HarvestPlant.CanDrag = canDrag;
			this.ClickDetectionRadius = 0.02f;
			this.pot = _pot;
			this.pot.SetPlayerUser(Player.Local.NetworkObject);
			this.pot.PositionCameraContainer();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.pot.FullshotPosition.position, this.pot.FullshotPosition.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.pot.Plant.Collider.enabled = false;
			this.pot.IntObj.GetComponent<Collider>().enabled = false;
			if (this.pot.AlignLeafDropToPlayer)
			{
				this.pot.LeafDropPoint.transform.rotation = Quaternion.LookRotation(Player.Local.Avatar.CenterPoint - this.pot.LeafDropPoint.position, Vector3.up);
			}
			this.HarvestTotal = this.pot.Plant.ActiveHarvestables.Count;
			this.UpdateInstructionText();
			if (soundLoopPrefab != null)
			{
				this.SoundLoop = UnityEngine.Object.Instantiate<AudioSourceController>(soundLoopPrefab, NetworkSingleton<GameManager>.Instance.Temp);
				this.SoundLoop.VolumeMultiplier = 0f;
				this.SoundLoop.transform.position = this.pot.transform.position + Vector3.up * 1f;
				this.SoundLoop.Play();
			}
			Singleton<InputPromptsCanvas>.Instance.LoadModule("harvestplant");
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00054EC0 File Offset: 0x000530C0
		private void UpdateInstructionText()
		{
			if (this.pot == null || this.pot.Plant == null)
			{
				return;
			}
			if (HarvestPlant.CanDrag)
			{
				base.CurrentInstruction = string.Concat(new string[]
				{
					"Click and hold over ",
					this.pot.Plant.HarvestTarget,
					" to harvest (",
					this.HarvestCount.ToString(),
					"/",
					this.HarvestTotal.ToString(),
					")"
				});
				return;
			}
			base.CurrentInstruction = string.Concat(new string[]
			{
				"Click ",
				this.pot.Plant.HarvestTarget,
				" to harvest (",
				this.HarvestCount.ToString(),
				"/",
				this.HarvestTotal.ToString(),
				")"
			});
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00054FB8 File Offset: 0x000531B8
		public override void StopTask()
		{
			base.StopTask();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.pot.Plant != null)
			{
				this.pot.Plant.Collider.enabled = true;
			}
			if (this.SoundLoop != null)
			{
				UnityEngine.Object.Destroy(this.SoundLoop.gameObject);
			}
			this.pot.IntObj.GetComponent<Collider>().enabled = true;
			this.pot.SetPlayerUser(null);
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00055089 File Offset: 0x00053289
		protected override void UpdateCursor()
		{
			if (this.GetHoveredHarvestable() != null)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Scissors);
				return;
			}
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000550B0 File Offset: 0x000532B0
		public override void Update()
		{
			base.Update();
			if (this.pot == null || this.pot.Plant == null)
			{
				this.StopTask();
				return;
			}
			PlantHarvestable hoveredHarvestable = this.GetHoveredHarvestable();
			if (this.SoundLoop != null)
			{
				if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
				{
					this.SoundLoop.VolumeMultiplier = Mathf.MoveTowards(this.SoundLoop.VolumeMultiplier, 1f, Time.deltaTime * 4f);
				}
				else
				{
					this.SoundLoop.VolumeMultiplier = Mathf.MoveTowards(this.SoundLoop.VolumeMultiplier, 0f, Time.deltaTime * 4f);
				}
			}
			if (hoveredHarvestable != null)
			{
				if (!PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.pot.Plant.GetHarvestedProduct(1), hoveredHarvestable.ProductQuantity))
				{
					Singleton<MouseTooltip>.Instance.ShowIcon(Singleton<MouseTooltip>.Instance.Sprite_Cross, Singleton<MouseTooltip>.Instance.Color_Invalid);
					Singleton<MouseTooltip>.Instance.ShowTooltip("Inventory full", Singleton<MouseTooltip>.Instance.Color_Invalid);
				}
				else if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && HarvestPlant.CanDrag))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pot.Plant.SnipSound.gameObject);
					gameObject.transform.position = hoveredHarvestable.transform.position;
					gameObject.GetComponent<AudioSourceController>().PlayOneShot(false);
					UnityEngine.Object.Destroy(gameObject, 1f);
					hoveredHarvestable.Harvest(true);
					this.HarvestCount++;
					this.UpdateInstructionText();
					if (this.pot.Plant == null)
					{
						this.Success();
					}
				}
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Left))
			{
				this.rotation -= Time.deltaTime * 100f;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Right))
			{
				this.rotation += Time.deltaTime * 100f;
			}
			this.pot.OverrideRotation(this.rotation);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000552B0 File Offset: 0x000534B0
		private PlantHarvestable GetHoveredHarvestable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(3f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				return raycastHit.collider.gameObject.GetComponentInParent<PlantHarvestable>();
			}
			return null;
		}

		// Token: 0x04001294 RID: 4756
		protected Pot pot;

		// Token: 0x04001295 RID: 4757
		private int HarvestCount;

		// Token: 0x04001296 RID: 4758
		private int HarvestTotal;

		// Token: 0x04001297 RID: 4759
		private float rotation;

		// Token: 0x04001298 RID: 4760
		private static bool hintShown;

		// Token: 0x04001299 RID: 4761
		private static bool CanDrag;

		// Token: 0x0400129A RID: 4762
		private AudioSourceController SoundLoop;
	}
}
