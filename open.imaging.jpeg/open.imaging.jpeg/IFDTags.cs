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

namespace open.imaging.jpeg
{

  [ComVisible(true)]
  public enum IFDTag //: ushort
  {    
    _VOID_ = 0xFFFF,

    #region ### IFD POINTER (type: LONG) ###

    IFDExifPointer = 0x8769,
    IFDExifPointer_ = 0x6987,

    IFDGPSInfoPointer = 0x8825,

    IFDInteropPointer = 0xA005,

    #endregion

    #region ### Tiff version 6.0 Attribute Information ###
    #region ### Tiff.A - Tags relating to image data structure ###
    /// <summary>Tiff.A - Image width - SHORT | LONG (count 1)</summary>
    ImageWidth = 0x0100,
    /// <summary>Tiff.A - Image height - SHORT | LONG (count 1)</summary>
    ImageLength = 0x0101,
    /// <summary>Tiff.A - Number of bits per component - SHORT (count 3)</summary>
    BitsPerSample = 0x0102,
    /// <summary>Tiff.A - Compression scheme - SHORT (count 1)</summary>
    Compression = 0x0103,
    /// <summary>Tiff.A - Pixel composition - SHORT (count 1)</summary>
    PhotometricIntepretation = 0x0106,
    /// <summary>Tiff.A - Orientation of image - SHORT (count 1)</summary>
    Orientation = 0x0112,
    /// <summary>Tiff.A - Number of components - SHORT (count 1)</summary>
    SamplesPerPixel = 0x0115,
    /// <summary>Tiff.A - Image data arrangement - SHORT (count 1)</summary>
    PlanarConfiguration = 0x011C,
    /// <summary>Tiff.A - Subsampling radio of Y to C - SHORT (count 2)</summary>
    YCbCrSubSampling = 0x0212,
    /// <summary>Tiff.A - Y and C positioning - SHORT (count 1)</summary>
    YCbCrPositioning = 0x0213,
    /// <summary>Tiff.A - Image resolution in width direction - RATIONAL (count 1)</summary>
    XResolution = 0x011A,
    /// <summary>Tiff.A - Image resolution in height direction - RATIONAL (count 1)</summary>
    YResolution = 0x011B,
    /// <summary>Tiff.A - Unit of X and Y resoultion - SHORT (count 1)</summary>
    ResolutionUnit = 0x0128,
    #endregion
    #region ### Tiff.B - Tags relating to recording offset ###
    /// <summary>Tiff.B - Image data location - SHORT or LONG (count *S)</summary>
    StripOffsets = 0x0111,
    /// <summary>Tiff.B - Number of rows per strip - SHORT or LONG (count 1)</summary>
    RowsPerStrip = 0x0116,
    /// <summary>Tiff.B - Bytes per compressed strip - SHORT or LONG (count *S)</summary>
    StripByteCounts = 0x0117,
    /// <summary>Tiff.B - Offset to JPEG SOI - LONG (count 1)</summary>
    JPEGInterchangeFormat = 0x0201,
    /// <summary>Tiff.B - Bytes of JPEG data - LONG (count 1)</summary>
    JPEGInterchangeFormatLength = 0x0202,
    #endregion
    #region ### Tiff.C - Tags relating to image data characteristics ###
    /// <summary>Tiff.C - Transfer function - SHORT (count 3 * 256)</summary>
    TransferFunction = 0x012D,
    /// <summary>Tiff.C - White point chromaticity - RATIONAL (count 2)</summary>
    WhitePoint = 0x013E,
    /// <summary>Tiff.C - Chromaticities of primaries - RATIONAL (count 6)</summary>
    PrimaryChromaticities = 0x013F,
    /// <summary>Tiff.C - Color space transformation matrix coefficients - RATIONAL (count 3)</summary>
    YCbCrCoefficients = 0x0211,
    /// <summary>Tiff.C - Pair of black and white reference values - RATIONAL (count 6)</summary>
    ReferenceBlackWhite = 0x0214,
    #endregion
    #region ### Tiff.D - Other Tags ###
    /// <summary>Tiff.D - File change date and time - ASCII (count 20)</summary>
    DateTime = 0x0132,
    /// <summary>Tiff.D - Image title - ASCII (count Any)</summary>
    ImageDescription = 0x010E,
    /// <summary>Tiff.D - Image input equipment manufacturer - ASCII (count Any)</summary>
    Make = 0x010F,
    /// <summary>Tiff.D - Image input equipment model - ASCII (count Any)</summary>
    Model = 0x0110,
    /// <summary>Tiff.D - Software used - ASCII (count Any)</summary>
    Software = 0x0131,
    /// <summary>Tiff.D - Person who created the image - ASCII (count Any)</summary>
    Artist = 0x013b,
    /// <summary>Tiff.D - Copyright holder - ASCII (count Any)</summary>
    Copyright = 0x8298,
    #endregion
    #endregion
    #region ### Exif IFD Attribute Information ###
    #region ### Exif.A - Tags relating to Version ###
    /// <summary>Exif.A - Exif version - UNDEFINED (count 4)</summary>
    ExifVersion = 0x9000,
    /// <summary>Exif.A - Supported Flashpix version - UNDEFINED (count 4)</summary>
    FlashpixVersion = 0xA000,
    #endregion
    #region ### Exif.B - tags relating to Image Data Characteristics ###
    /// <summary>Exif.B - Color space information - SHORT (count 1)</summary>
    ColorSpace = 0x9000,
    /// <summary>Exif.B - Gamma - RATIONAL (count 1)</summary>
    Gamma = 0xA000,
    #endregion
    #region ### Exif.C - Tags relating to Image Configuration ###
    /// <summary>Exif.C - Meaning of each component - UNDEFINED (count 4)</summary>
    ComponentsConfiguration = 0x9101,
    /// <summary>Exif.C - Image compression mode - RATIONAL (count 1)</summary>
    CompressedBitsPerPixel = 0x9102,
    /// <summary>Exif.C - Valid image width - SHORT or LONG (count 1)</summary>
    PixelXDimension = 0xA002,
    /// <summary>Exif.C - Valid image height - SHORT or LONG (count 1)</summary>
    PixelYDimension = 0xA003,
    #endregion
    #region ### Exif.D - Tags relating to User Information ###
    /// <summary>Exif.D - Manufacturer notes - UNDEFINED (count Any)</summary>
    MakerNote = 0x927C,
    /// <summary>Exif.D - User comments - UNDEFINED (count Any)</summary>
    UserComment = 0x9286,
    #endregion
    #region ### Exif.E - Tags relating to Related File Information ###
    /// <summary>Exif.E - Related audio file - ASCII (count 13)</summary>
    RelatedSoundFile = 0xA004,
    #endregion
    #region ### Exif.F - Tags relating to Date and Time ###
    /// <summary>Exif.F - Date and time of original data generation. The format is "YYYY:MM:DD HH:MM:SS" - ASCII (count 20)</summary>
    DateTimeOriginal = 0x9003,
    /// <summary>Exif.F - Date and time of digital data generation. The format is "YYYY:MM:DD HH:MM:SS" - ASCII (count 20)</summary>
    DateTimeDigitized = 0x9004,
    /// <summary>Exif.F - DateTime subseconds. Tag used to record fractions of seconds for the DateTime tag - ASCII (count Any)</summary>
    SubSecTime = 0x9290,
    /// <summary>Exif.F - DateTimeOriginal subseconds. Tag used to record fractions of seconds for the DateTimeOriginal tag - ASCII (count Any)</summary>
    SubSecTimeOriginal = 0x9291,
    /// <summary>Exif.F - DateTimeDigitized subseconds. Tag used to record fractions of seconds for the DateTimeDigitized tag - ASCII (count Any)</summary>
    SubSecTimeDigitized = 0x9292,
    #endregion
    #region ### Exif.G - Tags relating to Picture-Taking Conditions (0xAnnn Flashpix) ###
    // see table 8
    /// <summary>Exif.G - Exposure Time - RATIONAL (count 1)</summary>
    ExposureTime = 0x829A,
    /// <summary>Exif.G - F Number - RATIONAL (count 1)</summary>
    FNumber = 0x829D,
    /// <summary>Exif.G - Exposure program - SHORT (count 1)</summary>
    ExposureProgram = 0x8822,
    /// <summary>Exif.G - Spectral sensitivity - ASCII (count Any)</summary>
    SpectralSenditivity = 0x8824,
    /// <summary>Exif.G - Photographic sensitivity - SHORT (count Any)</summary>
    PhotographicSensitivity = 0x8827,
    /// <summary>Exif.G - Optoelectric conversion factor - UNDEFINED (count Any)</summary>
    OECF = 0x8828,
    /// <summary>Exif.G - Sensitivity type - SHORT (count 1)</summary>
    SensitivityType = 0x8830,
    /// <summary>Exif.G - Standard Output Sensitivity - LONG (count 1)</summary>
    StandardOutputSensitivity = 0x8831,
    /// <summary>Exif.G - Recommended Exposure Index - LONG (count 1)</summary>
    RecommendedExposureIndex = 0x8832,
    /// <summary>Exif.G - ISO Speed - LONG (count 1)</summary>
    ISOSpeed = 0x8833,
    /// <summary>Exif.G - ISO Speed Latitude yyy - LONG (count 1)</summary>
    ISOSpeedLatitudeyyy = 0x8834,
    /// <summary>Exif.G - ISO Speed Latitude zzz - LONG (count 1)</summary>
    ISOSpeedLatitudezzz = 0x8835,
    /// <summary>Exif.G - Shutter Speed - SRATIONAL (count 1)</summary>
    ShutterSpeedValue = 0x9201,
    /// <summary>Exif.G - Lens aperture - RATIONAL (count 1)</summary>
    ApertureValue = 0x9202,
    /// <summary>Exif.G - Brightness - SRATIONAL (count 1)</summary>
    BrightnessValue = 0x9203,
    /// <summary>Exif.G - Exposure bias - SRATIONAL (count 1)</summary>
    ExposureBiasValue = 0x9204,
    /// <summary>Exif.G - Maximum lens aperture - RATIONAL (count 1)</summary>
    MaxApertureValue = 0x9205,
    /// <summary>Exif.G - Subject distance - RATIONAL (count 1)</summary>
    SubjectDistance = 0x9206,
    /// <summary>Exif.G - Metering mode - SHORT (count 1)</summary>
    MeteringMode = 0x9207,
    /// <summary>Exif.G - Light source - SHORT (count 1)</summary>
    LightSource = 0x9208,
    /// <summary>Exif.G - Flash - SHORT (count 1)</summary>
    Flash = 0x9209,
    /// <summary>Exif.G - Focal length - RATIONAL (count 1)</summary>
    FocalLength = 0x920A,
    /// <summary>Exif.G - Subject area - SHORT (count 2 or 3 or 4)</summary>
    SubjectArea = 0x9214,
    /// <summary>Exif.G - Flash energy - RATIONAL (count 1)</summary>
    FlashEnergy = 0xA20B,
    /// <summary>Exif.G - Spatial frenquency response - UNDEFINED (count Any)</summary>
    SpatialFrequencyResponse = 0xA20C,
    /// <summary>Exif.G - Focal plane X resolution - RATIONAL (count 1)</summary>
    FocalPlaneXResolution = 0xA20E,
    /// <summary>Exif.G - Focal plane Y resolution - RATIONAL (count 1)</summary>
    FocalPlaneYResolution = 0xA20F,
    /// <summary>Exif.G - Focal plane resolution unit - SHORT (count 1)</summary>
    FocalPlaneResolutionUnit = 0xA210,
    /// <summary>Exif.G - Subject location - SHORT (count 2)</summary>
    SubjectLocation = 0xA214,
    /// <summary>Exif.G - Exposure index - RATIONAL (count 1)</summary>
    ExposureIndex = 0xA215,
    /// <summary>Exif.G - Sensing method - SHORT (count 1)</summary>
    SensingMethod = 0xA217,
    /// <summary>Exif.G - File source - UNDEFINED (count 1)</summary>
    FileSource = 0xA300,
    /// <summary>Exif.G - Scene type - UNDEFINED (count 1)</summary>
    SceneType = 0xA301,
    /// <summary>Exif.G - CFA pattern - UNDEFINED (count Any)</summary>
    CFAPattern = 0xA302,
    /// <summary>Exif.G - Custom image processing - SHORT (count 1)</summary>
    CustomRendered = 0xA401,
    /// <summary>Exif.G - Exposure mode - SHORT (count 1)</summary>
    ExposureMode = 0xA402,
    /// <summary>Exif.G - White balance - SHORT (count 1)</summary>
    WhiteBalance = 0xA403,
    /// <summary>Exif.G - Digital zoom ratio - RATIONAL (count 1)</summary>
    DigitalZoomRatio = 0xA404,
    /// <summary>Exif.G - Focal length in 35 mm film - SHORT (count 1)</summary>
    FocalLengthIn35mmFilm = 0xA405,
    /// <summary>Exif.G - Screne capture type - SHORT (count 1)</summary>
    ScreneCaptureType = 0xA406,
    /// <summary>Exif.G - Gain control - RATIONAL (count 1)</summary>
    GainControl = 0xA407,
    /// <summary>Exif.G - Contrast - SHORT (count 1)</summary>
    Contrast = 0xA408,
    /// <summary>Exif.G - Saturation - SHORT (count 1)</summary>
    Saturation = 0xA409,
    /// <summary>Exif.G - Sharpness - SHORT (count 1)</summary>
    Sharpness = 0xA40A,
    /// <summary>Exif.G - Device settings description - UNDEFINED (count Any)</summary>
    DeviceSettingDescription = 0xA40B,
    /// <summary>Exif.G - Subject distance range - SHORT (count 1)</summary>
    SubjectDistanceRange = 0xA40C,

