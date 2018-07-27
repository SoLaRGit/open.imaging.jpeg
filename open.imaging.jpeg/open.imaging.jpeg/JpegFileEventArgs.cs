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
  public enum JpegFileEventId : int
  {
    WARNING = 0x1000,
    WARNING_INVALID_MARKER_ID,
    WARNING_SIGNATURE_UNKNOWN,    
    WARNING_EXIFHDR_OFFSET_TO_1ST,
    WARNING_EXIFIFD_END_OF_BLOCK,    
    WARNING_EXIFENTRY_INVALID_COUNT,
    
    ERROR = 0x8000,
    ERROR_INVALID_MARKER_ID,    
    ERROR_EXIFIFD_READ = 0x8001,
    ERROR_EXIFIFD1ST_READ = 0x8002,
  }

  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileEventArgs : EventArgs
  {
    public long Address;
    public JpegFileEventId Id;
    public string Message;

    public JpegFileEventArgs()
    { }

    public JpegFileEventArgs(long Address, JpegFileEventId Id, string Message)
    {
      this.Address = Address;
      this.Id = Id;
      this.Message = Message;
    }

    public override string ToString()
    {
      return string.Format("[ @:0x{0:x8}, id:({1}), Message:({2}) ]",
        this.Address,
        this.Id,
        this.Message);
    }

  }

}
