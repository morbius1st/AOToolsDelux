﻿<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <AssemblyName>AODeluxMeasure</AssemblyName>
    <RootNamespace>CsDeluxMeasure</RootNamespace>
    <FileAlignment>512</FileAlignment>
    <OutputType>Library</OutputType>
    <ProjectGuid>{9013462A-6B25-424B-85C1-92957C68E6AC}</ProjectGuid>
    <ProjectTypeGuids>{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <RvtVersion>2021</RvtVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' or '$(Configuration)|$(Platform)' == 'Rvt21Dbg|AnyCPU' or '$(Configuration)|$(Platform)' == 'Rvt22Dbg|AnyCPU'">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <DefineConstants>TRACE;DEBUG;USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <PlatformTarget>AnyCPU</PlatformTarget>
    <WarningLevel>4</WarningLevel>
    <StartAction>Program</StartAction>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Rvt21Dbg|AnyCPU'">
    <StartProgram>$(ProgramW6432)\Autodesk\Revit 2021\Revit.exe</StartProgram>
    <RvtVersion>2021</RvtVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Rvt22Dbg|AnyCPU'">
    <StartProgram>$(ProgramW6432)\Autodesk\Revit 2022\Revit.exe</StartProgram>
    <RvtVersion>2022</RvtVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <DefineConstants>TRACE;USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS</DefineConstants>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <StartAction>Program</StartAction>
    <StartProgram>$(ProgramW6432)\Autodesk\Revit 2021\Revit.exe</StartProgram>
  </PropertyGroup>
  <PropertyGroup>
    <RunPostBuildEvent>Always</RunPostBuildEvent>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Rvt21Rel|AnyCPU' ">
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE;USER_SETTINGS, APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS</DefineConstants>
    <Optimize>true</Optimize>
    <DebugType>pdbonly</DebugType>
    <PlatformTarget>AnyCPU</PlatformTarget>
    <LangVersion>7.3</LangVersion>
    <ErrorReport>prompt</ErrorReport>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="RevitAPI">
      <HintPath>C:\Program Files\Autodesk\Revit 2021\RevitAPI.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="RevitAPIUI">
      <HintPath>C:\Program Files\Autodesk\Revit 2021\RevitAPIUI.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Runtime.Serialization" />
    <Reference Include="System.Xml" />
    <Reference Include="System.Core" />
    <Reference Include="System.Net" />
    <Reference Include="System.Xaml" />
    <Reference Include="PresentationCore" />
    <Reference Include="PresentationFramework" />
    <Reference Include="System.XML" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="WindowsBase" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="..\..\..\UtilityLibrary\UtilityLibrary\CsConversions.cs">
      <Link>.Linked\CsConversions.cs</Link>
    </Compile>
    <Compile Include="..\..\..\UtilityLibrary\UtilityLibrary\CsStringUtil.cs">
      <Link>.Linked\CsStringUtil.cs</Link>
    </Compile>
    <Compile Include="..\..\..\UtilityLibrary\UtilityLibrary\CsWpfUtilities.cs">
      <Link>.Linked\CsWpfUtilities.cs</Link>
    </Compile>
    <Compile Include="..\..\LibraryRevit\LibraryRevit\RevitLibrary.cs">
      <Link>.Linked\RevitLibrary.cs</Link>
    </Compile>
    <Compile Include="Properties\Annotations.cs" />
    <Compile Include="RevitSupport\RevitInterOp.cs" />
    <Compile Include="RevitSupport\RevitUtil.cs" />
    <Compile Include="Settings\AppSettings.cs" />
    <Compile Include="Settings\DataSet.cs" />
    <Compile Include="Settings\HeadingSetting.cs" />
    <Compile Include="Settings\MachineSettings.cs" />
    <Compile Include="Settings\SiteSettings.cs" />
    <Compile Include="Settings\SuiteSettings.cs" />
    <Compile Include="Settings\UserSettings.cs" />
    <Page Include="..\SharedCode\Windows\Resources\IconDelete.xaml">
      <Link>Windows\ResourceFiles\SVG\IconDelete.xaml</Link>
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\About.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Privacy.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\ResourceFiles\SVG\padlock.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\ResourceFiles\XamlResources\MasterColorList.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\ResourceFiles\XamlResources\VisualStates.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Skin\CyberStudioSkin.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\MainWindow.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Compile Include="UnitsUtil\UnitData.cs" />
    <Compile Include="UnitsUtil\UnitsInListMgr.cs" />
    <Compile Include="UnitsUtil\UnitsInListsCurrent.cs" />
    <Compile Include="UnitsUtil\UnitsInListsWorking.cs" />
    <Compile Include="UnitsUtil\UnitsStdUStyles.cs" />
    <Compile Include="UnitsUtil\UnitsSupport.cs" />
    <Compile Include="UnitsUtil\UnitsSettings.cs" />
    <Compile Include="UnitsUtil\UnitsManager.cs" />
    <Compile Include="UnitsUtil\UnitUData.cs" />
    <Compile Include="UnitsUtil\UnitUStyle.cs" />
    <Compile Include="Windows\About.xaml.cs">
      <DependentUpon>About.xaml</DependentUpon>
    </Compile>
    <Compile Include="Windows\AWindow.cs" />
    <Compile Include="Windows\ColData.cs" />
    <Compile Include="Windows\Privacy.xaml.cs">
      <DependentUpon>Privacy.xaml</DependentUpon>
    </Compile>
    <Compile Include="Windows\Support\CsButtonsAp.cs" />
    <Compile Include="Windows\Support\CsCheckBoxAp.cs" />
    <Compile Include="Windows\Support\CsComboBoxAp.cs" />
    <Compile Include="Windows\Support\CsCommonAp.cs" />
    <Compile Include="Windows\Support\CsBorderAp.cs" />
    <Compile Include="Windows\Support\CsPathAp.cs" />
    <Compile Include="Windows\Support\CsScrollBarAp.cs" />
    <Compile Include="Windows\Support\CsScrollViewerAp.cs" />
    <Compile Include="Windows\Support\CustomProperties.cs" />
    <Compile Include="Windows\Support\junk.cs" />
    <Compile Include="Windows\Support\UnitStylesMgrWinData.cs" />
    <Compile Include="RevitSupport\UnitStyleCmd.cs" />
    <Compile Include="Windows\Support\Validation.cs" />
    <Compile Include="Windows\Support\ValueConverters.cs" />
    <Compile Include="Windows\Support\VisualStates.cs" />
    <Compile Include="Windows\Support\XmalMarkup.cs" />
    <Compile Include="Windows\UnitStyleOrder.xaml.cs">
      <DependentUpon>UnitStyleOrder.xaml</DependentUpon>
    </Compile>
    <Compile Include="Windows\UnitStylesMgr.xaml.cs">
      <DependentUpon>UnitStylesMgr.xaml</DependentUpon>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilitiesMedia.cs">
      <Link>.Linked\CsUtilitiesMedia.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsExtensions.cs">
      <Link>.Linked\CsExtensions.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilities.cs">
      <Link>.Linked\CsUtilities.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsXmlUtilities.cs">
      <Link>.Linked\CsXmlUtilities.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\MessageUtilities.cs">
      <Link>.Linked\MessageUtilities.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\FilePath\V3.0\CsFilePath.cs">
      <Link>.Linked\CsFilePath.cs</Link>
    </Compile>
    <Compile Include="\\CRONOS\Users\jeffs\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.4\SettingsMgr.cs">
      <Link>.Linked\SettingsMgr.cs</Link>
    </Compile>
    <Compile Include="AppRibbon.cs" />
    <Compile Include="RevitSupport\Commands.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <Compile Include="Windows\MainWindow.xaml.cs">
      <DependentUpon>MainWindow.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Page Include="Windows\Support\CsButtons.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Support\CsCheckBox.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Support\CsComboBox.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Support\CsExpander.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Support\CsScrollBar.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\Support\CsScrollViewer.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="Windows\UnitStyleOrder.xaml">
      <SubType>Designer</SubType>
      <Generator>MSBuild:Compile</Generator>
    </Page>
    <Page Include="Windows\UnitStylesMgr.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
  </ItemGroup>
  <ItemGroup>
    <Content Include="Other\Delux Measure Main Window Help.htm" />
    <Resource Include="Resources\CreateSheetsAbout v2.png" />
    <Resource Include="Resources\CyberStudio Icon.png" />
    <Resource Include="Resources\CyberStudio Logo - Narrow.png" />
    <Resource Include="Resources\Tape Measure32.png" />
    <Resource Include="Resources\Gear32.png" />
    <Resource Include="Resources\Delux Measure dec-ft 32.png" />
    <Resource Include="Resources\Delux Measure dec-in 32.png" />
    <Resource Include="Resources\Delux Measure frac-in 32.png" />
    <Resource Include="Resources\Delux Measure cm 32.png" />
    <Resource Include="Resources\Delux Measure dm 32.png" />
    <Resource Include="Resources\Delux Measure ft-dec-in 32.png" />
    <Resource Include="Resources\Delux Measure ft-frac-in 32.png" />
    <Resource Include="Resources\Delux Measure m 32.png" />
    <Resource Include="Resources\Delux Measure m-cm 32.png" />
    <Resource Include="Resources\Delux Measure mm 32.png" />
    <Resource Include="Resources\Delux Measure us-ft 32.png" />
    <Resource Include="Resources\information16.png" />
    <Resource Include="Resources\information32.png" />
    <Content Include="Other\CsDeluxMeasure.addin" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <PropertyGroup>
    <PostBuildEvent>echo Building: $(ConfigurationName)
echo Testing Revit Version: $(RvtVersion)
if exist "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)" copy "$(ProjectDir)$(OutputPath)$(TargetFileName)" "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)"
if exist "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)" copy "$(ProjectDir)other\Addin.addin" "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)\$(TargetName).addin"
if exist "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)" xcopy /i/y/s/e "$(ProjectDir)other\Help Files" "$(AppData)\Autodesk\REVIT\Addins\$(RvtVersion)\Resources"

	</PostBuildEvent>
  </PropertyGroup>
  <PropertyGroup>
    <PreBuildEvent>
    </PreBuildEvent>
  </PropertyGroup>
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it.
       Other similar extension points exist, see Microsoft.Common.targets.
       
  <PropertyGroup>
    <PreBuildEvent>
</PreBuildEvent>
  </PropertyGroup>

  <Target Name="AfterBuild">
  </Target>
  <Target Name="BeforeBuild">
		<Delete Files="C:\ProgramData\CyberStudio\CsDeluxMeasure\CsDeluxMeasure.app.setting.xml" />
		<Delete Files="C:\Users\jeffs\AppData\Roaming\CyberStudio\CsDeluxMeasure\CsDeluxMeasure.user.setting.xml" />
	</Target>
  -->
  <Target Name="AfterClean">
    <Delete Files="$(AppData)\Autodesk\REVIT\Addins\2021\CsDeluxMeasure.addin" />
    <Delete Files="$(AppData)\Autodesk\REVIT\Addins\2021\CsDeluxMeasure.dll" />
  </Target>
</Project>