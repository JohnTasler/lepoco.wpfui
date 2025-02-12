// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Markup;

/// <summary>
/// Provides a dictionary implementation that contains <c>WPF UI</c> theme resources used by components and other elements of a WPF application.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;Application
///     xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"&gt;
///     &lt;Application.Resources&gt;
///         &lt;ResourceDictionary&gt;
///             &lt;ResourceDictionary.MergedDictionaries&gt;
///                 &lt;ui:ThemesDictionary Theme = "Dark" /&gt;
///             &lt;/ResourceDictionary.MergedDictionaries&gt;
///         &lt;/ResourceDictionary&gt;
///     &lt;/Application.Resources&gt;
/// &lt;/Application&gt;
/// </code>
/// </example>
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ThemesDictionary : ResourceDictionary
{
    private ApplicationTheme _theme;

    /// <summary>
    /// Gets or sets the default application theme.
    /// </summary>
    public ApplicationTheme Theme
    {
        get => _theme;
        set => SetSourceBasedOnSelectedTheme(value);
    }

    public ThemesDictionary()
    {
        ApplicationTheme theme = TranslateSystemTheme();

        SetSourceBasedOnSelectedTheme(theme);
    }

    private static ApplicationTheme TranslateSystemTheme()
    {
        return ApplicationThemeManager.GetSystemTheme() switch
        {
            SystemTheme.Dark => ApplicationTheme.Dark,
            SystemTheme.HC1 => ApplicationTheme.HighContrast,
            SystemTheme.HC2 => ApplicationTheme.HighContrast,
            SystemTheme.HCBlack => ApplicationTheme.HighContrast,
            SystemTheme.HCWhite => ApplicationTheme.HighContrast,
            _ => ApplicationTheme.Light,
        };
    }

    public void SetTheme(ApplicationTheme theme)
    {
        SetSourceBasedOnSelectedTheme(theme);
    }

    private void SetSourceBasedOnSelectedTheme(ApplicationTheme selectedApplicationTheme)
    {
        if (selectedApplicationTheme == _theme)
        {
            return;
        }

        if (selectedApplicationTheme == ApplicationTheme.Unknown)
        {
            selectedApplicationTheme = TranslateSystemTheme();
        }

        _theme = selectedApplicationTheme;

        var themeName = selectedApplicationTheme switch
        {
            ApplicationTheme.Dark => "Dark",
            ApplicationTheme.HighContrast => "HighContrast",
            _ => "Light",
        };

        var sourceUri = $"{ApplicationThemeManager.ThemesDictionaryPath}{themeName}.xaml";
        Source = new Uri(sourceUri, UriKind.Absolute);
    }
}
