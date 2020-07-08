using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Collections.Generic;
using System.Linq;

namespace Axiom.SxaCustom.Variants.Pipeline
{
    public class GetSharedVariants : GetVariantsBase
    {
        private readonly ISharedSitesContext _sharedSiteContext;
        private readonly IPresentationContext _presentationContext;

        public GetSharedVariants(
          ISharedSitesContext sharedSiteContext,
          IPresentationContext presentationContext)
        {
            _sharedSiteContext = sharedSiteContext;
            _presentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            if (_sharedSiteContext == null || _presentationContext == null)
                return;
            List<Item> variants = new List<Item>();
            foreach (Item obj1 in _sharedSiteContext.GetSharedSitesWithoutCurrent(args.ContextItem))
            {
                Item presentationItem = _presentationContext.GetPresentationItem(obj1);
                Item obj2 = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : null;
                if (obj2 != null)
                {
                    foreach (Item child in obj2.Children)
                    {
                        if (child.Name.Equals(args.RenderingName))
                            variants.AddRange(child.Children);
                        CheckLinkedVariants(args.RenderingId, child, variants);
                    }
                    args.Variants = args.Variants.Concat(variants).ToList();
                }
            }
        }
    }
}