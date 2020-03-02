﻿using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFabric
{
    public class CommandButton : ButtonBase
    {
        private ICollection<Rule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss();

            return rules;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenComponent<GlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<System.Collections.Generic.ICollection<BlazorFabric.Rule>>(CreateGlobalCss));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            StartRoot(builder, "ms-Button--action ms-Button--command");

        }
    }
}
