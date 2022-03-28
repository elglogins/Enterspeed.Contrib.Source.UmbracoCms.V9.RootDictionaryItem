using System;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Exceptions;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Enterspeed.Source.UmbracoCms.V9.Handlers.Dictionaries;
using Enterspeed.Source.UmbracoCms.V9.Handlers.PreviewDictionaries;
using Enterspeed.Source.UmbracoCms.V9.Models;
using Enterspeed.Source.UmbracoCms.V9.Providers;
using Enterspeed.Source.UmbracoCms.V9.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers.Dictionaries.Preview
{
    public class NonRootPublishedDictionaryItemJobHandler : EnterspeedPreviewDictionaryItemPublishJobHandler, IEnterspeedJobHandler
    {
        private readonly IEnterspeedPropertyService _enterspeedPropertyService;
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly IEnterspeedConnectionProvider _enterspeedConnectionProvider;

        public NonRootPublishedDictionaryItemJobHandler(
            IEnterspeedPropertyService enterspeedPropertyService,
            IEnterspeedIngestService enterspeedIngestService,
            IEntityIdentityService entityIdentityService,
            IEnterspeedGuardService enterspeedGuardService,
            ILocalizationService localizationService, 
            IEnterspeedConnectionProvider enterspeedConnectionProvider)
            : base(enterspeedPropertyService, enterspeedIngestService, entityIdentityService, enterspeedGuardService, localizationService, enterspeedConnectionProvider)
        {
            _enterspeedPropertyService = enterspeedPropertyService;
            _entityIdentityService = entityIdentityService;
            _enterspeedConnectionProvider = enterspeedConnectionProvider;
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
                && _enterspeedConnectionProvider.GetConnection(ConnectionType.Preview) != null
                && job.EntityType == EnterspeedJobEntityType.Dictionary 
                && job.ContentState == EnterspeedContentState.Preview
                && job.JobType == EnterspeedJobType.Publish;
        }
    }
}
