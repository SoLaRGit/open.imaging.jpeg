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
using System.Linq;
using System.Text;

using open.imaging.icc.types;
using System.Xml.Serialization;
using System.ComponentModel;

namespace open.imaging.icc
{
  [XmlRoot("iccTag")]
  [XmlType("iccTag")]
  public class ICCTag
  {
    [XmlIgnore]
    public uint signature;

    [XmlIgnore]
    public uint offset;

    [XmlIgnore]
    public uint length;

    [XmlElement("chromacityType", typeof(ICCTagDataTypeChromacity))]
    [XmlElement("colorantOrderType", typeof(ICCTagDataTypeColorantOrder))]
    [XmlElement("curveType", typeof(ICCTagDataTypeCurve))]
    [XmlElement("dataType", typeof(ICCTagDataTypeData))]
    [XmlElement("datetimeType", typeof(ICCTagDataTypeDateTime))]
    [XmlElement("descriptionType", typeof(ICCTagDataTypeDescription))]
    [XmlElement("lut16Type", typeof(ICCTagDataTypeLut16))]
    [XmlElement("lut8Type", typeof(ICCTagDataTypeLut8))]
    [XmlElement("lutAToBType", typeof(ICCTagDataTypeLutAToB))]
    [XmlElement("lutBToAType", typeof(ICCTagDataTypeLutBToA))]
    [XmlElement("measurementType", typeof(ICCTagDataTypeMeasurement))]
    [XmlElement("multiLocalizedUnicodeType", typeof(ICCTagDataTypeMultiLocalizedUnicode))]
    [XmlElement("multiProcessElementsType", typeof(ICCTagDataTypeMultiProcessElements))]
    [XmlElement("parametricCurveType", typeof(ICCTagDataTypeParametricCurve))]
    [XmlElement("s15Fixed16ArrayType", typeof(ICCTagDataTypeS15Fixed16Array))]
    [XmlElement("signatureType", typeof(ICCTagDataTypeSignature))]
    [XmlElement("textType", typeof(ICCTagDataTypeText))]
    [XmlElement("viewingConditionsType", typeof(ICCTagDataTypeViewingConditions))]
    [XmlElement("XYZType", typeof(ICCTagDataTypeXYZ))]
    [XmlElement("baseType")]
    public ICCTagData data;

    public ICCTag()
    { }

    [XmlIgnore]
    public string signatureString
    {
      get { return signature.chararray_as_str(); }
      set { this.signature = value.chararray_to_u4(); }
    }

    [XmlIgnore]
    public ICCTagType tagType
    {
      get { return (ICCTagType)signature; }
      set { /* ignore */ }
    }

    [XmlAttribute(AttributeName = "s")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_signature
    {
      get { return string.Format("{0:x8}", signature); }
      set { signature = value.hex_to_u4(); }
    }

    [XmlAttribute(AttributeName = "o")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_offset
    {
      get { return string.Format("{0:x4}", offset); }
      set { offset = value.hex_to_u4(); }
    }

    [XmlAttribute(AttributeName = "l")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_length
    {
      get { return string.Format("{0:x4}", length); }
      set { length = value.hex_to_u4(); }
    }

    [XmlAttribute(AttributeName = "ss")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_signatureString
    {
      get { return signature.chararray_as_str(); }
      set { /*ignore*/ }
    }

    [XmlAttribute(AttributeName = "tt")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_signatureType
    {
      get { return ((ICCTagType)signature).ToString(); }
      set { /*ignore*/ }
    }

    public override string ToString()
    {
      return string.Format("'{0}':{1}, O:{2}, L:{3}, [{4}]", this.signatureString, this.tagType, this.offset, this.length, data != null ? data.ToString() : "<NULL>");
    }    


  }

  [XmlRoot("iccTags")]
  [XmlType("iccTags")]
  public class ICCTags : List<ICCTag>
  {
    public uint TagCount;
  }

  #region ### VALIDATION ###
#if false
  public class ICCTagValidationItem
  {
    public ICCTagType TagType;
    public List<ICCTagDataType> TagDataTypes;

    public ICCTagValidationItem(ICCTagType TagType, ICCTagDataType[] TagDataTypes)
    {
      this.TagType = TagType;
      this.TagDataTypes = new List<ICCTagDataType>(TagDataTypes);
    }
  }

  public static class ICCTagValidation
  {
    public static List<ICCTagValidationItem> ICC1v43_2010_12 = new List<ICCTagValidationItem>(
      new ICCTagValidationItem[]
      {

        new ICCTagValidationItem(ICCTagType.AToB0Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type, 
                                                       ICCTagDataType.lutAToBType }),
        new ICCTagValidationItem(ICCTagType.AToB1Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type, 
                                                       ICCTagDataType.lutAToBType }),
        new ICCTagValidationItem(ICCTagType.AToB2Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type, 
                                                       ICCTagDataType.lutAToBType }),

        new ICCTagValidationItem(ICCTagType.blueMatrixColumnTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),

        new ICCTagValidationItem(ICCTagType.blueTRCTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.curveType, 
                                                       ICCTagDataType.parametricCurveType }),
        new ICCTagValidationItem(ICCTagType.BToA0Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type, 
                                                       ICCTagDataType.lutBToAType }),
        new ICCTagValidationItem(ICCTagType.BToA1Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type, 
                                                       ICCTagDataType.lutBToAType }),
        new ICCTagValidationItem(ICCTagType.BToA2Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.lut8Type, 
                                                       ICCTagDataType.lut16Type,
                                                       ICCTagDataType.lutBToAType }),
        new ICCTagValidationItem(ICCTagType.BToD0Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiProcessElementsType }),

