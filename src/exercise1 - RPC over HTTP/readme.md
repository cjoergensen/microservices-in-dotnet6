
# Exercise 1 - RPC over HTTP

## Objective

TODO

## Introduction to the solution

The solution contains two services and the web frontend:

### SelfService.WebApp.Client
Blazor WebAssembly app with a simple UI. The UI contains two pages to edit the Profile and Notifications.

### CustomerProfileService

This service is responsible for managing the Customers Profile information. Data is store in a LiteDB database and it exposes a REST Api for clients to retrieve and update data.

The _CustomerProfileService_ consists of these projects:

| Project  | Description  |
|---|---|
| **Contracts** | The Commands, Event and Queries integration DTO's. These are the messages that the service publishes og consumes |
| **Data** | Data Repositories and entities used internaly by the service  |
| **Models** | Internal models |
| **WebApi** | The WebApi that the service exposes to its clients  |

### ConsumptionNotificationSubscriptionService

The _ConsumptionNotificationSubscriptionService_ uses the same structure as descriped in [CustomerProfileService](#CustomerProfileService) and is responsible for managing the notifications that customer wants to recieve and the communication channel to use.



## Step 1 - Retrieve the Customer Profile
The first objective is to be able to get the Customer Profile information from the  _CustomerProfileService_ and use display it in the _SelfService.WebApp.Client_

## Step 2
## Step 3
## Step 4
## Step 5
## Step 6
## Step 7