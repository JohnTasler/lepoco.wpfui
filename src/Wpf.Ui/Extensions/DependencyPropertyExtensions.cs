// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Extensions;

public static class DependencyPropertyExtensions
{
    public static PropertyChangedCallback AddChangedHandler<TOwner, TMetadata>(this DependencyProperty @this, Action<TOwner> typedAction)
        where TOwner : DependencyObject
        where TMetadata : PropertyMetadata, new()
    {
        PropertyChangedCallback callback = CreateChangedHandler(typedAction);
        @this.OverrideMetadata(typeof(TOwner), new TMetadata { PropertyChangedCallback = callback });
        return callback;
    }

    public static PropertyChangedCallback AddChangedHandler<TOwner, TMetadata, TValue>(this DependencyProperty @this, Action<TOwner, TValue> typedAction)
        where TOwner : DependencyObject
        where TMetadata : PropertyMetadata, new()
    {
        PropertyChangedCallback callback = CreateChangedHandler(typedAction);
        @this.OverrideMetadata(typeof(TOwner), new TMetadata { PropertyChangedCallback = callback });
        return callback;
    }

    public static PropertyChangedCallback AddChangedHandler<TOwner, TMetadata, TValue>(this DependencyProperty @this, Action<TOwner, TValue, TValue> typedAction)
        where TOwner : DependencyObject
        where TMetadata : PropertyMetadata, new()
    {
        PropertyChangedCallback callback = CreateChangedHandler(typedAction);
        @this.OverrideMetadata(typeof(TOwner), new TMetadata { PropertyChangedCallback = callback });
        return callback;
    }

    public static PropertyChangedCallback CreateChangedHandler<TOwner>(
        Action<TOwner> typedAction)
            where TOwner : DependencyObject
    {
        return (d, e) =>
        {
            if (d is TOwner attachedTo)
            {
                typedAction(attachedTo);
            }
        };
    }

    public static PropertyChangedCallback CreateChangedHandler<TOwner, TValue>(
        Action<TOwner, TValue> typedAction)
            where TOwner : DependencyObject
    {
        return (d, e) =>
        {
            if (d is TOwner attachedTo)
            {
                typedAction(attachedTo, (TValue)e.NewValue);
            }
        };
    }

    public static PropertyChangedCallback CreateChangedHandler<TOwner, TValue>(
        Action<TOwner, TValue, TValue> typedAction)
            where TOwner : DependencyObject
    {
        return (d, e) =>
        {
            if (d is TOwner attachedTo)
            {
                typedAction(attachedTo, (TValue)e.NewValue, (TValue)e.OldValue);
            }
        };
    }

    public static CoerceValueCallback AddCoerceHandler<TOwner, TMetadata, TValue>(this DependencyProperty @this, Func<TOwner, TValue, TValue> typedFunc)
        where TOwner : DependencyObject
        where TMetadata : PropertyMetadata, new()
    {
        CoerceValueCallback callback = CreateCoerceHandler(typedFunc);

        @this.OverrideMetadata(typeof(TOwner), new TMetadata { CoerceValueCallback = callback });
        return callback;
    }

    public static CoerceValueCallback CreateCoerceHandler<TOwner, TValue>(Func<TOwner, TValue, TValue> typedFunc)
        where TOwner : DependencyObject
    {
        return (d, v) =>
        {
            return (d is TOwner owner) ? typedFunc(owner, (TValue)v) : v;
        };
    }
}
