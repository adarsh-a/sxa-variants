
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;


namespace sc_milka.sitecore.Pipeline
{
    public class GetVariantsBase
    {
        protected virtual void CheckLinkedVariants(
         ID renderingId,
         Item variantGrouping,
         List<Item> variants)
        {
            if (variantGrouping[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(renderingId.ToString()))
                variants.AddRange((IEnumerable<Item>)variantGrouping.Children);
            else
                variants.AddRange(variantGrouping.Children.Where<Item>((Func<Item, bool>)(variantRoot => 
                variantRoot[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(renderingId.ToString()))));
        }
    }
}