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
    [Guid("F5DD9950-F693-42e6-830E-7B833E8146A9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal interface IMetaDataEmit
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetModuleProps([In]String szName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Save([In]String szFile, [In] Int32 dwSaveFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SaveToStream([In] IntPtr pIStream, [In] Int32 dwSaveFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetSaveSize([In] Int32 fSave, [Out] out Int32 pdwSaveSize);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineTypeDef([In, MarshalAs(UnmanagedType.LPWStr)] String szTypeDef, [In] Int32 dwTypeDefFlags,[In] Int32 tkExtends, [Out] out Int32 rtkImplements);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineNestedType([In, MarshalAs(UnmanagedType.LPWStr)] String szTypeDef, [In] Int32 dwTypeDefFlags, [In] Int32 tkExtends, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]Int32[] rtkImplements, [In] Int32 tdEncloser, [Out] out Int32 ptd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetHandler([MarshalAs(UnmanagedType.IUnknown), In]Object pUnk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineMethod(Int32 td, [In, MarshalAs(UnmanagedType.LPWStr)] String zName, Int32 dwMethodFlags, [In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, [In] Int32 cbSigBlob, Int32 ulCodeRVA, Int32 dwImplFlags, [Out] out Int32 ptd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineMethodImpl([In] Int32 td, [In] Int32 tkBody, [In] Int32 tkDecl);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineTypeRefByName([In]Int32 tkResolutionScope, [In, MarshalAs(UnmanagedType.LPWStr)] String szName, [Out] out Int32 ptr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineImportType([In] IMetaDataAssemblyImport pAssemImport, [In] IntPtr pbHashValue, [In] Int32 cbHashValue, [In] IMetaDataImport pImport, [In] Int32 tdImport, [In] IMetaDataAssemblyEmit pAssemEmit, [Out] out Int32 ptr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineMemberRef([In] Int32 tkImport, [In, MarshalAs(UnmanagedType.LPWStr)] String szName,[In] IntPtr pvSigBlob, [In] Int32 cbSigBlob, [Out] out Int32 pmr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineImportMember(IMetaDataAssemblyImport pAssemImport, IntPtr pbHashValue, Int32 cbHashValue, IMetaDataImport pImport, Int32 mbMember, IMetaDataAssemblyEmit pAssemEmit, Int32 tkParent, [Out] out Int32 pmr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineEvent(Int32 td, String szEvent, Int32 dwEventFlags, Int32 tkEventType, Int32 mdAddOn, Int32 mdRemoveOn, Int32 mdFire, IntPtr rmdOtherMethods);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetClassLayout(Int32 td, Int32 dwPackSize, IntPtr rFieldOffsets, Int32 ulClassSize);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DeleteClassLayout(Int32 td);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetFieldMarshal(Int32 tk, IntPtr pvNativeType, Int32 cbNativeType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DeleteFieldMarshal(Int32 tk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefinePermissionSet(Int32 tk, Int32 dwAction, IntPtr pvPermission, Int32 cbPermission);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetRVA(Int32 md, Int32 ulRVA);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetTokenFromSig([In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSig, [In] Int32 cbSig, [Out] out Int32 pmsig);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineModuleRef([In, MarshalAs(UnmanagedType.LPWStr)] String szName, [Out] out Int32 pmur);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetParent(uint mr, uint tk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetTokenFromTypeSpec([In, MarshalAs(UnmanagedType.LPArray)]Byte[] pvSig, Int32 cbSig, out Int32 ptypespec);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SaveToMemory(out IntPtr pbData, Int32 cbData);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 DefineUserString([In, MarshalAs(UnmanagedType.LPWStr)] String szString, [In] Int32 cchString, [Out] out Int32 pstk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DeleteToken(uint tkObj);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, [In, MarshalAs(UnmanagedType.SafeArray)]uint[] rtkImplements);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, [MarshalAs(UnmanagedType.SafeArray)]uint[] rmdOtherMethods);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetPermissionSetProps(uint tk, uint dwAction, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pvPermission, uint cbPermission, out uint ppm);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefinePinvokeMap(uint tk, uint dwMappingFlags, [In, MarshalAs(UnmanagedType.LPWStr)]string szImportName, uint mrImportDLL);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetPinvokeMap(uint tk, uint dwMappingFlags, [In, MarshalAs(UnmanagedType.LPWStr)]string szImportName, uint mrImportDLL);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DeletePinvokeMap(uint tk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineCustomAttribute(uint tkObj, uint tkType, [In, MarshalAs(UnmanagedType.LPArray)]uint[] pCustomAttribute, uint cbCustomAttribute, out uint pcv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetCustomAttributeValue(uint pcv, [MarshalAs(UnmanagedType.LPArray)]byte[] pCustomAttribute, uint cbCustomAttribute);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineField(uint td, [In, MarshalAs(UnmanagedType.LPWStr)] string szName,
                         uint dwFieldFlags, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag,
                         [In, MarshalAs(UnmanagedType.LPArray)]byte[] pValue, uint cchValue, out uint pmd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineProperty(uint md, [In, MarshalAs(UnmanagedType.LPWStr)]string szProperty, uint dwPropFlags, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pvSig,
                            uint cbSig, uint dwCPlusTypeFlag, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pValue, uint cchValue, uint mdSetter, uint mdGetter,
                            [In, MarshalAs(UnmanagedType.LPArray)]uint[] rmdOtherMethods, out uint pmdProp);
 	
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineParam(uint md, uint ulParamSeq, [In, MarshalAs(UnmanagedType.LPWStr)]string szName, uint dwParamFlags,
                         uint dwCPlusTypeFlag, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pValue, uint cchValue, out uint ppd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, [MarshalAs(UnmanagedType.LPArray)]byte[] pValue, uint cchValue);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pValue,
                              uint cchValue, uint mdSetter, uint mdGetter, [In, MarshalAs(UnmanagedType.SafeArray)]uint[] rmdOtherMethods);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetParamProps(uint pd, [In, MarshalAs(UnmanagedType.LPWStr)]string szName, uint dwParamFlags,
                           uint dwCPlusTypeFlag, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pValue, uint cchValue);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineSecurityAttributeSet(uint tkObj, [In, MarshalAs(UnmanagedType.LPArray)]IntPtr[] rSecAttrs, uint cSecAttrs, out uint pulErrorAttr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ApplyEditAndContinue([In, MarshalAsAttribute(UnmanagedType.IUnknown)]Object pImport);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void TranslateSigWithScope([In, MarshalAs(UnmanagedType.IUnknown)]Object pAssemImport, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pbHashValue,
                                   uint cbHashValue, [In, MarshalAs(UnmanagedType.IUnknown)]IMetaDataImport import, [In, MarshalAs(UnmanagedType.LPArray)]byte[] pbSigBlob,
                                   uint cbSigBlob, [In, MarshalAs(UnmanagedType.IUnknown)]Object pAssemEmit, [In, MarshalAs(UnmanagedType.IUnknown)]IMetaDataEmit emit,
                                   [In, MarshalAs(UnmanagedType.LPArray)]byte[] pvTranslatedSig, uint cbTranslatedSigMax, out uint pcbTranslatedSig);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetMethodImplFlags(uint md, uint dwImplFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetFieldRVA(uint fd, uint ulRVA);
 	
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Merge([In, MarshalAs(UnmanagedType.IUnknown)]IMetaDataImport pImport, [In, MarshalAs(UnmanagedType.IUnknown)]Object pHostMapToken, [In, MarshalAs(UnmanagedType.IUnknown)]Object pHandler);

        void MergeEnd();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DefineMethodSpec([In] Int32 tkParent, [In, MarshalAs(UnmanagedType.LPArray)] Byte[] pvSigBlob, [In] Int32 cbSigBlob,[Out] out Int32 pmi) ;



        void GetDeltaSaveSize(            // S_OK or error.

            Int32 fSave,                  // [IN] cssAccurate or cssQuick.

            out Int32 pdwSaveSize) ;    // [OUT] Put the size here.



        void SaveDelta(                   // S_OK or error.

            String szFile,                 // [IN] The filename to save to.

            Int32 dwSaveFlags) ;     // [IN] Flags for the save.



        void SaveDeltaToStream(           // S_OK or error.

            IntPtr pIStream,              // [IN] A writable stream to save to.

            Int32 dwSaveFlags) ;     // [IN] Flags for the save.



        void SaveDeltaToMemory(           // S_OK or error.

            out IntPtr pbData,                // [OUT] Location to write data.

            Int32 cbData) ;          // [IN] Max size of data buffer.



        void DefineGenericParam(          // S_OK or error.

            Int32 tk,                    // [IN] TypeDef or MethodDef

            Int32 ulParamSeq,            // [IN] Index of the type parameter

            Int32 dwParamFlags,          // [IN] Flags, for future use (e.g. variance)

            String szname,                // [IN] Name

            Int32 reserved,              // [IN] For future use (e.g. non-type parameters)

            Int32[] rtkConstraints,      // [IN] Array of type constraints (TypeDef,TypeRef,TypeSpec)

        IntPtr pgp) ;         // [OUT] Put GenericParam token here



        void SetGenericParamProps(        // S_OK or error.

            Int32 gp,                  // [IN] GenericParam

            Int32 dwParamFlags,          // [IN] Flags, for future use (e.g. variance)

            String szName,                // [IN] Optional name

            Int32 reserved,              // [IN] For future use (e.g. non-type parameters)

            Int32 rtkConstraints);// [IN] Array of type constraints (TypeDef,TypeRef,TypeSpec)

    

    void ResetENCLog();         // S_OK or error.
    }
}
