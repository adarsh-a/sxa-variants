using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Collections.Generic;
using System.Linq;

namespace Axiom.SxaCustom.Variants.sitecore.Pipeline
{
    public class GetBrandVariants : GetVariantsBase
    {
        private readonly IPresentationContext _presentationContext;
        public GetBrandVariants(IPresentationContext presentationContext)
        {
            _presentationContext = presentationContext;
        }
        public void Process(GetVariantsArgs args)
        {
            if (_presentationContext == null)
                return;

            var currentItem = args.ContextItem;
           
            List<Item> variants = new List<Item>();
            
            //get tenant brand
            var tenant = currentItem.GetParentOfTemplate(Sitecore.XA.Foundation.Multisite.Templates.Tenant.ID);            

            //get presentation container
            Item presentationItem = tenant.FirstChildInheritingFrom(Sitecore.XA.Foundation.Presentation.Templates.Presentation.ID);

            Item obj = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : null;
            if (obj == null)
                return;

            //get variants
            foreach (Item child in obj.Children)
                CheckLinkedVariants(args.RenderingId, child, variants);
            args.Variants = args.Variants.Concat(variants).ToList();           
        }
    }
}