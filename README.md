# Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem &middot; [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/elglogins/Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem/pulls) [![NuGet version](https://img.shields.io/nuget/v/Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem)](https://www.nuget.org/packages/Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem/)

## What is this package about?

It is an extension on top of Enterspeeds source connector for Umbraco v9, during source entities ingestion, it ingests additional root dictionary item entity per culture. 

Ideas for usage:
- Use in schemas to leverage `actions` and avoid `route handle -> view` conflicts.

### Example of dictionary root item source entity
```json
{
	"sourceId": "gid://Source/531fd49c-4191-4ff0-a3c2-391189f9f1ae",
	"id": "gid://Source/531fd49c-4191-4ff0-a3c2-391189f9f1ae/Entity/dictionaries-root-en-US",
	"type": "umbDictionaryRoot", // always 'umbDictionaryRoot'
	"originId": "dictionaries-root-en-US", // dictionaries-root-{p.culture}
	"originParentId": null,
	"url": null,
	"properties": {
		"culture": "en-US"
	}
}
```

### Example of regular dictionary item source entity
```json
{
	"sourceId": "gid://Source/531fd49c-4191-4ff0-a3c2-391189f9f1ae",
	"id": "gid://Source/531fd49c-4191-4ff0-a3c2-391189f9f1ae/Entity/e98023c9-d79c-4a4a-962a-5d295ff4ff4d-en-US",
	"type": "umbDictionary",
	"originId": "e98023c9-d79c-4a4a-962a-5d295ff4ff4d-en-US",
	"originParentId": "dictionaries-root-en-US", // This is also new! Previously = null
	"url": null,
	"properties": {
		"key": "Buttons.Refresh",
		"translation": "Refresh!",
		"culture": "en-US"
	}
}
```

## Installation
```
dotnet add package Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem
```

Check other [installation options](https://www.nuget.org/packages/Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem/).


## Contributing

Pull requests are very welcome.  
Please fork this repository and make a PR when you are ready.

Otherwise you are welcome to open an Issue in our [issue tracker](https://github.com/elglogins/Enterspeed.Contrib.Source.UmbracoCms.V9.RootDictionaryItem/issues).

## License

This project is [MIT licensed](./LICENSE)