    #endregion
    #region ### Exif.H - Other Tags ###
    /// <summary>Exif.H - Unique image ID - ASCII (count 33)</summary>
    ImageUniqueID = 0xA420,
    /// <summary>Exif.H - Camera Owner Name - ASCII (count Any)</summary>
    CameraOwnerName = 0xA430,
    /// <summary>Exif.H - Body Serial Number - ASCII (count Any)</summary>
    BodySerialNumber = 0xA431,
    /// <summary>Exif.H - Lens Specification - RATIONAL (count 4)</summary>
    LensSpecification = 0xA432,
    /// <summary>Exif.H - Lens Make - ASCII (count Any)</summary>
    LensMake = 0xA433,
    /// <summary>Exif.H - Lens Model - ASCII (count Any)</summary>
    LensModel = 0xA434,
    /// <summary>Exif.H - Lens Serial Number - ASCII (count Any)</summary>
    LensSerialNumber = 0xA435,
    #endregion
    #endregion
    #region ### 4.6.7 Interoperability IFD Attribute Information (Table 16) ###
    /// <summary>IIFD.A - Interoperability Index - ASCII (count Any)</summary>
    InteroperabilityIndex = 0x0001,
    /// <summary>IIFD.A - Interoperability Index - UNDEFINED (count Any)</summary>
    InteropVersion = 0x0002,
    #endregion

