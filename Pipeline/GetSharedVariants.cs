using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sc_milka.sitecore.Pipeline
{
    public class GetSharedVariants : GetVariantsBase
    {
        private readonly ISharedSitesContext _sharedSiteContext;
        private readonly IPresentationContext _presentationContext;

        public GetSharedVariants(
          ISharedSitesContext sharedSiteContext,
          IPresentationContext presentationContext)
        {
            this._sharedSiteContext = sharedSiteContext;
            this._presentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            if (this._sharedSiteContext == null || this._presentationContext == null)
                return;
            List<Item> variants = new List<Item>();
            foreach (Item obj1 in this._sharedSiteContext.GetSharedSitesWithoutCurrent(args.ContextItem))
            {
                Item presentationItem = this._presentationContext.GetPresentationItem(obj1);
                Item obj2 = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : (Item)null;
                if (obj2 != null)
                {
                    foreach (Item child in obj2.Children)
                    {
                        if (child.Name.Equals(args.RenderingName))
                            variants.AddRange((IEnumerable<Item>)child.Children);
                        this.CheckLinkedVariants(args.RenderingId, child, variants);
                    }
                    args.Variants = (IList<Item>)args.Variants.Concat<Item>((IEnumerable<Item>)variants).ToList<Item>();
                }
            }
        }
    }
}