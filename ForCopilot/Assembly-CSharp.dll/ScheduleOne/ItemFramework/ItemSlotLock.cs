using System;
using FishNet.Object;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000992 RID: 2450
	public class ItemSlotLock
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x00116A92 File Offset: 0x00114C92
		// (set) Token: 0x0600423F RID: 16959 RVA: 0x00116A9A File Offset: 0x00114C9A
		public ItemSlot Slot { get; protected set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x00116AA3 File Offset: 0x00114CA3
		// (set) Token: 0x06004241 RID: 16961 RVA: 0x00116AAB File Offset: 0x00114CAB
		public NetworkObject LockOwner { get; protected set; }

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06004242 RID: 16962 RVA: 0x00116AB4 File Offset: 0x00114CB4
		// (set) Token: 0x06004243 RID: 16963 RVA: 0x00116ABC File Offset: 0x00114CBC
		public string LockReason { get; protected set; } = "";

		// Token: 0x06004244 RID: 16964 RVA: 0x00116AC5 File Offset: 0x00114CC5
		public ItemSlotLock(ItemSlot slot, NetworkObject lockOwner, string lockReason)
		{
			this.Slot = slot;
			this.LockOwner = lockOwner;
			this.LockReason = lockReason;
		}
	}
}
