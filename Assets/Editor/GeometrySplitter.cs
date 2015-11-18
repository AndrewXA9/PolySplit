using UnityEngine;
using UnityEditor;
using System.Collections;

//all stuff written by Andrew Horobin pls don't steal
//pretty much this reads in geometry and splits the polygons
public class GeometrySplitter {

	//add this to the gameobject menu, Shift+B for shortcut
	//must have an object with a mesh filter selected in the editor
	[MenuItem ("GameObject/SplitGeometry #b")]
	static void Split() {

		//get the selected gameobject
		GameObject input = Selection.activeGameObject;
		
		//get the mesh of the selected object and get the mesh information
		Mesh mesh = input.GetComponent<MeshFilter>().sharedMesh;
		int[] tris = mesh.triangles;
		Vector3[] verts = mesh.vertices;
		Vector3[] norms = mesh.normals;
	 	Color[] cols = mesh.colors;
	 	
		//create the new mesh
	 	Mesh newmesh = new Mesh();
	 	
	 	//name the mesh
	 	newmesh.name = mesh.name+"_trisplit";

		//iterate through every third vertex
		for(int i=0;i<tris.Length;i+=3){

			//iterate over the next 3 vetices, add them to the new mesh, and build a new tri from those 3;
			int[] newtri = new int[0];
			for(int j=0;j<3;j++){
				newmesh.vertices = AppendValue(newmesh.vertices,mesh.vertices[mesh.triangles[i+j]]);
				newtri = AppendValue(newtri,i+j);

			}

			//append the new tri
			//this must happen AFTER all 3 verticies are added
			newmesh.triangles = AppendValue(newmesh.triangles,newtri);
		}

		//assign a new vertex color to all 3 verticies of each tri
		Color[] newcols = new Color[newmesh.vertices.Length];
		for(int i=0;i<newcols.Length;i+=3){
			Color tricolor = RandoColor();
			for(int j=0;j<3;j++){
				newcols[i+j] = tricolor;
			}
		}
		newmesh.colors = newcols;

		//recalcualte normals
		newmesh.RecalculateNormals ();
	 	
	 	//create split folder if not there
		if(!AssetDatabase.IsValidFolder("Assets/Split")){
			AssetDatabase.CreateFolder("Assets","Split");
		}
	 	
		//save new model data to split folder
		AssetDatabase.CreateAsset(newmesh,"Assets/Split/"+newmesh.name+".asset");
		AssetDatabase.SaveAssets();
		
		//assign new mesh to object
		input.GetComponent<MeshFilter>().mesh = (Mesh)EditorGUIUtility.Load("Assets/Split/"+newmesh.name+".asset");
	 	
	}


	//A bunch of helper functinos for appending various things to various kinds of lists

	//append vector 3 to list of vector3s
	public static Vector3[] AppendValue(Vector3[] _input, Vector3 _addition){
		
		Vector3[] tArray = new Vector3[_input.Length+1];
		for(int i=0;i<_input.Length;i++){
		//Debug.Log(i.ToString()+" of "+(tArray.Length-1).ToString());
			tArray[i] = _input[i];
		}
		tArray[_input.Length] = _addition;
		
		return tArray;
	
	}

	//append Color to list of Colors
	public static Color[] AppendValue(Color[] _input, Color _addition){
		
		Color[] tArray = new Color[_input.Length+1];
		for(int i=0;i<_input.Length;i++){
			tArray[i] = _input[i];
		}
		tArray[_input.Length] = _addition;
		
		return tArray;
	}
	//append list of Colors to list of Colors
	public static Color[] AppendValue(Color[] _input, Color[] _addition){
		
		Color[] tArray = new Color[_input.Length+_addition.Length];
		for(int i=0;i<_input.Length;i++){
			tArray[i] = _input[i];
		}
		for(int i=0;i<_addition.Length;i++){
			tArray[_input.Length+i] = _addition[i];
		}
		
		return tArray;
	}

	//append int to list of ints
	public static int[] AppendValue(int[] _input, int _addition){
		
		int[] tArray = new int[_input.Length+1];
		for(int i=0;i<_input.Length;i++){
			tArray[i] = _input[i];
		}
		tArray[_input.Length] = _addition;
		
		return tArray;
	}
	//append list of ints to list of ints
	public static int[] AppendValue(int[] _input, int[] _addition){
		
		int[] tArray = new int[_input.Length+_addition.Length];
		for(int i=0;i<_input.Length;i++){
			tArray[i] = _input[i];
		}
		for(int i=0;i<_addition.Length;i++){
			tArray[_input.Length+i] = _addition[i];
		}
		
		return tArray;
	}

	//Generate a random color
	public static Color RandoColor(){
		return new Color(Random.value%1,Random.value%1,Random.value%1);
	}
	
	
}
