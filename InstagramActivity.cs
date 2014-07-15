//
//  InstagramActivity.cs
//
//  Copyright (c) 2014 Lukas Lipka <lukaslipka@gmail.com>
//

using System;
using System.IO;
using System.Text;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MonoTouch.Instagram
{
	public class InstagramActivity : UIActivity
	{
		UIView _parentView;
		UIImage _image;
		StringBuilder _caption = new StringBuilder();

		public override NSString Type {
			get { return new NSString ("UIActivityTypePostToInstagram"); }
		}

		public override string Title {
			get { return "Instagram"; }
		}

		public override UIImage Image {
			get { return UIImage.FromFile ("instagram"); }
		}

		public InstagramActivity (UIView parentView)
		{
			_parentView = parentView;
		}

		public override void Perform ()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var tmp = Path.Combine (documents, "..", "tmp", "instagram.igo");

			NSData data = _image.AsJPEG ();
			NSError error;
			if (!data.Save (tmp, NSDataWritingOptions.Atomic, out error))
			{
				Finished (false);
			}

			NSUrl URL = NSUrl.FromFilename(tmp);
			UIDocumentInteractionController controller = UIDocumentInteractionController.FromUrl (URL);
			controller.Delegate = new DocumentInteractionControllerDelegate (this);
			controller.Uti = "com.instagram.instagramexclusive";

			if (_caption.Length > 0)
			{
				controller.Annotation = new NSDictionary ("InstagramCaption", _caption.ToString ());
			}

			controller.PresentOpenInMenu (_parentView.Bounds, _parentView, true);
		}

		public override bool CanPerform (NSObject[] activityItems)
		{
			NSUrl instagramURL = NSUrl.FromString ("instagram://app");
			if (UIApplication.SharedApplication.CanOpenUrl (instagramURL))
			{
				foreach (var o in activityItems)
				{
					var i = o as UIImage;
					if (i != null)
					{
						if (IsImageLargeEnough (i))
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public override void Prepare (NSObject[] activityItems)
		{
			foreach (var o in activityItems)
			{
				if (o is UIImage)
				{
					_image = o as UIImage;
				}
				else if (o is NSUrl)
				{
					if (_caption.Length > 0)
					{
						_caption.Append (" ");
					}

					var u = o as NSUrl;
					_caption.Append (u.AbsoluteUrl);
				}
				else if (o is NSString)
				{
					if (_caption.Length > 0)
					{
						_caption.Append (" ");
					}

					var s = o as NSString;
					_caption.Append (s.ToString ());
				}
			}
		}

		bool IsImageLargeEnough (UIImage image)
		{
			return image.Size.Width * image.CurrentScale >= 612 && image.Size.Height * image.CurrentScale >= 612;
		}

		class DocumentInteractionControllerDelegate : UIDocumentInteractionControllerDelegate {
			protected InstagramActivity _activity;

			public DocumentInteractionControllerDelegate (InstagramActivity activity)
			{
				_activity = activity;
			}

			public override void WillBeginSendingToApplication (UIDocumentInteractionController controller, string application)
			{
				_activity.Finished (true);
			}
		}
	}
}
