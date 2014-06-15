using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp.Services
{
	/// <summary>
	/// Interface responsible for abstracting ViewModels from Views.
	/// </summary>
	public interface IDialogService
	{
		/// <summary>
		/// Gets the registered views.
		/// </summary>
		ReadOnlyCollection<FrameworkElement> Views { get; }

		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param displayName="view">The registered View.</param>
		void Register(FrameworkElement view);

		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param displayName="view">The unregistered View.</param>
		void Unregister(FrameworkElement view);

		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <remarks>
		/// The dialog used to represent the ViewModel is retrieved from the registered mappings.
		/// </remarks>
		/// <param displayName="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <param displayName="viewModel">The ViewModel of the new dialog.</param>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		bool? ShowDialog(object ownerViewModel, object viewModel);

		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <param displayName="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <param displayName="viewModel">The ViewModel of the new dialog.</param>
		/// <typeparam displayName="T">The type of the dialog to show.</typeparam>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		bool? ShowDialog<T>(object ownerViewModel, object viewModel) where T : Window;

		/// <summary>
		/// Shows a message box.
		/// </summary>
		/// <param displayName="ownerViewModel">
		/// A ViewModel that represents the owner window of the message box.
		/// </param>
		/// <param displayName="messageBoxText">A string that specifies the text to display.</param>
		/// <param displayName="caption">A string that specifies the title bar caption to display.</param>
		/// <param displayName="button">
		/// A MessageBoxButton value that specifies which button or buttons to display.
		/// </param>
		/// <param displayName="icon">A MessageBoxImage value that specifies the icon to display.</param>
		/// <returns>
		/// A MessageBoxResult value that specifies which message box button is clicked by the user.
		/// </returns>
		MessageBoxResult ShowMessageBox(
		  object ownerViewModel,
		  string messageBoxText,
		  string caption,
		  MessageBoxButton button,
		  MessageBoxImage icon);

		DialogResult ShowSaveFileDialog(string filter, string filename, string title);

		string SaveFileName { get; }
	}
}
