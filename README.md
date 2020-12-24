# GeometryService

## Projects
1. [GeometryService.Domain](##GeometryService.Domain)
1. [GeometryService.WebApp](##GeometryService.WebApp)
1. [GeometryService.WebApp.Tests](##GeometryService.WebApp.Tests)


## GeometryService.Domain
GeometryService.Domain contains domain logic, models and database mechanisms for working with geometry figures.
For geometry figure range extending you should execute next steps:
1. Extend types in enum `GeometryService.Domain.Models.FigureType`;
1. Create new classes in namespace `GeometryService.Domain.Models` by implementing `GeometryService.Domain.Models.FigureBase`.
1. Add cases to switches of `FigureType`.

## GeometryService.WebApp
GeometryService.WebApp realizes RestAPI of geometry figures functions.
### Add figure to database
For setting figure to database you should make `Post`-request to address: `/api/figure`. 
Request supports the next body json-structures:
- circle
```json
{
    "Type": 0,
    "Center": {
        "X": 1.0,
        "Y": 1.0
    },
    "Radius": 3.5
}
```
- triangle
```json
{
    "Type": 1,
    "Point1": {
        "X": 1.1,
        "Y": 1.2
    },
    "Point2": {
        "X": 1.4,
        "Y": 2.5
    },
    "Point3": {
        "X": 2.1,
        "Y": 2.6
    }
}
```
The server answer returns json-structure of Guid-type.

### Get figure square
For getting figure square from server you should make `Get`-request to address: `/api/figure/{id}`.
The server answer returns json-structure of double-type.

### Swagger
You can get more information about realized RestAPI by link: `/swagger`.

## GeometryService.WebApp.Tests
This project contains integration tests of RestAPI and SqlLite features. 
`WebApplicationFactory` is used at project.

Current solution coverage is 89%.