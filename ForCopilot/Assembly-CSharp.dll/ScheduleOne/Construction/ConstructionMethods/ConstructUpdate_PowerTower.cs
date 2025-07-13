using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.Tiles;
using ScheduleOne.UI;
using ScheduleOne.UI.Construction;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x02000771 RID: 1905
	public class ConstructUpdate_PowerTower : ConstructUpdate_OutdoorGrid
	{
		// Token: 0x06003348 RID: 13128 RVA: 0x000D516C File Offset: 0x000D336C
		protected override void Start()
		{
			base.Start();
			this.tempPowerLineContainer = new GameObject("TempPowerLine").transform;
			this.tempPowerLineContainer.SetParent(base.transform);
			for (int i = 0; i < PowerLine.powerLine_MaxSegments; i++)
			{
				Transform transform = UnityEngine.Object.Instantiate<GameObject>(Singleton<PowerManager>.Instance.powerLineSegmentPrefab, this.tempPowerLineContainer).transform;
				transform.Find("Model").GetComponent<MeshRenderer>().material = this.powerLine_GhostMat;
				transform.gameObject.SetActive(false);
				this.tempSegments.Add(transform);
			}
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000D5215 File Offset: 0x000D3415
		public override void ConstructionStop()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			base.ConstructionStop();
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000D5230 File Offset: 0x000D3430
		protected override void Update()
		{
			base.Update();
			this.hoveredPowerNode = this.GetHoveredPowerNode();
			this.GhostModel.gameObject.SetActive(true);
			this.cosmeticPowerNode.SetActive(false);
			if (!base.isMoving)
			{
				if (this.startNode == null)
				{
					if (this.hoveredPowerNode != null)
					{
						this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
						this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
						this.GhostModel.gameObject.SetActive(false);
						this.cosmeticPowerNode.SetActive(true);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
						{
							this.startNode = this.hoveredPowerNode;
							this.powerLineInitialDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.startNode.pConnectionPoint.transform.position);
							return;
						}
					}
				}
				else if (this.hoveredPowerNode != null && this.hoveredPowerNode != this.startNode && PowerLine.CanNodesBeConnected(this.hoveredPowerNode, this.startNode))
				{
					this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
					this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
					this.GhostModel.gameObject.SetActive(false);
					this.cosmeticPowerNode.SetActive(true);
				}
			}
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000D53D4 File Offset: 0x000D35D4
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.startNode != null && !base.isMoving)
			{
				Vector3 position = this.startNode.pConnectionPoint.transform.position;
				Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.powerLineInitialDistance));
				RaycastHit raycastHit;
				if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(100f, out raycastHit, this.detectionMask, true, 0f))
				{
					vector = raycastHit.point;
				}
				if (this.validPosition)
				{
					vector = this.ConstructableClass.PowerNode.pConnectionPoint.position;
					if (Vector3.Distance(this.startNode.pConnectionPoint.position, vector) > PowerLine.maxLineLength)
					{
						for (int i = 0; i < this.tempSegments.Count; i++)
						{
							this.tempSegments[i].gameObject.SetActive(false);
						}
						return;
					}
				}
				else
				{
					this.GhostModel.gameObject.SetActive(false);
				}
				if (this.hoveredPowerNode != null && PowerLine.CanNodesBeConnected(this.startNode, this.hoveredPowerNode))
				{
					vector = this.hoveredPowerNode.pConnectionPoint.position;
				}
				if (position == vector)
				{
					for (int j = 0; j < this.tempSegments.Count; j++)
					{
						this.tempSegments[j].gameObject.SetActive(false);
					}
					return;
				}
				PowerNode powerNode = this.GetHoveredPowerNode();
				int segmentCount = PowerLine.GetSegmentCount(position, vector);
				List<Transform> list = new List<Transform>();
				for (int k = 0; k < this.tempSegments.Count; k++)
				{
					if (k < segmentCount)
					{
						this.tempSegments[k].gameObject.SetActive(true);
						list.Add(this.tempSegments[k]);
					}
					else
					{
						this.tempSegments[k].gameObject.SetActive(false);
					}
				}
				PowerLine.DrawPowerLine(position, vector, list, this.LengthFactor);
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && powerNode != null && PowerLine.CanNodesBeConnected(this.startNode, powerNode))
				{
					this.CompletePowerLine(powerNode);
				}
			}
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000D560E File Offset: 0x000D380E
		public void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.startNode != null)
			{
				exit.Used = true;
				this.StopCreatingPowerLine();
			}
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000D5634 File Offset: 0x000D3834
		private PowerTower GetHoveredPowerTower()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(100f, out raycastHit, this.detectionMask, true, 0f))
			{
				if (raycastHit.collider.GetComponentInParent<PowerTower>() != null)
				{
					return raycastHit.collider.GetComponentInParent<PowerTower>();
				}
				if (raycastHit.collider.GetComponentInChildren<Tile>() != null)
				{
					Tile componentInChildren = raycastHit.collider.GetComponentInChildren<Tile>();
					if (componentInChildren.ConstructableOccupants.Count > 0 && componentInChildren.ConstructableOccupants[0] is PowerTower)
					{
						return componentInChildren.ConstructableOccupants[0] as PowerTower;
					}
				}
			}
			return null;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000D56D8 File Offset: 0x000D38D8
		protected PowerNode GetHoveredPowerNode()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(200f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f) && raycastHit.collider.GetComponentInParent<PowerNodeTag>())
			{
				return raycastHit.collider.GetComponentInParent<PowerNodeTag>().powerNode;
			}
			return null;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000D5738 File Offset: 0x000D3938
		protected override Constructable_GridBased PlaceNewConstructable()
		{
			Constructable_GridBased constructable_GridBased = base.PlaceNewConstructable();
			if (this.startNode != null && Vector3.Distance(this.startNode.pConnectionPoint.position, constructable_GridBased.PowerNode.pConnectionPoint.position) <= PowerLine.maxLineLength)
			{
				PowerLine c = Singleton<PowerManager>.Instance.CreatePowerLine(this.startNode, constructable_GridBased.PowerNode, Singleton<ConstructionManager>.Instance.currentProperty);
				if (Singleton<ConstructionManager>.Instance.onNewConstructableBuilt != null)
				{
					Singleton<ConstructionManager>.Instance.onNewConstructableBuilt(c);
				}
				this.StopCreatingPowerLine();
				this.startNode = constructable_GridBased.PowerNode;
			}
			return constructable_GridBased;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000D57D8 File Offset: 0x000D39D8
		private void CompletePowerLine(PowerNode target)
		{
			PowerLine c = Singleton<PowerManager>.Instance.CreatePowerLine(this.startNode, target, Singleton<ConstructionManager>.Instance.currentProperty);
			if (Singleton<ConstructionManager>.Instance.onNewConstructableBuilt != null)
			{
				Singleton<ConstructionManager>.Instance.onNewConstructableBuilt(c);
			}
			this.StopCreatingPowerLine();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.startNode = target;
				return;
			}
			this.startNode = null;
			Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000D5848 File Offset: 0x000D3A48
		private void StopCreatingPowerLine()
		{
			Singleton<HUD>.Instance.HideTopScreenText();
			this.startNode = null;
			for (int i = 0; i < this.tempSegments.Count; i++)
			{
				this.tempSegments[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x04002428 RID: 9256
		[Header("Materials")]
		public Material specialMat;

		// Token: 0x04002429 RID: 9257
		public Material powerLine_GhostMat;

		// Token: 0x0400242A RID: 9258
		[Header("References")]
		[SerializeField]
		protected GameObject cosmeticPowerNode;

		// Token: 0x0400242B RID: 9259
		public float LengthFactor = 1.002f;

		// Token: 0x0400242C RID: 9260
		protected Transform tempPowerLineContainer;

		// Token: 0x0400242D RID: 9261
		protected List<Transform> tempSegments = new List<Transform>();

		// Token: 0x0400242E RID: 9262
		private PowerNode hoveredPowerNode;

		// Token: 0x0400242F RID: 9263
		protected PowerNode startNode;

		// Token: 0x04002430 RID: 9264
		protected float powerLineInitialDistance;
	}
}
