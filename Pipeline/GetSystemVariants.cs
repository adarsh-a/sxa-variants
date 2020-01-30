using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sc_milka.sitecore.Pipeline
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
            Item obj1 = this.ContentRepository.GetItem(Sitecore.XA.Foundation.Variants.Abstractions.Items.SystemVariants);
            Item obj2 = obj1 != null ? obj1.Children.FirstOrDefault<Item>((Func<Item, bool>)(item => item.Name.Equals(args.RenderingName))) : (Item)null;
            if (obj2 == null)
                return;
            args.Variants = (IList<Item>)args.Variants.Concat<Item>((IEnumerable<Item>)obj2.Children).ToList<Item>();
        }
    }
}