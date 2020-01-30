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
    public class GetBrandVariants : GetVariantsBase
    {
        private readonly IPresentationContext _presentationContext;
        public GetBrandVariants(IPresentationContext presentationContext)
        {
            this._presentationContext = presentationContext;
        }


        public void Process(GetVariantsArgs args)
        {
            if (this._presentationContext == null)
                return;

            var currentItem = args.ContextItem;
            if (!currentItem.Paths.FullPath.ToLower().Contains("milka"))
                return;
            //check if site is milka
            List<Item> variants = new List<Item>();
            
            //get milka brand
            var tenant = currentItem.GetParentOfTemplate(Sitecore.XA.Foundation.Multisite.Templates.Tenant.ID);
            

            //get presentation container
            Item presentationItem = tenant.FirstChildInheritingFrom(Sitecore.XA.Foundation.Presentation.Templates.Presentation.ID);

            Item obj = presentationItem != null ? presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID) : (Item)null;
            if (obj == null)
                return;

            //get variants

            foreach (Item child in obj.Children)
                this.CheckLinkedVariants(args.RenderingId, child, variants);
            args.Variants = (IList<Item>)args.Variants.Concat<Item>((IEnumerable<Item>)variants).ToList<Item>();


           
        }
    }
}