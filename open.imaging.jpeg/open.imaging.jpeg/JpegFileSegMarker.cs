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
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{
  [ComVisible(true)]
  public enum JpegFileSegMarker //: ushort
  {
    /// <summary>length 0 | jpeg marker base 0.</summary>
    _FF00 = 0xff00,

    /// <summary>length 0 | jpeg marker resume reading at current position - 1 byte.</summary>
    _FFFF = 0xffff,

    /// <summary>For temporary private use in arithmetic coding.</summary>
    TEM = 0xff01,

    /// <summary>Huffman coding - baseline DCT</summary>
    SOF0 = 0xffc0,
    /// <summary>Huffman coding - extended sequential DCT.</summary>
    SOF1 = 0xffc1,
    /// <summary>Huffman coding - progressive DCT.</summary>
    SOF2 = 0xffc2,
    /// <summary>Huffman coding - lossless (sequential).</summary>
    SOF3 = 0xffc3,
    /// <summary>Huffman coding - Differential sequential DCT.</summary>
    SOF5 = 0xffc5,
    /// <summary>Huffman coding - Differential progressive DCT.</summary>
    SOF6 = 0xffc6,
    /// <summary>Huffman coding - Differential lossless (sequential).</summary>
    SOF7 = 0xffc7,

    /// <summary>Arithmetic coding - reserved for JPEG extensions.</summary>
    JPG = 0xffc8,

    /// <summary>Arithmetic coding - extended sequential DCT.</summary>
    SOF9 = 0xffc9,
    /// <summary>Arithmetic coding - progressive DCT.</summary>
    SOF10 = 0xffca,
    /// <summary>Arithmetic coding - lossless (sequential).</summary>
    SOF11 = 0xffcb,
    /// <summary>Arithmetic coding - Differential sequential DCT.</summary>
    SOF13 = 0xffcd,
    /// <summary>Arithmetic coding - Differential progressive DCT</summary>
    SOF14 = 0xffce,
    /// <summary>Arithmetic coding - Differential lossless (sequential).</summary>
    SOF15 = 0xffcf,

    /// <summary>Huffman table specification.</summary>
    DHT = 0xffc4,

    /// <summary></summary>
    DAC = 0xffcc,

    /// <summary>length 0 | Restart with modulo 8 count “m” 0</summary>
    RSTm0 = 0xffd0,
    /// <summary>length 0 | Restart with modulo 8 count “m” 1</summary>
    RSTm1 = 0xffd1,
    /// <summary>length 0 | Restart with modulo 8 count “m” 2</summary>
    RSTm2 = 0xffd2,
    /// <summary>length 0 | Restart with modulo 8 count “m” 3</summary>
    RSTm3 = 0xffd3,
    /// <summary>length 0 | Restart with modulo 8 count “m” 4</summary>
    RSTm4 = 0xffd4,
    /// <summary>length 0 | Restart with modulo 8 count “m” 5</summary>
    RSTm5 = 0xffd5,
    /// <summary>length 0 | Restart with modulo 8 count “m” 6</summary>
    RSTm6 = 0xffd6,
    /// <summary>length 0 | Restart with modulo 8 count “m” 7</summary>
    RSTm7 = 0xffd7,

    /// <summary>Start Of Image</summary>
    SOI = 0xffd8,
    /// <summary>length 0 | End of image</summary>
    EOI = 0xffd9,
    /// <summary>start of scan - requires decoder to determine correct length</summary>
    SOS = 0xffda,
    /// <summary>defines quantization table(s)</summary>
    DQT = 0xffdb,
    /// <summary>defines number of lines</summary>
    DNL = 0xffdc,
    /// <summary>defines restart interval</summary>
    DRI = 0xffdd,
    /// <summary>defines hierarchical progression</summary>
    DHP = 0xffde,
    /// <summary>expand reference component(s)</summary>
    EXP = 0xffdf,

    /// <summary>APP0 - application data 0</summary>
    APP0 = 0xffe0,
    /// <summary>APP1 - application data 1</summary>
    APP1 = 0xffe1,
    /// <summary>APP2 - application data 2</summary>
    APP2 = 0xffe2,
    /// <summary>APP3 - application data 3</summary>
    APP3 = 0xffe3,
    /// <summary>APP4 - application data 4</summary>
    APP4 = 0xffe4,
    /// <summary>APP5 - application data 5</summary>
    APP5 = 0xffe5,
    /// <summary>APP6 - application data 6</summary>
    APP6 = 0xffe6,
    /// <summary>APP7 - application data 7</summary>
    APP7 = 0xffe7,
    /// <summary>APP8 - application data 8</summary>
    APP8 = 0xffe8,
    /// <summary>APP9 - application data 9</summary>
    APP9 = 0xffe9,
    /// <summary>APP10 - application data 10</summary>
    APP10 = 0xffea,
    /// <summary>APP11 - application data 11</summary>
    APP11 = 0xffeb,
    /// <summary>APP12 - application data 12</summary>
    APP12 = 0xffec,
    /// <summary>APP13 - application data 13</summary>
    APP13 = 0xffed,
    /// <summary>APP14 - application data 14</summary>
    APP14 = 0xffee,
    /// <summary>APP15 - application data 15</summary>
    APP15 = 0xffef,

    /// <summary>jpeg extenssions</summary>
    JEX0 = 0xfff0,
    /// <summary>jpeg extenssions</summary>
    JEX1 = 0xfff1,
    /// <summary>jpeg extenssions</summary>
    JEX2 = 0xfff2,
    /// <summary>jpeg extenssions</summary>
    JEX3 = 0xfff3,
    /// <summary>jpeg extenssions</summary>
    JEX4 = 0xfff4,
    /// <summary>jpeg extenssions</summary>
    JEX5 = 0xfff5,
    /// <summary>jpeg extenssions</summary>
    JEX6 = 0xfff6,
    /// <summary>jpeg extenssions</summary>
    JEX7 = 0xfff7,
    /// <summary>jpeg extenssions</summary>
    JEX8 = 0xfff8,
    /// <summary>jpeg extenssions</summary>
    JEX9 = 0xfff9,
    /// <summary>jpeg extenssions</summary>
    JEX10 = 0xfffa,
    /// <summary>jpeg extenssions</summary>
    JEX11 = 0xfffb,
    /// <summary>jpeg extenssions</summary>
    JEX12 = 0xfffc,
    /// <summary>jpeg extenssions</summary>
    JEX13 = 0xfffd,

    /// <summary>comment</summary>
    COM = 0xfffe,
  }

}