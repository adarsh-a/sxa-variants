using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Linq;

namespace Axiom.SxaCustom.Variants.Pipeline
{
    public class GetSiteVariants
    {
        public IPresentationContext PresentationContext { get; }
        public GetSiteVariants(IPresentationContext presentationContext)
        {
            PresentationContext = presentationContext;
        }
        public void Process(GetVariantsArgs args)
        {
            Item presentationItem = PresentationContext.GetPresentationItem(args.ContextItem);
            if (presentationItem == null)
                return;
            Item obj1 = presentationItem.FirstChildInheritingFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping.ID);
            Item obj2 = obj1 != null ? obj1.Children.FirstOrDefault(item => item.Name.Equals(args.RenderingName)) : null;
            if (obj2 == null)
                return;
            args.Variants = args.Variants.Concat(obj2.Children).ToList();
        }
    }
}