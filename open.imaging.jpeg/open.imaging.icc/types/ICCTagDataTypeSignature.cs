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
using System.Xml.Serialization;

namespace open.imaging.icc.types
{
  /// <summary>
  /// 10.21 signatureType
  /// <remarks>Table 74 — signatureType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeSignature")]
  [XmlType("ICCTagDataTypeSignature")]
  public class ICCTagDataTypeSignature: ICCTagData
  {
    public const uint ID = 0x73696720; // 'sig '

    public ICCTagDataTypeSignature()
    { }

    public ICCTagDataTypeSignature(byte[] data)
      : base(data)
    { }
    
    public new uint value
    {
      get { return base.get_u4(8); }
      set { base.set_u4(8, value); NotifyPropertyChanged(); }
    }

    public string valueName
    {
      get
      {
        unchecked
        {
          return ASCIIEncoding.ASCII.GetString(base.data, 8, 4);
        }
      }
      set
      {
        unchecked
        {
          base.data[8] = (byte)(value[0]);
          base.data[9] = (byte)(value[1]);
          base.data[10] = (byte)(value[2]);
          base.data[11] = (byte)(value[3]);
        }
        NotifyPropertyChanged();
      }
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeSignature:'{0}':{1,8:X8}:'{2}'", typeName, value, valueName);
    }
  }

  /// <summary>
  /// Table 29 — Technology signatures
  /// </summary>
  public enum ICCTechnologySignatures : uint
  {
    FilmScanner = 0x6673636E, // ‘fscn’
    DigitalCamera = 0x6463616D, // ‘dcam’
    ReflectiveScanner = 0x7273636E, // ‘rscn’
    InkJetPrinter = 0x696A6574, // ‘ijet’
    ThermalWaxPrinter = 0x74776178, // ‘twax’
    ElectrophotographicPrinter = 0x6570686F, // ‘epho’
    ElectrostaticPrinter = 0x65737461, // ‘esta’
    DyeSublimationPrinter = 0x64737562, // ‘dsub’
    PhotographicPaperPrinter = 0x7270686F, // ‘rpho’
    FilmWriter = 0x6670726E, // ‘fprn’
    VideoMonitor = 0x7669646D, // ‘vidm’
    VideoCamera = 0x76696463, // ‘vidc’
    ProjectionTelevision = 0x706A7476, // ‘pjtv’
    CathodeRayTubeDisplay = 0x43525420, // ‘CRT ’
    PassiveMatrixDisplay = 0x504D4420, // ‘PMD ’
    ActiveMatrixDisplay = 0x414D4420, // ‘AMD ’
    PhotoCD = 0x4B504344, // ‘KPCD’
    PhotographicImageSetter = 0x696D6773, // ‘imgs’
    Gravure = 0x67726176, // ‘grav’
    OffsetLithography = 0x6F666673, // ‘offs’
    Silkscreen = 0x73696C6B, // ‘silk’
    Flexography = 0x666C6578, // ‘flex’
    MotionPictureFilmScanner = 0x6D706673, // ‘mpfs’
    MotionPictureFilmRecorder = 0x6D706672, // ‘mpfr’
    DigitalMotionPictureCamera = 0x646D7063, // ‘dmpc’
    DigitalCinemaProjector = 0x64636A70, // ‘dcpj’
  }

}
