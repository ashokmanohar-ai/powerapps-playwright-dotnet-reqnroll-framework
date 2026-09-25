# Contributing

1. Create a feature branch from `main`.
2. Keep credentials, tokens, `.env`, and Playwright storage state out of Git.
3. Add or update Reqnroll scenarios for behavior changes.
4. Run `dotnet restore PowerApps.Automation.sln` and `dotnet build PowerApps.Automation.sln`.
5. Run the credential-free framework validation before opening a pull request:
   `dotnet test tests/PowerApps.Automation.Tests --filter "TestCategory=framework"`.
6. Use stable Power Apps selectors and keep business behavior in feature files.
7. Include evidence and a clear description for framework changes.
