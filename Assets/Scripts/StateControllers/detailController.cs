using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class detailController : baseController
{
	//singleton
	public static detailController instance = null;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		
		likeThis.onClick.AddListener(likeThisOne);
		noInterest.onClick.AddListener(noInterestInThis);
	}
	//singleton

	
	public GameObject detailField;
	public movieInfo myInfo;
	public Text descriptions;
	public Text title;
	public RawImage poster;
	public Image scorePortion;
	public Text xRates;
	public Text score;
	public Button likeThis;
	public Button noInterest;

	public override void enterState()
	{
		//do the registration here? on the main controller for current state
		base.enterState();
		//switch to location panel
		detailField.SetActive(true);
		updateDetails();
	}

	public override void inputEventHandler()
	{
		//get the input event data and parse it to responses

	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		detailField.SetActive(false);
	}

	/*-----------------LayOUt--------------------------
	*	movieTitle
	*	titleYear/region
	*	genres                   CONtentRating
	*	duration:				  IMDB-Rating

	Director:
	actor1/actor2/actor3
	description:
	--------------------------------------------*/
	private void updateDetails() {
		title.text = myInfo.movie_title + " (" + myInfo.title_year.ToString() + ")";
		string tmp = "";
		tmp += myInfo.country + "/" + myInfo.language + "\n";
		tmp += myInfo.genres + "\n";
		tmp += myInfo.content_rating + "\n\n";
		tmp += "<size=40><b>Director</b> " + myInfo.director_name + "</size>\n";
		tmp += "<color=#515151ff>" + myInfo.actor_1_name + "/"
			 + myInfo.actor_2_name + "/"
			 + myInfo.actor_3_name + "</color>\n\n";
		tmp += myInfo.description;
		descriptions.text = tmp;
		score.text = myInfo.imdb_score.ToString();
		scorePortion.fillAmount = 0f;
		xRates.text = myInfo.num_voted_users.ToString() + " <color=#515151ff>Rates</color>";
		IEnumerator c = updatePoster(myInfo.image_url);

		likeThis.image.color = infoContainer.inFavorites(myInfo) ? Color.red : Color.gray;
		noInterest.image.color = infoContainer.isDislike(myInfo) ? Color.black : Color.grey;

		StartCoroutine(c);
		StartCoroutine("updateScoreBar");
	}

	IEnumerator updatePoster(string url){
		// Start a download of the given URL
		WWW www = new WWW(url);
		yield return www;
		poster.texture = www.texture;
	}

	const float delta = 1e-5f;
	const float accSpeed = 0.1f;
	IEnumerator updateScoreBar() {
		float target = myInfo.imdb_score / 10f;
		while (target - scorePortion.fillAmount > delta) {
			scorePortion.fillAmount += accSpeed * (target - scorePortion.fillAmount); 
			yield return new WaitForEndOfFrame();
		}

	}

	void likeThisOne(){
		if (!infoContainer.inFavorites(myInfo))
		{
			//add to the list
			infoContainer.addToMyFav(myInfo);
			//change sprite to red 
			likeThis.image.color = Color.red;
		}
		else{
			infoContainer.removeFromFav(myInfo);
			//change the sprite back to grey
			likeThis.image.color = Color.grey;
		}
	}

	void noInterestInThis(){
		if (!infoContainer.isDislike(myInfo)){
			//add to the list
			infoContainer.addToDislike(myInfo);
			//change sprite to red 
			noInterest.image.color = Color.black;
		}
		else{
			infoContainer.removeFromDislike(myInfo);
			//change the sprite back to grey
			noInterest.image.color = Color.grey;
		}
	}
}
