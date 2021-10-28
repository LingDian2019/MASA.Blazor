﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlazorComponent.Components.Core.CssProcess;
using MASA.Blazor;
using OneOf;

namespace BlazorComponent
{
    public static class CssBuilderExtensions
    {
        public static CssBuilder AddTheme(this CssBuilder cssBuilder, bool isDark, bool exDark = false)
        {
            var dark = isDark || exDark;

            cssBuilder.Add(() =>
            {
                var suffix = dark ? "dark" : "light";
                return $"theme--{suffix}";
            });

            return cssBuilder;
        }

        public static CssBuilder AddElevation(this CssBuilder cssBuilder, StringNumber elevation)
        {
            cssBuilder
                .AddIf($"elevation-{elevation}", () => elevation != null);

            return cssBuilder;
        }

        public static CssBuilder AddBackgroundColor(this CssBuilder cssBuilder, string color)
        {
            if (!string.IsNullOrEmpty(color) && !IsCssColor(color))
            {
                cssBuilder
                    .Add(color);
            }

            return cssBuilder;
        }

        private static bool IsCssColor(string color)
        {
            return Regex.Match(color, @"^(#|var\(--|(rgb|hsl)a?\()").Success;
        }

        public static CssBuilder AddBackgroundColor(this CssBuilder cssBuilder, string color, Func<bool> func)
        {
            return cssBuilder.AddColor(color, false, func);
        }

        public static CssBuilder AddColor(this CssBuilder cssBuilder, string color, bool isText = false)
        {
            return cssBuilder.AddColor(color, isText, () => true);
        }

        public static CssBuilder AddColor(this CssBuilder cssBuilder, string color, bool isText, Func<bool> func)
        {
            if (string.IsNullOrEmpty(color) || color.StartsWith("#") || color.StartsWith("rgb"))
            {
                return cssBuilder;
            }

            if (isText)
            {
                var colors = color.Split(" ");
                var firstColor = colors[0];

                cssBuilder
                    .AddIf($"{firstColor}--text", func);

                if (colors.Length == 2)
                {
                    // TODO: 是否需要正则表达式验证格式，Vuetify没有
                    // {darken|lighten|accent}-{1|2}

                    var secondColor = colors[1];
                    cssBuilder
                        .AddIf($"text--{secondColor}", func);
                }
            }
            else
            {
                cssBuilder
                    .AddIf(color, func);
            }

            return cssBuilder;
        }

        public static CssBuilder AddTextColor(this CssBuilder cssBuilder, string color, Func<bool> func)
        {
            return cssBuilder.AddColor(color, true, func);
        }

        public static CssBuilder AddTextColor(this CssBuilder cssBuilder, string color)
        {
            if (!string.IsNullOrEmpty(color) && !IsCssColor(color))
            {
                var colors = color.Trim().Split(' ');
                cssBuilder
                    .Add($"{colors[0]}--text");

                if (colors.Length == 2)
                {
                    cssBuilder
                        .Add($"text--{colors[1]}");
                }
            }

            return cssBuilder;
        }

        public static CssBuilder AddRounded(this CssBuilder cssBuilder, StringBoolean rounded)
        {
            if (rounded != null)
            {
                if (rounded.IsT0)
                {
                    var values = rounded.AsT0.Split(' ');

                    foreach (var val in values)
                    {
                        cssBuilder.Add($"rounded-{val}");
                    }
                }
                else if (rounded.IsT1 && rounded.AsT1)
                {
                    cssBuilder.Add("rounded");
                }
            }

            return cssBuilder;
        }

        public static CssBuilder AddRounded(this CssBuilder cssBuilder, StringBoolean rounded, bool tile)
        {
            if (tile)
            {
                cssBuilder.Add("rounded-0");
            }
            else
            {
                if (rounded != null)
                {
                    if (rounded.IsT0)
                    {
                        var values = rounded.AsT0.Split(' ');

                        foreach (var val in values)
                        {
                            cssBuilder.Add($"rounded-{val}");
                        }
                    }
                    else if (rounded.IsT1 && rounded.AsT1)
                    {
                        cssBuilder.Add("rounded");
                    }
                }
            }

            return cssBuilder;
        }

        public static CssBuilder AddRoutable(this CssBuilder cssBuilder, IMRoutable routable)
        {
            return cssBuilder.AddIf(routable.ActiveClass, () => routable.To is not null && routable.ActiveClass is not null);
        }

        public static CssBuilder AddTheme(this CssBuilder cssBuilder, IMThemeable themeable, bool isCommonDark,Func<bool> ifTrue=null)
        {
            if (ifTrue is null) ifTrue = () => true;

            return cssBuilder.AddIf(() => 
            {
                if (themeable.Dark || isCommonDark) return "theme--dark";
                else return "theme--light";
            }, ifTrue);
        }

        public static CssBuilder AddElevatable(this CssBuilder cssBuilder, IMElevatable elevatable,Func<bool> ifTrue = null)
        {
            if (ifTrue is null) ifTrue = () => true;

            return cssBuilder.AddIf(ElevationClasses, ifTrue);

            string ElevationClasses()
            {
                if (elevatable.Elevation is null) return "";
                if (int.TryParse(elevatable.Elevation.ToString(), out var number))
                {
                    return $"elevation-{number}";
                }
                else return "";
            }
        }

        public static CssBuilder AddRoundable(this CssBuilder cssBuilder, IMRoundable roundable, Func<bool> ifTrue = null)
        {
            if (ifTrue is null) ifTrue = () => true;

            return cssBuilder.AddIf(RoundedClasses, ifTrue);

            string RoundedClasses()
            {
                var composite = new List<string>();
                if (roundable.Tile is true)
                {
                    composite.Add("rounded-0");
                }
                else if (roundable.Rounded == true)
                {
                    composite.Add("rounded");
                }
                else if (roundable.Rounded == false)
                {
                }
                else if (roundable.Rounded is not null)
                {
                    var values = roundable.Rounded.ToString().Split(" ");
                    foreach (var value in values)
                    {
                        composite.Add($"rounded-{value}");
                    }
                }
                return String.Join(" ", composite);
            }
        }
    }
}