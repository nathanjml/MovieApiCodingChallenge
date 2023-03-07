# Destify Movies API

## Additional Packages Used
* AutoMapper
* FluentValidation
* SimpleInjector
* NSubstitute

## Setup

**Press F5 from Visual Studio**

### Notes	
  * Solution is configured into 3 projects:
     * Web Api Project
     * Core (Shared) Project
     * Unit Test Project
  * using in-memory database provider
## Authentication

Api Token

## General Structure of API Project

* Configuration - (MVC, dependency injection, swagger, etc.)
* Controllers - API controllers
* Filters - Web API Pipeline filters (API Rate Limiting)

## General Structure of Core Project

* Configuration - (AutoMapper, database, dependency injection)
* Domain - Data models and Entity Configuration
* Extensions - Extension methods
* Features - All major application features and their parts (Dtos, AutoMapper Profiles, Handlers, Validators)
* Services - services designed to aid the architecture of the project (IApiKeyService, IMediator, etc.)

### Additional Comments
All of the code for this project is authored by myself, Nathan Murphy, for the .Net Coding Challenge at Destify. There were several design decisions that I considered, and of those, one of the most important was whether or not to use this package: https://github.com/unstable-sort. Although the .Net Coding Challenge states that any nuget package/lib is allowed - I felt as if this was "cheating". This package was mostly written by my collegue and friend Ronald Brohn at Turner Industries as a way to improve our efficiency using the Mediator design pattern in our Web API projects.

Out of the box, it is completely configurable and supports templated requests for:
* All CRUD operations
* Request Paging, Filtering, & Sorting
* Hooks for custom code execution before/after requests
* Request validation

I opted not to use this package and instead write my own simpler version of the mediator pattern as I felt as if it would have over simplified the process - thus destroying the point of the coding challenge. In hindsight, the package would have probably saved me hours of time creatings CRUD requests, and in a *real* scenario, this package (or similar) is more practical.

I expected to have this challenge finished in just a couple hours, but I was way, way off. I underestimated just how much the features in this project would expands with only a handful of entities. As such, I completely neglected to commit any of the code to source control (in my blind naivety) until the very end. In total, there were only two unique problems I had to tackle for this challenge: API Rate Limiting and API Token auth. 

##### Api Token
As mentioned in the interview, all of my experience stems from building APIs that are used internally. Our security concerns are very different and I have never implemented either of these features before; however, the concept for both were easy to visualize. For the API Token, I knew I wanted issue a unique token per user. I also knew I wanted to be able to pull an identity from the token (this is used for the `CreateMovieRatingRequest`).

##### Rate Limiting
For rate limiting, I did some light research which revealed a few packages I could have used. However, the concept was fairly simple and I wanted to incorporate custom rate limiting for anonymous users and users that had an API token. It seemed like I could either use ASP.NET Core Middleware or Filters. I decided to implement it with an `IActionFilter`, and it was fairly easy to get working. I then added variables in `appsettings.json` so that these values can be configured more easily.

##### Testing
Normally, testing is pretty important. I don't practice TDD like I used to, but unit testing is still something that isn't overlooked. However, due to time constraints, I opted to only include a few unit tests. The challenge documents states that there needs to be at least *one*, so I assumed this was mostly just to show the ability to create tests. I've included more than one, but definitely less than I would have on a real application.

##### Wrapping Up
This was incredibly fun and kept me busy the past couple of days. I initially wanted to have this complete by Sunday 3/5/2023, but that didn't quite happen. I did feel rushed towards the end. 

Regardless, thank you again for the opportunity to share my experience and technical ability for the role at Destify. I look forward to any follow-up interviews where I can explain the code in more detail (if needed) and some of the design decisions. There are a lot of improvements that could be made, such as paging requests, filtering, sorting, the CRUD (to something like UnstableSort package), but ultimately, I think this is a good reflection of myself for a coding challenge project.

References:
  * https://docs.microsoft.com/en-us/ef/core/ (Entity Framework Core)
  * https://github.com/AutoMapper/AutoMapper/wiki/Getting-started (AutoMapper)
  * https://github.com/JeremySkinner/FluentValidation/wiki (Fluent Validation)
  * https://simpleinjector.readthedocs.io/en/latest/index.html (Simple Injector)
  