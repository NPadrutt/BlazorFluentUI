﻿using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class IconButton : ButtonBase
    {

        private ICollection<Rule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss();

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon" },
                Properties = new CssString()
                {
                    Css = $"padding:0 4px;" +
                          $"min-width:32px;" +
                          $"height:32px;" +
                          $"background-color:transparent;"+
                          $"border:none;"+
                          $"color:{Theme.SemanticTextColors.Link}"
                }
            });
                       
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDarkAlt};" +
                          $"background-color:{Theme.Palette.NeutralLighter};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon:active" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDark};" +
                         $"background-color:{Theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon.is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDark};" +
                         $"background-color:{Theme.Palette.NeutralLight};" +
                         $"border:none;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon.is-checked:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralQuaternaryAlt};" +
                          $"color:{Theme.Palette.ThemeDark};" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--icon.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"boder-color:{Theme.Palette.NeutralQuaternaryAlt};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Button.ms-Button--icon:hover{color: Highlight; background-color:Highlight;}"
                }
            });


            return rules;
        }
        protected override void OnParametersSet()
        {
            this.Text = null;
            base.OnParametersSet();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<GlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<System.Collections.Generic.ICollection<BlazorFabric.Rule>>(CreateGlobalCss));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            base.BuildRenderTree(builder);
            StartRoot(builder, "ms-Button--icon");
        }

    }
}
