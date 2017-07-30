using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class movieInfo{
	//update the content with api documentation
	/*-------------------------------------------
	*	movieTitle
	*	titleYear/region
	*	genres                   CONtentRating
	*	duration:				  IMDB-Rating

	Director:
	description:
	actor1/actor2/actor3
	--------------------------------------------*/
	#region openContent - file to parse
	public string id;
	public string movie_title;
	public string director_name;

	public string actor_1_name;
	public string actor_2_name;
	public string actor_3_name;

	public int title_year;
	//public List<string> genres = new List<string>();
	public string genres;

	public int duration;
	public string content_rating;
	public string language;
	public string country;

	public float aspect_ratio;
	public float imdb_score;

	public string movie_imdb_link;
	public string image_url;
	public string description;
	#endregion

	public static movieInfo createFromJson(string jsonString) {
		return JsonUtility.FromJson<movieInfo>(jsonString);
	}

	
	//#region todo
	
	//string trailerLink;
	//
	//#endregion

	//connecting
}
