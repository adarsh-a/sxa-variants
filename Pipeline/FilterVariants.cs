using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Abstractions.Configuration;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sc_milka.sitecore.Pipeline
{
    public class FilterVariants : GetVariantsBase
    {
        public IContentRepository ContentRepository { get; set; }

        public FilterVariants(IContentRepository contentRepository)
        {
            this.ContentRepository = contentRepository;
        }

        public void Process(GetVariantsArgs args)
        {
            args.Variants = (IList<Item>)args.Variants.Where<Item>((Func<Item, bool>)(i => this.AllowedInTemplate(i, args.PageTemplateId))).Distinct<Item>((IEqualityComparer<Item>)new ItemIdComparer()).ToList<Item>();
        }

        protected virtual bool AllowedInTemplate(Item item, string pageTemplateId)
        {
            Field field = item.Fields[Sitecore.XA.Foundation.Variants.Abstractions.Templates.IVariantDefinition.Fields.AllowedInTemplates];
            if (field != null && (!(field.Value == string.Empty) || !this.InheritsFromAllowedTemplate(pageTemplateId)))
                return field.Value.Contains(pageTemplateId);
            return true;
        }

        protected virtual bool InheritsFromAllowedTemplate(string pageTemplateId)
        {
            TemplateItem templateItem = this.ContentRepository.GetTemplate(new ID(pageTemplateId));
            return ServiceProviderServiceExtensions.GetService<IConfiguration<Sitecore.XA.Foundation.Variants.Abstractions.VariantsConfiguration>>(ServiceLocator.ServiceProvider).GetConfiguration().AllowedTemplates.Any<ID>((Func<ID, bool>)(id => templateItem.DoesTemplateInheritFrom(id)));
        }
    }
}