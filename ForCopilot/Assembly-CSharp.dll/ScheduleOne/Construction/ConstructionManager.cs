using System;
using FishNet.Object;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction.ConstructionMethods;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.UI.Construction;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Construction
{
	// Token: 0x0200075E RID: 1886
	public class ConstructionManager : Singleton<ConstructionManager>
	{
		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x000D34CF File Offset: 0x000D16CF
		// (set) Token: 0x060032CA RID: 13002 RVA: 0x000D34D7 File Offset: 0x000D16D7
		public bool constructionModeEnabled { get; protected set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x000D34E0 File Offset: 0x000D16E0
		// (set) Token: 0x060032CC RID: 13004 RVA: 0x000D34E8 File Offset: 0x000D16E8
		public bool isDeployingConstructable { get; protected set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000D34F1 File Offset: 0x000D16F1
		// (set) Token: 0x060032CE RID: 13006 RVA: 0x000D34F9 File Offset: 0x000D16F9
		public bool isMovingConstructable { get; protected set; }

		// Token: 0x060032CF RID: 13007 RVA: 0x000D3502 File Offset: 0x000D1702
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000D351C File Offset: 0x000D171C
		public void EnterConstructionMode(Property prop)
		{
			this.currentProperty = prop;
			this.constructionModeEnabled = true;
			prop.SetBoundsVisible(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			if (this.onConstructionModeEnabled != null)
			{
				this.onConstructionModeEnabled();
			}
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000D3574 File Offset: 0x000D1774
		public void ExitConstructionMode()
		{
			this.currentProperty.SetBoundsVisible(false);
			this.constructionModeEnabled = false;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<BirdsEyeView>.Instance.Disable(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			this.currentProperty = null;
			if (this.onConstructionModeDisabled != null)
			{
				this.onConstructionModeDisabled();
			}
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000D35DC File Offset: 0x000D17DC
		public void DeployConstructable(ConstructionMenu.ConstructionMenuListing listing)
		{
			this.isDeployingConstructable = true;
			if (Registry.GetConstructable(listing.ID)._constructionHandler_Asset != null)
			{
				this.constructHandler = UnityEngine.Object.Instantiate<GameObject>(Registry.GetConstructable(listing.ID)._constructionHandler_Asset, base.transform);
				this.constructHandler.GetComponent<ConstructStart_Base>().StartConstruction(listing.ID, null);
				return;
			}
			Console.LogWarning("Constructable doesn't have a construction handler!", null);
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000D364C File Offset: 0x000D184C
		public void StopConstructableDeploy()
		{
			this.isDeployingConstructable = false;
			this.constructHandler.GetComponent<ConstructStop_Base>().StopConstruction();
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000D3668 File Offset: 0x000D1868
		public void MoveConstructable(Constructable_GridBased c)
		{
			this.isMovingConstructable = true;
			if (c._constructionHandler_Asset != null)
			{
				this.constructHandler = UnityEngine.Object.Instantiate<GameObject>(c._constructionHandler_Asset, base.transform);
				this.constructHandler.GetComponent<ConstructStart_Base>().StartConstruction(c.PrefabID, c);
				return;
			}
			Console.LogWarning("Constructable doesn't have a construction handler!", null);
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000D36C4 File Offset: 0x000D18C4
		public void StopMovingConstructable()
		{
			this.isMovingConstructable = false;
			this.constructHandler.GetComponent<ConstructStop_Base>().StopConstruction();
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000D36E0 File Offset: 0x000D18E0
		private void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.constructionModeEnabled)
			{
				if (this.isDeployingConstructable)
				{
					exit.Used = true;
					Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
					return;
				}
				if (this.isMovingConstructable)
				{
					exit.Used = true;
					this.StopMovingConstructable();
					return;
				}
				if (exit.exitType == ExitType.Escape)
				{
					exit.Used = true;
					this.ExitConstructionMode();
				}
			}
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000D3744 File Offset: 0x000D1944
		public Constructable_GridBased CreateConstructable_GridBased(string ID, Grid grid, Vector2 originCoordinate, float rotation)
		{
			Constructable_GridBased component = UnityEngine.Object.Instantiate<GameObject>(Registry.GetPrefab(ID), null).GetComponent<Constructable_GridBased>();
			component.InitializeConstructable_GridBased(grid, originCoordinate, rotation);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000D3789 File Offset: 0x000D1989
		public Constructable CreateConstructable(string prefabID)
		{
			return UnityEngine.Object.Instantiate<GameObject>(Registry.GetPrefab(prefabID), null).GetComponent<Constructable>();
		}

		// Token: 0x040023DC RID: 9180
		public NetworkObject networkObject;

		// Token: 0x040023DE RID: 9182
		public Action onConstructionModeEnabled;

		// Token: 0x040023DF RID: 9183
		public Action onConstructionModeDisabled;

		// Token: 0x040023E1 RID: 9185
		public GameObject constructHandler;

		// Token: 0x040023E3 RID: 9187
		public ConstructionManager.ConstructableNotification onNewConstructableBuilt;

		// Token: 0x040023E4 RID: 9188
		public ConstructionManager.ConstructableNotification onConstructableMoved;

		// Token: 0x040023E5 RID: 9189
		public Property currentProperty;

		// Token: 0x0200075F RID: 1887
		public class WorldIntersection
		{
			// Token: 0x040023E6 RID: 9190
			public FootprintTile footprint;

			// Token: 0x040023E7 RID: 9191
			public Tile tile;
		}

		// Token: 0x02000760 RID: 1888
		// (Invoke) Token: 0x060032DC RID: 13020
		public delegate void ConstructableNotification(Constructable c);
	}
}
