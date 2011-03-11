using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace FRom.Helper
{
	public class ResourceDisposerComponent : IContainer
	{
		private List<IComponent> _components;

		public ResourceDisposerComponent()
		{
			_components = new List<IComponent>();
		}

		#region IContainer Members

		public void Add(IComponent component, string name)
		{
			Add(component);
		}

		public void Add(IComponent component)
		{
			if (component != null)
				_components.Add(component);
		}

		public ComponentCollection Components
		{
			get
			{
				return new ComponentCollection(_components.ToArray());
			}
		}

		public void Remove(IComponent component)
		{
			_components.Remove(component);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach (IComponent i in _components)
				i.Dispose();
		}

		#endregion
	}
}
