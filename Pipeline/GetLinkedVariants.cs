using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Collections.Generic;
using System.Linq;

namespace Axiom.SxaCustom.Variants.Pipeline
{
    public class GetLinkedVariants : GetVariantsBase
    {
        public IPresentationContext PresentationContext { get; }

        public GetLinkedVariants(IPresentationContext presentationContext)
        {
            PresentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            Item presentationItem = PresentationContext.GetPresentationItem(args.ContextItem);
            List<Item> variants = new List<Item>();
            Item obj = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : null;
            if (obj == null)
                return;
            foreach (Item child in obj.Children)
                this.CheckLinkedVariants(args.RenderingId, child, variants);
            args.Variants = args.Variants.Concat(variants).ToList();
        }
    }
}