        new ICCTagValidationItem(ICCTagType.BToD1Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiProcessElementsType }),

        new ICCTagValidationItem(ICCTagType.BToD2Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiProcessElementsType }),

        new ICCTagValidationItem(ICCTagType.BToD3Tag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiProcessElementsType }),

        new ICCTagValidationItem(ICCTagType.calibrationDateTimeTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.dateTimeType }),

        new ICCTagValidationItem(ICCTagType.charTargetTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.textType }),

        new ICCTagValidationItem(ICCTagType.chromaticAdaptationTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.s15Fixed16ArrayType }),


        new ICCTagValidationItem(ICCTagType.copyrightTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiLocalizedUnicodeType }),

        new ICCTagValidationItem(ICCTagType.deviceMfgDescTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiLocalizedUnicodeType }),

        new ICCTagValidationItem(ICCTagType.deviceModelDescTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiLocalizedUnicodeType }),

        new ICCTagValidationItem(ICCTagType.greenMatrixColumnTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),

        new ICCTagValidationItem(ICCTagType.greenTRCTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.curveType,
                                                       ICCTagDataType.parametricCurveType }),

        new ICCTagValidationItem(ICCTagType.luminanceTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),

        new ICCTagValidationItem(ICCTagType.measurementTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.measurementType }),

        new ICCTagValidationItem(ICCTagType.mediaWhitePointTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),



        new ICCTagValidationItem(ICCTagType.profileDescriptionTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiLocalizedUnicodeType }),

        new ICCTagValidationItem(ICCTagType.redMatrixColumnTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),

        new ICCTagValidationItem(ICCTagType.redTRCTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.curveType,
                                                       ICCTagDataType.parametricCurveType }),

        new ICCTagValidationItem(ICCTagType.saturationRenderingIntentGamutTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.signatureType }),

        new ICCTagValidationItem(ICCTagType.technologyTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.signatureType }),

        new ICCTagValidationItem(ICCTagType.viewingCondDescTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.multiLocalizedUnicodeType }),

        new ICCTagValidationItem(ICCTagType.viewingConditionsTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.viewingConditionsType }),

  #region ### UNDOCUMENTED ###

        new ICCTagValidationItem(ICCTagType.mediaBlackPointTag, 
                                 new ICCTagDataType[]{ ICCTagDataType.XYZType }),

        #endregion
      });
  }
