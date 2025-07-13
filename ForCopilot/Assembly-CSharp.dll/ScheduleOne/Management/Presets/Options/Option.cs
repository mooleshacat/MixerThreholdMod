using System;

namespace ScheduleOne.Management.Presets.Options
{
	// Token: 0x020005D8 RID: 1496
	public abstract class Option
	{
		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060024C1 RID: 9409 RVA: 0x00095FF5 File Offset: 0x000941F5
		// (set) Token: 0x060024C2 RID: 9410 RVA: 0x00095FFD File Offset: 0x000941FD
		public string Name { get; protected set; } = "OptionName";

		// Token: 0x060024C3 RID: 9411 RVA: 0x00096006 File Offset: 0x00094206
		public Option(string name)
		{
			this.Name = name;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00096020 File Offset: 0x00094220
		public virtual void CopyTo(Option other)
		{
			other.Name = this.Name;
		}

		// Token: 0x060024C5 RID: 9413
		public abstract string GetDisplayString();
	}
}
