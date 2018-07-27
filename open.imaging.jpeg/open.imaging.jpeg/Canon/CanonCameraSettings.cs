/////////////////////////////////////////////////////////////////////////////// 
// 
// This file is part of open.imaging.jpeg project
//
// Copyright (c) 2017 Nikola Bozovic. All rights reserved. 
// 
// This code is licensed under the MIT License (MIT). 
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE. 
// 
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace open.imaging.jpeg.Canon
{

  [ComVisible(true)]
  public enum CanonCs //: ushort
  {
    Macro = 0,
    Selftimer = 1,
    Quality = 2,
    FlashMode = 3,
    DriveMode = 4,
    x0005 = 5,
    FocusMode = 6,
    x0007 = 7,
    x0008 = 8,
    ImageSize = 9,
    EasyMode = 10,
    DigitalZoom = 11,
    Contrast = 12,
    Saturation = 13,
    Sharpness = 14,
    ISOSpeed = 15,
    MeteringMode = 16,
    FocusType = 17,
    AFPoint = 18,
    ExposureProgram = 19,
    x0015 = 20,
    LensType = 21,
    Lens = 22,
    ShortFocal = 23,
    FocalUnits = 24,
    MaxAperture = 25,
    MinAperture = 26,
    FlashActivity = 27,
    FlashDetails = 28,
    x001e = 29,
    x001f = 30,
    FocusContinuous = 31,
    AESetting = 32,
    ImageStabilization = 33,
    DisplayAperture = 34,
    ZoomSourceWidth = 35,
    ZoomTargetWidth = 36,
    x0026 = 37,
    SpotMeteringMode = 38,
    PhotoEffect = 39,
    ManualFlashOutput = 40,
    ColorTone = 41,
    x002b = 42,
    x002c = 43,
    x002d = 44,
    SRAWQuality = 45,
  }

  public enum CanonCsMacro : ushort
  {
    Macro = 1,
    Normal = 2
  }
  public enum CanonCsQuality : ushort
  { 
    Normal = 2,
    Fine = 3,
    SuperFine = 5
  }
  public enum CanonCsFlashMode : ushort
  {
    FlashNotFired = 0,
    Auto = 1,
    On = 2,
    RedEyeReduction = 3,
    SlowSynchro = 4,
    AutoAndRedEyeReduction = 5,
    OnAndRedEyeReduction = 6,
    ExternalFlash = 16
  }
  public enum CanonCsDriveMode : ushort
  { 
    SingleFrameOrTimerMode = 0,
    Continuous = 1
  }
  public enum CanonCsFocusMode : ushort
  { 
    OneShot = 0,
    AIServo = 1,
    AIFocus = 2,
    ManualFocus = 3,
    Single = 4,
    Continuous = 5,
    ManualFocus2 = 6
  }
  public enum CanonCsImageSize : ushort
  { 
    Large = 0,
    Medium = 1,
    Small = 2
  }
  public enum CanonCsEasyMode : ushort
  {
    FullAuto = 0,
    Manual = 1,
    Landscape = 2,
    FastShutter = 3,
    SlowShutter = 4,
    Night = 5,
    BlackAndWhite = 6,
    Sepia = 7,
    Portrait = 8,
    Sports = 9,
    MacroCloseUp = 10,
    PanFocus = 11
  }
  public enum CanonCsDigitalZoom : ushort
  { 
    NoZoom = 0,
    Zoom2X = 1,
    Zoom4X = 2
  }
  public enum CanonCsContrast : ushort
  { 
    Normal = 0,
    High = 1,
    Low = 65535
  }
  public enum CanonCsSaturation : ushort
  {
    Normal = 0,
    High = 1,
    Low = 65535
  }
  public enum CanonCsSharpness : ushort
  {
    Normal = 0,
    High = 1,
    Low = 65535
  }
  public enum CanonCsISOSpeed : ushort
  { 
    EXIFISOSpeedRatings = 0,
    AutoISO = 15,
    ISO50 = 16,
    ISO100 = 17,
    ISO200 = 18,
    ISO400  = 19
  }
  public enum CanonCsMeteringMode : ushort
  { 
    Evaluative = 3,
    Partial = 4,
    CentreWeighted = 5
  }
  public enum CanonCsFocusType : ushort
  { 
    Manual = 0,
    Auto = 1,
    MacroCloseUp = 3,
    LockedPanMode = 8
  }
  public enum CanonCsAFPoint : ushort
  { 
    NoneManual = 12288,
    Auto = 12289,
    Right = 12290,
    Centre = 12291,
    Left = 12292
  }
  public enum CanonCsExposureMode : ushort
  { 
    // (See Easy Shooting Mode)
    EasyShooting = 0,
    Program = 1,
    TvPriority = 2,
    AvPriority = 3,
    Manual = 4,
    ADEP = 5,
  }
  public enum CanonCsFlashActivity : ushort
  { 
    DidNotFire = 0,
    Fired = 1
  }
  public enum CanonCsFocusContinuous : ushort
  { 
    Single = 0,
    Continuous = 1
  }

  public class CanonCameraSettings
  {
    public ushort[] data;

    public CanonCameraSettings(ushort[] value)
    {
      this.data = value;      
    }

    public ushort this[CanonCs tag]
    {
      get { return this.data[(ushort)tag]; }
      set { this.data[(ushort)tag] = value; }
    }

    public T Get<T>(CanonCs tag) where T : struct
    {
      return (T)(object)this.data[(ushort)tag];
      // return (T)Convert.ChangeType(this.data[(ushort)tag], typeof(T));
    }

    public void Set<T>(CanonCs tag, T value)
      where T : EnumConstraint
    {
      // this.data[(ushort)tag] = (ushort)(object)value;
      this.data[(ushort)tag] = EnumConstraint.to_u2<T>(value);
    }
  }



}
