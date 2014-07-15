# MonoTouch.InstagramActivity

A MonoTouch implementation of an Instagram share activity for iOS.

Provides a UIActivity subclass that you can use to share photos and photo captions.

## Usage

``` c#
using MonoTouch.Instagram;

...

var activity = new InstagramActivity (View);
var activityItems = new NSObject[] { UIImage.FromBundle ("photo-to-share.jpg"), new NSString ("Optional image caption. #hashtag"), };
var activityController = new UIActivityViewController (activityItems, new UIActivity[] { activity });
PresentViewController (activityController, true, null);
```

For more information see [Instagram's iPhone Hooks](http://instagram.com/developer/iphone-hooks/).

## Installation

- Just add InstagramActivity.cs and the instagram PNG icons to your project.

## Requirements

MonoTouch.InstagramActivity is tested on iOS7, but should work with lower iOS versions as well.

## Contact

Lukas Lipka

- http://github.com/lipka
- http://twitter.com/lipec
- http://lukaslipka.com

## License

MonoTouch.InstagramActivity is available under the [MIT license](LICENSE). See the LICENSE file for more info.
