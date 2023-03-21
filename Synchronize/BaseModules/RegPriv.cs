﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace Synchronize.BaseModules
{
    class RegPriv
    {
        public const string CreateToken = "SeCreateTokenPrivilege";
        public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";
        public const string LockMemory = "SeLockMemoryPrivilege";
        public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";
        public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";
        public const string MachineAccount = "SeMachineAccountPrivilege";
        public const string TrustedComputingBase = "SeTcbPrivilege";
        public const string Security = "SeSecurityPrivilege";
        public const string TakeOwnership = "SeTakeOwnershipPrivilege";
        public const string LoadDriver = "SeLoadDriverPrivilege";
        public const string SystemProfile = "SeSystemProfilePrivilege";
        public const string SystemTime = "SeSystemtimePrivilege";
        public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";
        public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";
        public const string CreatePageFile = "SeCreatePagefilePrivilege";
        public const string CreatePermanent = "SeCreatePermanentPrivilege";
        public const string Backup = "SeBackupPrivilege";
        public const string Restore = "SeRestorePrivilege";
        public const string Shutdown = "SeShutdownPrivilege";
        public const string Debug = "SeDebugPrivilege";
        public const string Audit = "SeAuditPrivilege";
        public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";
        public const string ChangeNotify = "SeChangeNotifyPrivilege";
        public const string RemoteShutdown = "SeRemoteShutdownPrivilege";
        public const string Undock = "SeUndockPrivilege";
        public const string SyncAgent = "SeSyncAgentPrivilege";
        public const string EnableDelegation = "SeEnableDelegationPrivilege";
        public const string ManageVolume = "SeManageVolumePrivilege";
        public const string Impersonate = "SeImpersonatePrivilege";
        public const string CreateGlobal = "SeCreateGlobalPrivilege";
        public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";
        public const string ReserveProcessor = "SeReserveProcessorPrivilege";

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public Int32 lowPart;
            public Int32 highPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public Int32 Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public Int32 PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }

        [Flags]
        public enum PrivilegeAttributes
        {
            /// <summary>Privilège est désactivé.</summary>
            Disabled = 0,

            /// <summary>Privilège activé par défaut.</summary>
            EnabledByDefault = 1,

            /// <summary>Privilège est activé.</summary>
            Enabled = 2,

            /// <summary>Privilège est supprimé.</summary>
            Removed = 4,

            /// <summary>Privilège utilisé pour accéder à un objet ou un service.</summary>
            UsedForAccess = -2147483648
        }

        [Flags]
        public enum TokenAccessRights
        {
            /// <summary>Right to attach a primary token to a process.</summary>
            AssignPrimary = 0,

            /// <summary>Right to duplicate an access token.</summary>
            Duplicate = 1,

            /// <summary>Right to attach an impersonation access token to a process.</summary>
            Impersonate = 4,

            /// <summary>Right to query an access token.</summary>
            Query = 8,

            /// <summary>Right to query the source of an access token.</summary>
            QuerySource = 16,

            /// <summary>Right to enable or disable the privileges in an access token.</summary>
            AdjustPrivileges = 32,

            /// <summary>Right to adjust the attributes of the groups in an access token.</summary>
            AdjustGroups = 64,

            /// <summary>Right to change the default owner, primary group, or DACL of an access token.</summary>
            AdjustDefault = 128,

            /// <summary>Right to adjust the session ID of an access token.</summary>
            AdjustSessionId = 256,

            /// <summary>Combines all possible access rights for a token.</summary>
            AllAccess = AccessTypeMasks.StandardRightsRequired |
                AssignPrimary |
                Duplicate |
                Impersonate |
                Query |
                QuerySource |
                AdjustPrivileges |
                AdjustGroups |
                AdjustDefault |
                AdjustSessionId,

            /// <summary>Combines the standard rights required to read with <see cref="Query"/>.</summary>
            Read = AccessTypeMasks.StandardRightsRead |
                Query,

            /// <summary>Combines the standard rights required to write with <see cref="AdjustDefault"/>, <see cref="AdjustGroups"/> and <see cref="AdjustPrivileges"/>.</summary>
            Write = AccessTypeMasks.StandardRightsWrite |
                AdjustPrivileges |
                AdjustGroups |
                AdjustDefault,

            /// <summary>Combines the standard rights required to execute with <see cref="Impersonate"/>.</summary>
            Execute = AccessTypeMasks.StandardRightsExecute |
                Impersonate
        }

        [Flags]
        internal enum AccessTypeMasks
        {
            Delete = 65536,
            ReadControl = 131072,
            WriteDAC = 262144,
            WriteOwner = 524288,
            Synchronize = 1048576,
            StandardRightsRequired = 983040,
            StandardRightsRead = ReadControl,
            StandardRightsWrite = ReadControl,
            StandardRightsExecute = ReadControl,
            StandardRightsAll = 2031616,
            SpecificRightsAll = 65535
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AdjustTokenPrivileges(
            [In] IntPtr accessTokenHandle,
            [In, MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
            [In] ref TOKEN_PRIVILEGES newState,
            [In] int bufferLength,
            [In, Out] ref TOKEN_PRIVILEGES previousState,
            [In, Out] ref int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(
            [In] IntPtr handle);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LookupPrivilegeName(
           [In] string systemName,
           [In] ref LUID luid,
           [In, Out] StringBuilder name,
           [In, Out] ref int nameLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LookupPrivilegeValue(
            [In] string systemName,
            [In] string name,
            [In, Out] ref LUID luid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenProcessToken(
            [In] IntPtr processHandle,
            [In] TokenAccessRights desiredAccess,
            [In, Out] ref IntPtr tokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Int32 GetLastError();

        /**********************************************************************/

        /*                        Fonction  MySetPrivilege                    */

        /**********************************************************************/

        public static bool SetMyPrivilege(string sPrivilege, bool enablePrivilege)
        {
            bool blRc;
            TOKEN_PRIVILEGES newTP = new TOKEN_PRIVILEGES();
            TOKEN_PRIVILEGES oldTP = new TOKEN_PRIVILEGES();
            LUID luid = new LUID();
            int retrunLength = 0;
            IntPtr processToken = IntPtr.Zero;

            blRc = OpenProcessToken(GetCurrentProcess(), TokenAccessRights.AllAccess, ref processToken);
            if (blRc == false)
                return false;

            blRc = LookupPrivilegeValue(null, sPrivilege, ref luid);
            if (blRc == false)
                return false;

            newTP.PrivilegeCount = 1;
            newTP.Privileges = new LUID_AND_ATTRIBUTES[64];
            newTP.Privileges[0].Luid = luid;

            if (enablePrivilege)
                newTP.Privileges[0].Attributes = (Int32)PrivilegeAttributes.Enabled;
            else
                newTP.Privileges[0].Attributes = (Int32)PrivilegeAttributes.Disabled;

            oldTP.PrivilegeCount = 64;
            oldTP.Privileges = new LUID_AND_ATTRIBUTES[64];
            blRc = AdjustTokenPrivileges(processToken,
                                          false,
                                          ref newTP,
                                          16,
                                          ref oldTP,
                                          ref retrunLength);
            if (blRc == false)
            {
                Int32 iRc = GetLastError();
                return false;
            }
            return true;
        }
        private static void ShowSecurity(RegistrySecurity security)
        {
            Console.WriteLine("\r\nCurrent access rules:\r\n");

            foreach (RegistryAccessRule ar in security.GetAccessRules(true, true, typeof(NTAccount)))
            {

                Console.WriteLine("        User: {0}", ar.IdentityReference);
                Console.WriteLine("        Type: {0}", ar.AccessControlType);
                Console.WriteLine("      Rights: {0}", ar.RegistryRights);
                Console.WriteLine(" Inheritance: {0}", ar.InheritanceFlags);
                Console.WriteLine(" Propagation: {0}", ar.PropagationFlags);
                Console.WriteLine("   Inherited? {0}", ar.IsInherited);
                Console.WriteLine();
            }

        }

        public static void SetCommandPromptIntoConetxMenu(string key_path,bool blShow)
        {
            try
            {
                /*
                 * Get the ID of the current user (aka Admin)
                 */
                WindowsIdentity id = WindowsIdentity.GetCurrent();

                /*
                 * Add the TakeOwnership Privilege
                 */
                bool blRc = RegPriv.SetMyPrivilege(RegPriv.TakeOwnership, true);
                if (!blRc)
                    throw new PrivilegeNotHeldException(RegPriv.TakeOwnership);

                /*
                 * Add the Restore Privilege (must be done to change the owner)
                 */
                blRc = RegPriv.SetMyPrivilege(RegPriv.Restore, true);
                if (!blRc)
                    throw new PrivilegeNotHeldException(RegPriv.Restore);

                /*
                 * Open a registry which I don't own
                 */
                RegistryKey rkADSnapInsNodesTypes = Registry.ClassesRoot.OpenSubKey(key_path,
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.TakeOwnership);
                RegistrySecurity regSecTempo = rkADSnapInsNodesTypes.GetAccessControl(AccessControlSections.All);


                /*
                 * Get the real owner
                 */
                IdentityReference oldId = regSecTempo.GetOwner(typeof(SecurityIdentifier));
                SecurityIdentifier siTrustedInstaller = new SecurityIdentifier(oldId.ToString());

                //Console.WriteLine(oldId.ToString());

                /*
                 * process user become the owner
                 */

                regSecTempo.SetOwner(id.User);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);

                /*
                 * Add the full control
                 */

                RegistryAccessRule regARFullAccess = new RegistryAccessRule(id.User, RegistryRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow);
                regSecTempo.AddAccessRule(regARFullAccess);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);

                /*
                 * What I have TO DO
                 */

                if (blShow)
                {
                    RegistryKey key;
                    key = Registry.ClassesRoot.OpenSubKey(key_path, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                    object val = key.GetValue("HideBasedOnVelocityId");
                    if (val != null)
                    {
                        key.SetValue("ShowBasedOnVelocityId", val);
                        key.DeleteValue("HideBasedOnVelocityId");
                    }
                }
                else
                {
                    RegistryKey key;
                    key = Registry.ClassesRoot.OpenSubKey(key_path, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                    object val = key.GetValue("ShowBasedOnVelocityId");
                    if (val != null)
                    {
                        key.SetValue("HideBasedOnVelocityId", val);
                        key.DeleteValue("ShowBasedOnVelocityId");
                    }
                }

                /*
                 * Put back the original owner
                 */

                //regSecTempo.SetOwner(siFinalOwner);

                regSecTempo.SetOwner(siTrustedInstaller);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);

                /*
                 * Put back the original Rights
                 */
                regSecTempo.RemoveAccessRule(regARFullAccess);
                rkADSnapInsNodesTypes.SetAccessControl(regSecTempo);
            }
            catch (Exception excpt)
            {
                throw excpt;
            }
        }

    }
}
