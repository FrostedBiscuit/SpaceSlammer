using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISocialLinks : MonoBehaviour {

    [SerializeField]
    string TwitterLink = "";
    [SerializeField]
    string InstagramLink = "";
    [SerializeField]
    string WebisteLink = "";

    public void OpenTwitter()
    {
        Application.OpenURL(TwitterLink);
    }

    public void OpenInstagram()
    {
        Application.OpenURL(InstagramLink);
    }

    public void OpenWebsite()
    {
        Application.OpenURL(WebisteLink);
    }
}
