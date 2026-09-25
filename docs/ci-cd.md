# CI/CD

Workflow: `.github/workflows/powerapps-dotnet.yml`

## Automatic quality gate

Pull requests and pushes that touch this module run a credential-free job:

1. checkout;
2. install .NET 10;
3. restore;
4. build Release;
5. execute the `@framework` Reqnroll scenario;
6. upload the TRX test result.

This verifies the project graph, package compatibility, Reqnroll code generation, bindings, and NUnit execution without requiring access to a customer tenant.

## Live suites

Use **Run workflow** and choose one of:

- `canvas`
- `mda`
- `dataverse`
- `ui`
- `all`

### UI secrets

```text
POWERAPPS_STORAGE_STATE_BASE64
CANVAS_APP_URL
MODEL_DRIVEN_APP_URL
```

### Dataverse secrets

```text
DATAVERSE_BASE_URL
DATAVERSE_ACCESS_TOKEN
```

The live job installs Chromium and injects secrets as environment variables. It must run against a dedicated test environment and identity.

## Recommended enterprise extension

- Use a protected environment for UAT/production-adjacent runs.
- Require human approval for destructive suites.
- Use OIDC/workload identity rather than long-lived Dataverse secrets where supported.
- Publish only sanitized evidence.
- Add nightly regression after the smoke suite is stable.
- Add release gates from historical reliability data, not arbitrary thresholds.
