# Authentication

## Local execution

Power Apps commonly uses Microsoft Entra ID, MFA, Conditional Access, and tenant-specific policies. Automating the login form on every test is fragile and can violate identity controls.

The framework therefore provides a separate authentication bootstrap:

```powershell
./scripts/auth.ps1
```

The tool:

1. Opens the configured Power App in a headed Chromium/Edge browser.
2. Lets the tester complete the organization's approved Entra authentication flow.
3. Saves Playwright browser storage state to `AUTH_STORAGE_STATE_PATH`.
4. Test scenarios load that state into a new isolated browser context.

Default:

```text
.playwright-auth/state.json
```

## Security

Storage-state files can contain cookies and tokens capable of impersonating the authenticated test user. Never commit them, attach them to tickets, or publish them as build artifacts.

Use:

- a dedicated non-production automation identity;
- least-privilege Power Platform security roles;
- tenant-approved MFA/Conditional Access treatment;
- short retention for CI secrets;
- rotation/re-authentication when the session expires.

## GitHub Actions

For manually triggered live UI suites, encode the storage-state JSON and store it in the repository secret:

```text
POWERAPPS_STORAGE_STATE_BASE64
```

Example PowerShell encoding:

```powershell
[Convert]::ToBase64String([IO.File]::ReadAllBytes('.playwright-auth/state.json'))
```

The workflow decodes the secret only in the ephemeral runner and never commits it.

## Dataverse API authentication

The reference API client accepts a bearer token through `DATAVERSE_ACCESS_TOKEN`. In enterprise CI, replace manual token injection with your organization's approved Entra workload identity/client credential flow and token acquisition policy. Keep token acquisition outside feature files.
