//using Microsoft.AspNetCore.Components;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
//using System.Threading.Tasks;
//using Microsoft.JSInterop;
//using System.Reactive;
//using BlazorFluentUI.Style;
//using Microsoft.AspNetCore.Components.Rendering;
//using System.Reflection.Emit;
//using System.Reflection;
//using System.Threading;

//namespace BlazorFluentUI
//{
//    public partial class BFUListOld<TItem> : BFUComponentBase, IAsyncDisposable
//    {
//        //protected bool firstRender = false;

//        protected const int DEFAULT_ITEMS_PER_PAGE = 10;
//        protected const int DEFAULT_RENDERED_WINDOWS_BEHIND = 2;
//        protected const int DEFAULT_RENDERED_WINDOWS_AHEAD = 2;

//        private double thresholdChangePercent = 0.10;

//        //protected ElementReference rootDiv;
//        protected ElementReference surfaceDiv;
//        protected ElementReference contentDiv;
//        protected ElementReference spacerBefore;
//        protected ElementReference spacerAfter;

//        private bool hasMeasuredAverageHeightOnce = false;


//        long renderCount;
//        //private double _averagePageHeight = 100;
//        private bool isFirstRender = true;
//        private bool _shouldRender = false;

//        private int listId;
//        private int numItemsToSkipBefore;
//        private int numItemsToShow;
//        private double averageHeight = 43;
//        private int ItemsToSkipAfter => _itemsSource.Count() - numItemsToSkipBefore - numItemsToShow;


//        //private int minRenderedPage;
//        //private int maxRenderedPage;
//        private Rectangle _lastScrollRect = new Rectangle();
//        private ElementMeasurements _scrollRect = new ElementMeasurements();
//        //private double _scrollHeight;
//        private Rectangle surfaceRect = new Rectangle();
//        private double _height;
//        public double CurrentHeight => _height;
        
        
//        private bool _jsAvailable = false;
//        private bool _lastIsVirtualizing = true;

//        //private object _lastVersion = null;

//        [Inject] 
//        private IJSRuntime JSRuntime { get; set; }

//        [Parameter] 
//        public object Data { get; set; }

//        //[Parameter] 
//        //public Func<int, Rectangle, int> GetItemCountForPage { get; set; }

//        [Parameter]
//        public bool IsVirtualizing { get; set; } = true;

//        [Parameter] 
//        public bool ItemFocusable { get; set; } = false;

//        [Parameter] 
//        public IEnumerable<TItem> ItemsSource { get; set; }

//        [Parameter] 
//        public RenderFragment<IndexedItem<TItem>> ItemTemplate { get; set; }

//        [Parameter] 
//        public EventCallback<(double, object)> OnListScrollerHeightChanged { get; set; }

//        [Parameter]
//        public EventCallback<Viewport> OnViewportChanged { get; set; }

//        private IEnumerable<TItem> _itemsSource;

//        private List<TItem> selectedItems = new List<TItem>();
//        private string _resizeRegistration;

//        private Dictionary<int, double> _pageSizes = new Dictionary<int, double>();
//        private bool _needsRemeasure = true;

//        private Viewport _viewport = new Viewport();
//        private ElementMeasurements _surfaceRect = new ElementMeasurements();

//        Dictionary<string, double> heightCache = new Dictionary<string, double>();
       
//        protected RenderFragment<RenderFragment<TItem>> ItemContainer { get; set; }

//        protected override Task OnInitializedAsync()
//        {
            
//            var props = typeof(TItem).GetProperties();
//            var fields = typeof(TItem).GetFields().Where(x=>x.IsPublic);
            

//            return base.OnInitializedAsync();
//        }

//        protected override async Task OnParametersSetAsync()
//        {

//            if (_itemsSource != ItemsSource)
//            {
//                if (this._itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
//                {
//                    (this._itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
//                }

//                _itemsSource = ItemsSource;
//                //if (_itemsSource != null)
//                //    itemContainers = _itemsSource.Select((x, i) => new ItemContainer<TItem> { Item = x, Index = i }).ToList();
//                //else
//                //    itemContainers.Clear();

//                if (this.ItemsSource is System.Collections.Specialized.INotifyCollectionChanged)
//                {
//                    (this.ItemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged += ListBase_CollectionChanged;
//                }
                
//                _shouldRender = true;
//                _needsRemeasure = true;
//            }


            
//            //CreateCss();
//            await base.OnParametersSetAsync();
//        }


//        private void ListBase_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        {

//            _shouldRender = true;
//            InvokeAsync(StateHasChanged);
//        }

