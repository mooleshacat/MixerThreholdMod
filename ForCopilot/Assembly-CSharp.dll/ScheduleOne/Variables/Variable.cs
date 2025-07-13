using System;
using FishNet.Connection;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Quests;
using UnityEngine.Events;

namespace ScheduleOne.Variables
{
	// Token: 0x0200029F RID: 671
	public class Variable<T> : BaseVariable
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x0003D863 File Offset: 0x0003BA63
		public Variable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, T value) : base(name, replicationMode, persistent, mode, owner)
		{
			this.Value = value;
			this.ReplicateValue(null);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003D88C File Offset: 0x0003BA8C
		public override object GetValue()
		{
			return this.Value;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003D89C File Offset: 0x0003BA9C
		public override void SetValue(object value, bool replicate)
		{
			if (value is string)
			{
				T t;
				if (this.TryDeserialize((string)value, out t))
				{
					value = t;
				}
				else
				{
					string[] array = new string[6];
					array[0] = "Failed to deserialize value '";
					int num = 1;
					T t2 = t;
					array[num] = ((t2 != null) ? t2.ToString() : null);
					array[2] = "' for variable ";
					array[3] = this.Name;
					array[4] = " of type ";
					array[5] = typeof(T).Name;
					Console.LogWarning(string.Concat(array), null);
				}
			}
			this.Value = (T)((object)value);
			if (replicate)
			{
				this.ReplicateValue(null);
			}
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged.Invoke(this.Value);
			}
			StateMachine.ChangeState();
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0003D960 File Offset: 0x0003BB60
		public virtual bool TryDeserialize(string valueString, out T value)
		{
			value = default(T);
			return false;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0003D96C File Offset: 0x0003BB6C
		public override void ReplicateValue(NetworkConnection conn)
		{
			if (this.VariableMode == EVariableMode.Global)
			{
				NetworkSingleton<VariableDatabase>.Instance.SendValue(conn, this.Name, this.Value.ToString());
				return;
			}
			if (base.Owner.IsOwner)
			{
				Player.Local.SendValue(this.Name, this.Value.ToString(), false);
				return;
			}
			base.Owner.SendValue(this.Name, this.Value.ToString(), true);
		}

		// Token: 0x04000E5A RID: 3674
		public T Value;

		// Token: 0x04000E5B RID: 3675
		public UnityEvent<T> OnValueChanged = new UnityEvent<T>();
	}
}
