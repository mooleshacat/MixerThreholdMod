using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005CA RID: 1482
	public class TransitRoute
	{
		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x0009547D File Offset: 0x0009367D
		// (set) Token: 0x06002479 RID: 9337 RVA: 0x00095485 File Offset: 0x00093685
		public ITransitEntity Source { get; protected set; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x0009548E File Offset: 0x0009368E
		// (set) Token: 0x0600247B RID: 9339 RVA: 0x00095496 File Offset: 0x00093696
		public ITransitEntity Destination { get; protected set; }

		// Token: 0x0600247C RID: 9340 RVA: 0x0009549F File Offset: 0x0009369F
		public TransitRoute(ITransitEntity source, ITransitEntity destination)
		{
			this.Source = source;
			this.Destination = destination;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000954DC File Offset: 0x000936DC
		public void Destroy()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			if (this.visuals != null)
			{
				UnityEngine.Object.Destroy(this.visuals.gameObject);
			}
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x00095530 File Offset: 0x00093730
		public void SetVisualsActive(bool active)
		{
			if (this.visuals == null)
			{
				this.visuals = UnityEngine.Object.Instantiate<GameObject>(Singleton<ManagementWorldspaceCanvas>.Instance.TransitRouteVisualsPrefab.gameObject, GameObject.Find("_Temp").transform).GetComponent<TransitLineVisuals>();
			}
			this.visuals.gameObject.SetActive(active);
			if (active)
			{
				this.Update();
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00095594 File Offset: 0x00093794
		private void Update()
		{
			this.ValidateEntities();
			if (this.visuals == null || !this.visuals.gameObject.activeSelf)
			{
				return;
			}
			if (this.Source == null || this.Destination == null)
			{
				this.visuals.gameObject.SetActive(false);
				return;
			}
			Vector3.Distance(this.Source.LinkOrigin.position, this.Destination.LinkOrigin.position);
			this.visuals.SetSourcePosition(this.Source.LinkOrigin.position);
			this.visuals.SetDestinationPosition(this.Destination.LinkOrigin.position);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00095646 File Offset: 0x00093846
		public virtual void SetSource(ITransitEntity source)
		{
			this.Source = source;
			if (this.onSourceChange != null)
			{
				this.onSourceChange(this.Source);
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00095668 File Offset: 0x00093868
		public bool AreEntitiesNonNull()
		{
			this.ValidateEntities();
			return this.Source != null && this.Destination != null;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00095683 File Offset: 0x00093883
		public virtual void SetDestination(ITransitEntity destination)
		{
			this.Destination = destination;
			if (this.onDestinationChange != null)
			{
				this.onDestinationChange(this.Destination);
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000956A5 File Offset: 0x000938A5
		private void ValidateEntities()
		{
			if (this.Source != null && this.Source.IsDestroyed)
			{
				this.SetSource(null);
			}
			if (this.Destination != null && this.Destination.IsDestroyed)
			{
				this.SetDestination(null);
			}
		}

		// Token: 0x04001B0D RID: 6925
		protected TransitLineVisuals visuals;

		// Token: 0x04001B0E RID: 6926
		public Action<ITransitEntity> onSourceChange;

		// Token: 0x04001B0F RID: 6927
		public Action<ITransitEntity> onDestinationChange;
	}
}