    #region ### IFD0 Tags ###

    /// <summary>IFD0 - used by ACD Systems Digital Imaging - ASCII (count Any)</summary>
    ProcessingSoftware = 0x000b,

    /// <summary>IFD0 - complex - LONG (count ?)</summary>
    SubfileType = 0x00fe,

    /// <summary>IFD0 - complex - LONG (count ?)</summary>
    OldSubfileType = 0x00ff,

    #endregion

    #region ### 4.6.6 GPS Attribute Information (Table 15) ###
    #region ### GPS.A - Tags Relating to GPS ###
    /// <summary>GPS.A - GPS tag version - BYTE (count 4)</summary>
    GPSVersionID = 0x00,
    /// <summary>GPS.A - North or South Latitude - ASCII (count 2)</summary>
    GPSLatitudeRef = 0x01,
    /// <summary>GPS.A - Latitude - RATIONAL (count 3)</summary>
    GPSLatitude = 0x02,
    /// <summary>GPS.A - East or West Longitude - ASCII (count 2)</summary>
    GPSLongitudeRef = 0x03,
    /// <summary>GPS.A - Longitude - RATIONAL (count 3)</summary>
    GPSLongitude = 0x04,
    /// <summary>GPS.A - Altitude reference - BYTE (count 1)</summary>
    GPSAltitudeRef = 0x05,
    /// <summary>GPS.A - Altitude - RATIONAL (count 1)</summary>
    GPSAltitude = 0x06,
    /// <summary>GPS.A - GPS Time (atomic clock) - RATIONAL (count 3)</summary>
    GPSTimeStamp = 0x07,
    /// <summary>GPS.A - GPS satellites used for measurement - ASCII (count Any)</summary>
    GPSSatellites = 0x08,
    /// <summary>GPS.A - GPS receiver status - ASCII (count 2)</summary>
    GPSStatus = 0x09,
    /// <summary>GPS.A - GPS measurement mode - ASCII (count 2)</summary>
    GPSMeasureMode = 0x0A,
    /// <summary>GPS.A - Measurement precision - RATIONAL (count 1)</summary>
    GPSDOP = 0x0B,
    /// <summary>GPS.A - Speed unit - ASCII (count 2)</summary>
    GPSSpeedRef = 0x0C,
    /// <summary>GPS.A - Speed of GPS receiver - RATIONAL (count 1)</summary>
    GPSSpeed = 0x0D,
    /// <summary>GPS.A - Reference for direction of movement - ASCII (count 2)</summary>
    GPSTrackRef = 0x0E,
    /// <summary>GPS.A - Direction of movement - RATIONAL (count 1)</summary>
    GPSTrack = 0x0F,
    /// <summary>GPS.A - Reference for direction of image - ASCII (count 2)</summary>
    GPSImgDirectionRef = 0x10,
    /// <summary>GPS.A - Direction of image - RATIONAL (count 1)</summary>
    GPSImgDirection = 0x11,
    /// <summary>GPS.A - Geodetic survey data used - ASCII (count Any)</summary>
    GPSMapDatum = 0x12,
    /// <summary>GPS.A - Reference for latitude of destination - ASCII (count 2)</summary>
    GPSDestLatitudeRef = 0x13,
    /// <summary>GPS.A - Latitude of destination - RATIONAL (count 3)</summary>
    GPSDestLatitude = 0x14,
    /// <summary>GPS.A - Reference for longiture of destination - ASCII (count 2)</summary>
    GPSDestLongitudeRef = 0x15,
    /// <summary>GPS.A - Longiture of destination - RATIONAL (count 3)</summary>
    GPSDestLongitude = 0x16,
    /// <summary>GPS.A - Reference for bearing of destination - ASCII (count 2)</summary>
    GPSDestBearingRef = 0x17,
    /// <summary>GPS.A - Bearing of destination - RATIONAL (count 1)</summary>
    GPSDestBearing = 0x18,
    /// <summary>GPS.A - Reference for distance to destination - ASCII (count 2)</summary>
    GPSDestDistanceRef = 0x19,
    /// <summary>GPS.A - Distance to destination - RATIONAL (count 1)</summary>
    GPSDestDistance = 0x1A,
    /// <summary>GPS.A - Name of GPS processing method - UNDEFINED (count Any)</summary>
    GPSProcessingMethod = 0x1B,
    /// <summary>GPS.A - Name of GPS area - UNDEFINED (count Any)</summary>
    GPSAreaInformation = 0x1C,
    /// <summary>GPS.A - GPS date - ASCII (count 11)</summary>
    GPSDateStamp = 0x1D,
    /// <summary>GPS.A - GPS differential correction - SHORT (count 1)</summary>
    GPSDifferential = 0x1E,
    /// <summary>GPS.A - Horizontal positioning error - RATIONAL (count 1)</summary>
    GPSHPositioningError = 0x1F,
    #endregion
    #endregion
    #region ### 4.6.8 TIFF Tag Support Levels (1) - 0th IFD TIFF Tags (Table 17) - See the JPEG Draft International Standard (ISO DIS 10918-1) for more details ###