//        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
//        //{
//        //    var listRules = new HashSet<IRule>();
//        //    //creates a method that pulls in focusstyles the way the react controls do it.
//        //    var focusStyleProps = new FocusStyleProps(theme);
//        //    var mergeStyleResults = FocusStyle.GetFocusStyle(focusStyleProps, ".ms-List-cell-default");

//        //    listRules.Clear();
//        //    // Cell only
//        //    listRules.Add(new Rule()
//        //    {
//        //        Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default" },
//        //        Properties = new CssString()
//        //        {
//        //            Css = $"padding-top:11px;" +
//        //                  $"padding-bottom:11px;" +
//        //                  $"min-height:42px;" +
//        //                  $"min-width:100%;" +
//        //                  $"overflow:hidden;" +
//        //                  $"box-sizing:border-box;" +
//        //                  $"border-bottom:1px solid {theme.Palette.NeutralLighter};" +
//        //                  $"display:inline-flex;"
//        //                  +
//        //                  mergeStyleResults.MergeRules
//        //        }
//        //    });
//        //    listRules.Add(new Rule()
//        //    {
//        //        Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default:hover" },
//        //        Properties = new CssString()
//        //        {
//        //            Css = $"background-color:{theme.Palette.NeutralLighter};"
//        //        }
//        //    });
//        //    listRules.Add(new Rule()
//        //    {
//        //        Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default.is-selected" },
//        //        Properties = new CssString()
//        //        {
//        //            Css = $"background-color:{theme.Palette.NeutralLight};"
//        //        }
//        //    });

//        //    foreach (var rule in mergeStyleResults.AddRules)
//        //        listRules.Add(rule);

//        //    return listRules;
//        //}




//        protected override async Task OnAfterRenderAsync(bool firstRender)
//        {            
//            if (firstRender)
//            {
//                _lastIsVirtualizing = IsVirtualizing;
//                _jsAvailable = true;
//                if (IsVirtualizing)
//                {
//                    var objectRef = DotNetObjectReference.Create(this);
//                    var initResult = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.initialize", objectRef, RootElementReference, spacerBefore, spacerAfter);
//                    this.listId = (int)initResult.Left;
//                    await UpdateViewportAsync(initResult.Right, initResult.Width, initResult.Bottom, initResult.Height);
//                }
//                else
//                {
//                     var viewportMeasurement = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.getViewport", surfaceDiv);
//                    await UpdateViewportAsync(viewportMeasurement.Right, viewportMeasurement.Width, viewportMeasurement.Bottom, viewportMeasurement.Height);
//                }
//            }
//            else
//            {
//                if (_lastIsVirtualizing != IsVirtualizing)
//                {
//                    _lastIsVirtualizing = IsVirtualizing;  //need to make sure this area is run once, otherwise mulitple observers will be set for this viewport leading to blinking
//                    if (IsVirtualizing)
//                    {
//                        var objectRef = DotNetObjectReference.Create(this);
//                        var initResult = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.initialize", objectRef, RootElementReference, spacerBefore, spacerAfter, true);
//                        this.listId = (int)initResult.Left;
//                        await UpdateViewportAsync(initResult.Right, initResult.Width, initResult.Bottom, initResult.Height);
//                    }
//                    else
//                    {
//                        await JSRuntime.InvokeVoidAsync("BlazorFluentUiList.removeList", this.listId);
//                    }
//                }                
//            }
            

//            if (IsVirtualizing)//(!hasMeasuredAverageHeightOnce)
//            {

//                var averageHeight = await JSRuntime.InvokeAsync<double>("BlazorFluentUiList.getInitialAverageHeight", this.listId);
//                if (averageHeight != 0 && averageHeight != this.averageHeight)
//                {
//                    hasMeasuredAverageHeightOnce = true;
//                    this.averageHeight = averageHeight;
//                    StateHasChanged();
//                }

//            }

//            await base.OnAfterRenderAsync(firstRender);
//        }

//        [JSInvokable]
//        public async void OnSpacerVisible(string spacerType, DOMRect visibleRect, double containerHeight, double spacerBeforeHeight, double spacerAfterHeight)
//        {
//            // Reset to match values corresponding to this event
//            //numItemsToSkipBefore = (int)Math.Round(spacerBeforeHeight / averageHeight);
//            //numItemsToShow = _itemsSource.Count() - numItemsToSkipBefore - (int)Math.Round(spacerAfterHeight / averageHeight);

