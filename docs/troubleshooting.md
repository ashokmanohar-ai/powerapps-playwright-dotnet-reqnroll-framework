# Troubleshooting

## `Executable doesn't exist` / browser missing

Run:

```powershell
./scripts/setup.ps1
```

or build the test project and execute its generated `playwright.ps1 install chromium` script.

## Power App redirects to sign-in

The storage state is missing or expired. Run:

```powershell
./scripts/auth.ps1
```

Confirm `AUTH_STORAGE_STATE_PATH` matches the generated file.

## Canvas control cannot be found

1. Confirm the app is running in play mode.
2. Confirm the player frame is `iframe[name="fullscreen-app-host"]`.
3. Inspect the element and verify `data-control-name`.
4. Check that the feature uses the exact control name.
5. Increase `TIMEOUT_MS` only after confirming the selector is correct.

## Model-Driven App loads but scenario fails early

Set `MODEL_DRIVEN_READY_SELECTOR` to a stable element that is unique to your app shell or landing page.

## Dataverse returns 401

- Token is expired.
- Token audience/resource is not the Dataverse environment.
- Test identity/app registration lacks permission.

Refresh the token using your approved Entra authentication flow.

## Dataverse returns 403

Authentication succeeded but the caller lacks the required Dataverse privilege/security role. Do not bypass this in test code; fix the test identity's approved permissions.

## Tests pass locally and fail in CI

Check:

- GitHub secret names;
- Base64 storage-state decoding;
- tenant Conditional Access restrictions on hosted runners;
- browser channel differences (local Edge vs CI Chromium);
- test data/environment drift.
