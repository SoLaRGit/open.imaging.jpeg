using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace open.source.imaging.jpeg.markersffffffffffffffff
{
  class OpenJpegMarkers
  {
  }

  public class APP1
  {
    public ushort length;
    public int ExifIdentCode;
    public int TiffHeader;
    public int IFD0th;
    public int IFD0thValue;
    public int IFD1th;
    public int IFD1thValue;
    public int IFD0thImageData;
  }

  public enum IFDType : ushort  
  {
    /// <summary>An 8-bit unsigned integer.</summary>
    BYTE = 1,
    /// <summary>An 8-bit byte containing one 7-bit ASCII code. The final byte is terminated with NULL.</summary>
    ASCII = 2,
    /// <summary>A 16-bit (2-byte) unsigned integer.</summary>
    SHORT = 3,
    /// <summary>A 32-bit (4-byte) unsigned integer.</summary>
    LONG = 4,
    /// <summary>Two LONG's (8-byte). the first represents the numerator of a fraction; the second, the denominator.</summary>
    RATIONAL = 5,
    /// <summary>An 8-bit signed integer.</summary>
    SBYTE = 6,
    /// <summary>An 8-bit byte that may take any value depending on the field definition.</summary>
    UNDEFINED = 7,
    /// <summary>A 16-bit (2-byte) signed integer.</summary>
    SSHORT = 8,
    /// <summary>A 32-bit (4-byte) signed integer (2's complement notation).</summary>    
    SLONG = 9,
    /// <summary>Two SLONG's (8-byte). the first represents the numerator of a fraction, the second the denominator.</summary>
    SRATIONAL = 10,
    /// <summary>Single precision (4-byte) IEEE format.</summary>
    FLOAT = 11,
    /// <summary>Double precision (8-byte) IEEE format.</summary>
    DOUBLE = 12,
    /// <summary>Adobe Type, content is IFD</summary>
    IFD = 13
  }

  /// <summary>
  /// Tags TIFF/EXIF taken from DC-008-2012_E (Exif v2.3 standard)
  /// </summary>
  public enum Tags : ushort
  {
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
    YCbCrPositioning = 0x0212,
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
    #region ### Exif.G - Tags relating to Picture-Taking Conditions ###
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
    #region ### 4.6.7 Interoperability IFD Attribute Information (Table 16) ###
    /// <summary>IIFD.A - Interoperability Index - ASCII (count Any)</summary>
    InteroperabilityIndex = 0x01,
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
    #region ### TIFF Adobe PageMaker Tags ###

    AdobePMSubIFDs = 0x014A,

    AdobePMClipPath = 0x0157,
    AdobePMClipPathUnits = 0x0158,

    #endregion
  }

  public class CharacterCodes
  {
    /// <summary>ITU-T T.50 IA5</summary>
    public static byte[] ASCII      = { 0x41, 0x53, 0x43, 0x49, 0x49, 0x00, 0x00, 0x00 };
    /// <summary>JIS X208-1990</summary>
    public static byte[] JIS        = { 0x4a, 0x49, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00 };
    /// <summary>Unicode Standard</summary>
    public static byte[] Unicode    = { 0x55, 0x4e, 0x49, 0x43, 0x4f, 0x44, 0x45, 0x00 };
    /// <summary>Undefined</summary>
    public static byte[] Undefined  = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
  }

  public enum IFDAttr : ushort
  { 
  
  }

  public class IFDEntry
  {
    #region ### TIFF IFD ENTRY ###

    /// <summary>Item tag</summary>
    public Tags tag;
    /// <summary>Item element type</summary>
    public IFDType type;
    /// <summary>Number of elements (not the size of value).</summary>
    public uint vcount;

    /// <summary>Offset from TIFF header to a value it self is recorded. 
    /// In case where value is smaller than 4 bytes, the value it self is recorded.
    /// depending on the format of machine creating this (big endian, small endian).</summary>
    public uint voffset;

    #endregion

    /// <summary>IFD entry value bytes</summary>
    public byte[] v;

    /// <summary>IFD entry value byte size</summary>
    public uint vBytes;
    
    public IFDEntry()
    { }

    public IFDEntry(Tags tag, IFDType type, uint count, uint value)
    {
      this.tag = tag;
      this.type = type;
      this.vcount = count;
      this.voffset = value;      
    }

    public IFDEntry(Tags tag, IFDType type, uint count, byte[] value)
    {
      this.tag = tag;
      this.type = type;
      this.vcount = count;
      this.v = value;
    }

    private const int SIZE_SHORT      = sizeof(ushort);
    private const int SIZE_LONG       = sizeof(uint);
    private const int SIZE_SLONG      = sizeof(int);
    private const int SIZE_RATIONAL   = sizeof(double);
    private const int SIZE_SRATIONAL  = sizeof(double);
    private const int SIZE_DOUBLE     = sizeof(double);
    private const int SIZE_FLOAT      = sizeof(float);

    public string getString()
    {
      return System.Text.ASCIIEncoding.ASCII.GetString(v, 0, (int)(vBytes - 1));
    }

    public ushort getSHORT(int index)
    {
      return BitConverter.ToUInt16(v, index * SIZE_SHORT);
    }
    
    public uint getLONG(int index)
    {
      return BitConverter.ToUInt32(v, index * SIZE_LONG);
    }

    public int getSLONG(int index)
    {
      return BitConverter.ToInt32(v, index * SIZE_SLONG);
    }

    public double getRATIONAL(int index)
    {
      int vindex = index * SIZE_RATIONAL;
      return (double)BitConverter.ToUInt32(v, vindex) / (double)BitConverter.ToUInt32(v, vindex + 4);
    }

    public double getSRATIONAL(int index)
    {
      int vindex = index * SIZE_SRATIONAL;
      return (double)BitConverter.ToInt32(v, vindex) / (double)BitConverter.ToInt32(v, vindex + 4);
    }

    public double getDOUBLE(int index)
    {
      return BitConverter.ToDouble(v, index * SIZE_DOUBLE);
    }

    public float getFLOAT(int index)
    {
      return BitConverter.ToSingle(v, index * SIZE_FLOAT);
    }

    public object get(int index)
    {
      switch (type)
      {
        case IFDType.ASCII: return getString();
        case IFDType.BYTE: return getString();
        case IFDType.SHORT: return getSHORT(index);
        case IFDType.LONG: return getLONG(index);
        case IFDType.SLONG: return getSLONG(index);
        case IFDType.RATIONAL: return getRATIONAL(index);
        case IFDType.SRATIONAL: return getSRATIONAL(index);
        case IFDType.UNDEFINED: return getString();
        case IFDType.DOUBLE: return getDOUBLE(index);
        case IFDType.FLOAT: return getFLOAT(index);
      }
      return "???";
    }

    public void setSHORT(int index, ushort value)
    {
      byte[] b = BitConverter.GetBytes(value);
      int minlength = ((index + 1) * SIZE_SHORT);
      if (v.Length < minlength) Array.Resize<byte>(ref v, minlength);
      Buffer.BlockCopy(b, 0, v, index * SIZE_SHORT, SIZE_SHORT);
    }

    public void setLONG(int index, uint value)
    {
      byte[] b = BitConverter.GetBytes(value);
      int minlength = ((index + 1) * SIZE_LONG);
      if (v.Length < minlength) Array.Resize<byte>(ref v, minlength);
      Buffer.BlockCopy(b, 0, v, index * SIZE_LONG, SIZE_LONG);
    }

    public void setSLONG(int index, int value)
    {
      byte[] b = BitConverter.GetBytes(value);
      int minlength = ((index + 1) * SIZE_SLONG);
      if (v.Length < minlength) Array.Resize<byte>(ref v, minlength);
      Buffer.BlockCopy(b, 0, v, index * SIZE_SLONG, SIZE_SLONG);
    }

    public void setDOUBLE(int index, double value)
    {
      byte[] b = BitConverter.GetBytes(value);
      int minlength = ((index + 1) * SIZE_DOUBLE);
      if (v.Length < minlength) Array.Resize<byte>(ref v, minlength);
      Buffer.BlockCopy(b, 0, v, index * SIZE_DOUBLE, SIZE_DOUBLE);
    }

    public void setFLOAT(int index, float value)
    {
      byte[] b = BitConverter.GetBytes(value);
      int minlength = ((index + 1) * SIZE_FLOAT);
      if (v.Length < minlength) Array.Resize<byte>(ref v, minlength);
      Buffer.BlockCopy(b, 0, v, index * SIZE_FLOAT, SIZE_FLOAT);
    }

    public void set(int index, object value)
    {
      switch (type)
      {
        //case IFDType.ASCII: return setString();
        //case IFDType.BYTE: return getString();
        case IFDType.SHORT: setSHORT(index, unchecked((ushort)value)); return;
        case IFDType.LONG: setLONG(index, unchecked((uint)value)); return;
        case IFDType.SLONG: setSLONG(index, unchecked((int)value)); return;
        // case IFDType.RATIONAL: setRATIONAL(unchecked((double)value)); return;
        // case IFDType.SRATIONAL: setSRATIONAL(unchecked((double)value)); return;
        //case IFDType.UNDEFINED: return getString(); return;
        case IFDType.DOUBLE: setDOUBLE(index, unchecked((double)value)); return;
        case IFDType.FLOAT: setFLOAT(index, unchecked((float)value)); return;
      }      
    }

    public override string ToString()
    {
      return "{ tag(0x" + ((ushort)tag).ToString("X4") + "): " + tag + ", type:" + type + "[" + vcount + "], value:" + get(0) + " }";
    }
  }

  

  public class IFD : List<IFDEntry>
  {
    BinaryReader br;

    public byte[] signature;
    public long address;
    public ushort length;

    /// <summary>
    /// 4949 LSB -> MSB (little endian), 4D4D  MSB->LSB (big endian)
    /// </summary>
    public ushort byteorder;

    /// <summary>42 An arbitrary but carefully chosen number (42) that further identifies the file as a TIFF file.</summary>
    public ushort arbitarynumber;

    /// <summary>The offset (in bytes) of the first IFD.</summary>
    public uint offset1IFD;

    public ushort entrycount;

    public IFD(byte[] input, ushort length)      
    {
      using (MemoryStream ms = new MemoryStream(input))
      {
        ms.Position = 0;
        br = new BinaryReader(ms);

        this.address = ms.Position;
        this.length = length;

        signature = br.ReadBytes(6);
        
        // Image File Header - TIFF rev 6 standard
        //-----------------------------------------------
        byteorder = read_ui2();       // 2 bytes
        arbitarynumber = read_ui2();  // 2 bytes
        offset1IFD = read_ui4();      // 4 bytes
        //-----------------------------------------------
        ms.Position = 6 + offset1IFD; // goto to IFD offset

        // Image File Directory - TIFF rev 6 standard
        //-----------------------------------------------
        entrycount = read_ui2();      // 2 bytes
        //- repeat for each entry --- // 12 bytes (less value > 4 bytes reading part)
        for (int i = 0; i < entrycount; i++)
        {
          // 12 bytes ???
          IFDEntry entry = new IFDEntry();
          entry.tag = this.Read_Tag();
          entry.type = this.Read_IFDType();
          entry.vcount = this.read_ui4();
          entry.vBytes = getValueSize(entry.type, entry.vcount);
          if (entry.vBytes <= 4)
          {
            // offset is value // they must complicate things even more         
            entry.voffset = br.ReadUInt32();
            switch (entry.type)
            {
              case IFDType.ASCII:
              case IFDType.UNDEFINED:
              case IFDType.BYTE: 
              case IFDType.SBYTE:
              case IFDType.LONG:
              case IFDType.SLONG:
                entry.v = BitConverter.GetBytes(entry.voffset);
                Array.Resize<byte>(ref entry.v, (int)entry.vBytes);
                break;
              case IFDType.SSHORT:
              case IFDType.SHORT:
                entry.v = BitConverter.GetBytes((ushort)entry.voffset);
                break;
              case IFDType.FLOAT:
                entry.v = BitConverter.GetBytes((float)entry.voffset);
                break;                
            }
            // else can't fit 4-byte of offset, or unsupported type
          }
          else
          {            
            // value is at offset .. (jump trough position hoops..)
            entry.voffset = br.ReadUInt32();
            long address_current = ms.Position;         // remmember previous position
            ms.Position = 6 + entry.voffset;            // set offset
            entry.v = br.ReadBytes((int)entry.vBytes);  // read bytes (v is directly mapped)
            ms.Position = address_current;              // restore previous position
          }
          this.Add(entry);
        }
        //-----------------------------------------------
      }
    }

    /// <summary>
    /// not implemented yet
    /// </summary>
    /// <returns></returns>
    public byte[] GetBytes()
    {
      // example how to determine hardware endian
      this.byteorder = (ushort)(BitConverter.IsLittleEndian ? 0x4949 : 0x4d4d);


      throw (new NotImplementedException());
    }

    public uint getValueSize(IFDType type, uint count)
    {
      switch (type)
      {
        case IFDType.BYTE: return (count);
        case IFDType.SBYTE: return (count);
        case IFDType.ASCII: return (count);
        case IFDType.SHORT: return (count << 1); // * 2
        case IFDType.SSHORT: return (count << 1); // * 2
        case IFDType.LONG: return (count << 2); // * 4
        case IFDType.SLONG: return (count << 2); // * 4
        case IFDType.RATIONAL: return (count << 3); // * 8
        case IFDType.UNDEFINED: return (count);        
        case IFDType.SRATIONAL: return (count << 3); // * 8
        case IFDType.FLOAT: return (count << 2); // * 4
        case IFDType.DOUBLE: return (count << 3); // * 8
        case IFDType.IFD:
          // this is actually funny there is no fixed size I can return (must calc)
          Debug.Assert(false);
          return (count); 
        default:
          // error - ouch, someone wants to tell us there is new new new new TYPE?
          // ... all tags must be ordered by tag (ushort) value order ...  
          // and now how can we skip it if we do not know what sizeof(TYPE) is?          
          // dudes EXIF standard sucks multiple times on multiple levels.
          // someone needs to make some rules in this orderly dissorder.
          Debug.Assert(false);
          return 0;
      }
    }

    public ushort read_ui2()
    {
      UInt16 value = br.ReadUInt16();
      if (byteorder == 0x4949) return value;
      value = (ushort)(((value & 0xff00) >> 8) | ((value & 0x00ff) << 8));
      return value;    
    }
    public uint read_ui4()
    {
      uint value = br.ReadUInt32();
      if (byteorder == 0x4949) return value;
      value = (uint)(((value & 0xff000000) >> 24) | ((value & 0x00ff0000) >> 8) |
                     ((value & 0x0000ff00) << 8) | ((value & 0x000000ff) << 24));
      return value;    
    }
    public Tags Read_Tag()
    {
      return (Tags)read_ui2();
    }

    public IFDType Read_IFDType()
    {
      return (IFDType)read_ui2();
    }

  }

}
