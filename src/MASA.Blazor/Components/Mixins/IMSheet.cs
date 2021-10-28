﻿using BlazorComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.Blazor
{
    public interface IMSheet: IMRoundable, IMThemeable, IMElevatable, IMeasurable//, IColorable
    {
        bool Outlined { get; }

        bool Shaped { get; }

        string Tag { get; }
    }
}