//            if (spacerType == "before" && numItemsToSkipBefore > 0)
//            {
//                var visibleTop = visibleRect.Top;
//                var firstVisibleItemIndex = (int)Math.Floor(visibleTop / averageHeight);
//                numItemsToShow = (int)Math.Ceiling(containerHeight / averageHeight) + 1;
//                numItemsToSkipBefore = Math.Max(0, firstVisibleItemIndex);
//                StateHasChanged();
//            }
//            else if (spacerType == "after" && ItemsToSkipAfter > 0)
//            {
//                var visibleBottom = visibleRect.Top + visibleRect.Height;
//                var lastVisibleItemIndex = (int)Math.Ceiling(visibleBottom / averageHeight);//numItemsToSkipBefore + numItemsToShow + (int)Math.Ceiling(visibleBottom / averageHeight);
//                numItemsToShow = (int)Math.Ceiling(containerHeight / averageHeight) + 1;
//                numItemsToSkipBefore = Math.Max(0, lastVisibleItemIndex - numItemsToShow);
//                StateHasChanged();
//            }

//            await UpdateViewportAsync(visibleRect.Right, visibleRect.Width, visibleRect.Bottom, visibleRect.Height);
//        }

//        [JSInvokable]
//        public void UpdateHeightCache(Dictionary<string,double> cacheUpdate)
//        {
//            foreach (var pair in cacheUpdate)
//            {
//                if (heightCache.ContainsKey(pair.Key))
//                    heightCache[pair.Key] = pair.Value;
//                else
//                    heightCache.Add(pair.Key, pair.Value);
//            }

//        }

//        [JSInvokable]
//        public void AverageHeightChanged(double averageHeight)
//        {
//            this.averageHeight = averageHeight;
//        }



//        //[JSInvokable]
//        //public async void ResizeHandler(double width, double height)
//        //{
//        //    //await MeasureContainerAsync();

//        //    _viewport.Height = _surfaceRect.cheight;
//        //    _viewport.Width = _surfaceRect.cwidth;
//        //    _viewport.ScrollHeight = _scrollRect.height;
//        //    _viewport.ScrollWidth = _scrollRect.width;
//        //    await OnViewportChanged.InvokeAsync(_viewport);
//        //}

//        //[JSInvokable]
//        //public async void OnScroll(ScrollEventArgs args)
//        //{            
//        //    averageHeight = args.AverageHeight;
//        //    // TODO: Support horizontal scrolling too
//        //    var relativeTop = args.ContainerRect.Top - args.ContentRect.Top;
//        //    numItemsToSkipBefore = Math.Max(0, (int)(relativeTop / averageHeight));

//        //    var visibleHeight = args.ContainerRect.Bottom - (args.ContentRect.Top + numItemsToSkipBefore * averageHeight);
//        //    numItemsToShow = (int)Math.Ceiling(visibleHeight / averageHeight) * 3;

//        //    await UpdateViewportAsync(args.ScrollRect.width, args.ContainerRect.Width, args.ScrollRect.height, args.ContainerRect.Height);

//        //    _lastScrollRect = args.ScrollRect;

//        //    StateHasChanged();
//        //}

//        private async Task UpdateViewportAsync(double scrollWidth, double width, double scrollHeight, double height)
//        {
//            bool hasChanged = false;
//            if (_viewport.ScrollWidth != scrollWidth)
//            {
//                hasChanged = true;
//                _viewport.ScrollWidth = scrollWidth;
//            }
//            if (_viewport.Width != width)
//            {
//                hasChanged = true;
//                _viewport.Width = width;
//            }
//            if (_viewport.ScrollHeight != scrollHeight)
//            {
//                hasChanged = true;
//                _viewport.ScrollHeight = scrollHeight;
//            }
//            if (_viewport.Height != height)
//            {
//                hasChanged = true;
//                _viewport.Height = height;
//            }

//            if (hasChanged)
//                await OnViewportChanged.InvokeAsync(_viewport);

//        }
                  

//        public async ValueTask DisposeAsync()
//        {
//            if (_itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
//            {
//                (_itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
//            }
//            if (_resizeRegistration != null)
//            {
//                await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.deregisterResizeEvent", _resizeRegistration);
//            }
//            Debug.WriteLine("List was disposed");
//        }

//        public class ScrollEventArgs
//        {
//            public DOMRect ContainerRect { get; set; }
//            public Rectangle ScrollRect { get; set; }
//            public DOMRect ContentRect { get; set; }

//            public double AverageHeight { get; set; }
//        }

//        public class DOMRect
//        {
//            public double Top { get; set; }
//            public double Bottom { get; set; }
//            public double Left { get; set; }
//            public double Right { get; set; }
//            public double Width { get; set; }
//            public double Height { get; set; }
//        }
//    }
//}
