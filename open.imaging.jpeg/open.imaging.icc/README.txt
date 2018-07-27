
Not even sure why I've rasterized ICC profile, since I've found only few in use
in thousands images that were checked. Maybe ICC would be found in non-generic 
images like for astronomy, medical, or industry use (worth checking), otherwise
pretty pointless exercise. So I hope someone will find some use for this ICC 
classes, I haven't found any at this time. I think I've done it for complete-
ness sake only, wanting to complete full reading, and data deserialization of 
all jpeg headers.

To have some sense of ICC DOM organization, this is how ICC DOM structures look
alike when dumped in immediate (without LUT's values):

{open.imaging.icc.ICCProfile}
    CIEXYZValues: {[x:0.8396912,y:256,z:0.1790009]}
    CIEXYZValuesBytes: {byte[0x0000000c]}
    CMMType: 0x4c696e6f
    CMMTypeString: "Lino"
    ColourSpace: RGB
    CreationDateTime: {09/02/1998 06:49:00}
    CreationDateTimeBytes: {byte[0x0000000c]}
    data: {byte[0x00000c48]}
    DeviceAttributes: Reflective
    DeviceManufacturer: 0x49454320
    DeviceManufacturerString: "IEC "
    DeviceModel: 0x73524742
    DeviceModelString: "sRGB"
    Length: 0x00000c48
    PCS: nCIEXYZorPCSXYZa
    PrimaryPlatform: MicrosoftCorporation
    ProfileCreatorSignature: 0x48502020
    ProfileCreatorSignatureString: "HP  "
    ProfileDeviceClass: Display
    ProfileFileSignature: 0x61637370
    ProfileFileSignatureString: "acsp"
    ProfileFlags: ProfileIndenpendent
    ProfileID: {byte[0x00000010]}
    ProfileIDString: "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 "
    ProfileVersionNumber: 0x02100000
    RenderingIntent: Perceptual
    Reserved: {byte[0x0000001c]}
    Tags: Count = 0x00000011

    [0x00000000]: {'cprt':copyrightTag, O:336, L:51, [ICCTagDataTypeText:'text':'Copyright (c) 1998 Hewlett-Packard Company']}
    [0x00000001]: {'desc':profileDescriptionTag, O:388, L:108, [ICCTagDataTypeDescription:'desc':'sRGB IEC61966-2.1']}
    [0x00000002]: {'wtpt':mediaWhitePointTag, O:496, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:0.3201141,y:256,z:256.7972]]}
    [0x00000003]: {'bkpt':mediaBlackPointTag, O:516, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:0,y:0,z:0]]}
    [0x00000004]: {'rXYZ':redMatrixColumnTag, O:536, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:0.6345062,y:0.9578857,z:0.5625458]]}
    [0x00000005]: {'gXYZ':greenMatrixColumnTag, O:556, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:0.5991516,y:0.5223236,z:0.8519287]]}
    [0x00000006]: {'bXYZ':blueMatrixColumnTag, O:576, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:0.6255493,y:0.5158539,z:0.8113708]]}
    [0x00000007]: {'dmnd':deviceMfgDescTag, O:596, L:112, [ICCTagDataTypeDescription:'desc':'IEC http://www.iec.ch']}
    [0x00000008]: {'dmdd':deviceModelDescTag, O:708, L:136, [ICCTagDataTypeDescription:'desc':'IEC 61966-2.1 Default RGB colour space - sRGB']}
    [0x00000009]: {'vued':viewingCondDescTag, O:844, L:134, [ICCTagDataTypeDescription:'desc':'Reference Viewing Condition in IEC61966-2.1']}
    [0x0000000a]: {'view':viewingConditionsTag, O:980, L:36, [ICCTagDataTypeViewingConditions:'view':(IlluminantType:1,Illuminant:[x:4864.995,y:5120.181,z:4096.081],Surround:[x:768.8005,y:1024.043,z:768.6186])]}
    [0x0000000b]: {'lumi':luminanceTag, O:1016, L:20, [ICCTagDataTypeXYZ:'XYZ ':value[0]:[x:19456.34,y:20480,z:22272.9]]}
    [0x0000000c]: {'meas':measurementTag, O:1036, L:36, [ICCTagDataTypeMeasurement:'meas':(StandardObserver:CIE1931,TristimulusBacking:[x:0,y:0,z:0],MeasurementGeometry:Unknown,MeasurementFlare:0.009994507,StandardIllumination:D65)]}
    [0x0000000d]: {'tech':technologyTag, O:1072, L:12, [ICCTagDataTypeSignature:'sig ':43525420:'CRT ']}
    [0x0000000e]: {'rTRC':redTRCTag, O:1084, L:2060, [ICCTagDataTypeCurve:'curv':ushort[1024]]}
    [0x0000000f]: {'gTRC':greenTRCTag, O:1084, L:2060, [ICCTagDataTypeCurve:'curv':ushort[1024]]}
    [0x00000010]: {'bTRC':blueTRCTag, O:1084, L:2060, [ICCTagDataTypeCurve:'curv':ushort[1024]]}

