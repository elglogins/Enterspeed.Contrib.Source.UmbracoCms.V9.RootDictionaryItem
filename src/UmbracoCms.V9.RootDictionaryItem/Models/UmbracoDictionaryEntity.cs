using Enterspeed.Source.UmbracoCms.V9.Services;
using Umbraco.Cms.Core.Models;

namespace UmbracoCms.V9.RootDictionaryItem.Models
{
    public class UmbracoDictionaryEntity : Enterspeed.Source.UmbracoCms.V9.Models.UmbracoDictionaryEntity
    {
        private readonly IDictionaryItem _dictionaryItem;
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly string _culture;

        public UmbracoDictionaryEntity(
            IDictionaryItem dictionaryItem,
            IEnterspeedPropertyService propertyService,
            IEntityIdentityService entityIdentityService,
            string culture)
            : base(dictionaryItem, propertyService, entityIdentityService, culture)
        {
            _dictionaryItem = dictionaryItem;
            _entityIdentityService = entityIdentityService;
            _culture = culture;
        }

        public new string ParentId
        {
            get
            {
                if (!_dictionaryItem.ParentId.HasValue)
                {
                    return _entityIdentityService.GetId(DictionaryRootConstants.EntityId, _culture);
                }

                return base.ParentId;
            }
        }
    }
}
