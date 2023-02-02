# ef30191

An extension is added in `OnConfiguring`. This extension does nothing whatsoever.
It doesn't actually matter which extension we use here. The same issue occurs if you 
use the No-Op one in this repository, or something like `EF.NamingConventions` (our 
actual use-case)

When setting up tests, if we don't re-add this extension in our test harness then 
we will end up with `ManyServiceProvidersCreatedWarning`. This applies to **all**
extensions that may have been added in `OnConfiguring`.

See `Repro.Tests/TestFixture.cs` which is reponsible for creating Context/Options. 
In our normal setup, each test class gets a **dedicated docker container** (server). 
We mimic that here by adding a Guid to the connection string. 

1. You will need to set up the connection string for your local database
2. Run all tests in the solution 
  - All test classes are identical, just duplicated to give enough dedicated test fixtures
3. Repeat for the 4 permuatations identified below

In `CreateOptions` there are four different setup scenarios. Comment them out in 
order and re-run all tests.

| No. | Throws | Source           | Extension Re-Added |
| --- | ------ | ---------------- | ------------------ |
| 1   | YES    | DbDataSource     | no                 |
| 2   | no     | DbDataSource     | yes                |
| 3   | no     | ConnectionString | no                 |
| 4   | no     | ConnectionString | Yes                |

Ideally, #1 is how we **want** to perform configuration, without being required to 
re-specify every extension that our context automatically adds. (We also need the 
DbDataSource/Builder for other features)

We fear that this some how impacts normal operation (`AddDbContext`, etc.)