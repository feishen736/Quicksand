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
    [Guid("EE62470B-E94B-424E-9B7C-2F00C9249F93")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal interface IMetaDataAssemblyImport
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetAssemblyProps([In] Int32 mda, [Out] out IntPtr ppbPublicKey, [Out] out Int32 pcbPublicKey, [Out] out Int32 pulHashAlgId, [Out, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 cchName, [Out] out Int32 pchName, [Out] out IntPtr pMetaData, [Out] out Int32 pdwAssemblyFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetAssemblyRefProps(Int32 mdar, [Out] out IntPtr ppbPublicKeyOrToken, [Out] out Int32 pcbPublicKeyOrToken, [Out, MarshalAs(UnmanagedType.LPArray)] Char[] szName, [In] Int32 cchName, [Out] out Int32 pchName, ref AssemblyMetadata pMetaData, [Out] out IntPtr ppbHashValue, [Out] out Int32 pcbHashValue, [Out] out Int32 pdwAssemblyRefFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetFileProps([In] Int32 mdf, [Out, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 cchName, [Out] out Int32 pchName, [Out] out IntPtr ppbHashValue, [Out] out Int32 pcbHashValue, [Out] out Int32 pdwFileFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetExportedTypeProps([In] Int32 mdct, [Out, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 cchName, [Out] out Int32 pchName, [Out] out Int32 ptkImplementation, [Out] out Int32 ptkTypeDef, [Out] out Int32 pdwExportedTypeFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetManifestResourceProps([In] Int32 mdmr, [Out, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 cchName, [Out] out Int32 pchName, [Out] out Int32 ptkImplementation, [Out] out Int32 pdwOffset, [Out] out Int32 pdwResourceFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumAssemblyRefs([In] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] Int32[] rAssemblyRefs, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumFiles([In]ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] Int32[] rFiles, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumExportedTypes([In]ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] Int32[] rExportedTypes, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 EnumManifestResources([In] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] Int32[] rManifestResources, [In] Int32 cMax, [Out] out Int32 pcTokens);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetAssemblyFromScope([Out] out Int32 ptkAssembly);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindExportedTypeByName([In, MarshalAs(UnmanagedType.LPWStr)] String szName, [In] Int32 mdtExportedType, [Out] out Int32 ptkExportedType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindManifestResourceByName([In, MarshalAs(UnmanagedType.LPWStr)] String szName, [Out] out Int32 ptkManifestResource);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CloseEnum(IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        Int32 FindAssembliesByName([In, MarshalAs(UnmanagedType.LPWStr)] String szAppBase, [In, MarshalAs(UnmanagedType.LPWStr)] String szPrivateBin, [In, MarshalAs(UnmanagedType.LPWStr)] String szAssemblyName, [Out, MarshalAs(UnmanagedType.IUnknown)] out Object ppIUnk, [In] Int32 cMax, [Out] out Int32 pcAssemblies);
    }

    internal struct AssemblyMetadata
    {
        public ushort usMajorVersion;
        public ushort usMinorVersion;
        public ushort usBuildNumber;
        public ushort usRevisionNumber;
        public Char[] szLocale;
        public Int32 cbLocale;
        public IntPtr rdwProcessor;
        public Int32 ulProcessor;
        public IntPtr rOS;
        public Int32 ulOS;
    }
}
