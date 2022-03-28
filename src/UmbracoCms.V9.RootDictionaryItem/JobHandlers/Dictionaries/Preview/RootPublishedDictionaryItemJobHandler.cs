using System;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Exceptions;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Enterspeed.Source.UmbracoCms.V9.Models;
using Enterspeed.Source.UmbracoCms.V9.Providers;
using Enterspeed.Source.UmbracoCms.V9.Services;
using UmbracoCms.V9.RootDictionaryItem.Models;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers.Dictionaries.Preview
{
    public class RootPublishedDictionaryItemJobHandler : IEnterspeedJobHandler
    {
        private readonly IEnterspeedIngestService _enterspeedIngestService;
        private readonly IEntityIdentityService _entityIdentityService;
        private readonly IEnterspeedConnectionProvider _enterspeedConnectionProvider;

        public RootPublishedDictionaryItemJobHandler(
            IEnterspeedIngestService enterspeedIngestService,
            IEntityIdentityService entityIdentityService,
            IEnterspeedConnectionProvider enterspeedConnectionProvider)
        {
            _enterspeedIngestService = enterspeedIngestService;
            _entityIdentityService = entityIdentityService;
            _enterspeedConnectionProvider = enterspeedConnectionProvider;
        }

        public bool CanHandle(EnterspeedJob job)
        {
            return DictionaryRootConstants.EntityId.Equals(job.EntityId, StringComparison.InvariantCultureIgnoreCase)
                &&  _enterspeedConnectionProvider.GetConnection(ConnectionType.Preview) != null
                && job.EntityType == EnterspeedJobEntityType.Dictionary
                && job.ContentState == EnterspeedContentState.Preview
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
            var ingestResponse = _enterspeedIngestService.Save(umbracoData, _enterspeedConnectionProvider.GetConnection(ConnectionType.Preview));
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
