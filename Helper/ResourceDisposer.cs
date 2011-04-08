using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Helper
{
	public class ResourceDisposer : IContainer
	{
		class ComponentWithPrio : IComparable<ComponentWithPrio>
		{
			public int Prio;
			public IComponent Component;
			public ComponentWithPrio(IComponent component, int prio = 500)
			{
				Prio = prio;
				Component = component;
			}

			public override bool Equals(object obj)
			{
				ComponentWithPrio other = obj as ComponentWithPrio;
				if (other == null)
					return false;

				return this.Component == other.Component;
			}

			#region IComparable<ComponentWithPrio> Members

			public int CompareTo(ComponentWithPrio other)
			{
				return this.Prio - other.Prio;
			}

			#endregion
		}

		private int _currentPrio = 500;
		private BaseSortedCollection<ComponentWithPrio> _components;

		public ResourceDisposer()
		{
			_components = new BaseSortedCollection<ComponentWithPrio>();
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
				_components.Add(new ComponentWithPrio(component, _currentPrio));
		}
		/// <summary>
		/// Add component with priority
		/// </summary>
		/// <param name="component">IComponent</param>
		/// <param name="priority">Priority (1-first, 500-default 1000-lastest)</param>
		public void Add(IComponent component, int priority)
		{
			if (component != null)
				_components.Add(new ComponentWithPrio(component, priority));
		}


		public ComponentCollection Components
		{
			get
			{
				List<IComponent> lst = new List<IComponent>();
				foreach (ComponentWithPrio i in _components)
				{
					lst.Add(i.Component);
				}

				return new ComponentCollection(lst.ToArray());
			}
		}

		public void Remove(IComponent component)
		{
			_components.Remove(new ComponentWithPrio(component));
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach (ComponentWithPrio i in _components)
				i.Component.Dispose();
		}

		#endregion
	}
}
