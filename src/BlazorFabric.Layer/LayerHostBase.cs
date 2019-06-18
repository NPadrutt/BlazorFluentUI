﻿using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Layer
{
    public class LayerHostBase : FabricComponentBase
    {
        internal LayerHostBase() { }
               
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected RenderFragment HostedContent { get; set; }

        [Parameter] protected bool IsFixed { get; set; } = true;

        [Parameter] public Func<UIEventArgs, Task> OnScroll { get; set; }  // CAN'T USE THESE... NEED TO USE FUNC INSTEAD
        [Parameter] public EventCallback<UIEventArgs> OnResize { get; set; } 
        [Parameter] public EventCallback<UIFocusEventArgs> OnFocusIn { get; set; }
        [Parameter] public EventCallback<UIMouseEventArgs> OnClick { get; set; }

        //public bool IsSet { get; set; } = false;

        
        //protected override void BuildRenderTree(RenderTreeBuilder builder)
        //{
        //    base.BuildRenderTree(builder);

        //    builder.OpenComponent<CascadingValue<LayerHost>>(0);


        //    builder.CloseComponent();

        //}

        protected LayerPortal layerPortal;

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment)
        {
            //until we can get references from a loop, looks like we can only use one portal at a time.
            //maybe with preview 6
            layerPortal.SetChildContent(layerId, renderFragment, IsFixed);
        }

        public void RemoveHostedContent(string layerId)
        {
            layerPortal.RemoveChildContent(layerId);
        }

        protected Task ScrollHandler(UIEventArgs args)
        {
            if (OnScroll != null)
            {
                return OnScroll.Invoke(args);
            }
            return Task.CompletedTask;
            //return OnScroll.InvokeAsync(args);
        }

        //protected Task ResizeHandler(UIEventArgs args)
        //{
        //    return OnResize.InvokeAsync(args);
        //}

        //protected Task FocusInHandler(UIFocusEventArgs args)
        //{
        //    return OnFocusIn.InvokeAsync(args);
        //}

        //protected Task ClickHandler(UIMouseEventArgs args)
        //{
        //    return OnClick.InvokeAsync(args);
        //}

    }
}