using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x0600018F RID: 399 RVA: 0x000085DB File Offset: 0x000067DB
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000190 RID: 400 RVA: 0x000085DB File Offset: 0x000067DB
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x000085E4 File Offset: 0x000067E4
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00008714 File Offset: 0x00006914
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening);
			base.transform.rotation = this.rotation;
			this.idleTimer = 0f;
			this.idleSmooth = 0f;
		}
		else
		{
			this.idleTimer += 0.02f;
			if (this.idleTimer > this.autoRotate && this.autoRotate > 0f)
			{
				this.idleSmooth += (0.02f + this.idleSmooth) * 0.005f;
				this.idleSmooth = Mathf.Clamp(this.idleSmooth, 0f, 1f);
				this.xDeg += this.xSpeed * Time.deltaTime * this.idleSmooth * this.autoRotateSpeed;
			}
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening * 2f);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, 0.02f * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00008A3D File Offset: 0x00006C3D
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x04000157 RID: 343
	public Transform target;

	// Token: 0x04000158 RID: 344
	public Vector3 targetOffset;

	// Token: 0x04000159 RID: 345
	public float distance = 5f;

	// Token: 0x0400015A RID: 346
	public float maxDistance = 20f;

	// Token: 0x0400015B RID: 347
	public float minDistance = 0.6f;

	// Token: 0x0400015C RID: 348
	public float xSpeed = 200f;

	// Token: 0x0400015D RID: 349
	public float ySpeed = 200f;

	// Token: 0x0400015E RID: 350
	public int yMinLimit = -80;

	// Token: 0x0400015F RID: 351
	public int yMaxLimit = 80;

	// Token: 0x04000160 RID: 352
	public int zoomRate = 40;

	// Token: 0x04000161 RID: 353
	public float panSpeed = 0.3f;

	// Token: 0x04000162 RID: 354
	public float zoomDampening = 5f;

	// Token: 0x04000163 RID: 355
	public float autoRotate = 1f;

	// Token: 0x04000164 RID: 356
	public float autoRotateSpeed = 0.1f;

	// Token: 0x04000165 RID: 357
	private float xDeg;

	// Token: 0x04000166 RID: 358
	private float yDeg;

	// Token: 0x04000167 RID: 359
	private float currentDistance;

	// Token: 0x04000168 RID: 360
	private float desiredDistance;

	// Token: 0x04000169 RID: 361
	private Quaternion currentRotation;

	// Token: 0x0400016A RID: 362
	private Quaternion desiredRotation;

	// Token: 0x0400016B RID: 363
	private Quaternion rotation;

	// Token: 0x0400016C RID: 364
	private Vector3 position;

	// Token: 0x0400016D RID: 365
	private float idleTimer;

	// Token: 0x0400016E RID: 366
	private float idleSmooth;
}
