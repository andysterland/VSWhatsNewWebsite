# Website for Visual Studio What's New Content

The dev site is deployed to Azure at: [https://webfrontend.redflower-c5201961.westus2.azurecontainerapps.io/](https://webfrontend.redflower-c5201961.westus2.azurecontainerapps.io/)

# Projects

## WhatsNewWebsite.ApiService
An asp.net minimal Api for fetching the content.

## WhatsNewWebsite.AppHost
An Aspire host for the ApiService and Web Projects

## WhatsNewWebsite.DataModel
Classes used to represent common objects across the different projects.

## WhatsNewWebsite.GenerateContent
An exe that takes the content from the WhatsNewContent repo and generates the JSON files for use by the ApiService.

## WhatsNewWebsite.Tests
Not used...

## WhatsNewWebsite.
An ASP.NET Blazor app that is the front end using server side interactivty.

# To do

. Improve the design of the main layout
. Implement a modal detail view with the markdown article
. Implement a mechanism to export the images for the markdown
. Implement Azure Cognitive Search
. Implement filter for version number
