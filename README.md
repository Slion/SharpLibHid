<img align="right" src="/Publish/usb.png">

# SharpLibHid

C# HID Library using Windows Raw Input. Most useful to handle inputs from IR remotes, gamepads, joysticks or any Human Interface Devices.

## HID Demo

You can get a pretty good understanding of what this library can do be running our [HID Demo](http://publish.slions.net/HidDemo/).

## Binary Distribution
The easiest way to make use of this library in your own project is to add a reference to the following [NuGet package](https://www.nuget.org/packages/SharpLibHid/).

## Usage

To get started with this library take a look at [HID Demo code](/MainForm.cs).
Basically there are just three things you need to do to get it working:
1. Instantiate a [`SharpLib.Hid.Handler`](https://github.com/Slion/SharpLibHid/blob/master/Hid/HidHandler.cs) specifying which kind of raw input you want to listen to.
2. Register with your HID handler `OnHidEvent` event. 
3. Feed your HID handler the Windows Raw Input from `WM_INPUT` messages.

Taking a look at older and simpler version of the [HID Demo code](https://github.com/Slion/SharpLibHid/blob/dd80a25b7c20e280abaecf014318891316224c7b/MainForm.cs#L188) can give you a better idea of what a minimal implementaiton looks like.
