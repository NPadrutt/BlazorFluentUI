using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUDetailsRow<TItem> : BFUComponentBase, IAsyncDisposable
    {
        [CascadingParameter]
        public BFUSelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public bool AnySelected { get; set; }

        [Parameter]
        public IEnumerable<BFUDetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool EnableUpdateAnimations { get; set; }

        [Parameter]
        public Func<TItem, object> GetKey { get; set; }

        [Parameter]
        public int GroupNestingDepth { get; set; }

        [Parameter]
        public double IndentWidth { get; set; } = 36;

        [Parameter]
        public bool IsCheckVisible { get; set; }

        [Parameter]
        public bool IsRowHeader { get; set; }

        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public int ItemIndex { get; set; }

        [Parameter]
        public EventCallback<BFUDetailsRow<TItem>> OnRowDidMount { get; set; }

        [Parameter]
        public EventCallback<BFUDetailsRow<TItem>> OnRowWillUnmount { get; set; }

        [Parameter]
        public double RowWidth { get; set; } = 0;

        [Parameter]
        public Selection<TItem> Selection { get; set; }
        //public bool IsSelected { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private bool canSelect;
        private bool showCheckbox;
        private ColumnMeasureInfo<TItem> columnMeasureInfo = null;
        private ElementReference cellMeasurer;
        private bool isSelected;
        private bool isSelectionModal;
        private Rule _localCheckCoverRule;

        Selection<TItem> _selection;
        IDisposable _selectionSubscription;

        private ICollection<IRule> DetailsRowLocalRules { get; set; } = new List<IRule>();

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            return base.OnInitializedAsync();
        }

        private void CreateLocalCss()
        {
            _localCheckCoverRule = new Rule();
            _localCheckCoverRule.Selector = new ClassSelector() { SelectorName = "ms-DetailsRow-checkCover" };
            _localCheckCoverRule.Properties = new CssString() { Css = $"position:absolute;top:-1px;left:0;bottom:0;right:0;display:{(AnySelected ? "block" : "none")};" };
            DetailsRowLocalRules.Add(_localCheckCoverRule);
        }

        protected override Task OnParametersSetAsync()
        {
            showCheckbox = SelectionMode != SelectionMode.None && CheckboxVisibility != CheckboxVisibility.Hidden;

            canSelect = SelectionMode != SelectionMode.None;//Selection != null;



            if (Selection != _selection)
            {
                if (_selectionSubscription != null)
                    _selectionSubscription.Dispose();

                _selection = Selection;

                if (Selection != null)
                {

                    _selectionSubscription = Selection.SelectionChanged.Subscribe(_ =>
                    {
                        bool changed = false;
                        bool newIsSelected;
                        if (GetKey != null)
                        {
                            newIsSelected = Selection.IsKeySelected(GetKey(Item), false);
                        }
                        else
                        {
                            newIsSelected = Selection.IsIndexSelected(this.ItemIndex);
                        }
                        if (newIsSelected != isSelected)
                        {
                            changed = true;
                            isSelected = newIsSelected;
                        }
                        var newIsModal = Selection.IsModal();
                        if (newIsModal != isSelectionModal)
                        {
                            changed = true;
                            isSelectionModal = newIsModal;
                        }
                        if (changed)
                            InvokeAsync(StateHasChanged);
                    });
                }
            }
            if (Selection != null)
            {
                if (GetKey != null)
                {
                    isSelected = Selection.IsKeySelected(GetKey(Item), false);
                }
                else
                {
                    isSelected = Selection.IsIndexSelected(this.ItemIndex);
                }
            }

            //CreateCss();
            return base.OnParametersSetAsync();
        }

        public static int RowVerticalPadding = 11;
        public static int CompactRowVerticalPadding = 6;
        public static int RowHeight = 42;
        public static int CompactRowHeight = 32;
        public static int CellLeftPadding = 12;
        public static int CellRightPadding = 8;
        public static int CellExtraRightPadding = 24;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await OnRowDidMount.InvokeAsync(this);
            }

            if (columnMeasureInfo != null && columnMeasureInfo.Index >= 0 && cellMeasurer.Id != null)
            {
                var method = columnMeasureInfo.OnMeasureDone;
                var size = await JSRuntime.InvokeAsync<Rectangle>("BlazorFluentUiBaseComponent.measureElementRect", cellMeasurer);
                method(size.width);
                columnMeasureInfo = null;
                InvokeAsync(StateHasChanged);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public void MeasureCell(int index, Action<double> onMeasureDone)
        {
            var column = Columns.ElementAt(index);
            column.MinWidth = 0;
            column.MaxWidth = 999999;
            column.CalculatedWidth = double.NaN;

            columnMeasureInfo = new ColumnMeasureInfo<TItem> { Index = index, Column = column, OnMeasureDone = onMeasureDone };
            InvokeAsync(StateHasChanged);
        }

        public async ValueTask DisposeAsync()
        {
            await OnRowWillUnmount.InvokeAsync(this);
            _selectionSubscription?.Dispose();
        }
    }
}
