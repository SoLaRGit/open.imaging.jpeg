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
  public enum IFDType //: ushort
  {
    /// <summary>NOT DEFINED BY ANY STANDARD (0)</summary>
    TYPE_00 = 0,

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
    /// <summary>Adobe Type, content is IFD (where IFD is commonly encoded as type LONG)</summary>
    IFD = 13,
    /// <summary>NOT DEFINED BY ANY STANDARD (15)</summary>
    TYPE_14 = 14,
    /// <summary>NOT DEFINED BY ANY STANDARD (15)</summary>
    TYPE_15 = 15
  }

}
