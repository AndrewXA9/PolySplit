using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Demo : MonoBehaviour {
	
	private bool ui = true;
	
	public List<Material> mats;

	private List<ShaderEdit> properties;

	public float uisize = 20f;
	public float nameOffset = 100f;
	public Texture2D pixel;
	
	private Vector3 mousePos;
	
	void Start () {
		mousePos = Vector3.zero;
		properties = new List<ShaderEdit>();
		foreach (Material i in mats){
			properties.Add(new ShaderEdit(i));
			Debug.Log(i.GetColor("_Color1").ToString()+", "+i.GetColor("_Color2").ToString());
		}
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			if(ui){
				ui = false;
			}
			else{
				ui = true;
			}
		}
		
		if(Input.mousePosition != mousePos || Input.GetMouseButtonDown(0)){
			for(int i=0; i<mats.Count; i++){
				properties[i].Apply();
			}
		}
		mousePos = Input.mousePosition;
	}
	
	void OnGUI() {
		if(ui){
			GUI.color = Color.black;
			GUI.Box (new Rect (uisize, uisize, nameOffset+100f + uisize*2f, (mats.Count+1f)*uisize), GUIContent.none);
			for(int i=0; i<mats.Count; i++) {
				GUI.color = Color.white;
				GUI.Label(new Rect(uisize,uisize*(i+1),nameOffset,uisize),mats[i].name);
				
				GUI.color = properties[i].color1;
				if(GUI.Button(new Rect(nameOffset+uisize,uisize*(i+1),uisize,uisize),GUIContent.none)){
					properties[i].color1 = RandColor();
				}
				GUI.DrawTexture(new Rect(nameOffset+uisize,uisize*(i+1),uisize,uisize),pixel,ScaleMode.ScaleToFit);
				
				GUI.color = properties[i].color2;
				if(GUI.Button(new Rect(nameOffset+uisize+(uisize),uisize*(i+1),uisize,uisize),GUIContent.none)){
					properties[i].color2 = RandColor();
				}
				GUI.DrawTexture(new Rect(nameOffset+uisize*2f,uisize*(i+1),uisize,uisize),pixel,ScaleMode.ScaleToFit);
				
				GUI.color = Color.white;
				properties[i].seed = GUI.HorizontalSlider(new Rect(nameOffset+uisize*3f,uisize*(i+1),100f,uisize),properties[i].seed,0f,1f);
			}
			if(GUI.Button(new Rect(uisize,uisize*(mats.Count+1),nameOffset+(uisize*2f)+100f,uisize),"Randomize")){
				for(int i=0; i<mats.Count; i++){
					properties[i].color1 = RandColor();
					properties[i].color2 = RandColor();
					properties[i].seed = Random.value;
					properties[i].Apply();
				}
			}
			if(GUI.Button(new Rect(uisize,uisize*(mats.Count+2),nameOffset+(uisize*2f)+100f,uisize),"Apply Changes")){
				for(int i=0; i<mats.Count; i++){
					properties[i].Apply();
				}
			}
			
			
			GUI.Box(new Rect(Screen.width-uisize-260f,uisize,260f,uisize*5f),GUIContent.none);
			GUI.color = Color.white;
			GUI.Label(new Rect(Screen.width-uisize-250f,uisize,250f,uisize*5f),
			          "-Click on colors to get a random new color\n"+
			          "-Modify seed value for different distribution\n"+
			          "-Or just press 'Randomize' for nonsesne\n"+
			          "-Space bar to hide UI");
			
		}
		
	}
	
	private Color RandColor(){
		return new Color(Random.value,Random.value,Random.value);
	}

}
	

[System.Serializable]
public class ShaderEdit{
	public Material mat;
	public Color color1;
	public Color color2;
	public float seed;
	public ShaderEdit(Material _material){
		mat = _material;
		color1 = _material.GetColor("_Color1");
		color2 = _material.GetColor("_Color2");
		seed = _material.GetFloat("_Seed");
	}
	public void Apply(){
		mat.SetColor("_Color1",color1);
		mat.SetColor("_Color2",color2);
		mat.SetFloat("_Seed",seed);
	}

}