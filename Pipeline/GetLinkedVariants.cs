using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Collections.Generic;
using System.Linq;

namespace sc_milka.sitecore.Pipeline
{
    public class GetLinkedVariants : GetVariantsBase
    {
        public IPresentationContext PresentationContext { get; }

        public GetLinkedVariants(IPresentationContext presentationContext)
        {
            this.PresentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            Item presentationItem = this.PresentationContext.GetPresentationItem(args.ContextItem);
            List<Item> variants = new List<Item>();
            Item obj = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : (Item)null;
            if (obj == null)
                return;
            foreach (Item child in obj.Children)
                this.CheckLinkedVariants(args.RenderingId, child, variants);
            args.Variants = (IList<Item>)args.Variants.Concat<Item>((IEnumerable<Item>)variants).ToList<Item>();
        }
    }
}