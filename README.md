# Transaction API

## Contents

- [Problem Summary](#problem-summary)
- [Getting Started](#getting-started)
- [Testing](#testing)
- [Assumptions and Caveats](#assumptions-and-caveats)

## Problem Summary

This Project is a REST API for retreiving and updating transactions to a SQlite Database. Transactions received from the database are sorted in time order and a transaction's status can only be updated by authenticated users.

Read more here:
https://github.com/parpera/parpera-code-challenges/blob/main/netcore-api.md

## Getting Started

1. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/)
2. Install the .Net Core 3.1 Runtime, this can be done through the Visual Studio installer
3. clone this project with `git clone https://github.com/JoshDubar/transaction-api.git`
4. Open this project in Visual Studio
5. Change the project next to the "play" button to `Transaction API`
6. press the "play" button to start the project
7. (Optional) Install [DB Browser for SQlite](https://sqlitebrowser.org/dl/) and open the Transaction.db file from the project in order to manually interact with the Database

## Testing

1. Repeat steps 1-4
2. Right click on the `UnitTests` Solution folder and click `Run Tests`

## Assumptions and Caveats

1. Due to time constraints I only ended up implementing tests for the controller class
2. I was unable to implement tests for the `ValidationProblem` branch of the `UpdateTransactionStatus` as I was unable to figure out how to mock the model validation to return false
3. Because the problem stated that `You do not need to issue tokens, simply assume the client has already acquired a valid token`, I simply checked if any `Authorization` header existed and was not an empty string, and if it is then I let the request through.
