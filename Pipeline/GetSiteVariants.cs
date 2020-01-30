using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sc_milka.sitecore.Pipeline
{
    public class GetSiteVariants
    {
        public IPresentationContext PresentationContext { get; }

        public GetSiteVariants(IPresentationContext presentationContext)
        {
            this.PresentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            Item presentationItem = this.PresentationContext.GetPresentationItem(args.ContextItem);
            if (presentationItem == null)
                return;
            Item obj1 = presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID);
            Item obj2 = obj1 != null ? obj1.Children.FirstOrDefault<Item>((Func<Item, bool>)(item => item.Name.Equals(args.RenderingName))) : (Item)null;
            if (obj2 == null)
                return;
            args.Variants = (IList<Item>)args.Variants.Concat<Item>((IEnumerable<Item>)obj2.Children).ToList<Item>();
        }
    }
}