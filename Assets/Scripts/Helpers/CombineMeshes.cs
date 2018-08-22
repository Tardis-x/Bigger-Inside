using UnityEngine;
using UnityEditor;

namespace ua.org.gdg.devfest
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]

	public class CombineMeshes : MonoBehaviour
	{

		GameObject highlight;

		void Start()
		{
			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			int i = 0;
			while (i < meshFilters.Length)
			{
				Debug.Log(meshFilters[i].gameObject.transform.name);
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				i++;
			}



			transform.GetComponent<MeshFilter>().mesh = new Mesh();
			transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			transform.gameObject.active = true;

			saveMesh();
		}

		void saveMesh()
		{
			Debug.Log("Saving Mesh?");
			Mesh m1 = transform.GetComponent<MeshFilter>().mesh;
			AssetDatabase.CreateAsset(m1, "Assets/Meshes/" + transform.name + ".asset");
			AssetDatabase.SaveAssets();
		}
	}
}