    ///// <summary>TIFF(1) | LONG (count 1) | 
    ///// This Field indicates whether a JPEG interchange format bitstream is present in the
    ///// TIFF file. If a JPEG interchange format bitstream is present, then this Field points to the Start of Image (SOI) marker code.
    ///// </summary
    //JPEGInterchangeFormat = 0x0201,

    ///// <summary>TIFF(1) | LONG (count 1) | 
    ///// This Field indicates the length in bytes of the JPEG interchange format bitstream.
    ///// This Field is useful for extracting the JPEG interchange format bitstream without parsing the bitstream.
    ///// </summary>
    //JPEGInterchangeFormatLength = 0x0202,

    /// <summary>TIFF(1) | SHORT (count 1) | 
    /// This Field indicates the length of the restart interval used in the compressed image
    /// data. The restart interval is defined as the number of Minimum Coded Units (MCUs) between restart markers.
    /// </summary>
    JPEGRestartInterval = 0x0203,

    /// <summary>TIFF(1) | SHORT (count SamplesPerPixel) | 
    /// This Field points to a list of lossless predictor-selection values, one per component.
    /// </summary>
    JPEGLosslessPredictors = 0x0205,

    /// <summary>TIFF(1) | SHORT (count SamplesPerPixel) | 
    /// This Field points to a list of point transform values, one per component. This Field is relevant only for lossless processes.
    /// </summary>
    JPEGPointTransforms = 0x0206,

