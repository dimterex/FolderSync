using System;
using System.Windows;
using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;

namespace UI.View.TriggerAction
{
    /// <summary>
    ///     Shows a popup window in response to an <see cref="InteractionRequest{T}" /> being raised.
    /// </summary>
    public class CustomPopupWindowAction : PopupWindowAction
    {
        /// <summary>
        ///     Returns the window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to be set as a DataContext in the window.</param>
        /// <returns></returns>
        protected override Window GetWindow(INotification notification)
        {
            Window wrapperWindow;

            if (WindowContent != null || WindowContentType != null)
            {
                wrapperWindow = CreateWindow();

                if (wrapperWindow == null)
                    throw new NullReferenceException("CreateWindow cannot return null");

                // If the WindowContent does not have its own DataContext, it will inherit this one.
                wrapperWindow.DataContext = notification;
                if (!string.IsNullOrEmpty(notification.Title))
                    wrapperWindow.Title = notification.Title;

                PrepareContentForWindow(notification, wrapperWindow);
            }
            else
            {
                wrapperWindow = CreateDefaultWindow(notification);
            }

            if (AssociatedObject != null)
                wrapperWindow.Owner = Window.GetWindow(AssociatedObject);

            // If the user provided a Style for a Window we set it as the window's style.
            if (WindowStyle != null)
                wrapperWindow.Style = WindowStyle;

            // If the user has provided a startup location for a Window we set it as the window's startup location.
            if (WindowStartupLocation.HasValue)
                wrapperWindow.WindowStartupLocation = WindowStartupLocation.Value;

            return wrapperWindow;
        }
    }
}