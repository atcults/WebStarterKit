My WebShop using AngularJS, RequireJS and NodeJS
=====================================

##Preamble
There are many sample code available in GitHub that implements real scenario. But most of them are monotonous.
Here we are trying to show how you can use Angular in real application with help of require.js and node.js

##Modules
* Brochure Website
* Authentication (OAuth 2.0)
* Shopping Cart
* Administration

## Brochure Website
* Splash (Auto hide when page is loaded)
* Home (Default lending screen)
* About Us
* Contact Us

## Authentication (OAuth 2.0)
* Sign Up
* Login
* Forget Password
* Change Password
* Token and Refresh Token (For development and testing use only)
* User Profile

## Shopping Cart
* Product page
* My Cart

## Administration
* User sign up approval
* Role management
* Product management
* Order management
* Administration dashboard


## Data Schema
* User: Id, Username, PasswordSalt, PasswordHash, Role, Full Name, Email, Mobile, Address
* Role: Enumerated roles.
* Product: Id, Name, Description, Price, Image
* Order: Id, UserId, OrderDate, OrderStatus
* OrderLine: Id, OrderId, ProductId, Qty, Price

```
Please send email at info@sanelib.com for your suggestion, feedback or interest to join this project.
```