    /// <summary>TIFF(1) | LONG (count SamplesPerPixel) | 
    /// This Field points to a list of offsets to the quantization tables, one per component.Each table consists of 64 BYTES 
    /// (one for each DCT coefficient in the 8x8 block). The quantization tables are stored in zigzag order.
    /// </summary>
    JPEGQTables = 0x0207,

    /// <summary>TIFF(1) | LONG (count SamplesPerPixel) | 
    /// This Field points to a list of offsets to the DC Huffman tables or the lossless Huffman tables, one per component.
    /// </summary>
    JPEGDCTables = 0x0208,

    /// <summary>TIFF(1) | LONG (count SamplesPerPixel) | 
    /// This Field points to a list of offsets to the Huffman AC tables, one per component.
    /// </summary>
    JPEGACTables = 0x0209,



    #endregion
    #region ### EXIF: Conversion between Exif Tags and Flashpix Property Set(2) (Table 60) ###

    // missing :
    
    FlashpixColorSpace = 0xA001,

    #endregion
    #region ### TIFF Adobe PageMaker Tags ###

    AdobePMSubIFDs = 0x014A,

    AdobePMClipPath = 0x0157,
    AdobePMClipPathUnits = 0x0158,

    #endregion



    Canon0x0000 = 0x0000,
    CanonCameraSettings = 0x0001,
    CanonFocalLength = 0x0002,
    Canon0x0003 = 0x0003,
    CanonShortInfo = 0x0004,
    CanonPanorama = 0x0005,
    CanonImageType = 0x0006,
    CanonFirmwareVersion = 0x0007,
    CanonFileNumber = 0x0008,
    CanonOwnerName = 0x0009,
    CanonSerialNumber = 0x000c,
    CanonCameraInfo = 0x000d,
    CanonCustomFunctions1 = 0x000f,
    CanonModelID = 0x0010,
    CanonPictureInfo = 0x0012,
    CanonThumbnailImageValidArea = 0x0013,
    CanonSerialNumberFormat = 0x0015,
    CanonSuperMacro = 0x001a,
    CanonAFInfo = 0x0026,
    CanonOriginalDecisionDataOffset = 0x0083,
    CanonWhiteBalanceTable = 0x00a4,
    CanonLensModel = 0x0095,
    CanonInternalSerialNumber = 0x0096,
    CanonDustRemocalData = 0x0097,
    CanonCustomFunctions2 = 0x0099,
    CanonProcessingInfo = 0x00a0,
    CanonMeasuredColor = 0x00aa,
    CanonColorSpace = 0x00b4,
    Canon0x00b5 = 0x00b5,
    Canon0x00c0 = 0x00c0,
    Canon0x00c1 = 0x00c1,
    CanonVRDOffset = 0x00d0,
    CanonSensorInfo = 0x00e0,
    CanonColorData = 0x4001,

