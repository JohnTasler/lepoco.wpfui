// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using Wpf.Ui.Input;
using Wpf.Ui.Interop;
using Button = System.Windows.Controls.Button;
using Size = System.Windows.Size;

#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Customized window for notifications.
/// </summary>
public class MessageBox : Window
{
    // Template parts
    private Button? _firstEnabledButton;

    /// <summary>Identifies the <see cref="ShowTitle"/> dependency property.</summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
        nameof(PrimaryButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        nameof(SecondaryButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="CloseButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        nameof(CloseButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata("Close")
    );

    /// <summary>Identifies the <see cref="PrimaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonIconProperty = DependencyProperty.Register(
        nameof(PrimaryButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonIconProperty = DependencyProperty.Register(
        nameof(SecondaryButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="CloseButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonIconProperty = DependencyProperty.Register(
        nameof(CloseButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(PrimaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(SecondaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="CloseButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(
        nameof(CloseButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="IsPrimaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPrimaryButtonEnabled),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsSecondaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondaryButtonEnabled),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    private static readonly DependencyPropertyKey MessageBoxResultPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(MessageBoxResult),
        typeof(MessageBoxResult),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="MessageBoxResult"/> dependency property.</summary>
    public static readonly DependencyProperty MessageBoxResultProperty = MessageBoxResultPropertyKey.DependencyProperty;

    /// <summary>Identifies the <see cref="ButtonPanelAlignment"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonPanelAlignmentProperty = DependencyProperty.Register(
        nameof(ButtonPanelAlignment),
        typeof(HorizontalAlignment),
        typeof(MessageBox),
        new PropertyMetadata(HorizontalAlignment.Right)
    );

    private static readonly DependencyPropertyKey TemplateButtonCommandPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = TemplateButtonCommandPropertyKey.DependencyProperty;

    /// <summary>
    /// Gets or sets a value indicating whether to show the <see cref="System.Windows.Window.Title"/> in <see cref="TitleBar"/>.
    /// </summary>
    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to display on the primary button.
    /// </summary>
    public string PrimaryButtonText
    {
        get => (string)GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to be displayed on the secondary button.
    /// </summary>
    public string SecondaryButtonText
    {
        get => (string)GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to display on the close button.
    /// </summary>
    public string CloseButtonText
    {
        get => (string)GetValue(CloseButtonTextProperty);
        set => SetValue(CloseButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the primary button
    /// </summary>
    public IconElement? PrimaryButtonIcon
    {
        get => (IconElement?)GetValue(PrimaryButtonIconProperty);
        set => SetValue(PrimaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the secondary button
    /// </summary>
    public IconElement? SecondaryButtonIcon
    {
        get => (IconElement?)GetValue(SecondaryButtonIconProperty);
        set => SetValue(SecondaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the close button
    /// </summary>
    public IconElement? CloseButtonIcon
    {
        get => (IconElement?)GetValue(CloseButtonIconProperty);
        set => SetValue(CloseButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the primary button
    /// </summary>
    public ControlAppearance PrimaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);
        set => SetValue(PrimaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the secondary button.
    /// </summary>
    public ControlAppearance SecondaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);
        set => SetValue(SecondaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the close button.
    /// </summary>
    public ControlAppearance CloseButtonAppearance
    {
        get => (ControlAppearance)GetValue(CloseButtonAppearanceProperty);
        set => SetValue(CloseButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="MessageBox"/> primary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="MessageBox"/> secondary button is enabled.
    /// </summary>
    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    public MessageBoxResult MessageBoxResult
    {
        get => (MessageBoxResult)GetValue(MessageBoxResultProperty);
        private set => SetValue(MessageBoxResultPropertyKey, value);
    }

    /// <summary>
    /// Gets or sets a value indicating how the Panel containing the buttons are horizontally aligned.
    /// </summary>
    public HorizontalAlignment ButtonPanelAlignment
    {
        get => (HorizontalAlignment)GetValue(ButtonPanelAlignmentProperty);
        set => SetValue(ButtonPanelAlignmentProperty, value);
    }

    /// <summary>
    /// Gets the command triggered after clicking the button on the Footer.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandPropertyKey.DependencyProperty);

#if !NET8_0_OR_GREATER
    private static readonly PropertyInfo CanCenterOverWPFOwnerPropertyInfo = typeof(Window).GetProperty(
        "CanCenterOverWPFOwner",
        BindingFlags.NonPublic | BindingFlags.Instance
    )!;
#endif

    static MessageBox()
    {
        _ = SizeToContentProperty.AddChangedHandler<MessageBox, FrameworkPropertyMetadata, SizeToContent>(OnSizeToContentChanged);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBox"/> class.
    /// </summary>
    public MessageBox()
    {
        SetValue(
            TemplateButtonCommandPropertyKey,
            new RelayCommand<MessageBoxButton>(OnButtonCommandExecuted));

        PreviewMouseDoubleClick += static (_, args) => args.Handled = true;

        Loaded += static (sender, _) =>
        {
            var self = (MessageBox)sender;
            self.OnLoaded();
        };
    }

    protected TaskCompletionSource<MessageBoxResult>? Tcs { get; set; }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new void Show()
    {
        throw new InvalidOperationException($"Use {nameof(ShowDialogAsync)} instead");
    }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new bool? ShowDialog()
    {
        throw new InvalidOperationException($"Use {nameof(ShowDialogAsync)} instead");
    }

    [Obsolete($"Use {nameof(Close)} with MessageBoxResult instead")]
    public new void Close()
    {
        throw new InvalidOperationException($"Use {nameof(Close)} with MessageBoxResult instead");
    }

    /// <summary>
    /// Displays a message box
    /// </summary>
    /// <returns><see cref="MessageBoxResult"/></returns>
    /// <exception cref="TaskCanceledException">Thrown if the operation is canceled.</exception>
    public async Task<MessageBoxResult> ShowDialogAsync(
        bool showAsDialog = true,
        CancellationToken cancellationToken = default
    )
    {
        Tcs = new TaskCompletionSource<MessageBoxResult>();
        CancellationTokenRegistration tokenRegistration = cancellationToken.Register(
            o => Tcs.TrySetCanceled((CancellationToken)o!),
            cancellationToken
        );

#if NET6_0_OR_GREATER // Use IAsyncDisposable on platforms that support it
        await
#endif
        using (tokenRegistration)
        {
            RemoveTitleBarAndApplyMica();

            if (showAsDialog)
            {
                base.ShowDialog();
            }
            else
            {
                base.Show();
            }
        }

        return await Tcs.Task;
    }

    /// <summary>
    /// Occurs after Loading event
    /// </summary>
    protected virtual void OnLoaded()
    {
        FocusManager.SetFocusedElement(this, _firstEnabledButton);

        ResizeToContentSize(SizeToContent);

        switch (WindowStartupLocation)
        {
            case WindowStartupLocation.Manual:
            case WindowStartupLocation.CenterScreen:
                CenterWindowOnScreen();
                break;
            case WindowStartupLocation.CenterOwner:
                if (
                    !CanCenterOverWPFOwner()
                    || Owner.WindowState is WindowState.Maximized or WindowState.Minimized
                )
                {
                    CenterWindowOnScreen();
                }
                else
                {
                    CenterWindowOnOwner();
                }

                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public override void OnApplyTemplate()
    {
        Button? button = this.GetVisualDescendantsRecursive().OfType<Button>().FirstOrDefault(d => d.IsEnabled);
        if (button is Button)
        {
            _firstEnabledButton = button;
        }

        if (this.Template.FindName("PART_RootElement", this) is UIElement border)
        {
            DiagnosticExtensions.AddAnyObject1ChangedHandler(border, (d, e) =>
            {
                Type type = d.GetType();
                Debug.WriteLine($"Wpf.Ui.Extensions: INFO | {e.Arguments?.Property.Name} property on {type.FullName} instance changed from {e.Arguments?.OldValue} to {e.Arguments?.NewValue}");
            });
        }

        base.OnApplyTemplate();
    }

    // CanCenterOverWPFOwner property see https://source.dot.net/#PresentationFramework/System/Windows/Window.cs,e679e433777b21b8
    private bool CanCenterOverWPFOwner()
    {
#if NET8_0_OR_GREATER
        return CanCenterOverWPFOwnerAccessor(this);
#else
        return (bool)CanCenterOverWPFOwnerPropertyInfo.GetValue(this)!;
#endif
    }

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_CanCenterOverWPFOwner")]
    private static extern bool CanCenterOverWPFOwnerAccessor(Window w);
#endif

    /// <summary>
    /// Resizes the MessageBox to fit the content's size, including margins.
    /// </summary>
    protected virtual void ResizeToContentSize(SizeToContent newValue)
    {
        if (newValue == SizeToContent.Manual)
        {
            return;
        }

        UIElement? rootElement = this.GetVisualChildren().OfType<UIElement>().FirstOrDefault();
        if (rootElement is null)
        {
            return;
        }

        rootElement.InvalidateMeasure();
        Size size = new(double.PositiveInfinity, double.PositiveInfinity);
        rootElement.Measure(size);
        Size desiredSize = rootElement.DesiredSize;

        // left and right margin
//        const double margin = 12.0 * 2;

        void UpdateWidth()
        {
            SetCurrentValue(WidthProperty, desiredSize.Width); // + margin);
            ResizeWidth(rootElement);
        }

        void UpdateHeight()
        {
            SetCurrentValue(HeightProperty, desiredSize.Height);
            ResizeHeight(rootElement);
        }

        switch (newValue)
        {
            case SizeToContent.WidthAndHeight:
                UpdateWidth();
                UpdateHeight();
                break;
            case SizeToContent.Height:
                UpdateHeight();
                break;
            case SizeToContent.Width:
                UpdateWidth();
                break;
            case SizeToContent.Manual:
            default:
                break;
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (e.Cancel)
        {
            return;
        }

        _ = Tcs?.TrySetResult(MessageBoxResult.None);
    }

    protected virtual void CenterWindowOnScreen()
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        SetCurrentValue(LeftProperty, (screenWidth / 2) - (Width / 2));
        SetCurrentValue(TopProperty, (screenHeight / 2) - (Height / 2));
    }

    private void CenterWindowOnOwner()
    {
        double left = Owner.Left + ((Owner.Width - Width) / 2);
        double top = Owner.Top + ((Owner.Height - Height) / 2);

        SetCurrentValue(LeftProperty, left);
        SetCurrentValue(TopProperty, top);
    }

    /// <summary>
    /// Occurs after the <see cref="MessageBoxButton"/> is clicked
    /// </summary>
    /// <param name="button">The MessageBox button</param>
    protected virtual void OnButtonCommandExecuted(MessageBoxButton button)
    {
        MessageBoxResult result = button switch
        {
            MessageBoxButton.Primary => MessageBoxResult.Primary,
            MessageBoxButton.Secondary => MessageBoxResult.Secondary,
            _ => MessageBoxResult.None,
        };

        MessageBoxResult = result;

        _ = Tcs?.TrySetResult(result);
        base.Close();
    }

    private void RemoveTitleBarAndApplyMica()
    {
        _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        _ = WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
    }

    private void ResizeWidth(UIElement element)
    {
        if (Width <= MaxWidth)
        {
            return;
        }

        SetCurrentValue(WidthProperty, MaxWidth);
        element.UpdateLayout();

        SetCurrentValue(HeightProperty, element.DesiredSize.Height);

        if (Height > MaxHeight)
        {
            SetCurrentValue(MaxHeightProperty, Height);
        }
    }

    private void ResizeHeight(UIElement element)
    {
        if (Height <= MaxHeight)
        {
            return;
        }

        SetCurrentValue(HeightProperty, MaxHeight);
        element.UpdateLayout();

        SetCurrentValue(WidthProperty, element.DesiredSize.Width);

        if (Width > MaxWidth)
        {
            SetCurrentValue(MaxWidthProperty, Width);
        }
    }

    private static void OnSizeToContentChanged(MessageBox @this, SizeToContent newValue)
    {
        @this.ResizeToContentSize(newValue);
    }
}
