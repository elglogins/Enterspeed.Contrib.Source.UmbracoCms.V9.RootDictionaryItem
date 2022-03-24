using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.UmbracoCms.V9.Services;
using Umbraco.Cms.Core.Models;
using UmbracoCms.V9.RootDictionaryItem;

namespace UmbracoCms.V9.RootDictionaryItem.Models
{
    public class UmbracoDictionariesRootEntity : IEnterspeedEntity
    {
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly string _culture;

        public UmbracoDictionariesRootEntity(
            IEntityIdentityService entityIdentityService,
            string culture)
        {
            _entityIdentityService = entityIdentityService;
            _culture = culture;
            Properties = new Dictionary<string, IEnterspeedProperty>();
            Properties.Add("culture", new StringEnterspeedProperty(culture));
        }

        public string Id => _entityIdentityService.GetId(DictionaryRootConstants.EntityId, _culture);
        public string Type => "umbDictionaryRoot";
        public string Url => null;
        public string[] Redirects => null;
        public string ParentId => null;
        public IDictionary<string, IEnterspeedProperty> Properties { get; }
    }
}