    //#region ### (@CanonCameraSettings) Canon Camera Settings Tags ###
    ///// <summary>Canon | SSHORT (1) | Macro mode </summary>
    //CanonCsMacro = 0x0001,
    ///// <summary>Canon | SSHORT (1) | Self timer </summary>
    //CanonCsSelftimer = 0x0002,
    ///// <summary>Canon | SSHORT (1) | Quality </summary>
    //CanonCsQuality = 0x0003,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFlashMode = 0x0004,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsDriveMode = 0x0005,
    ///// <summary>Canon | SHORT (1) | </summary>
    //CanonCs0x0006 = 0x0006,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFocusMode = 0x0007,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x0008 = 0x0008,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x0009 = 0x0009,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsImageSize = 0x000a,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsEasyMode = 0x000b,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsDigitalZoom = 0x000c,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsContrast = 0x000d,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsSaturation = 0x000e,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsSharpness = 0x000f,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsISOSpeed = 0x0010,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsMeteringMode = 0x0011,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFocusType = 0x0012,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsAFPoint = 0x0013,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsExposureProgram = 0x0014,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x0015 = 0x0015,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsLensType = 0x0016,
    ///// <summary>Canon | SHORT (??) | </summary>
    //CanonCsLens = 0x0017,
    ///// <summary>Canon | SHORT (1) | </summary>
    //CanonCsShortFocal = 0x0018,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFocalUnits = 0x0019,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsMaxAperture = 0x001a,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsMinAperture = 0x001b,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFlashActivity = 0x001c,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFlashDetails = 0x001d,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x001e = 0x001e,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x001f = 0x001f,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsFocusContinuous = 0x0020,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsAESetting = 0x0021,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsImageStabilization = 0x0022,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsDisplayAperture = 0x0023,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsZoomSourceWidth = 0x0024,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsZoomTargetWidth = 0x0025,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x0026 = 0x0026,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsSpotMeteringMode = 0x0027,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsPhotoEffect = 0x0028,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsManualFlashOutput = 0x0029,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsColorTone = 0x002a,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x002b = 0x002b,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x002c = 0x002c,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCs0x002d = 0x002d,
    ///// <summary>Canon | SSHORT (1) | </summary>
    //CanonCsSRAWQuality = 0x002e,

