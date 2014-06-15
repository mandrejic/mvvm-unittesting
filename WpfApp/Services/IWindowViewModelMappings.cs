using System;

namespace WpfApp.Services
{
	/// <summary>
	/// Interface describing the Window-ViewModel mappings used by the dialog service.
	/// </summary>	
	public interface IWindowViewModelMappings
	{
		/// <summary>
		/// Gets the window type based on registered ViewModel type.
		/// </summary>
		/// <param displayName="viewModelType">The type of the ViewModel.</param>
		/// <returns>The window type based on registered ViewModel type.</returns>
		Type GetWindowTypeFromViewModelType(Type viewModelType);
	}
}
