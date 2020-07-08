using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace Axiom.SxaCustom.Variants.Pipeline
{
    public class GetVariantsBase
    {
        protected virtual void CheckLinkedVariants(ID renderingId, Item variantGrouping, List<Item> variants)
        {
            if (variantGrouping[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(renderingId.ToString()))
                variants.AddRange(variantGrouping.Children);
            else
                variants.AddRange(variantGrouping.Children.Where(variantRoot =>
                variantRoot[Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.Fields.CompatibleRenderings].Contains(renderingId.ToString())));
        }
    }
}