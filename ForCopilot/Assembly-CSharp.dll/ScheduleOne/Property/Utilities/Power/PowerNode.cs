using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Power
{
	// Token: 0x02000863 RID: 2147
	public class PowerNode : MonoBehaviour
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x000F7A3C File Offset: 0x000F5C3C
		public Transform pConnectionPoint
		{
			get
			{
				return this.connectionPoint;
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000F7A44 File Offset: 0x000F5C44
		public bool IsConnectedTo(PowerNode node)
		{
			for (int i = 0; i < this.connections.Count; i++)
			{
				if (this.connections[i].nodeA == this)
				{
					if (this.connections[i].nodeB == node)
					{
						return true;
					}
				}
				else if (this.connections[i].nodeA == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000F7AB8 File Offset: 0x000F5CB8
		public void RecalculatePowerNetwork()
		{
			List<PowerNode> connectedNodes = this.GetConnectedNodes(new List<PowerNode>());
			bool flag = false;
			using (List<PowerNode>.Enumerator enumerator = connectedNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.poweredNode)
					{
						flag = true;
					}
				}
			}
			foreach (PowerNode powerNode in connectedNodes)
			{
				if (flag)
				{
					powerNode.isConnectedToPower = true;
				}
				else
				{
					powerNode.isConnectedToPower = false;
				}
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000F7B60 File Offset: 0x000F5D60
		public List<PowerNode> GetConnectedNodes(List<PowerNode> exclusions)
		{
			List<PowerNode> list = new List<PowerNode>();
			list.Add(this);
			exclusions.Add(this);
			for (int i = 0; i < this.connections.Count; i++)
			{
				if (!exclusions.Contains(this.connections[i].GetOtherNode(this)))
				{
					List<PowerNode> connectedNodes = this.connections[i].GetOtherNode(this).GetConnectedNodes(exclusions);
					exclusions.AddRange(connectedNodes);
					list.AddRange(connectedNodes);
				}
			}
			return list;
		}

		// Token: 0x04002A04 RID: 10756
		public bool poweredNode;

		// Token: 0x04002A05 RID: 10757
		public bool consumptionNode;

		// Token: 0x04002A06 RID: 10758
		public bool isConnectedToPower;

		// Token: 0x04002A07 RID: 10759
		[Header("References")]
		[SerializeField]
		protected Transform connectionPoint;

		// Token: 0x04002A08 RID: 10760
		public List<PowerLine> connections = new List<PowerLine>();
	}
}
