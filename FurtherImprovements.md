# Further Refactoring

## Given more time

Given more time to improve the application I would:

* Continue Unit Testing to achieve full coverage of `TicketService.cs`.

* Investigate adding logging, specifically to log the custom exceptions thrown by the application, and then handle the exceptions.

* Investigate how to Unit Test a Static class effectively, potentially could add a method to reset the state between tests.

## Opportunities with lifted restrictions

If some task restrictions were eliminated there is potential for further improvement:

* Improve implementation of Dependency Injection by creating a `ServiceCollection` in `Program.cs`. This would allow the application to become loosely coupled.

* Changing `TaskRepository.cs` from a Static class to a Singleton would allow it to be used with Dependency Injection and improve the testability of the app.

* Refactor `GetUser` in `UserRepository` to use a `Using` block within the `try` block to create and dispose of the `SqlConnection`.

## Notes

* Unit Tests for `TicketRepository` have been disabled due to static field persisting between tests.