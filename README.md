![Build status](https://github.com/slion/sharplibhid/actions/workflows/dotnet.yml/badge.svg)
<img align="right" src="/Publish/usb.png">
<!--
<img align="right" src="https://slions.visualstudio.com/GitHub%20builds/_apis/build/status/SharpLibHid" />
-->

# SharpLibHid

C# HID Library using Windows Raw Input. Most useful to handle inputs from IR remotes, gamepads, joysticks or any Human Interface Devices.

## HID Demo

You can get a pretty good understanding of what this library can do be running our [HID Demo](http://publish.slions.net/HidDemo/).

## Download
The easiest way to use this library in your own project is to add a reference to the NuGet package that suits your needs.

### Nuget

#### x64

[![NuGet Badge](https://buildstats.info/nuget/Slions.SharpLib.Hid-x64)](https://www.nuget.org/packages/Slions.SharpLib.Hid-x64/)

#### x86

[![NuGet Badge](https://buildstats.info/nuget/Slions.SharpLib.Hid-x86)](https://www.nuget.org/packages/Slions.SharpLib.Hid-x86/)

#### Legacy - AnyCPU

[![NuGet Badge](https://buildstats.info/nuget/SharpLibHid)](https://www.nuget.org/packages/SharpLibHid/)

## Usage

To get started with this library take a look at [HID Demo code](/MainForm.cs).
Basically there are just three things you need to do to get it working:
1. Instantiate a [`SharpLib.Hid.Handler`](https://github.com/Slion/SharpLibHid/blob/master/Hid/HidHandler.cs) specifying which kind of raw input you want to listen to.
2. Register with your HID handler `OnHidEvent` event. 
3. Feed your HID handler the Windows Raw Input from `WM_INPUT` messages.

Taking a look at older and simpler version of the [HID Demo code](https://github.com/Slion/SharpLibHid/blob/dd80a25b7c20e280abaecf014318891316224c7b/MainForm.cs#L188) can give you a better idea of what a minimal implementaiton looks like.

## References

- https://stackoverflow.com/questions/956669/does-setupdigetclassdevs-work-with-device-instance-ids-as-documented
- https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw
- https://docs.microsoft.com/en-us/windows-hardware/drivers/install/determining-the-parent-of-a-device
- https://docs.microsoft.com/en-us/windows-hardware/drivers/install/retrieving-device-relations
- https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdevicepropertyw
- https://github.com/BrianPeek/WiimoteLib
- https://social.msdn.microsoft.com/Forums/vstudio/en-US/8a8ae70a-fdcc-423a-a5c5-5b02d2f207a7/enumerate-human-interface-devices?forum=csharpgeneral
