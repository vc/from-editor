using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace FRom.Logger
{
	[DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated]
	internal class UserStrings
	{
		// Fields
		private static CultureInfo resourceCulture;
		private static ResourceManager resourceMan;

		// Methods
		internal UserStrings()
		{
		}

		// Properties
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string DefaultMailNotSet
		{
			get
			{
				return ResourceManager.GetString("DefaultMailNotSet", resourceCulture);
			}
		}

		internal static string MessageNotSend
		{
			get
			{
				return ResourceManager.GetString("MessageNotSend", resourceCulture);
			}
		}

		internal static string MessageSend
		{
			get
			{
				return ResourceManager.GetString("MessageSend", resourceCulture);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager manager = new ResourceManager("IBP.UserStrings", typeof(UserStrings).Assembly);
					resourceMan = manager;
				}
				return resourceMan;
			}
		}
	}


}
