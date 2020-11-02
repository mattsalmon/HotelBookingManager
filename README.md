# Hotel Booking Manager

This repository contains code for a simple Hotel Booking class, along with supporting Unit Tests. 
It has been developed using TDD practices. First the unit tests were written and executed to show they were failing, and then the method implementation completed until the Unit Test passed.

# Getting Started
To run the code within this solution, you will need to have Dotnet Core v3.1 installed. Once this dependency is installed you should be able to clone the repo and at the command line run:
**> dotnet build**
**> dotnet test** 
# Frameworks
To facilitate testing, the solution uses the following frameworks (added as local project references):
- xUnit as the Unit Testing framework
- Moq as the mocking library (https://github.com/Moq/moq4/wiki/Quickstart)
# File Structure

There is a single master solution file, with references to 3 projects as follows:
- **HotelBooking.sln**: The top level solution
- **HotelBookingData/HotelBookingData.csproj**: Interfaces describing a simple data access layer. Note there are no concrete implementations within this project. It exists only to facilitate mocking of this data access layer as a dependency
- **HotelBookingDomain/HotelBookingDomain.csproj**: This is a class library project which contains the business logic for the hotel booking domain, most specifically within the BookingManager.cs class. This project has a dependency on the HotelBookingData project.
- **HotelBookingTests/HotelBookingTests.csproj**: This project houses the Unit Tests which verify the behaviour of the business logic project. The tests project therefore has dependencies on both of the other projects

 > **Note:** The **DateTimeExtensions** class contains an extension method which is used to determine the next given instance of a particular weekday. This is used so that regardless of when the tests are ran, the results are determinate.
