using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [ComImport]
    [Guid("FCE5EFA0-8BBA-4f8e-A036-8F2022B08466")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public interface IMetaDataImport
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CloseEnum([In] IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 CountEnum([In] IntPtr hEnum, [Out] out Int32 count);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 ResetEnum([In] IntPtr hEnum, [In] Int32 ulPos);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumTypeDefs([In] ref IntPtr phEnum,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rTypeDefs, [In] Int32 cMax, [Out] out Int32 pcTypeDefs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumInterfaceImpls([In] ref IntPtr phEnum, [In] Int32 td,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rImpls, [In] Int32 cMax, [Out] out Int32 pcImpls);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumTypeRefs([In] ref IntPtr phEnum,[In, MarshalAs(UnmanagedType.LPArray)]  Int32[] rTypeRefs, [In] Int32 cMax, [Out] out Int32 pcTypeRefs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindTypeDefByName([In, MarshalAs(UnmanagedType.LPWStr)] String szTypeDef, [In] Int32 tkEnclosingClass, [Out] out Int32 ptd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetScopeProps([In, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 cchName, [Out] out Int32 pchName, [In] ref Guid pmvid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetModuleFromScope([Out] out Int32 pmd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetTypeDefProps([In] Int32 td, [Out, MarshalAs(UnmanagedType.LPArray)]Char[] szName, Int32 cchTypeDef, [Out] out Int32 pchTypeDef, [Out] out Int32 pdwTypeDefFlags, [Out] out Int32 ptkExtends);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetInterfaceImplProps([In] Int32 iiImpl, [Out] out Int32 pClass, [Out] out Int32 ptkIface);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetTypeRefProps([In] Int32 tr, [Out] out Int32 ptkResolutionScope, [Out, MarshalAs(UnmanagedType.LPArray)]Char[] szName, [In] Int32 cchName, [Out] out Int32 pchName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 ResolveTypeRef([In] Int32 tr, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)]out Object ppIScope, [Out] out Int32 ptd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMembers([In] ref IntPtr phEnum, Int32 cl,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rMembers, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMembersWithName([In] ref IntPtr phEnum, Int32 cl, [MarshalAs(UnmanagedType.LPWStr)]String szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]Int32[] rMembers, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMethods([In] ref IntPtr phEnum, Int32 cl,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rMethods, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMethodsWithName([In] ref IntPtr phEnum, Int32 cl, [MarshalAs(UnmanagedType.LPWStr)]String szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]Int32[] rMethods, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumFields([In] ref IntPtr phEnum, Int32 cl,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rFields, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumFieldsWithName([In] ref IntPtr phEnum, Int32 cl, [MarshalAs(UnmanagedType.LPWStr)]String szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]Int32[] rFields, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumParams([In] ref IntPtr phEnum, [In] Int32 mb,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rParams, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMemberRefs([In] ref IntPtr phEnum, Int32 tkParent,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rMemberRefs, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMethodImpls([In] ref IntPtr phEnum, [In] Int32 td,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rMethodBody,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rMethodDecl, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumPermissionSets([In] ref IntPtr phEnum, [In] Int32 tk, Int32 dwActions,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rPermission, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindMember([In] Int32 td, [In, MarshalAs(UnmanagedType.LPWStr)]String szName,[In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, [In] Int32 cbSigBlob, [Out] out Int32 pmb);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindMethod([In] Int32 td, [In, MarshalAs(UnmanagedType.LPWStr)]String szName,[In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, [In] Int32 cbSigBlob, [Out] out Int32 pmb);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindField([In] Int32 td, [In, MarshalAs(UnmanagedType.LPWStr)]String szName,[In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, [In] Int32 cbSigBlob, [Out] out Int32 pmb);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindMemberRef([In] Int32 td, [In, MarshalAs(UnmanagedType.LPWStr)]String szName,[In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, Int32 cbSigBlob, [Out] out Int32 pmr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetMethodProps([In] Int32 mb, [Out] out Int32 pClass, [In, MarshalAs(UnmanagedType.LPWStr)] String  szMethod, Int32 cchMethod, [Out] out Int32 pchMethod, [Out] out Int32 pdwAttr, [Out] out IntPtr ppvSigBlob, [Out] out Int32 pcbSigBlob, [Out] out Int32 pulCodeRVA, [Out] out Int32 pdwImplFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetMemberRefProps(Int32 mr, [Out] out Int32 ptk, [Out, MarshalAs(UnmanagedType.LPArray)]Char[] szMember, Int32 cchMember, [Out] out Int32 pchMember, [Out] out IntPtr ppvSigBlob, [Out] out Int32 pbSigBlob);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumProperties([In] ref IntPtr phEnum, [In] Int32 td,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rProperties, [In] Int32 cMax, [Out] out Int32 pcProperties);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumEvents([In] ref IntPtr phEnum, [In] Int32 td,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rEvents, [In] Int32 cMax, [Out] out Int32 pcEvents);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetEventProps(Int32 ev, [Out] out Int32 pClass, [In, MarshalAs(UnmanagedType.LPWStr)] String  szEvent, Int32 cchEvent, [Out] out Int32 pchEvent, [Out] out Int32 pdwEventFlags, [Out] out Int32 ptkEventType, [Out] out Int32 pmdAddOn, [Out] out Int32 pmdRemoveOn, [Out] out Int32 pmdFire, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 10)]Int32[] rmdOtherMethod, [In] Int32 cMax, [Out] out Int32 pcOtherMethod);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMethodSemantics([In] ref IntPtr phEnum, [In] Int32 mb,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rEventProp, [In] Int32 cMax, [Out] out Int32 pcEventProp);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetMethodSemantics([In] Int32 mb, Int32 tkEventProp, [Out] out Int32 pdwSemanticsFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetClassLayout([In] Int32 td, [Out] out Int32 pdwPackSize,[In, MarshalAs(UnmanagedType.LPArray)] long[] rFieldOffset, [In] Int32 cMax, [Out] out Int32 pcFieldOffset, [Out] out Int32 pulClassSize);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetFieldMarshal([In] Int32 tk, [Out] out IntPtr ppvNativeType, [Out] out Int32 pcbNativeType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetRVA([In] Int32 tk, [Out] out Int32 pulCodeRVA, [Out] out Int32 pdwImplFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetPermissionSetProps([In] Int32 pm, [Out] out Int32 pdwAction, [Out] out IntPtr ppvPermission, [Out] out Int32 pcbPermission);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetSigFromToken([In] Int32 mdSig, [Out] out IntPtr ppvSig, [Out] out Int32 pcbSig);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetModuleRefProps([In] Int32 mur, [Out, MarshalAs(UnmanagedType.LPArray)]Char[] szName, [In] Int32 cchName, [Out] out Int32 pchName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumModuleRefs([In] ref IntPtr phEnum, [Out, MarshalAs(UnmanagedType.LPArray)]Int32[] rModuleRefs, Int32 cmax, [Out] out Int32 pcModuleRefs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetTypeSpecFromToken(Int32 typespec, [Out] out IntPtr ppvSig, [Out] out Int32 pcbSig);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetNameFromToken([In] Int32 tk, [Out] out IntPtr pszUtf8NamePtr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumUnresolvedMethods([In] ref Int32 phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]Int32[] rMethods, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetUserString(Int32 stk, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Char[] szString, Int32 cchString, [Out] out Int32 pchString);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetPinvokeMap([In] Int32 tk, [Out] out Int32 pdwMappingFlags, [In, MarshalAs(UnmanagedType.LPWStr)] String  szImportName, Int32 cchImportName, [Out] out Int32 pchImportName, [Out] out Int32 pmrImportDLL);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumSignatures([In] ref IntPtr phEnum, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]Int32[] rSignatures, Int32 cmax, [Out] out Int32 pcSignatures);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumTypeSpecs([In] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]Int32[] rTypeSpecs, [In] Int32 cMax, [Out] out Int32 pcTypeSpecs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumUserStrings([In] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]Int32[] rStrings, Int32 cMax, [Out] out Int32 pcStrings);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetParamForMethodIndex([In] Int32 md, Int32 ulParamSeq, [Out] out Int32 ppd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumCustomAttributes([In] ref IntPtr phEnum, [In] Int32 tk, Int32 tkType,[In, MarshalAs(UnmanagedType.LPArray)] Int32[] rCustomAttributes, [In] Int32 cMax, [Out] out Int32 pcCustomAttributes);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetCustomAttributeProps(Int32 cv, [Out] out Int32 ptkObj, [Out] out Int32 ptkType, [Out] out IntPtr ppBlob, [Out] out Int32 pcbSize);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindTypeRef(Int32 tkResolutionScope, [MarshalAs(UnmanagedType.LPWStr)]String szName, [Out] out Int32 ptr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetMemberProps([In] Int32 mb, [Out] out Int32 pClass, [Out, MarshalAs(UnmanagedType.LPArray)]Char[] szMember, Int32 cchMember, [Out] out Int32 pchMember, [Out] out Int32 pdwAttr, out IntPtr ppvSigBlob, out Int32 pcbSigBlob, [Out] out Int32 pulCodeRVA, [Out] out Int32 pdwImplFlags, [Out] out Int32 pdwCPlusTypeFlag, [Out] out IntPtr ppValue, [Out] out Int32 pcchValue);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetFieldProps([In] Int32 mb, [Out] out Int32 pClass, [In, MarshalAs(UnmanagedType.LPWStr)] String  szField, Int32 cchField, [Out] out Int32 pchField, [Out] out Int32 pdwAttr, [Out] out IntPtr ppvSigBlob, [Out] out Int32 pcbSigBlob, [Out] out Int32 pdwCPlusTypeFlag, [Out] out IntPtr ppValue, [Out] out Int32 pcchValue);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetPropertyProps(Int32 prop, [Out] out Int32 pClass, [In, MarshalAs(UnmanagedType.LPWStr)] String  szProperty, Int32 cchProperty, [Out] out Int32 pchProperty, [Out] out Int32 pdwPropFlags, [Out] out IntPtr ppvSig, [Out] out Int32 pbSig, [Out] out Int32 pdwCPlusTypeFlag, [Out] out IntPtr ppDefaultValue, [Out] out Int32 pcchDefaultValue, [Out] out Int32 pmdSetter, [Out] out Int32 pmdGetter, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 13)]Int32[] rmdOtherMethod, [In] Int32 cMax, [Out] out Int32 pcOtherMethod);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetParamProps([In] Int32 tk, [Out] out Int32 pmd, [Out] out Int32 pulSequence, [In, MarshalAs(UnmanagedType.LPWStr)]String szName, [In] Int32 cchName, [Out] out Int32 pchName, [Out] out Int32 pdwAttr, [Out] out Int32 pdwCPlusTypeFlag, [Out] out IntPtr ppValue, [Out] out Int32 pcchValue);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetCustomAttributeByName([In] Int32 tkObj, [MarshalAs(UnmanagedType.LPWStr)]String szName, [Out] out IntPtr ppData, [Out] out Int32 pcbData);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Boolean IsValidToken([In] Int32 tk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetNestedClassProps([In] Int32 tdNestedClass, [Out] out Int32 ptdEnclosingClass);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetNativeCallConvFromSig([In] IntPtr pvSig, [In] Int32 cbSig, [Out] out Int32 pCallConv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 IsGlobal([In] Int32 pd, [Out] out Int32 pbGlobal);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumGenericParams();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetGenericParamProps();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetMethodSpecProps([In] Int32 mi, [Out] out Int32 tkParent, [Out] out IntPtr ppvSigBlob, [Out] out Int32 pcbSigBlob);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumGenericParamConstraints();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetGenericParamConstraintProps();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetPEKind();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetVersionString();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumMethodSpecs([In] ref IntPtr phEnum, [In] Int32 tk, [MarshalAs(UnmanagedType.LPArray)] Int32[] rImplsrMethodSpecs, [In] Int32 cMax, [Out] out Int32 pcMethodSpecs);
    }
}
