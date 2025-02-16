// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Extensions;

public sealed class DiagnosticExtensions
{
    public class PropertyChangedRoutedEventArgs : RoutedEventArgs
    {
        public PropertyChangedRoutedEventArgs(RoutedEvent routedEvent, DependencyPropertyChangedEventArgs args)
            : base(routedEvent)
        {
            Arguments = args;
        }

        public DependencyPropertyChangedEventArgs? Arguments { get; private set; }
    }

    public delegate void PropertyChangedRoutedEventHandler(object sender, PropertyChangedRoutedEventArgs e);

    /// <summary>
    /// Identifies a <see cref="DependencyProperty"/> that can be bound-to from XAML or code-behind.
    /// This can be useful for diagnosing bound property changes.
    /// </summary>
    public static readonly DependencyProperty AnyObject1Property = DependencyProperty.RegisterAttached(
        nameof(GetAnyObject1).Substring(3),
        typeof(object),
        typeof(DiagnosticExtensions),
        new PropertyMetadata(AnyObject1PropertyChanged));

    public static readonly RoutedEvent AnyObject1ChangedEvent = EventManager.RegisterRoutedEvent(
        "AnyObject1Changed",
        RoutingStrategy.Direct,
        typeof(PropertyChangedRoutedEventHandler),
        typeof(DiagnosticExtensions));

    private static void AnyObject1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            var args = new PropertyChangedRoutedEventArgs(AnyObject1ChangedEvent, e);
            element.RaiseEvent(args);
        }
    }

    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static object? GetAnyObject1(DependencyObject d) => d.GetValue(AnyObject1Property);

    public static void SetAnyObject1(DependencyObject d, object? value) => d.SetValue(AnyObject1Property, value);

    public static void AddAnyObject1ChangedHandler(DependencyObject dependencyObject, PropertyChangedRoutedEventHandler handler)
    {
        if (dependencyObject is UIElement element)
        {
            element.AddHandler(AnyObject1ChangedEvent, handler!);
        }
    }

    public static void RemoveAnyObject1ChangedHandler(DependencyObject dependencyObject, PropertyChangedRoutedEventHandler handler)
    {
        if (dependencyObject is UIElement element)
        {
            element.RemoveHandler(AnyObject1ChangedEvent, handler!);
        }
    }
}
