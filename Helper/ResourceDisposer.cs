using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Helper
{
	public class ResourceDisposer : IContainer
	{
		private int _currentPrio = 500;
		private SortedList<int, IComponent> _components;

		public ResourceDisposer()
		{
			_components = new SortedList<int, IComponent>();
		}

		#region IContainer Members

		public void Add(IComponent component, string name)
		{
			Add(component);
		}

		/// <summary>
		/// Add component with default priority (500)
		/// </summary>
		/// <param name="component">IComponent</param>
		public void Add(IComponent component)
		{
			if (component != null)
				_components.Add(_currentPrio, component);
		}
		/// <summary>
		/// Add component with priority
		/// </summary>
		/// <param name="component">IComponent</param>
		/// <param name="priority">Priority (1-first, 500-default 1000-lastest)</param>
		public void Add(IComponent component, int priority)
		{
			if (component != null)
				_components.Add(priority, component);
		}


		public ComponentCollection Components
		{
			get
			{
				return new ComponentCollection(new List<IComponent>(_components.Values).ToArray());
			}
		}

		public void Remove(IComponent component)
		{
			_components.Values.Remove(component);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach (KeyValuePair<int, IComponent> i in _components)
				i.Value.Dispose();
		}

		#endregion
	}
}
