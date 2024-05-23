# Services Developer Testing

## Day 1 Setup

Please edit the file [.git/config](./.git/config) and add your Github Username and the Name you use at Github where indicated.

## Post Class Changes

- Fixed the WireMock Thing

- On the "AddingSithGivesNotification" System Tests, I noticed we weren't using the fixture for the database.
  - Changed that.
  - Created a subclass of the HostFixture where we could ovverride testing services, and add mocks.
  - This is a sort of "template method pattern".

## Post-Post Class Changes

-- A ton, but a couple of interesting? things.

Wrote a Unit Test for the LoggingNotifier for the weird sith thing.
