using System;
using System.Collections;
using UnityEngine;

public class WhatsAppMessaging : MonoBehaviour
{
	private string Url = "api.twilio.com/2010-04-01/Accounts/";
	private string Service="/Messages.json";
	private const string AccountSid = "ACfbd57f50e199a28eac49de4cc4acfb8a";
	private const string AuthToken = "81795970808380267013bf04070a5936";
	private const string From = "whatsapp:+14155238886";
	public bool Sent;
	public string Returned;
	private Action<bool, string> OnMessageSent;

	//public string Send(string to, string body, Action<bool, string> onMessageSent)
	public void Send(string to, string body, Action<bool, string> onMessageSent)
	{
		if (String.IsNullOrEmpty(to))
		{
			//return "Falta el número al que mandás el mensaje";
			throw new ExceptionPhone("Falta el número al que mandás el mensaje.");
		}
		
		if (!to.StartsWith("+598"))
		{
			//return "El número al que mandás el mensaje de comienzar con +598";
			throw new ExceptionPhone("El número al que mandás el mensaje de comienzar con +598.");
		}
		
		if (to.Length != 12)
		{
			//return "Le faltan o sobran dígitos al número al que querés mandar el mensaje";
			throw new ExceptionPhone("Le faltan o sobran dígitos al número al que querés mandar el mensaje.");
		}
		long number;
		
		if (!Int64.TryParse(to, out number))
		{
			//return "El número al que mandás el mensaje tiene que tener sólo números";
			throw new ExceptionPhone("El número al que mandás el mensaje tiene que tener sólo números.");
		}
		
		if (string.IsNullOrEmpty(body))
		{
			//return "Falta el mensaje a enviar";
			throw new ExceptionPhone("Falta el mensaje a enviar.");
		}

		if (onMessageSent == null)
		{
			//return "El parámetro callback no puede ser null" ;
			throw new ExceptionPhone("El parámetro callback no puede ser null.");
		}

		this.OnMessageSent = onMessageSent;

		WWWForm form = new WWWForm ();
		form.AddField ("To", "whatsapp:" + to);
		form.AddField ("From", From);
		form.AddField ("Body", body);

		string completeUrl = "https://" + AccountSid + ":" + AuthToken + "@" + Url + AccountSid + Service;
		Debug.Log (completeUrl);
		WWW www = new WWW(completeUrl, form);
		this.StartCoroutine(WaitForRequest(www));

	}
	
	private IEnumerator WaitForRequest (WWW www)
	{
		yield return www;

		bool sent;
		string result;

		sent = www.error == null;
		result = sent ? www.text : www.error;
		Debug.Log(result);

		this.OnMessageSent(sent, result);    
	}
}