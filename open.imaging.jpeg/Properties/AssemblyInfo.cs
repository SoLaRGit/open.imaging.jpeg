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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("open.imaging.jpeg library (" + BUILD.FRAMEWORK_VERSION_STRING + "|" + BUILD.PLATFORM_STRING + "|" + BUILD.CONFIGURATION_STRING + ")")]
[assembly: AssemblyDescription("open.imaging.jpeg library (" + BUILD.FRAMEWORK_VERSION_STRING + "|" + BUILD.PLATFORM_STRING + "|" + BUILD.CONFIGURATION_STRING + ")")]
[assembly: AssemblyConfiguration(BUILD.CONFIGURATION_STRING)]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("open.imaging.jpeg")]
[assembly: AssemblyCopyright("Copyright © Nikola Bozovic 2017.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5E24FB6E-1341-45DC-9039-51863ACB50B0")]

/****************************************************************************************************
 * Project PropertyGroup added to support compile condition pending framework version.
 * each build implements:
 * Debug: <DefineConstants>DEBUG;TRACE;$(DefineConstants)</DefineConstants>
 * or
 * Release: <DefineConstants>$(DefineConstants)</DefineConstants>
 * 
  <PropertyGroup Condition=" '$(TargetFrameworkVersion)' == 'v2.0' ">
    <DefineConstants>NET10;NET20;$(DefineConstants)</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(TargetFrameworkVersion)' == 'v3.0' ">
    <DefineConstants>NET10;NET20;NET30;$(DefineConstants)</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(TargetFrameworkVersion)' == 'v3.5' ">
    <DefineConstants>NET10;NET20;NET30;NET35;$(DefineConstants)</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(TargetFrameworkVersion)' == 'v4.0' ">
    <DefineConstants>NET10;NET20;NET30;NET35;NET40;$(DefineConstants)</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(TargetFrameworkVersion)' == 'v4.5' ">
    <DefineConstants>NET10;NET20;NET30;NET35;NET40;NET45;$(DefineConstants)</DefineConstants>
  </PropertyGroup>
 ****************************************************************************************************
 */
#if NET452
[assembly: AssemblyInformationalVersion("4.5.2")]
[assembly: AssemblyVersion("4.52.*")]
#elif NET451
[assembly: AssemblyInformationalVersion("4.5.1")]
[assembly: AssemblyVersion("4.51.*")]
#elif NET45
[assembly: AssemblyInformationalVersion("4.5")]
[assembly: AssemblyVersion("4.5.*")]
#elif NET40
[assembly: AssemblyInformationalVersion("4.0")]
[assembly: AssemblyVersion("4.0.*")]
#elif NET35
[assembly: AssemblyProduct("3.5")]
[assembly: AssemblyVersion("3.5.*")]
#elif NET30
[assembly: AssemblyProduct("3.0")]
[assembly: AssemblyVersion("3.0.*")]
#elif NET20
[assembly: AssemblyProduct("2.0")]
[assembly: AssemblyVersion("2.0.*")]
#endif


public static class BUILD
{
  public const string PLATFORM_STRING =
#if CPUx64  
  "x64"  
#elif CPUx86
  "x86"
#elif CPUANY
  "Any"
#elif CPUITANIUM
  "Ita"
#else
  "Unk"
#endif
  ;

  public const string FRAMEWORK_VERSION_STRING = 
#if NET452
  ".net452"
#elif NET451
  ".net451"
#elif NET45
  ".net45"
#elif NET40
  ".net40"
#elif NET30
  ".net30"
#elif NET20
  ".net20"
#else
  ".netx"
#endif
;

  public const string CONFIGURATION_STRING =
#if DEBUG
#if TRACE
  "D|T"
#else
  "D"
#endif
#else
#if TRACE
  "R|T"
#else
  "R"
#endif
#endif
  ;
}