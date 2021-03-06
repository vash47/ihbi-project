﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Foundation;
using UIKit;
using ihbiproject;
using Xamarin.Auth;


namespace ihbiproject.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			System.Diagnostics.Debug.WriteLine ("======>start app and get storedaccount");

			getStoredAccount ();

			window.RootViewController = App.Instance.GetMainPage().CreateViewController ();

			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB (252, 224, 223);

			window.MakeKeyAndVisible ();

            //for maps support
            Xamarin.FormsMaps.Init();

            return true;
		}


		public override void  OnActivated(UIApplication app)
		{
			// Handle when your app starts
			System.Diagnostics.Debug.WriteLine ("======>start OnActivated");
			if (!App.Instance.IsAuthenticated) {
				getStoredAccount ();
			}
		}

		public override void DidEnterBackground (UIApplication app)
		{
			// Handle when your app sleeps
			System.Diagnostics.Debug.WriteLine ("======>start DidEnterBackground");
		}

		public override void WillEnterForeground (UIApplication app)
		{
			// Handle when your app resumes
			System.Diagnostics.Debug.WriteLine ("======>start WillEnterForeground");
			if (!App.Instance.IsAuthenticated) {
				getStoredAccount ();
			}
		}
			
		public void getStoredAccount(){
			System.Diagnostics.Debug.WriteLine ("======>start getStoreAccount");

			var accounts = AccountStore.Create ().FindAccountsForService ("WellnessFB");
			var account = accounts.FirstOrDefault ();

			if (account != null) {
				var accessToken = account.Properties ["access_token"].ToString ();
				App.Instance.SaveToken (accessToken);
				System.Diagnostics.Debug.WriteLine ("======>WellnessFB account" + account);
				//fb(accessToken);

			} else {

			}
		}
			

	}
}
