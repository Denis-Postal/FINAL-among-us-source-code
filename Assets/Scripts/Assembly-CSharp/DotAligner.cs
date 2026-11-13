using Object = UnityEngine.Object;
using UnityEngine;

public class DotAligner : MonoBehaviour
{
	public float Width = 2f;

	public bool Even;

	public void Start()
	{
		Align(transform, Width, Even);
	}

	public unsafe static void Align(Transform target, float width, bool even)
	{
		int num = 0;
		for (int i = 0; i < target.childCount; i++)
		{
			if (target.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		float num2;
		float num3;
		if (even)
		{
			num2 = (0f - width) * (float)(num - 1) / 2f;
			num3 = width;
		}
		else if (num > 1)
		{
			num2 = (0f - width) / 2f;
			num3 = width / (float)(num - 1);
		}
		else
		{
			num2 = 0f;
			num3 = 0f;
		}
		int num4 = 0;
		for (int j = 0; j < target.childCount; j++)
		{
			Transform child = target.GetChild(j);
			if (child.gameObject.activeSelf)
			{
				child.transform.localPosition = new Vector3(num2 + (float)num4 * num3, 0f, 0f);
				num4++;
			}
		}
	}
}
