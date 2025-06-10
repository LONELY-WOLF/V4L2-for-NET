# V4L2-for-NET

Video4Linux2 (V4L2) API calls for .NET

## API Coverage

This lib has limited API coverage. It is made mainly to capture images on SBCs (RaspberryPi-like boards). I have no idea if it works on x86 with USB webcams or not.

## Dependencies

.NET and libc.

## Test Environment

This lib was tested on Rockchip SoCs. Those, however, use VIDEO_CAPTURE_MPLANE instead of VIDEO_CAPTURE. Please, be careful if your target platform uses more common VIDEO_CAPTURE.