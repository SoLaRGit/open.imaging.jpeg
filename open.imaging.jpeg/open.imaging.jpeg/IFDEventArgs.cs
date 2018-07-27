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
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class IFDEventArgs : JpegFileEventArgs
  {
    public IFD IFD;
    public IFDEntry Entry;

    public override string ToString()
    {
      return string.Format("[ @:0x{0:x8}, id:({1}), IFD:({2}), Entry:({3}), Message:({4}) ]",
        this.Address,
        this.Id,
        null == this.IFD ? "" : this.IFD.ToString(),
        null == this.Entry ? "" : this.Entry.ToString(),
        this.Message);
    }
  }


}
