using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ContactUsReuseable : MonoBehaviour
{
    public TMP_InputField[] ListOfInputFields;

    public string FormBaseURL = "";
    public string FormURLwithformResponse = "";

    public string[] FormEntryIDs;

    public UnityEvent successfulGoogleForm;

    public GameObject successText;



    private IEnumerator SendGoogleFormData(TMP_InputField[] inputs)
    {
        


        WWWForm form = new WWWForm();

        for (int i = 0; i < inputs.Length; i++)
        {
            form.AddField(FormEntryIDs[i], inputs[i].text);

        }

        

        string urlGFormResponse = FormBaseURL + "formResponse";

        if (String.IsNullOrEmpty(FormBaseURL)) {
            urlGFormResponse = FormURLwithformResponse;
        }

        using (UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form))
        {
            yield return www.SendWebRequest();
        }


    }

    public void SendMessageToGoogleFormClick()
    {

        for (int i = 0; i < ListOfInputFields.Length; i++) {
            if (String.IsNullOrEmpty(ListOfInputFields[i].text))
            {
                ListOfInputFields[i].placeholder.GetComponent<TextMeshProUGUI>().text = "Please enter the missing data";
                return;
            }
        }


        StartCoroutine(SendGoogleFormData(ListOfInputFields));

        StartCoroutine(ShowSuccessText());
        successfulGoogleForm.Invoke();

        /*string urlGFormView = kGFormBaseURL + "viewform";
        OpenLink( urlGFormView );*/
    }

    public void ResetInputs()
    {
        for (int i = 0; i < ListOfInputFields.Length; i++)
        {
            ListOfInputFields[i].text = "";
        }
    }

    // Send Email

    public void SendEmailClick()
    {
        SendEmail("farmerfund@lovepreferredcoffee.com", " ", " ");
    }

    void SendEmail(string emailAg, string subjectAg, string bodyAg)
    {
        //Application.OpenURL("http://www.google.com");

        //email Id to send the mail to
        string email = emailAg;
        //subject of the mail
        string subject = MyEscapeURL(subjectAg);
        //body of the mail which consists of Device Model and its Operating System
        string body = "";
        int k = 1;

        body = MyEscapeURL(bodyAg);

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

        //Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }



    private const string kReceiverEmailAddress = "me@gmail.com";
    private static void OpenEmailClient(string feedback)
    {
        string email = kReceiverEmailAddress;
        string subject = "Feedback";
        string body = "<" + feedback + ">";
        OpenLink("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private void OpenGFormLink()
    {
        string urlGFormView = FormBaseURL + "viewform";
        OpenLink(urlGFormView);
    }

    // We cannot have spaces in links for iOS
    public static void OpenLink(string link)
    {
        bool googleSearch = link.Contains("google.com/search");
        string linkNoSpaces = link.Replace(" ", googleSearch ? "+" : "%20");
        Application.OpenURL(linkNoSpaces);
    }

    private IEnumerator ShowSuccessText()
    {
        successText.SetActive(true);

        yield return new WaitForSeconds(3f);

        successText.SetActive(false);

    }


}
