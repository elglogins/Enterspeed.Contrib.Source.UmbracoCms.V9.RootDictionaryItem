using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Exceptions;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Enterspeed.Source.UmbracoCms.V9.Models;
using Enterspeed.Source.UmbracoCms.V9.Services;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers
{
    public class NonRootPublishedDictionaryItemJobHandler : EnterspeedPublishedDictionaryItemJobHandler, IEnterspeedJobHandler
    {
        private readonly IEnterspeedPropertyService _enterspeedPropertyService;
        private readonly IEntityIdentityService _entityIdentityService;

        public NonRootPublishedDictionaryItemJobHandler(
            IEnterspeedPropertyService enterspeedPropertyService,
            IEnterspeedIngestService enterspeedIngestService,
            IEntityIdentityService entityIdentityService,
            IEnterspeedGuardService enterspeedGuardService,
            ILocalizationService localizationService)
            : base(enterspeedPropertyService, enterspeedIngestService, entityIdentityService, enterspeedGuardService, localizationService)
        {
            _enterspeedPropertyService = enterspeedPropertyService;
            _entityIdentityService = entityIdentityService;
        }

        protected override UmbracoDictionaryEntity CreateUmbracoDictionaryEntity(IDictionaryItem dictionaryItem, EnterspeedJob job)
        {
            try
            {
                return new Models.UmbracoDictionaryEntity(
                    dictionaryItem, _enterspeedPropertyService, _entityIdentityService, job.Culture);
            }
            catch (Exception e)
            {
                throw new JobHandlingException(
                    $"Failed creating entity ({job.EntityId}/{job.Culture}). Message: {e.Message}. StackTrace: {e.StackTrace}");
            }
        }

        public new bool CanHandle(EnterspeedJob job)
        {
            return !DictionaryRootConstants.EntityId.Equals(job.EntityId, StringComparison.InvariantCultureIgnoreCase)
                && job.EntityType == EnterspeedJobEntityType.Dictionary
                && job.JobType == EnterspeedJobType.Publish;
        }
    }
}
