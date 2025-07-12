using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.UI;
using ScheduleOne.UI.Construction;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x02000770 RID: 1904
	public class ConstructUpdate_PowerLine : ConstructUpdate_Base
	{
		// Token: 0x0600333F RID: 13119 RVA: 0x000D4BD8 File Offset: 0x000D2DD8
		protected virtual void Start()
		{
			this.tempPowerLineContainer = new GameObject("TempPowerLine").transform;
			this.tempPowerLineContainer.SetParent(base.transform);
			for (int i = 0; i < PowerLine.powerLine_MaxSegments; i++)
			{
				Transform transform = UnityEngine.Object.Instantiate<GameObject>(Singleton<PowerManager>.Instance.powerLineSegmentPrefab, this.tempPowerLineContainer).transform;
				transform.Find("Model").GetComponent<MeshRenderer>().material = this.ghostPowerLine_Material;
				transform.gameObject.SetActive(false);
				this.tempSegments.Add(transform);
			}
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000D4C7B File Offset: 0x000D2E7B
		public override void ConstructionStop()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			Singleton<HUD>.Instance.HideTopScreenText();
			base.ConstructionStop();
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000D4C9E File Offset: 0x000D2E9E
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

		// Token: 0x06003342 RID: 13122 RVA: 0x000D4CC4 File Offset: 0x000D2EC4
		protected override void Update()
		{
			base.Update();
			this.cosmeticPowerNode.SetActive(false);
			this.hoveredPowerNode = this.GetHoveredPowerNode();
			if (this.startNode == null)
			{
				Singleton<HUD>.Instance.ShowTopScreenText("Choose start point");
				if (this.hoveredPowerNode != null)
				{
					this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
					this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
					this.cosmeticPowerNode.gameObject.SetActive(true);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						this.startNode = this.hoveredPowerNode;
						this.powerLineInitialDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.startNode.pConnectionPoint.transform.position);
						return;
					}
				}
			}
			else
			{
				Singleton<HUD>.Instance.ShowTopScreenText("Choose end point");
				if (this.hoveredPowerNode != null && PowerLine.CanNodesBeConnected(this.startNode, this.hoveredPowerNode))
				{
					this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
					this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
					this.cosmeticPowerNode.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000D4E3C File Offset: 0x000D303C
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.startNode != null)
			{
				Vector3 position = this.startNode.pConnectionPoint.transform.position;
				Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.powerLineInitialDistance));
				RaycastHit raycastHit;
				if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(this.powerLineInitialDistance, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
				{
					vector = raycastHit.point;
				}
				Vector3 vector2 = vector - position;
				vector2 = Vector3.ClampMagnitude(vector2, PowerLine.maxLineLength);
				vector = position + vector2;
				PowerNode powerNode = this.GetHoveredPowerNode();
				if (powerNode != null && PowerLine.CanNodesBeConnected(this.startNode, powerNode))
				{
					vector = this.GetHoveredPowerNode().pConnectionPoint.transform.position;
				}
				int segmentCount = PowerLine.GetSegmentCount(position, vector);
				List<Transform> list = new List<Transform>();
				for (int i = 0; i < this.tempSegments.Count; i++)
				{
					if (i < segmentCount)
					{
						this.tempSegments[i].gameObject.SetActive(true);
						list.Add(this.tempSegments[i]);
					}
					else
					{
						this.tempSegments[i].gameObject.SetActive(false);
					}
				}
				PowerLine.DrawPowerLine(position, vector, list, 1.002f);
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && powerNode != null && PowerLine.CanNodesBeConnected(this.startNode, powerNode) && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= Singleton<ConstructionMenu>.Instance.GetListingPrice("Utilities/PowerLine/PowerLine"))
				{
					this.CompletePowerLine(powerNode);
				}
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000D4FF8 File Offset: 0x000D31F8
		protected PowerNode GetHoveredPowerNode()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(200f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f) && raycastHit.collider.GetComponentInParent<PowerNodeTag>())
			{
				return raycastHit.collider.GetComponentInParent<PowerNodeTag>().powerNode;
			}
			return null;
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000D5058 File Offset: 0x000D3258
		private void CompletePowerLine(PowerNode target)
		{
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Power Line", -Singleton<ConstructionMenu>.Instance.GetListingPrice("Utilities/PowerLine/PowerLine"), 1f, string.Empty);
			PowerLine c = Singleton<PowerManager>.Instance.CreatePowerLine(this.startNode, target, Singleton<ConstructionManager>.Instance.currentProperty);
			if (Singleton<ConstructionManager>.Instance.onNewConstructableBuilt != null)
			{
				Singleton<ConstructionManager>.Instance.onNewConstructableBuilt(c);
			}
			this.StopCreatingPowerLine();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.startNode = target;
				if (this.startNode != null)
				{
					this.powerLineInitialDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, target.pConnectionPoint.transform.position);
				}
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000D5118 File Offset: 0x000D3318
		private void StopCreatingPowerLine()
		{
			this.startNode = null;
			for (int i = 0; i < this.tempSegments.Count; i++)
			{
				this.tempSegments[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x04002421 RID: 9249
		[Header("Settings")]
		[SerializeField]
		protected Material ghostPowerLine_Material;

		// Token: 0x04002422 RID: 9250
		[Header("References")]
		[SerializeField]
		protected GameObject cosmeticPowerNode;

		// Token: 0x04002423 RID: 9251
		protected Transform tempPowerLineContainer;

		// Token: 0x04002424 RID: 9252
		protected PowerNode hoveredPowerNode;

		// Token: 0x04002425 RID: 9253
		protected List<Transform> tempSegments = new List<Transform>();

		// Token: 0x04002426 RID: 9254
		protected PowerNode startNode;

		// Token: 0x04002427 RID: 9255
		protected float powerLineInitialDistance;
	}
}
