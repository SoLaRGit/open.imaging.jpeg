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
  /// Specifies type of reading used in JpegFile class.
  /// </summary>
  [ComVisible(true)]
  public enum JpegFileReadFlags
  {
    /// <summary>
    /// Reads segments info only.
    /// </summary>
    Segments = 0,

    /// <summary>
    /// Reads segments info, and parses APPn segments only.
    /// </summary>
    ParseAPPn = 1,

    /// <summary>
    /// Reads segments info, and segments data, without parsing APPn segments.
    /// </summary>
    ReadData = 2,

    /// <summary>
    /// Reads segments info, and segments data, and parses APPn segments.
    /// </summary>
    ReadAll = 3,
    
  }
}
