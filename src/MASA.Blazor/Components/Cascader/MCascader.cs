﻿using BlazorComponent;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MASA.Blazor
{
    public partial class MCascader : MSelect<BCascaderNode>
    {
        [Parameter]
        public bool IsFull { get; set; }

        protected double Left { get; set; }

        protected double Top { get; set; }

        private BCascaderNode GetNodeByValue(IEnumerable<BCascaderNode> items, string value)
        {
            BCascaderNode result = null;

            foreach (var item in items)
            {
                if (item.Value == value)
                {
                    result = item;
                    break;
                }
                else if (item.Children != null)
                {
                    result = GetNodeByValue(item.Children, value);
                }
            }

            return result;
        }

        protected override List<string> FormatText(string value)
        {
            var list = new List<string>();
            var result = GetNodeByValue(Items, value);

            if (result != null)
            {
                var text = IsFull ? string.Join('/', result.GetAllNodes().Select(t => t.Label))
                : result.Label;

                list.Add(text);
            }

            return list;
        }

        protected override void SetComponentClass()
        {
            SlotProvider
                .Apply<BPopover, MCascaderPopover>(props =>
                {
                    props[nameof(MPopover.Class)] = "m-menu__content menuable__content__active";
                    props[nameof(MPopover.Visible)] = (_visible && Items != null);
                    props[nameof(MPopover.ClientX)] = (StringNumber)Left;
                    props[nameof(MPopover.ClientY)] = (StringNumber)Top;
                })
                .Apply<BSelectSlot, MCascaderSelectSlot>(props =>
                {
                    props[nameof(MCascaderSelectSlot.Items)] = Items;
                    //props[nameof(MCascaderSelectSlot.SelectNode)] = SelectNode;
                    props[nameof(MCascaderSelectSlot.Visible)] = _visible;
                    props[nameof(MCascaderSelectSlot.Left)] = Left;
                    props[nameof(MCascaderSelectSlot.Top)] = Top;
                    props[nameof(MCascaderSelectSlot.OnOptionSelect)] = EventCallback.Factory.Create<MCascaderSelectOption>(this, async option =>
                     {
                         await SetSelectedAsync(option.Value);
                     });
                });

            base.SetComponentClass();

            Slot = true;
            Outlined = true;
            ItemText = r => IsFull ? string.Join('/', r.GetAllNodes().Select(t => t.Label)) : r.Label;
            ItemValue = r => r.Value;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var rect = await JsInvokeAsync<BoundingClientRect>(JsInteropConstants.GetBoundingClientRect, Ref);
                Left = rect.Left;
                Top = rect.Top + rect.Height;

                if (!string.IsNullOrEmpty(Value))
                {
                    var nodes = new List<BCascaderNode>();
                    var node = GetNode(Value, Items);
                    if (node != null)
                    {
                        await SetSelectedAsync(node);
                    }
                }

                StateHasChanged();
            }
        }

        private BCascaderNode GetNode(string value, IReadOnlyList<BCascaderNode> items)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }

            foreach (var item in items)
            {
                if (item.Value == value)
                {
                    return item;
                }
                else
                {
                    return GetNode(value, item.Children);
                }
            }

            return null;
        }
    }
}
