using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System.Linq;

namespace Axiom.SxaCustom.Variants.Pipeline
{
    public class GetSystemVariants
    {
        public IContentRepository ContentRepository { get; }
        public GetSystemVariants(IContentRepository contentRepository)
        {
            this.ContentRepository = contentRepository;
        }
        public void Process(GetVariantsArgs args)
        {
            Item obj1 = ContentRepository.GetItem(Sitecore.XA.Foundation.Variants.Abstractions.Items.SystemVariants);
            Item obj2 = obj1 != null ? obj1.Children.FirstOrDefault(item => item.Name.Equals(args.RenderingName)) : null;
            if (obj2 == null)
                return;
            args.Variants = args.Variants.Concat(obj2.Children).ToList<Item>();
        }
    }
}