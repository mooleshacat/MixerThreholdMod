using System;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097E RID: 2430
	public class ItemFilter_LegalStatus : ItemFilter
	{
		// Token: 0x06004198 RID: 16792 RVA: 0x00114C83 File Offset: 0x00112E83
		public ItemFilter_LegalStatus(ELegalStatus requiredLegalStatus)
		{
			this.RequiredLegalStatus = requiredLegalStatus;
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x00114C92 File Offset: 0x00112E92
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return instance != null && instance.Definition.legalStatus == this.RequiredLegalStatus && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002EC7 RID: 11975
		public ELegalStatus RequiredLegalStatus;
	}
}
