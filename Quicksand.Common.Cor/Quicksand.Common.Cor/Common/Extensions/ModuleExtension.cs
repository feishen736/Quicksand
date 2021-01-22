using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{

    #region 枚举
    [Flags]
    public enum SigCallingType : Byte
    {
        CallConvMask = 0x0f,
        Default = 0x00,
        C = 0x01,
        StdCall = 0x02,
        ThisCall = 0x03,
        FastCall = 0x04,
        Vararg = 0x05,
        Field = 0x06,
        LocalSig = 0x07,
        Property = 0x08,
        Unmgd = 0x09,
        GenericInst = 0x0a,
        Generic = 0x10,
        HasThis = 0x20,
        ExplicitThis = 0x40,
    }

    [Flags]
    public enum SigElementType : Byte
    {
        End = 0x00,
        Void = 0x01,
        Boolean = 0x02,
        Char = 0x03,
        I1 = 0x04,
        U1 = 0x05,
        I2 = 0x06,
        U2 = 0x07,
        I4 = 0x08,
        U4 = 0x09,
        I8 = 0x0A,
        U8 = 0x0B,
        R4 = 0x0C,
        R8 = 0x0D,
        String = 0x0E,
        Ptr = 0x0F,
        ByRef = 0x10,
        ValueType = 0x11,
        Class = 0x12,
        Var = 0x13,
        Array = 0x14,
        GenericInst = 0x15,
        TypedByRef = 0x16,
        I = 0x18,
        U = 0x19,
        FnPtr = 0x1B,
        Object = 0x1C,
        SzArray = 0x1D,
        MVar = 0x1E,
        CModReqd = 0x1F,
        CModOpt = 0x20,
        Internal = 0x21,
        Max = 0x22,
        Modifier = 0x40,
        Sentinel = 0x41,
        Pinned = 0x45,
    }

    [Flags]
    public enum SigTokenType : Int32
    {
        Module = 0x00000000,
        TypeRef = 0x01000000,
        TypeDef = 0x02000000,
        FieldDef = 0x04000000,
        MethodDef = 0x06000000,
        ParamDef = 0x08000000,
        InterfaceImpl = 0x09000000,
        MemberRef = 0x0a000000,
        CustomAttribute = 0x0c000000,
        Permission = 0x0e000000,
        Signature = 0x11000000,
        Event = 0x14000000,
        Property = 0x17000000,
        ModuleRef = 0x1a000000,
        TypeSpec = 0x1b000000,
        Assembly = 0x20000000,
        AssemblyRef = 0x23000000,
        File = 0x26000000,
        ExportedType = 0x27000000,
        ManifestResource = 0x28000000,
        GenericPar = 0x2a000000,
        MethodSpec = 0x2b000000,
        String = 0x70000000,
        Name = 0x71000000,
        BaseType = 0x72000000,
        Invalid = 0x7FFFFFFF,
    }
    #endregion

    /// <summary>
    /// Module 扩展方法
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static class ModuleExtension
    {
        #region 元数据令牌
        /// <summary>
        /// 获取元数据对象接口实例
        /// </summary>
        /// <param name="module"></param>
        /// <param name="protectValue"></param>
        /// <returns>元数据接口实例</returns>
        private static Object GetMetadataInstance(this Module module, UInt32 protectValue = 0)
        {
            var offset = Environment.Is64BitProcess ? 0 : 1;

            var dataField = module.GetType().GetField("m_pData", BindingFlags.NonPublic | BindingFlags.Instance);
            var dataValue = (IntPtr)dataField.GetValue(module);

            var fileField = dataValue + 2 * IntPtr.Size;
            var fileValue = Marshal.ReadIntPtr(fileField);

            var internalPtr = fileValue + IntPtr.Size * (6 + offset);
            var internalValue = Marshal.ReadIntPtr(internalPtr);

            var emitterPtr = fileValue + IntPtr.Size * (8 + offset);
            var emitterValue = Marshal.ReadIntPtr(emitterPtr);

            if (emitterValue == IntPtr.Zero)
            {
                MsCorWks.GetMetaDataPublicInterfaceFromInternal(internalValue, typeof(IMetaDataEmit).GUID.ToByteArray(), out emitterValue);

                Marshal.WriteIntPtr(emitterPtr, emitterValue);
            }
            if (emitterValue != IntPtr.Zero)
            {
                MsCorWks.GetMetaDataInternalInterfaceFromPublic(emitterValue, typeof(IMDInternalImport).GUID.ToByteArray(), out internalValue);

                Marshal.WriteIntPtr(internalPtr, internalValue);
            }

            return (IMetaDataEmit)Marshal.GetObjectForIUnknown(emitterValue);
        }
        /// <summary>
        /// 获取元数据对象接口实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="module"></param>
        /// <param name="instance">接口实例</param>
        /// <returns>元数据接口实例</returns>
        private static T GetMetadataInstance<T>(this Module module, Object instance = null) where T : class
        {
            if (instance == null)
                instance = module.GetMetadataInstance();

            return instance as T;
        }

        /// <summary>
        /// 定义类库引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="assemblyRef">类库引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类库引用元数据令牌</returns>
        internal static Int32 DefineAssemblyRefMetadataToken(this Module module, Assembly assemblyRef, Object instance = null)
        {
            var assemblyMetadata = new AssemblyMetadata();
            assemblyMetadata.ulOS = 0;
            assemblyMetadata.cbLocale = 0;
            assemblyMetadata.ulProcessor = 0;
            assemblyMetadata.usBuildNumber = 0;
            assemblyMetadata.usMajorVersion = 0;
            assemblyMetadata.usMinorVersion = 0;
            assemblyMetadata.usRevisionNumber = 0;
            assemblyMetadata.szLocale = null;
            assemblyMetadata.rOS = IntPtr.Zero;
            assemblyMetadata.rdwProcessor = IntPtr.Zero;

            return 0;
        }
        /// <summary>
        /// 获取类库引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="assemblyRef">类库引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类库引用元数据令牌</returns>
        internal static Int32 GetAssemblyRefMetadataToken(this Module module, Assembly assemblyRef, Object instance = null)
        {
            var metaDataAssemblyImport = module.GetMetadataInstance<IMetaDataAssemblyImport>(instance);

            var assemblyRefs = new List<Int32>();

            var assemblyRefMetadataToken = 0;

            // 获取类库引用
            {
                var cMax = 1024;
                var pcTokens = 0;
                var phEnum = IntPtr.Zero;
                var rAssemblyRefs = new Int32[cMax];

                if (cMax > pcTokens)
                    metaDataAssemblyImport.EnumAssemblyRefs(ref phEnum, rAssemblyRefs, cMax, out pcTokens);
                if (cMax < pcTokens)
                    metaDataAssemblyImport.EnumAssemblyRefs(ref phEnum, rAssemblyRefs, pcTokens, out pcTokens);

                metaDataAssemblyImport.CloseEnum(phEnum);

                assemblyRefs = rAssemblyRefs.ToList().Take(pcTokens).ToList();
            }
            // 查找类库引用
            {
                foreach (var item in assemblyRefs)
                {
                    var mdar = item;
                    var pchName = 0;
                    var cchName = 1024;
                    var pcbHashValue = 0;
                    var pcbPublicKeyOrToken = 0;
                    var pdwAssemblyRefFlags = 0;
                    var szName = new Char[1024];
                    var pMetaData = IntPtr.Zero;
                    var ppbHashValue = IntPtr.Zero;
                    var ppbPublicKeyOrToken = IntPtr.Zero;
                    var assemblyMetadata = new AssemblyMetadata();

                    metaDataAssemblyImport.GetAssemblyRefProps(mdar, out ppbPublicKeyOrToken, out pcbPublicKeyOrToken, szName, cchName, out pchName, ref assemblyMetadata, out ppbHashValue, out pcbHashValue, out pdwAssemblyRefFlags);

                    var sourceName = new String(szName).Substring(0, pchName - 1);
                    var targetName = assemblyRef.GetName().Name;

                    if (sourceName != targetName)
                        continue;

                    assemblyRefMetadataToken = mdar;
                    break;
                }
            }

            return assemblyRefMetadataToken;
        }

        /// <summary>
        /// 定义类型规则元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类型规则元数据令牌</returns>
        internal static Int32 DefineTypeSpecMetadataToken(this Module module, Type typeInfo, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);

            var typeSpecMetadataToken = 0;

            // 定义类型规则
            {
                var pvSig = module.GetTypeMetadataSignature(typeInfo).ToArray();
                var cbSig = pvSig.Length;

                metaDataEmit.GetTokenFromTypeSpec(pvSig, cbSig, out typeSpecMetadataToken);
            }

            return typeSpecMetadataToken;
        }
        /// <summary>
        /// 获取类型规则元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类型规则元数据令牌</returns>
        internal static Int32 GetTypeSpecMetadataToken(this Module module, Type typeInfo, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(instance);

            var typeSpecs = new List<Int32>();

            var typeSpecMetadataToken = 0;

            // 查找类型规则
            {
                var cMax = 1024;
                var pcTypeSpecs = 0;
                var phEnum = IntPtr.Zero;
                var rTypeSpecs = new Int32[cMax];

                if (cMax > pcTypeSpecs)
                    metaDataImport.EnumTypeSpecs(phEnum, rTypeSpecs, cMax, out pcTypeSpecs);
                if (cMax < pcTypeSpecs)
                    metaDataImport.EnumTypeSpecs(phEnum, rTypeSpecs, pcTypeSpecs, out pcTypeSpecs);

                metaDataImport.CloseEnum(phEnum);

                typeSpecs = rTypeSpecs.Take(pcTypeSpecs).ToList();
            }
            // 查找类型规则
            {
                foreach (var item in typeSpecs)
                {
                    var pcbSig = 0;
                    var typespec = item;
                    var ppvSig = IntPtr.Zero;

                    metaDataImport.GetTypeSpecFromToken(typespec, out ppvSig, out pcbSig);

                    var sourceData = new Byte[pcbSig];
                    var targetData = module.GetTypeMetadataSignature(typeInfo);

                    Marshal.Copy(ppvSig, sourceData, 0, pcbSig);

                    if (!sourceData.SequenceEqual(targetData))
                        continue;

                    typeSpecMetadataToken = item;
                    break;
                }
            }

            return typeSpecMetadataToken;
        }

        /// <summary>
        /// 定义成员规则元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memberInfo">成员信息</param>
        /// <param name="instance">实例接口</param>
        /// <returns>成员规则元数据令牌</returns>
        internal static Int32 DefineMemberSpecMetadataToken(this Module module, MemberInfo memberInfo, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);

            var methodSpecMetadataToken = 0;

            // 定义方法规则
            //{
            //    var tkParent = parten;
            //    var pvSigBlob = signatures;
            //    var cbSigBlob = signatures.Length;

            //    metaDataEmit.DefineMethodSpec(tkParent, pvSigBlob, cbSigBlob, out methodSpecMetadataToken);
            //}

            return methodSpecMetadataToken;
        }
        /// <summary>
        /// 获取成员规则元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memberInfo">成员信息</param>
        /// <param name="instance">实例接口</param>
        /// <returns>成员规则元数据令牌</returns>
        internal static Int32 GetMemberSpecMetadataToken(this Module module, MemberInfo memberInfo, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>();

            var methodSpecs = new List<Int32>();

            var methodSpecMetadataToken = 0;

            // 获取方法规则
            {

                var tk = memberInfo.MetadataToken;
                var cMax = 1024;
                var pcMethodSpecs = 0;
                var phEnum = IntPtr.Zero;
                var rMethodSpecs = new Int32[cMax];

                if (cMax > pcMethodSpecs)
                    metaDataImport.EnumMethodSpecs(phEnum, tk, rMethodSpecs, cMax, out pcMethodSpecs);
                if (cMax < pcMethodSpecs)
                    metaDataImport.EnumMethodSpecs(phEnum, tk, rMethodSpecs, pcMethodSpecs, out pcMethodSpecs);

                metaDataImport.CloseEnum(phEnum);

                methodSpecs = methodSpecs.Take(pcMethodSpecs).ToList();
            }
            // 查找方法规则
            {
                foreach (var item in methodSpecs)
                {
                    var mi = item;
                    var tkParent = 0;
                    var ppvSigBlob = IntPtr.Zero;
                    var pcbSigBlob = 0;

                    metaDataImport.GetMethodSpecProps(mi, out tkParent, out ppvSigBlob, out pcbSigBlob);
                }
            }

            return methodSpecMetadataToken;
        }

        /// <summary>
        /// 定义类型引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeRef">类型引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类型引用元数据令牌</returns>
        internal static Int32 DefineTypeRefMetadataToken(this Module module, Type typeRef, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);
            var metaDataAssemblyImport = module.GetMetadataInstance<IMetaDataAssemblyImport>(metaDataEmit);

            var cbHashValue = 0;
            var pbHashValue = IntPtr.Zero;

            var typeRefMetadataToken = 0;

            // 获取类库引用
            {
                var mdar = module.GetAssemblyRefMetadataToken(typeRef.Module.Assembly, instance);
                var pchName = 0;
                var cchName = 1024;
                var pcbPublicKeyOrToken = 0;
                var pdwAssemblyRefFlags = 0;
                var szName = new Char[1024];
                var ppbPublicKeyOrToken = IntPtr.Zero;
                var assemblyMetadata = new AssemblyMetadata();

                metaDataAssemblyImport.GetAssemblyRefProps(mdar, out ppbPublicKeyOrToken, out pcbPublicKeyOrToken, szName, cchName, out pchName, ref assemblyMetadata, out pbHashValue, out cbHashValue, out pdwAssemblyRefFlags);
            }
            // 定义类型引用
            {
                var pAssemImport = typeRef.Module.GetMetadataInstance<IMetaDataAssemblyImport>();
                var pImport = typeRef.Module.GetMetadataInstance<IMetaDataImport>(pAssemImport);
                var pAssemEmit = module.GetMetadataInstance<IMetaDataAssemblyEmit>(metaDataAssemblyImport);
                var tdImport = typeRef.MetadataToken;

                metaDataEmit.DefineImportType(pAssemImport, pbHashValue, cbHashValue, pImport, tdImport, pAssemEmit, out typeRefMetadataToken);
            }

            return typeRefMetadataToken;
        }
        /// <summary>
        /// 引用类型引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeRef">类型引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>类型引用元数据令牌</returns>
        internal static Int32 GetTypeRefMetadataToken(this Module module, Type typeRef, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(instance);

            var typeRefMetadataToken = 0;

            var typeRefs = new List<Int32>();

            // 获取类型引用
            {
                var cMax = 1024;
                var pcTypeRefs = 0;
                var phEnum = IntPtr.Zero;
                var rTypeRefs = new Int32[1024];

                if (cMax > pcTypeRefs)
                    metaDataImport.EnumTypeRefs(phEnum, rTypeRefs, cMax, out pcTypeRefs);
                if (cMax < pcTypeRefs)
                    metaDataImport.EnumTypeRefs(phEnum, rTypeRefs, pcTypeRefs, out pcTypeRefs);

                metaDataImport.CloseEnum(phEnum);

                typeRefs = rTypeRefs.ToList().Take(pcTypeRefs).ToList();
            }
            // 查找类型引用
            {
                foreach (var item in typeRefs)
                {
                    var tr = item;
                    var pchName = 0;
                    var cchName = 1024;
                    var ptkResolutionScope = 0;
                    var szName = new Char[1024];

                    metaDataImport.GetTypeRefProps(tr, out ptkResolutionScope, szName, cchName, out pchName);

                    var sourceName = new String(szName).Substring(0, pchName - 1);
                    var targetName = typeRef.FullName;

                    if (sourceName != targetName)
                        continue;

                    typeRefMetadataToken = item;
                    break;
                }
            }

            return typeRefMetadataToken;
        }

        /// <summary>
        /// 定义成员引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memberRef">成员引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>成员引用元数据令牌</returns>
        internal static Int32 DefineMemberRefMetadataToken(this Module module, MemberInfo memberRef, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);
            var metaDataAssemblyImport = module.GetMetadataInstance<IMetaDataAssemblyImport>(metaDataEmit);

            var cbHashValue = 0;
            var pbHashValue = IntPtr.Zero;
            var memberRefMetadataToken = 0;

            // 获取类库引用
            {
                var mdar = module.GetAssemblyRefMetadataToken(memberRef.Module.Assembly, instance);
                var pchName = 0;
                var cchName = 1024;
                var pcbPublicKeyOrToken = 0;
                var pdwAssemblyRefFlags = 0;
                var szName = new Char[1024];
                var pMetaData = IntPtr.Zero;
                var ppbPublicKeyOrToken = IntPtr.Zero;
                var assemblyMetadata = new AssemblyMetadata();

                metaDataAssemblyImport.GetAssemblyRefProps(mdar, out ppbPublicKeyOrToken, out pcbPublicKeyOrToken, szName, cchName, out pchName, ref assemblyMetadata, out pbHashValue, out cbHashValue, out pdwAssemblyRefFlags);
            }
            // 定义导入成员
            {
                var typeRefOrSpecMetadataToken = 0;

                if (memberRef.DeclaringType.IsGenericType)
                {
                    if (typeRefOrSpecMetadataToken == 0)
                        typeRefOrSpecMetadataToken = module.GetTypeSpecMetadataToken(memberRef.DeclaringType, metaDataAssemblyImport);
                    if (typeRefOrSpecMetadataToken == 0)
                        typeRefOrSpecMetadataToken = module.DefineTypeSpecMetadataToken(memberRef.DeclaringType, metaDataAssemblyImport);
                }
                else
                {
                    if (typeRefOrSpecMetadataToken == 0)
                        typeRefOrSpecMetadataToken = module.GetTypeRefMetadataToken(memberRef.DeclaringType, metaDataAssemblyImport);
                    if (typeRefOrSpecMetadataToken == 0)
                        typeRefOrSpecMetadataToken = module.DefineTypeRefMetadataToken(memberRef.DeclaringType, metaDataAssemblyImport);
                }

                var pAssemImport = memberRef.Module.GetMetadataInstance<IMetaDataAssemblyImport>();
                var pImport = memberRef.Module.GetMetadataInstance<IMetaDataImport>(pAssemImport);
                var pAssemEmit = module.GetMetadataInstance<IMetaDataAssemblyEmit>(metaDataAssemblyImport);
                var mbMember = memberRef.MetadataToken;
                var tkParent = typeRefOrSpecMetadataToken;

                metaDataEmit.DefineImportMember(pAssemImport, pbHashValue, cbHashValue, pImport, mbMember, pAssemEmit, tkParent, out memberRefMetadataToken);
            }

            return memberRefMetadataToken;
        }
        /// <summary>
        /// 获取成员引用元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memberRef">成员引用</param>
        /// <param name="instance">接口实例</param>
        /// <returns>成员引用元数据令牌</returns>
        internal static Int32 GetMemberRefMetadataToken(this Module module, MemberInfo memberRef, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(instance);

            var typeRefOrSpecMetadataToken = 0;

            var memberRefMetadataToken = 0;

            var memberRefs = new List<Int32>();

            // 获取成员引用
            {
                if (memberRef.DeclaringType.IsGenericType)
                    typeRefOrSpecMetadataToken = module.GetTypeSpecMetadataToken(memberRef.DeclaringType, instance);
                else
                    typeRefOrSpecMetadataToken = module.GetTypeRefMetadataToken(memberRef.DeclaringType, instance);

                var cMax = 1024;
                var pcTokens = 0;
                var phEnum = IntPtr.Zero;
                var rMemberRefs = new Int32[1024];
                var tkParent = typeRefOrSpecMetadataToken;

                if (cMax > pcTokens)
                    metaDataImport.EnumMemberRefs(ref phEnum, tkParent, rMemberRefs, cMax, out pcTokens);
                if (cMax < pcTokens)
                    metaDataImport.EnumMemberRefs(ref phEnum, tkParent, rMemberRefs, pcTokens, out pcTokens);

                metaDataImport.CloseEnum(phEnum);

                memberRefs = rMemberRefs.ToList().Take(pcTokens).ToList();
            }
            // 查找成员引用
            {
                foreach (var item in memberRefs)
                {
                    var ptk = 0;
                    var mr = item;
                    var pchMember = 0;
                    var cchMember = 1024;
                    var pbSigBlobRef = 0;
                    var pvSigBlobRef = IntPtr.Zero;
                    var szMember = new Char[1024];

                    metaDataImport.GetMemberRefProps(mr, out ptk, szMember, cchMember, out pchMember, out pvSigBlobRef, out pbSigBlobRef);

                    var sourceName = new String(szMember).Substring(0, pchMember - 1);
                    var targetName = memberRef.Name;

                    if (sourceName != targetName)
                        continue;

                    if (memberRef is MethodBase == false)
                    {
                        memberRefMetadataToken = item;
                        break;
                    }

                    var methodRef = module.ResolveMethod(mr);
                    var methodDef = memberRef.Module.ResolveMethod(memberRef.MetadataToken);

                    if (methodRef.MetadataToken != methodDef.MetadataToken)
                        continue;

                    memberRefMetadataToken = item;
                    break;
                }
            }

            return memberRefMetadataToken;
        }

        /// <summary>
        /// 定义字符信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="value">字符信息</param>
        /// <param name="instance">接口实例</param>
        /// <returns>字符信息元数据令牌</returns>
        internal static Int32 DefineStringMetadataToken(this Module module, String value, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);

            var stringMetadataToken = 0;

            // 定义字符信息
            {
                var szString = value;
                var cchString = value.Length;

                metaDataEmit.DefineUserString(szString, cchString, out stringMetadataToken);
            }

            return stringMetadataToken;
        }
        /// <summary>
        /// 获取字符信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="value">字符信息</param>
        /// <param name="instance">接口实例</param>
        /// <returns>字符信息元数据令牌</returns>
        internal static Int32 GetStringMetadataToken(this Module module, String value, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(instance);

            var stringMetadataToken = 0;

            var strings = new List<Int32>();

            // 获取字符信息
            {
                var cMax = 1024;
                var pcStrings = 0;
                var phEnum = IntPtr.Zero;
                var rStrings = new Int32[cMax];

                if (cMax > pcStrings)
                    metaDataImport.EnumUserStrings(ref phEnum, rStrings, cMax, out pcStrings);
                if (cMax < pcStrings)
                    metaDataImport.EnumUserStrings(ref phEnum, rStrings, pcStrings, out pcStrings);

                metaDataImport.CloseEnum(phEnum);

                strings = rStrings.ToList().Take(pcStrings).ToList();
            }
            // 查找字符信息
            {
                foreach (var item in strings)
                {
                    var stk = item;
                    var pchString = 0;
                    var cchString = 1024;
                    var szString = new Char[cchString];

                    metaDataImport.GetUserString(stk, szString, cchString, out pchString);

                    var sourceValue = new String(szString).Substring(0, pchString - 1);
                    var targetValue = value;

                    if (sourceValue != targetValue)
                        continue;

                    stringMetadataToken = item;
                }
            }

            return stringMetadataToken;
        }

        /// <summary>
        /// 定义签名信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="signatures">签名数据</param>
        /// <param name="signaturePoint">签名指针</param>
        /// <param name="instance">接口实例</param>
        /// <returns>签名元数据令牌</returns>
        internal static Int32 DefineSignatureMetadataToken(this Module module, Byte[] signatures, Object instance = null)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>(instance);

            var pmsig = 0;
            metaDataEmit.GetTokenFromSig(signatures, signatures.Length, out pmsig);

            return pmsig;
        }
        /// <summary>
        /// 获取签名信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="signatures">签名数据</param>
        /// <param name="signaturePoint">签名指针</param>
        /// <param name="instance">接口实例</param>
        /// <returns>签名元数据令牌</returns>
        internal static Int32 GetSignatureMetadataToken(this Module module, Byte[] signatures, Object instance = null)
        {
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(instance);

            var signatureMetadataToken = 0;

            var sigInfo = new List<Int32>();

            // 获取签名信息
            {
                var cmax = 1024;
                var pcSignatures = 0;
                var phEnum = IntPtr.Zero;
                var rSignatures = new Int32[cmax];

                if (cmax > pcSignatures)
                    metaDataImport.EnumSignatures(phEnum, rSignatures, cmax, out pcSignatures);
                if (cmax < pcSignatures)
                    metaDataImport.EnumSignatures(phEnum, rSignatures, pcSignatures, out pcSignatures);

                metaDataImport.CloseEnum(phEnum);

                sigInfo = rSignatures.ToList().Take(pcSignatures).ToList();
            }
            // 查找签名信息
            {
                foreach (var item in sigInfo)
                {
                    var pcbSig = 0;
                    var mdSig = item;
                    var ppvSig = IntPtr.Zero;
                    metaDataImport.GetSigFromToken(mdSig, out ppvSig, out pcbSig);

                    if (pcbSig != signatures.Length)
                        continue;

                    var sigData = new Byte[pcbSig];
                    Marshal.Copy(ppvSig, sigData, 0, pcbSig);

                    if (!sigData.SequenceEqual(signatures))
                        continue;

                    signatureMetadataToken = mdSig;
                    break;
                }
            }

            return signatureMetadataToken;
        }

        /// <summary>
        /// 获取类型信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <returns>类型信息元数据令牌</returns>
        public static Int32 GetTypeMetadataToken(this Module module, Type typeInfo)
        {
            if (typeInfo.Module == module)
                return typeInfo.MetadataToken;
            else
                return module.GetTypeRefMetadataToken(typeInfo);
        }
        /// <summary>
        /// 获取成员信息元数据令牌
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memberInfo">成员信息</param>
        /// <returns>成员信息元数据令牌</returns>
        public static Int32 GetMemberMetadataToken(this Module module, MemberInfo memberInfo)
        {
            if (memberInfo.Module == module)
                return memberInfo.MetadataToken;
            else
                return module.GetMemberRefMetadataToken(memberInfo);
        }
        /// <summary>
        /// 指定元数据令牌是否有效
        /// </summary>
        /// <param name="module"></param>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        public static Boolean IsValidMetadataToken(this Module module, Int32 metadataToken)
        {
            var metadataImport = module.GetType().GetProperty("MetadataImport", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);
            var metadataImportValue = metadataImport.GetValue(module);

            var isValidToken = metadataImportValue.GetType().GetMethod("IsValidToken");
            var isValidTokenValue = isValidToken.Invoke(metadataImportValue, new Object[] { metadataToken });

            return (Boolean)isValidTokenValue;
        }
        #endregion

        #region 元数据签名
        /// <summary>
        /// 获取元素信息签名类型
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <returns>元素信息签名类型</returns>
        private static SigElementType GetSignatureType(this Module module, Type typeInfo)
        {
            var getSigElementTypeInvoke = typeof(RuntimeTypeHandle).GetMethod("GetCorElementType", BindingFlags.Static | BindingFlags.NonPublic);
            var getSigElementTypeValue = (Byte)getSigElementTypeInvoke.Invoke(null, new Object[] { typeInfo });

            return (SigElementType)getSigElementTypeValue;
        }
        /// <summary>
        /// 获取数据信息签名数据
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <returns>数据信息签名数据</returns>
        private static List<Byte> GetSignatureData(this Module module, Type typeInfo)
        {
            var sigValue = new List<Byte>();

            var sigData = 0;
            var sigType = 0;
            var sigToken = 0;

            if (sigToken == 0)
                sigToken = module.GetTypeMetadataToken(typeInfo);
            if (sigToken == 0)
                sigToken = module.DefineTypeRefMetadataToken(typeInfo);

            sigData = (sigToken & 0x00FFFFFF);
            sigType = (sigToken & unchecked((int)0xFF000000));

            sigData = (sigData << 2);

            if ((SigTokenType)sigType == SigTokenType.TypeRef)
                sigData |= 0x1;
            else if ((SigTokenType)sigType == SigTokenType.TypeSpec)
                sigData |= 0x2;

            if (sigData <= 0x7F)
            {
                sigValue.Add((byte)(sigData & 0xFF));
            }
            else if (sigData <= 0x3FFF)
            {
                sigValue.Add((byte)((sigData >> 8) | 0x80));
                sigValue.Add((byte)(sigData & 0xFF));
            }
            else if (sigData <= 0x1FFFFFFF)
            {
                sigValue.Add((byte)((sigData >> 24) | 0xC0));
                sigValue.Add((byte)((sigData >> 16) & 0xFF));
                sigValue.Add((byte)((sigData >> 8) & 0xFF));
                sigValue.Add((byte)((sigData) & 0xFF));
            }

            return sigValue;
        }

        /// <summary>
        /// 获取类型信息元数据签名
        /// </summary>
        /// <param name="module"></param>
        /// <param name="typeInfo">类型信息</param>
        /// <param name="genericInst">泛型实例</param>
        /// <returns>类型信息元数据签名</returns>
        internal static List<Byte> GetTypeMetadataSignature(this Module module, Type typeInfo, Boolean genericInst = false)
        {
            var sigData = new List<Byte>();

            if (typeInfo.IsGenericParameter)
            {
                if (typeInfo.DeclaringMethod != null)
                    sigData.Add((Byte)SigElementType.MVar);
                else
                    sigData.Add((Byte)SigElementType.Var);

                sigData.Add((Byte)typeInfo.GenericParameterPosition);
            }
            else if (typeInfo.IsGenericType && (!typeInfo.IsGenericTypeDefinition || genericInst == false))
            {
                sigData.Add((Byte)SigElementType.GenericInst);
                sigData.AddRange(module.GetTypeMetadataSignature(typeInfo.GetGenericTypeDefinition(), true));

                var args = typeInfo.GetGenericArguments();
                sigData.Add((Byte)args.Length);

                foreach (Type type in args)
                    sigData.AddRange(module.GetTypeMetadataSignature(type, false));
            }
            else if (typeInfo.IsPointer)
            {
                sigData.Add((Byte)SigElementType.Ptr);
                sigData.AddRange(module.GetTypeMetadataSignature(typeInfo.GetElementType(), false));
            }
            else if (typeInfo.IsByRef)
            {
                sigData.Add((Byte)SigElementType.ByRef);
                sigData.AddRange(module.GetTypeMetadataSignature(typeInfo.GetElementType(), false));
            }
            else if (typeInfo.IsArray)
            {
                var isSzArray = module.GetSignatureType(typeInfo);

                if (isSzArray == SigElementType.SzArray)
                {
                    sigData.Add((Byte)SigElementType.SzArray);
                    sigData.AddRange(module.GetTypeMetadataSignature(typeInfo.GetElementType(), false));
                }
                else
                {
                    sigData.Add((Byte)SigElementType.Array);
                    sigData.AddRange(module.GetTypeMetadataSignature(typeInfo.GetElementType(), false));

                    var rank = typeInfo.GetArrayRank();
                    sigData.Add((Byte)rank);
                    sigData.Add((Byte)0);
                    sigData.Add((Byte)rank);

                    for (int i = 0; i < rank; i++)
                        sigData.Add((Byte)0);
                }
            }
            else
            {
                var sigType = module.GetSignatureType(typeInfo);

                if (typeInfo is TypeInfo)
                {
                    if (sigType == SigElementType.Class)
                        if (typeInfo == typeof(Object))
                            sigType = SigElementType.Object;
                        else if (typeInfo == typeof(String))
                            sigType = SigElementType.String;

                }
                if (sigType == SigElementType.TypedByRef ||
                    sigType == SigElementType.Object ||
                    sigType <= SigElementType.String ||
                    sigType == SigElementType.I ||
                    sigType == SigElementType.U)
                {
                    sigData.Add((Byte)sigType);
                }
                else if (typeInfo.IsValueType)
                {
                    var data = module.GetSignatureData(typeInfo);
                    sigData.Add((Byte)SigElementType.ValueType);
                    sigData.AddRange(data);
                }
                else
                {
                    var data = module.GetSignatureData(typeInfo);
                    sigData.Add((Byte)SigElementType.Class);
                    sigData.AddRange(data);
                }
            }

            return sigData;
        }
        #endregion

        #region 元数据指针
        /// <summary>
        /// 获取签名信息元数据指针
        /// </summary>
        /// <param name="module"></param>
        /// <param name="signatures">签名数据</param>
        /// <returns>签名信息元数据指针</returns>
        public static IntPtr GetSignatureMetadataPointer(this Module module, Byte[] signatures)
        {
            var metaDataEmit = module.GetMetadataInstance<IMetaDataEmit>();
            var metaDataImport = module.GetMetadataInstance<IMetaDataImport>(metaDataEmit);

            var pmsig = 0;
            metaDataEmit.GetTokenFromSig(signatures, signatures.Length, out pmsig);

            var pcbSig = 0;
            var mdSig = pmsig;
            var ppvSig = IntPtr.Zero;
            metaDataImport.GetSigFromToken(mdSig, out ppvSig, out pcbSig);

            return ppvSig;
        }
        #endregion
    }
}
