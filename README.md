# NET23-GrupprojektBank

# Class Diagram (UML) of the project
![image](https://github.com/Lefuden/NET23-GrupprojektBank/blob/master/__AssignmentInformation/GruppProjekt_UML.png "Hyper Hedgehogs Fundgins UML")

# Class Descriptions

## App
A class in charge of running the program.

## LogicManager
This class will handle all the user choices and help the user navigate their way through the app and making sure that depending on the user action we call the correct methods and handle all the logic on what happens next.

## UserCommunications
This is a static class in charge of all the communications with the user. This class will be used to print all the menus, receiving a user input, validating that input and passing it along to the other classes that will be needing it.
We decided to use the SpectreConsole Nuget Package for this due to having a big flexibility in design choices and the capability of handling validation and conversion in its own methods without us having to implement extra classes and logic to handle it.
This also gives us the luxury of having all the validation and conversion contained within the same class and in some cases methods that makes it easier to debug and expand or correct if needed during the development process.

## UserCommunications
This is a static class in charge of all the communications with the user. This class will be used to print all the menus, receiving a user input, validating that input and passing it along to the other classes that will be needing it.
We decided to use the SpectreConsole Nuget Package for this due to having a big flexibility in design choices and the capability of handling validation and conversion in its own methods without us having to implement extra classes and logic to handle it.
This also gives us the luxury of having all the validation and conversion contained within the same class and in some cases methods that makes it easier to debug and expand or correct if needed during the development process.

## LoginManager
This class is handling the login process of the application and will be in charge of controlling if the user is successful in login in to their account and, if the user fail 3 times locking the user for n amount of time.

## CurrencyExchangeRate
A static class which holds three dictionaries of the type of our enum CurrencyType and the current exchange rate value as a double.
Each dictionary holds values with these currencies as the main currency type
1. SEK
2. USD
3. EUR
This class have a method to update it the dictionaries if the user trying to do so, is an admin. If they are an admin we give them a choice to either load currency exchange rates from a file and update the dictionaries with those or to use ExchangeRate-Api.com as a source for currency exchange rates.

If the admin chooses to get them with the API they are asked for an api key, after getting a valid key the class makes a request and saves the three responses to file before serializing them to objects that is later used to update the values of our dictionaries.

## Log
A Data Transfer Object to handle the creation of Logs that all User accounts will have, each log will hold the information about what user, what event and at what time it happened.

## TransactionManager
We choose to break out the transactions to it's own class that handles that information due to the fact that the last piece of the backlog states that they want all transactions to happened on a schedule, every 15 minute to be exact.
To avoid having to redo a lot of code due to not taking this into account earlier, we started out by making this class to handle the transactions directly at first and when we got to the stage to handle the time based events, we just have to update this class, not the whole code base.

### Transaction
Is a Data Transfer Object for the transactions that the user want to make and will hold the information needed so that we can schedule our transaction later in the project.

_____

### User
This is the super class of User. The main component to be able to connect to the bank and this will hold all the information needed to be using our application.
User is an abstract class with two subclasses Customer and Admin.

#### PersonInformation
A Data Transfer Object that will be holding all the specific Personal User Information, we decided to break it out into smaller parts out from the User class to make it easier to organize and summarize a User object.

##### ContactInformation
A Data Transfer Object that is a part of PersonInformation that specifies a User object Email, Phone and Address information.

###### Email
Small data class to hold the User Email Address.

###### Phone
Small data class to hold the User Phone Number(s).

###### Address
Small data class to hold the User Address Information.

______

### Customer
A subclass of User that is our standard customer account. This class is also in charge of keeping track of each individual customers bank accounts

#### BankAccount
A abstract super class that will hold the information of a specific customers bank account.

##### Checking
A subclass of BankAccount that holds the functionality for the purpose of being a Checkings Account.

##### Savings
A subclass of BankAccount that holds the functionality for the purpose of being a Savings Account.

______

### Admin
A subclass of User that will be used for admin functionality in our application. The specific use case of being able to update the currency exchange rate and creating new user accounts will be managed with the Admin class.