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

// some old perfromance class, for more advanced version (statistics...) ask...

/********************************************************************************
 * High Performace Counter 
 * 
 * - measures realtime CPU clock and returns time as double value in seconds.
 * 
 * [2012-03-03] nikola.bozovic@gmail.com
 * 
 */

using System;
using System.Runtime.InteropServices;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class HighPerformanceCounter
{

  [DllImport("Kernel32.dll")]
  private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

  [DllImport("Kernel32.dll")]
  private static extern bool QueryPerformanceFrequency(out long lpFrequency);

  private long startTime, stopTime;
  private long freq;

  public HighPerformanceCounter()
  {
    if (!QueryPerformanceFrequency(out freq)) throw new Exception();
    QueryPerformanceCounter(out startTime);
  }

  public void Reset()
  {
    QueryPerformanceCounter(out startTime);
  }

  public double Duration
  {
    get
    {
      QueryPerformanceCounter(out stopTime);
      return (double)(stopTime - startTime) / (double)freq;
    }
  }

  public override string ToString()
  {
    return Duration.ToString();
  }

}
