//
// System.Security.HostSecurityManager class
//
// Author:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_2_0

using System.Reflection;
using System.Security.Policy;

namespace System.Security {

	[Serializable]
	public class HostSecurityManager {

		public HostSecurityManager ()
		{
		}

		public virtual PolicyLevel DomainPolicy {
			// always return null - may be overriden
			get { return null; }
		}

		public virtual HostSecurityManagerFlags Flags {
			get { return HostSecurityManagerFlags.AllFlags; }
		}

		[MonoTODO ("incomplete - docs talks about a System.Runtime.Hosting in corlib but it's not there (yet?)")]
		public virtual ApplicationTrust DetermineApplicationTrust (Evidence applicationEvidence, Evidence activatorEvidence, TrustManagerContext context)
		{
			if (applicationEvidence == null)
				throw new ArgumentNullException ("applicationEvidence");
			// TODO extract the ActivationContext from the ActivationArguments (inside the applicationEvidence)
			ActivationContext ac = null;
			ApplicationSecurityManager.DetermineApplicationTrust (ac, context);
			return null;
		}

		public virtual Evidence ProvideAppDomainEvidence (Evidence inputEvidence)
		{
			// no changes - may be overriden
			return inputEvidence;
		}

		public virtual Evidence ProvideAssemblyEvidence (Assembly loadedAssembly, Evidence inputEvidence)
		{
			// no changes - may be overriden
			return inputEvidence;
		}

		public virtual PermissionSet ResolvePolicy (Evidence evidence)
		{
			if (evidence == null)
				throw new NullReferenceException ("evidence");
			return SecurityManager.ResolvePolicy (evidence);
		}
	}
}

#endif
