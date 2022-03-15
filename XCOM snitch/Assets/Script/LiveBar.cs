using UnityEngine;
using System.Collections;

public class LiveBar : MonoBehaviour {

	public bool showBar = true;
	public bool actionOne = true;
	public bool actionTwo = true;
	public int currentAction = 2;
	public int totalLive = 100;
	public int currentLive = 100;
	public int barWidth = 140;
	public int actWidth = 85;
	public int barHeight = 40;
	public float yDelta=1.4f;
	public Color gradientTop=new Color(170f/255f, 255f/255f, 168f/255f, 0.8f);
	public Color gradientBottom=new Color(3f/255f, 80f/255f, 15f/255f, 0.8f);
	public Color topLeftBorder=new Color(0f, 0f, 0f, 0.8f);
	public Color bottomRightBorder=new Color(0f, 0f, 0f, 0.6f);

	public bool showText = false;
	public Color labelColor=Color.red;	//Text color
	public bool outlined=false;				//Set true for display outlined text
	public Color outlineColor=Color.black;	//Outline text color
	public Font textFont = null;			//Text font
	public int fontSize=18;					//Text size
	public FontStyle fontStyle=FontStyle.Bold; //Font style

	public Texture2D bar;
	public Texture2D bar75;
	public Texture2D bar50;
	public Texture2D bar25;
	public Texture2D bar0;
	public Texture2D actionTexture;
	public Texture2D actionTextureOFF;
	private GUIStyle style;

	//Use this public functions to control the live bar
	//Full compatible with PlayMaker event system

	//Show the live bar
	public void showLiveBar() {
		showBar = true;
	}

	//Hides live bar
	public void hideLiveBar() {
		showBar = false;
	}
	//Show text in liver bar
	public void showTextBar() {
		showText = true;
	}

	//Hides text in live bar
	public void hideTextBar() {
		showText = false;
	}

	//Set live on percentage
	//Values are clipped between 0 and totalLive
	public void setLiveInPercentage(float percenge) {
		currentLive = (int)((float)totalLive * percenge / 100.0f);
		if(currentLive>totalLive)
			currentLive=totalLive;
		if(currentLive<0)
			currentLive=0;
	}

	//Set live in absolute value
	//Values are clipped between 0 and totalLive
	public void setLive(int live) {
		currentLive = live;
		if(currentLive>totalLive)
			currentLive=totalLive;
		if(currentLive<0)
			currentLive=0;
	}

	//Decreases live in an absolute value
	public void decreaseLive(int liveToDecrease) {
		currentLive -= liveToDecrease;
		if(currentLive<0)
			currentLive=0;
	}

	//Decreases live in an percentage value
	public void decreaseLiveInPercentage(float percengeToDecrease) {
		int liveToDecrease = (int)((float)totalLive * percengeToDecrease / 100.0f);
		decreaseLive(liveToDecrease);
	}

	//Increases live in an absolute value
	public void increaseLive(int liveToIncrease) {
		currentLive += liveToIncrease;
		if(currentLive>totalLive)
			currentLive=totalLive;
	}
	
	//Increases live in an percentage value
	public void increaseLiveInPercentage(float percengeToIncrease) {
		int liveToIncrease = (int)((float)totalLive * percengeToIncrease / 100.0f);
		increaseLive(liveToIncrease);
	}

	//Get current live
	public int getCurrentLive() {
		return currentLive;
	}

	//Set total live value
	public void setTotalLive(int total) {
		totalLive = total;
	}

	//Get total live value
	public int getTotalLive() {
		return totalLive;
	}

	void Awake() {

		style = new GUIStyle();
		style.normal.textColor = labelColor;  
		style.alignment = TextAnchor.UpperCenter;
		style.wordWrap = true;
		style.fontSize = fontSize;
		style.fontStyle = fontStyle;
		if(textFont!=null)
			style.font=textFont;
		else
			style.font= (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
	}


	void OnGUI() {
		Vector3 posDelta = transform.position;
		posDelta.y += yDelta;
		Vector3 pos = UnityEngine.Camera.main.WorldToScreenPoint(posDelta);
		pos.y = Screen.height - pos.y;

		Rect posBar = new Rect(pos.x-barWidth/2, pos.y, barWidth, barHeight);
		Rect posAcOne = new Rect(pos.x-actWidth/2 + actWidth, pos.y, actWidth, barHeight);
		Rect posAcTwo = new Rect(pos.x-actWidth/2 + actWidth + actWidth/4, pos.y, actWidth, barHeight);
		if (currentAction == 2)
		{
			actionOne = true;
			actionTwo = true;
		}
		else if (currentAction == 1)
		{
			actionOne = true;
			actionTwo = false;
		}
		else if (currentAction == 0)
		{
			actionOne = false;
			actionTwo = false;
		}
		
		if (actionOne)
		{
			GUI.DrawTexture(posAcOne, actionTexture);
		}
		if (!actionOne)
		{
			GUI.DrawTexture(posAcOne, actionTextureOFF);
		}
		if (actionTwo)
		{
			GUI.DrawTexture(posAcTwo, actionTexture);
		}
		if (!actionTwo)
		{
			GUI.DrawTexture(posAcTwo, actionTextureOFF);
		}
		if(currentLive == 100)
		{
			showBar = true;
			GUI.DrawTexture(posBar, bar);
		}
		if(currentLive == 75)
		{
			showBar = true;
			GUI.DrawTexture(posBar, bar75);
		}
		if(currentLive == 50)
		{
			showBar = true;
			GUI.DrawTexture(posBar, bar50);
		}
		if(currentLive == 25)
		{
			showBar = true;
			GUI.DrawTexture(posBar, bar25);
		}
		if(currentLive == 0)
		{
			GUI.DrawTexture(posBar, bar0);
		}
		if (showText) {
			string labelToDisplay=currentLive+" / "+totalLive;
			if (outlined)
				DrawOutline (new Rect(posBar.x,posBar.y-(fontSize+2),barWidth,60), labelToDisplay, style, outlineColor, labelColor);
			else
				GUI.Label ( new Rect(posBar.x,posBar.y-(fontSize+2),barWidth,60), labelToDisplay, style);
		}
	}

	//draw text of a specified color, with a specified outline color
	void DrawOutline(Rect position, string text, GUIStyle theStyle, Color outColor, Color inColor){
		var backupStyle = theStyle;
		theStyle.normal.textColor = outColor;
		position.x--;
		GUI.Label(position, text, style);
		position.x +=2;
		GUI.Label(position, text, style);
		position.x--;
		position.y--;
		GUI.Label(position, text, style);
		position.y +=2;
		GUI.Label(position, text, style);
		position.y--;
		theStyle.normal.textColor = inColor;
		GUI.Label(position, text, style);
		theStyle = backupStyle;
	}

}