#endif
  public enum ICCTagType : uint
  {
    /// <summary>
    /// 9.2.1 AToB0Tag
    /// <remarks>
    /// This tag defines a colour transform from Device, Colour Encoding or PCS, to PCS, or a colour transform from Device 1 to
    /// Device 2, using lookup table tag element structures. For most profile classes it defines the transform to achieve perceptual 
    /// rendering (see Table 25). The processing mechanisms are described in lut8Type or lut16Type or lutAToBType (see 10.8, 10.9 and 10.10).
    /// </remarks>
    /// </summary>
    AToB0Tag = 0x41324230, // 'A2B0'

    /// <summary>
    /// 9.2.2 AToB1Tag
    /// <remarks>
    /// This tag describes the colour transform from Device or Colour Encoding to PCS using lookup table tag element structures. 
    /// For most profile classes, it defines the transform to achieve colorimetric rendering (see Table 25). The processing mechanisms 
    /// are described in lut8Type or lut16Type or lutAToBType (see 10.8, 10.9 and 10.10).
    /// </remarks>
    /// </summary>
    AToB1Tag = 0x41324231, // 'A2B1'

    /// <summary>
    /// 9.2.3 AToB1Tag
    /// <remarks>
    /// This tag describes the colour transform from Device or Colour Encoding to PCS using lookup table tag element structures. 
    /// For most profile classes it defines the transform to achieve saturation rendering (see Table 25). The processing mechanisms are 
    /// described in lut8Type or lut16Type or lutAToBType (see 10.8, 10.9 and 10.10).
    /// </remarks>
    /// </summary>
    AToB2Tag = 0x41324232, // 'A2B2'

    /// <summary>
    /// 9.2.4 blueMatrixColumnTag
    /// <remarks>This tag contains the third column in the matrix used in matrix/TRC transforms.</remarks>
    /// </summary>
    blueMatrixColumnTag = 0x6258595A, // 'bXYZ'

    /// <summary>
    /// 9.2.5 blueTRCTag
    /// <remarks>This tag contains the blue channel tone reproduction curve. The first element represents no colorant (white) or phosphor (black) and the last element represents 100 % colorant (blue) or 100 % phosphor (blue).</remarks>
    /// </summary>
    blueTRCTag = 0x62545243, // 'bTRC'

    /// <summary>
    /// 9.2.6 BToA0Tag
    /// <remarks>This tag defines a colour transform from PCS to Device or Colour Encoding using the lookup table tag element structures. For most profile classes, it defines the transform to achieve perceptual rendering (see Table 25). The processing mechanisms are described in lut8Type or lut16Type or lutBToAType (see 10.8, 10.9 and 10.11).</remarks>
    /// </summary>
    BToA0Tag = 0x42324130, // 'B2A0'

    /// <summary>
    /// 9.2.7 BToA0Tag
    /// <remarks>This tag defines a colour transform from PCS to Device or Colour Encoding using the lookup table tag element structures. For most profile classes it defines the transform to achieve colorimetric rendering (see Table 25). The processing mechanisms are described in lut8Type or lut16Type or lutBToAType (see 10.8, 10.9 and 10.11).</remarks>
    /// </summary>
    BToA1Tag = 0x42324131, // 'B2A1'

    /// <summary>
    /// 9.2.8 BToA0Tag
    /// <remarks>This tag defines a colour transform from PCS to Device or Colour Encoding using the lookup table tag element structures. For most profile classes it defines the transform to achieve saturation rendering (see Table 25). The processing mechanisms are described in lut8Type or lut16Type or lutBToAType (see 10.8, 10.9 and 10.11).</remarks>
    /// </summary>
    BToA2Tag = 0x42324132, // 'B2A2'

    /// <summary>
    /// 9.2.9 BToD0Tag
    /// <remarks>This tag defines a colour transform from PCS to Device. It supports float32Number-encoded input range, output range and transform, and provides a means to override the BToA0Tag. As with the BToA0Tag, it defines a transform to achieve a perceptual rendering. The processing mechanism is described in multiProcessElementsType (see 10.14).</remarks>
    /// </summary>
    BToD0Tag = 0x42324430, // 'B2D0'

    /// <summary>
    /// 9.2.10 BToD1Tag
    /// <remarks>This tag defines a colour transform from PCS to Device. It supports float32Number-encoded input range, output range and transform, and provides a means to override the BToA1Tag. As with the BToA1Tag, it defines a transform to achieve a media-relative colorimetric rendering. The processing mechanism is described in multiProcessElementsType (see 10.14).</remarks>
    /// </summary>
    BToD1Tag = 0x42324431, // 'B2D0'

    /// <summary>
    /// 9.2.11 BToD2Tag
    /// <remarks>This tag defines a colour transform from PCS to Device. It supports float32Number-encoded input range, output range and transform, and provides a means to override the BToA2Tag. As with the BToA2Tag, it defines a transform to achieve a saturation rendering. The processing mechanism is described in multiProcessElementsType (see 10.14).</remarks>
    /// </summary>
    BToD2Tag = 0x42324432, // 'B2D0'

    /// <summary>
    /// 9.2.12 BToD3Tag
    /// <remarks>This tag defines a colour transform from PCS to Device. It supports float32Number-encoded input range, output range and transform, and provides a means to override the BToA1Tag and associated ICC-absolute colorimetric rendering intent processing. As with the BToA1Tag and associated ICC-absolute colorimetric rendering intent processing, it defines a transform to achieve an ICC-absolute colorimetric rendering. The processing mechanism is described in multiProcessElementsType (see 10.14).</remarks>
    /// </summary>
    BToD3Tag = 0x42324433, // 'B2D0'

    /// <summary>
    /// 9.2.13 calibrationDateTimeTag
    /// <remarks>This tag contains the profile calibration date and time. This allows applications and utilities to verify if this profile matches a vendor’s profile and how recently calibration has been performed.</remarks>
    /// </summary>
    calibrationDateTimeTag = 0x63616C74, // 'calt'

    /// <summary>
    /// 9.2.14 charTargetTag
    /// <remarks>
    /// This tag contains the name of the registered characterization data set, or it contains the measurement data for a characterization target. This tag is provided so that distributed utilities can identify the underlying characterization data, create transforms “on the fly” or check the current performance against the original device performance.
    /// The first seven characters of the text shall identify the nature of the characterization data.
    /// </remarks>
    /// </summary>
    charTargetTag = 0x74617267, // 'targ'

    /// <summary>
    /// 9.2.15 chromaticAdaptationTag
    /// <remarks>
    /// This tag contains a matrix, which shall be invertible, and which converts an nCIEXYZ colour, measured using the actual illumination conditions and relative to the actual adopted white, to an nCIEXYZ colour relative to the PCS adopted white, with complete adaptation from the actual adopted white chromaticity to the PCS adopted white chromaticity.
    /// The tag reflects a survey of the currently used methods of conversion, all of which can be formulated as a matrix transformation. The Bradford transform (see Annex E) is recommended for ICC profiles.
    /// </remarks>
    /// </summary>
    chromaticAdaptationTag = 0x63686164, // 'chad'

    /// <summary>
    /// 9.2.21 copyrightTag
    /// <remarks>This tag contains the text copyright information for the profile.</remarks>
    /// </summary>
    copyrightTag = 0x63707274, // 'cprt'

    /// <summary>
    /// 9.2.22 deviceMfgDescTag
    /// <remarks>This tag describes the structure containing invariant and localizable versions of the device manufacturer for display. The content of this structure is described in 10.13.</remarks>
    /// </summary>
    deviceMfgDescTag = 0x646D6E64, // 'dmnd'

    /// <summary>
    /// 9.2.23 deviceModelDescTag
    /// <remarks>This tag describes the structure containing invariant and localizable versions of the device model for display. The content of this structure is described in 10.13.</remarks>
    /// </summary>
    deviceModelDescTag = 0x646D6464, // 'dmdd'

    /// <summary>
    /// 9.2.30 greenMatrixColumnTag
    /// <remarks>This tag contains the second column in the matrix, which is used in matrix/TRC transforms.</remarks>
    /// </summary>
    greenMatrixColumnTag = 0x6758595A, // 'gXYZ'

    /// <summary>
    /// 9.2.31 greenTRCTag
    /// <remarks>This tag contains the green channel tone reproduction curve. The first element represents no colorant (white) or phosphor (black) and the last element represents 100 % colorant (green) or 100 % phosphor (green).</remarks>
    /// </summary>
    greenTRCTag = 0x67545243, // 'gTRC'

    /// <summary>
    /// 9.2.32 luminanceTag
    /// <remarks>
    /// This tag contains the absolute luminance of emissive devices in candelas per square metre as described by the Y channel.
    /// NOTE The X and Z values are set to zero.
    /// </remarks>
    /// </summary>
    luminanceTag = 0x6C756D69, // 'lumi'

    /// <summary>
    /// 9.2.33 measurementTag
    /// <remarks>This tag describes the alternative measurement specification, such as a D65 illuminant instead of the default D50.</remarks>
    /// </summary>
    measurementTag = 0x6D656173, // 'meas'

    /// <summary>
    /// 9.2.34 mediaWhitePointTag
    /// <remarks>
    /// This tag, which is used for generating the ICC-absolute colorimetric intent, specifies the chromatically adapted nCIEXYZ tristimulus values of the media white point. When the measurement data used to create the profile were specified relative to an adopted white with a chromaticity different from that of the PCS adopted white, the media white point nCIEXYZ values shall be adapted to be relative to the PCS adopted white chromaticity using the chromaticAdaptationTag matrix, before recording in the tag. For capture devices, the media white point is the encoding maximum white for the capture encoding. For displays, the values specified shall be those of the PCS illuminant as defined in 7.2.16.
    /// See Clause 6 and Annex A for a more complete description of the use of the media white point.
    /// </remarks>
    /// </summary>
    mediaWhitePointTag = 0x77747074, // 'wtpt'

    /// <summary>
    /// 9.2.41 profileDescriptionTag
    /// <remarks>
    /// This tag describes the structure containing invariant and localizable versions of the profile description for display. The content of this structure is described in 10.13. This invariant description has no fixed relationship to the actual profile disk file name.
    /// NOTE It is helpful if an identification of the characterization data that was used in the creation of the profile is included in the profileDescriptionTag (e.g. “based on CGATS TR 001”)[10]. See also 8.2.11.
    /// </remarks>
    /// </summary>
    profileDescriptionTag = 0x64657363, // 'desc'

    /// <summary>
    /// 9.2.44 redMatrixColumnTag
    /// <remarks>This tag contains the first column in the matrix, which is used in matrix/TRC transforms.</remarks>
    /// </summary>    
    redMatrixColumnTag = 0x7258595A, // 'rXYZ'

    /// <summary>
    /// 9.2.45 redTRCTag
    /// <remarks>This tag contains the red channel tone reproduction curve. The first element represents no colorant (white) or phosphor (black) and the last element represents 100 % colorant (red) or 100 % phosphor (red).</remarks>
    /// </summary>    
    redTRCTag = 0x72545243, // 'rTRC'

    /// <summary>
    /// 9.2.46 saturationRenderingIntentGamutTag
    /// <remarks>There is only one standard reference medium gamut, as defined in ISO 12640-3. When the signature is present, the specified gamut is defined to be the reference medium gamut for the PCS side of both the A2B2 and B2A2 tags, if they are present. If this tag is not present, the saturation rendering intent reference gamut is unspecified. The standard PCS reference medium gamut signatures that shall be used are listed in Table 28.</remarks>
    /// </summary>    
    saturationRenderingIntentGamutTag = 0x72696732, // 'rig2'

    /// <summary>
    /// 9.2.47 technologyTag
    /// <remarks>The device technology signatures that shall be used are listed in Table 29.</remarks>
    /// </summary>    
    technologyTag = 0x74656368, // 'tech'

    /// <summary>
    /// 9.2.48 viewingCondDescTag
    /// <remarks>This tag describes the structure containing invariant and localizable versions of the viewing conditions. The content of this structure is described in 10.13.</remarks>
    /// </summary>    
    viewingCondDescTag = 0x76756564, // 'vued'

    /// <summary>
    /// 9.2.49 viewingConditionsTag
    /// <remarks>This tag defines the viewing conditions parameters. The content of this structure is described in 10.28.</remarks>
    /// </summary>    
    viewingConditionsTag = 0x76696577, // 'view'

    #region ### UNDOCUMENTED ###

    /// <summary>
    /// signatures index: ICC 626b7074 'bkpt' Tag
    /// </summary>
    mediaBlackPointTag = 0x626b7074, //'bkpt'

    #endregion

  }
#if false
  public enum ICCTagDataType : uint
  {
    lut8Type = ICCTagDataTypeLut8.ID,
    lut16Type = ICCTagDataTypeLut16.ID,
    lutAToBType = ICCTagDataTypeLutAToB.ID,
    lutBToAType = ICCTagDataTypeLutBToA.ID,    
    multiLocalizedUnicodeType = ICCTagDataTypeMultiLocalizedUnicode.ID,
    XYZType = ICCTagDataTypeXYZ.ID,
    curveType = ICCTagDataTypeCurve.ID,
    parametricCurveType = ICCTagDataTypeParametricCurve.ID,
    multiProcessElementsType = ICCTagDataTypeMultiProcessElements.ID,
    dateTimeType = ICCTagDataTypeDateTime.ID,
    textType = ICCTagDataTypeText.ID,
    s15Fixed16ArrayType = ICCTagDataTypeS15Fixed16Array.ID,
    measurementType = ICCTagDataTypeMeasurement.ID,
    viewingConditionsType = ICCTagDataTypeViewingConditions.ID,
    signatureType = ICCTagDataTypeSignature.ID,
  }
#endif
  #endregion

}
