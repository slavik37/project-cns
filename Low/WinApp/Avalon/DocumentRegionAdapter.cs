﻿using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Xceed.Wpf.AvalonDock.Layout;

namespace WinApp.Avalon
{
    class DocumentRegionAdapter : RegionAdapterBase<LayoutDocument>
    {
        public DocumentRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {

        }


        protected override void Adapt(IRegion region, LayoutDocument regionTarget)
        {
            if (regionTarget == null)
                throw new ArgumentNullException("regionTarget");

            if (regionTarget.Content != null)
            {
                throw new InvalidOperationException();
            }

            region.ActiveViews.CollectionChanged += delegate
            {
                regionTarget.Content = region.ActiveViews.FirstOrDefault();
            };

            region.Views.CollectionChanged +=
                (sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add && region.ActiveViews.Count() == 0)
                    {
                        region.Activate(e.NewItems[0]);
                    }
                };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

    }
}
