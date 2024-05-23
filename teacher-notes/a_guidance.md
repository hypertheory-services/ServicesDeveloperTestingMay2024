# Final Guidance

We have the following things:

- Source Code
- Unit Tests
- System Tests
- Systems Integration Tests

If we think of testing as "verifying this code's readiness to move into the next environment" then,

## Source Code

It has to build.
This isn't very confidence inspiring. Literally the least you can do.

I've written plenty of code that compiles, but isn't "correct". Well, not _plenty_, but I'm sure _you_ have (lol).

## Unit Tests

## Feature Tests

We use fakes for backing services, but also use test doubles for some of our code as a "buttress" to allow us to make progress until we "graduate" to the next level (System Tests)

## System Tests

Only "Fakes" are used for backing services. We are no longer using test doubles for any of our code.

## Systems Integrations Tests
