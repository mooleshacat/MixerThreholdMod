using System;
using ScheduleOne.Interaction;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E7 RID: 2279
	public class StorageEntityInteractable : InteractableObject
	{
		// Token: 0x06003DBE RID: 15806 RVA: 0x00104A32 File Offset: 0x00102C32
		private void Awake()
		{
			this.StorageEntity = base.GetComponentInParent<StorageEntity>();
			this.MaxInteractionRange = this.StorageEntity.MaxAccessDistance;
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00104A51 File Offset: 0x00102C51
		public override void Hovered()
		{
			base.Hovered();
			base.SetInteractableState(this.StorageEntity.CanBeOpened() ? InteractableObject.EInteractableState.Default : InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00104A70 File Offset: 0x00102C70
		public override void StartInteract()
		{
			base.StartInteract();
			this.StorageEntity.Open();
		}

		// Token: 0x04002C14 RID: 11284
		private StorageEntity StorageEntity;
	}
}
