# MaxCo introduction

This is an ECommerce website built with MVC and Dapper. It uses Identity for authorization and authentication. The purpose of the project is to have a larger project hosted that can be showcased.

There is a Worker service app, “MaxCoEmailService”, that takes care of handling emails. Currently it is a basic implementation of SMTP to send order confirms when a checkout occurs. 

The ViewModels have been placed in a class library for convenient access by both apps.

The SQL server DB uses several tables with different relationships to give a rich user-experience. The DB currently supports:
 - Products, which represent real world items. The search bar relies on product fields for its implementation
 - Categories that represent the different tags a product can have. An intermediary table holds the many to many relationship between Products and Categories
 - Orders with an intermediary table to handle the many to many relationship between products and multiple orders. 
 - Feature products, for displaying special deals or new releases in the frontpage carousel.

The frontend is built with Razor, Tag-helpers, Bootstrap and a little bit of Javascript.

## MaxCoEmailService

The intent of this abstraction is to have a central module responsible for sending emails. The Email method takes all the required information for sending any email. The ConfirmationSender builds the html string for customer orders and then calls the Email method. This structure makes it easy to extend the EmailService for additional features.

The choice to base this on a Worker template is to implement a collection for failed emails that can be retried on a schedule. This could also be extended into sending newsletters or updates on schedule.

Not entirely sure how to implement this in a good way. Need to improve my understanding of Background/Hosted services. Researching Microservice architecture might improve my understanding.

## Frontend

The website mainly looks like bootstrap at this point. Might put some time into distinguishing it from the default MVC template but that is not my priority.

Currently the website requires you to be logged in to add items to the cart. I imagine you could use caching to get around this but I had to limit the scope of the project at this time.

## Database relationships

![Database Relationships](https://user-images.githubusercontent.com/91058022/178955394-81de152c-197a-4669-97d4-c9cc650bf378.png)
