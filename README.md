# RedditSentimentAnalyzer

[![Build Status](https://rpm1984.visualstudio.com/_apis/public/build/definitions/c48bc504-32f0-48e7-9b2f-af5a0de677aa/1/badge)](https://rpm1984.visualstudio.com/RedditSentimentAnalyzer/_build/index?definitionId=1)
![Codecov](https://img.shields.io/codecov/c/github/RPM1984/RedditSentimentAnalyzer.svg)
[![NuGet](https://img.shields.io/nuget/dt/RedditSentimentAnalyzer.svg)](https://www.nuget.org/packages/RedditSentimentAnalyzer/)

## What is this?
A dead-simple library to help analyze the sentiment of a particular term on Reddit.

## How to use
1) Signup for a [Azure Cognitive Services](https://azure.microsoft.com/en-gb/try/cognitive-services/?api=text-analytics) account. You can get 5,000 free transactions/month for free. Note down your API key and region for your account.
2) Create a new instance of `SentimentAnalyzer`:
```
var analyzer = new SentimentAnalyzer(logger, 
                                     "your-cognitive-services-key", // replace with your key
                                     AzureRegions.WestUs); // replace with assigned region
```
3) Call `GetSentimentAsync`, passing through a subreddit and search term. For example:
```
var results = await analyzer.GetSentimentAsync("cryptocurrency", "bitcoin");
```

Nuget coming soon :)
