﻿using System;
using Xamarin.Auth;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using ihbiproject;
using UIKit;
using Newtonsoft.Json.Linq;
using ihbiproject.Views;
using Facebook;

//"maps" our LoginPageRenderer to the LoginPage
[assembly: ExportRenderer (typeof (ihbiproject.LoginPage), typeof (ihbiproject.iOS.LoginPageRenderer))]

namespace ihbiproject.iOS
{
	public class LoginPageRenderer : PageRenderer
	{
		bool IsShown;

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			// Fixed the issue that on iOS 8, the modal wouldn't be popped.
			// url : http://stackoverflow.com/questions/24105390/how-to-login-to-facebook-in-xamarin-forms
			if(	! IsShown ) {

				IsShown = true;

				var auth = new OAuth2Authenticator (
					clientId: App.Instance.OAuthSettings.ClientId, // your OAuth2 client id
					scope: App.Instance.OAuthSettings.Scope, // The scopes for the particular API you're accessing. The format for this will vary by API.
					authorizeUrl: new Uri (App.Instance.OAuthSettings.AuthorizeUrl), // the auth URL for the service
					redirectUrl: new Uri (App.Instance.OAuthSettings.RedirectUrl)); // the redirect URL for the service




				auth.Completed += async(sender, eventArgs) => {
					DismissViewController (true, null);

					if (eventArgs.IsAuthenticated) {
						
						var accessToken = eventArgs.Account.Properties ["access_token"].ToString ();
						var expiresIn = Convert.ToDouble (eventArgs.Account.Properties ["expires_in"]);
						var expiryDate = DateTime.Now + TimeSpan.FromSeconds (expiresIn);
						var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, eventArgs.Account);
						var response = await request.GetResponseAsync ();
						System.Diagnostics.Debug.WriteLine (response);


						var obj = JObject.Parse (response.GetResponseText ());
						var id = obj ["id"].ToString ().Replace ("\"", "");
						var name = obj ["name"].ToString ().Replace ("\"", "");

						App.Instance.SaveToken(accessToken);
						AccountStore.Create ().Save (eventArgs.Account, "WellnessFB");

						//fb(accessToken);

						//Once the login is successful, 
						//fire off a Xamarin.Forms navigation via App.SuccessfulLoginAction.Invoke();.
						App.Instance.SuccessfulLoginAction.Invoke();

					} else {
						// The user cancelled
					}
				};

				PresentViewController (auth.GetUI (), true, null);
			}
		}



	}
}


