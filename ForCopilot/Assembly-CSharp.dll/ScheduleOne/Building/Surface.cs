using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007D2 RID: 2002
	public class Surface : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x000E47C3 File Offset: 0x000E29C3
		// (set) Token: 0x06003638 RID: 13880 RVA: 0x000E47CB File Offset: 0x000E29CB
		public Guid GUID { get; protected set; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x000E47D4 File Offset: 0x000E29D4
		public Transform Container
		{
			get
			{
				return this.ParentProperty.Container.transform;
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000E47E8 File Offset: 0x000E29E8
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000E480E File Offset: 0x000E2A0E
		private void OnValidate()
		{
			if (this.ParentProperty == null)
			{
				this.ParentProperty = base.GetComponentInParent<Property>();
			}
			if (string.IsNullOrEmpty(this.BakedGUID))
			{
				this.RegenerateGUID();
			}
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000E4840 File Offset: 0x000E2A40
		protected virtual void Awake()
		{
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(this.BakedGUID)))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is already registered! Bad.", this);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000E48C8 File Offset: 0x000E2AC8
		public Vector3 GetRelativePosition(Vector3 worldPosition)
		{
			return base.transform.InverseTransformPoint(worldPosition);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000E48D6 File Offset: 0x000E2AD6
		public Quaternion GetRelativeRotation(Quaternion worldRotation)
		{
			return Quaternion.Inverse(base.transform.rotation) * worldRotation;
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000E48EE File Offset: 0x000E2AEE
		public bool IsFrontFace(Vector3 point, Collider collider)
		{
			return collider.transform.InverseTransformPoint(point).z > 0f;
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000E4908 File Offset: 0x000E2B08
		public bool IsPointValid(Vector3 point, Collider hitCollider)
		{
			Vector3 b = Vector3.zero;
			if (hitCollider is BoxCollider)
			{
				b = (hitCollider as BoxCollider).center;
			}
			else if (hitCollider is MeshCollider)
			{
				b = (hitCollider as MeshCollider).sharedMesh.bounds.center;
			}
			Vector3 vector = hitCollider.transform.InverseTransformPoint(point) - b;
			using (List<Surface.EFace>.Enumerator enumerator = this.ValidFaces.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Surface.EFace.Front:
						if (vector.z >= 0f)
						{
							return true;
						}
						break;
					case Surface.EFace.Back:
						if (vector.z <= 0f)
						{
							return true;
						}
						break;
					case Surface.EFace.Top:
						if (vector.y >= 0f)
						{
							return true;
						}
						break;
					case Surface.EFace.Bottom:
						if (vector.y <= 0f)
						{
							return true;
						}
						break;
					case Surface.EFace.Left:
						if (vector.x <= 0f)
						{
							return true;
						}
						break;
					case Surface.EFace.Right:
						if (vector.x >= 0f)
						{
							return true;
						}
						break;
					}
				}
			}
			return false;
		}

		// Token: 0x04002669 RID: 9833
		[Header("Settings")]
		public Surface.ESurfaceType SurfaceType;

		// Token: 0x0400266A RID: 9834
		public List<Surface.EFace> ValidFaces = new List<Surface.EFace>
		{
			Surface.EFace.Front
		};

		// Token: 0x0400266B RID: 9835
		public Property ParentProperty;

		// Token: 0x0400266C RID: 9836
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x020007D3 RID: 2003
		public enum ESurfaceType
		{
			// Token: 0x0400266E RID: 9838
			Wall,
			// Token: 0x0400266F RID: 9839
			Roof
		}

		// Token: 0x020007D4 RID: 2004
		public enum EFace
		{
			// Token: 0x04002671 RID: 9841
			Front,
			// Token: 0x04002672 RID: 9842
			Back,
			// Token: 0x04002673 RID: 9843
			Top,
			// Token: 0x04002674 RID: 9844
			Bottom,
			// Token: 0x04002675 RID: 9845
			Left,
			// Token: 0x04002676 RID: 9846
			Right
		}
	}
}
