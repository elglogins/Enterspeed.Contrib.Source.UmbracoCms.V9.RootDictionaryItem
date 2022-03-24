using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Exceptions;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Enterspeed.Source.UmbracoCms.V9.Services;
using System;
using UmbracoCms.V9.RootDictionaryItem.Models;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers
{
    public class RootPublishedDictionaryItemJobHandler : IEnterspeedJobHandler
    {
        private readonly IEnterspeedIngestService _enterspeedIngestService;
        private readonly IEntityIdentityService _entityIdentityService;

        public RootPublishedDictionaryItemJobHandler(
            IEnterspeedIngestService enterspeedIngestService,
            IEntityIdentityService entityIdentityService)
        {
            _enterspeedIngestService = enterspeedIngestService;
            _entityIdentityService = entityIdentityService;
        }

        public bool CanHandle(EnterspeedJob job)
        {
            return DictionaryRootConstants.EntityId.Equals(job.EntityId, StringComparison.InvariantCultureIgnoreCase)
                && job.EntityType == EnterspeedJobEntityType.Dictionary
                && job.JobType == EnterspeedJobType.Publish;
        }

        public void Handle(EnterspeedJob job)
        {
            var umbracoData = CreateUmbracoDictionaryEntity(job);
            Ingest(umbracoData, job);
        }

        internal UmbracoDictionariesRootEntity CreateUmbracoDictionaryEntity(EnterspeedJob job)
        {
            try
            {
                return new UmbracoDictionariesRootEntity(
                    _entityIdentityService, job.Culture);
            }
            catch (Exception e)
            {
                throw new JobHandlingException(
                    $"Failed creating entity ({job.EntityId}/{job.Culture}). Message: {e.Message}. StackTrace: {e.StackTrace}");
            }
        }

        internal void Ingest(IEnterspeedEntity umbracoData, EnterspeedJob job)
        {
            var ingestResponse = _enterspeedIngestService.Save(umbracoData);
            if (!ingestResponse.Success)
            {
                var message = ingestResponse.Exception != null
                    ? ingestResponse.Exception.Message
                    : ingestResponse.Message;
                throw new JobHandlingException(
                    $"Failed ingesting entity ({job.EntityId}/{job.Culture}). Message: {message}");
            }
        }
    }
}
