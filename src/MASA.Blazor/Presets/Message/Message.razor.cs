﻿using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BlazorComponent;

namespace MASA.Blazor.Presets
{
    public partial class Message
    {
        private string Icon => Type switch
        {
            AlertTypes.Success => "mdi-check-circle",
            AlertTypes.Info => "mdi-information",
            AlertTypes.Warning => "mdi-exclamation",
            AlertTypes.Error => "mdi-alert",
            _ => null,
        };

        private string IconColor => Type switch
        {
            AlertTypes.Success => "success",
            AlertTypes.Info => "info",
            AlertTypes.Warning => "warning",
            AlertTypes.Error => "error",
            _ => null
        };

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        [Parameter]
        public AlertTypes Type { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public int Timeout { get; set; } = 3000;

        private async Task HandleOnClick()
        {
            if (VisibleChanged.HasDelegate)
            {
                await VisibleChanged.InvokeAsync(false);
            }
        }
    }
}
