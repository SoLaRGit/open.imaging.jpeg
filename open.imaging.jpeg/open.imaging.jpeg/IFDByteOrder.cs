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
  
  /// <summary>
  /// Byte order types enum
  /// </summary>
  [ComVisible(true)]
  public enum IFDByteOrder //: ushort
  {
    /// <summary>
    /// 0x4949 LSB -> MSB (little endian)
    /// </summary>
    LittleEndian = 0x4949,

    /// <summary>
    /// 0x4D4D  MSB->LSB (big endian)
    /// </summary>
    BigEndian = 0x4d4d
  }

}
