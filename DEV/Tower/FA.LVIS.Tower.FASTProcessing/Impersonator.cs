using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace FA.LVIS.Tower.FASTProcessing
{
	public class Impersonator
	{

		[System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref System.IntPtr phToken);

		/*[DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource,
			int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);*/

		[System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public extern static bool CloseHandle(IntPtr handle);

		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
			int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

		private static IntPtr tokenHandle;
		private static WindowsImpersonationContext impersonatedUser = null;

		public static void Impersonate(string pDomain, string pUser, string pPass)
		{
			if (tokenHandle != IntPtr.Zero || impersonatedUser != null)
			{
				throw new ApplicationException("Impersonation was not cleaned up. Please close this application.");
			}

			tokenHandle = new IntPtr(0);
			IntPtr dupeTokenHandle = new IntPtr(0);
			const int LOGON32_PROVIDER_DEFAULT = 0;
			//This parameter causes LogonUser to create a primary token.
			const int LOGON32_LOGON_INTERACTIVE = 2;

			tokenHandle = IntPtr.Zero;

			// Call LogonUser to obtain a handle to an access token.
			bool returnValue = LogonUser(pUser, pDomain, pPass,
				LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
				ref tokenHandle);

			if (false == returnValue)
			{
				int ret = Marshal.GetLastWin32Error();
				Console.WriteLine("LogonUser failed with error code : {0}", ret);
				throw new System.ComponentModel.Win32Exception(ret);
			}

			WindowsIdentity newId = new WindowsIdentity(tokenHandle);
			impersonatedUser = newId.Impersonate();

			return;

		}

		internal static void EndImpersonate()
		{
			// Stop impersonating the user.
			impersonatedUser.Undo();

			// Check the identity.
			Console.WriteLine("After Undo: " + WindowsIdentity.GetCurrent().Name);

			// Free the tokens.
			if (tokenHandle != IntPtr.Zero)
				CloseHandle(tokenHandle);
			impersonatedUser = null;
			tokenHandle = IntPtr.Zero;

		}

	}
}
