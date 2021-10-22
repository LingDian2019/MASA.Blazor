﻿using BlazorComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MASA.Blazor
{
    public partial class MWindow : BWindow
    {
        [Parameter]
        public bool Dark { get; set; }

        [Parameter]
        public bool Light { get; set; }

        [Inject]
        public GlobalConfig GlobalConfig { get; set; }

        public bool InternalReverse => GlobalConfig.RTL ? !Reverse : Reverse;

        public string ComputedTransition
        {
            get
            {
                //TODO:isBooted

                var axis = Vertical ? "y" : "x";
                var reverse = InternalReverse;//TODO:isReverse
                var direction = reverse ? "-reverse" : "";

                return $"m-window-{axis}{direction}-transition";
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ActiveClass = "m-window-item--active";
            PrevIcon ??= "mdi-chevron-left";
            NextIcon ??= "mdi-chevron-right";
        }

        protected override void SetComponentClass()
        {
            CssProvider
                .Apply(cssBuilder =>
                {
                    cssBuilder
                        .Add("m-window")
                        .Add("m-item-group")
                        .AddIf("m-window--show-arrows-on-hover", () => ShowArrowsOnHover);
                })
                .Apply("container", cssBuilder =>
                {
                    cssBuilder
                        .Add("m-window__container")
                        .AddIf("m-window__container--is-active", () => IsActive);
                }, styleBuilder =>
                {
                    styleBuilder
                        .AddHeight(TransitionHeight);
                })
                .Apply("prev", cssBuilder =>
                {
                    cssBuilder
                        .Add("m-window__prev");
                })
                .Apply("next", cssBuilder =>
                {
                    cssBuilder
                        .Add("m-window__next");
                });

            AbstractProvider
                .Apply<BButton, MButton>()
                .Apply<BIcon, MIcon>();
        }
    }
}