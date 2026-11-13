using UnityEngine;

public class PlayerParticle : PoolableBehavior
{
	public SpriteRenderer myRend;

	public float maxDistance = 6f;

	public Vector2 velocity;

	public float angularVelocity;

	private Vector3 lastCamera;

	public Camera FollowCamera { get; set; }

	public unsafe void Update()
	{
		Vector3 localPosition = transform.localPosition;
		float sqrMagnitude = localPosition.sqrMagnitude;
		if ((bool)FollowCamera)
		{
			Vector3 position = FollowCamera.transform.position;
			position.z = 0f;
			localPosition += (position - lastCamera) * (1f - transform.localScale.x);
			lastCamera = position;
			sqrMagnitude = ((Vector2)(localPosition - position)).sqrMagnitude;
		}
		if (sqrMagnitude > maxDistance * maxDistance)
		{
			OwnerPool.Reclaim(this);
			return;
		}
		localPosition += (Vector3)(velocity * Time.deltaTime);
		transform.localPosition = localPosition;
		transform.Rotate(0f, 0f, Time.deltaTime * angularVelocity);
	}
}