    //#endregion

    //#region ### (@CanonShotInfo) Canon Shot Info Tags - all tags are of type SHORT ###

    //CanonSi0x0001 = 0x0001,
    //CanonSiISOSpeed = 0x0002,
    //CanonSiMeasuredEV = 0x0003,
    //CanonSiTargetAperture = 0x0004,
    //CanonSiTargetShutterSpeed = 0x0005,
    //CanonSi0x0006 = 0x0006,
    //CanonSiWhiteBalance = 0x0007,
    //CanonSi0x0008 = 0x0008,
    //CanonSiSequence = 0x0009,
    //CanonSi0x000a = 0x000a,
    //CanonSi0x000b = 0x000b,
    //CanonSi0x000c = 0x000c,
    //CanonSi0x000d = 0x000d,
    //CanonSiAFPointUsed = 0x000e,
    //CanonSiFlashBias = 0x000f,
    //CanonSi0x0010 = 0x0010,
    //CanonSi0x0011 = 0x0011,
    //CanonSi0x0012 = 0x0012,
    //CanonSiSubjectDistance = 0x0013,
    //CanonSi0x0014 = 0x0014,
    //CanonSiApertureValue = 0x0015,
    //CanonSiShutterSpeedValue = 0x0016,
    //CanonSiMeasuredEV2 = 0x0017,
    //CanonSi0x0018 = 0x0018,
    //CanonSi0x0019 = 0x0019,
    //CanonSi0x001a = 0x001a,

    //#endregion

    //#region ### (@CanonPanorama) Canon Panorama Tags - all tags are of type SHORT ###

    //CanonPaPanoramaFrame = 0x0002,
    //CanonPaPanoramaDirection = 0x0005,

    //#endregion

    //#region ### (@CanonCustomFunctions1) Canon Panorama Tags - all tags are of type SHORT ###

    //CanonCfNoiseReduction = 0x0001,
    //CanonCfShutterAeLock = 0x0002,
    //CanonCfMirrorLockup = 0x0003,
    //CanonCfExposureLevelIncrements = 0x0004,
    //CanonCfAFAssist = 0x0005,
    //CanonCfFlashSyncSpeedAv = 0x0006,
    //CanonCfAEBSequence = 0x0007,
    //CanonCfShutterCurtainSync = 0x0008,
    //CanonCfLensAFStopButton = 0x0009,
    //CanonCfFillFlashAutoReduction = 0x000a,
    //CanonCfMenuButtonReturn = 0x000b,
    //CanonCfSetButtonFunction = 0x000c,
    //CanonCfSensorCleaning = 0x000d,
    //CanonCfSuperimposedDisplay = 0x000e,
    //CanonCfShutterReleaseNoCFCard = 0x000f,

    //#endregion

    //#region ### (@CanonPictureInfo) Canon Picture Info Tags - all tags are of type SHORT ###

    //CanonPiImageWidth = 0x0002,
    //CanonPiImageHeight = 0x0003,
    //CanonPiImageWidthAsShot = 0x0004,
    //CanonPiImageHeightAsShot = 0x0005,
    //CanonPiAFPointsUsed = 0x0016,
    //CanonPiAFPointsUsed20D = 0x001a,

    //#endregion

    //#region ### () Canon File Info Tags - all tags are of type SSHORT except CanonFiFileNumber ###

    //CanonFiFileNumber = 0x0001,
    //CanonFiBracketMode = 0x0003,
    //CanonFiBracketValue = 0x0004,
    //CanonFiBracketShotNumber = 0x0005,
    //CanonFiRawJpgQuality = 0x0006,
    //CanonFiRawJpgSize = 0x0007,
    //CanonFiNoiseReduction = 0x0008,
    //CanonFiWBBracketMode = 0x0009,
    //CanonFiWBBracketValueAB = 0x000c,
    //CanonFiWBBracketValueGM = 0x000d,
    //CanonFiFilterEffect = 0x000e,
    //CanonFiToningEffect = 0x000f,
    //CanonFiMacroMagnification = 0x0010,
    //CanonFiLiveViewShooting = 0x0013,
    //CanonFiFocusDistanceUpper = 0x0014,
    //CanonFiFocusDistanceLower = 0x0015,
    //CanonFiFlashExposureLock = 0x0019,    

    //#endregion

  }
}
