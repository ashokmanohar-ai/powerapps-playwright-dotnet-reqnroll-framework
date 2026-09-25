# Selector Strategy

## Canvas Apps

Microsoft's Power Platform Playwright guidance shows Canvas Apps running inside the player iframe:

```css
iframe[name="fullscreen-app-host"]
```

The framework scopes all Canvas interactions to this frame.

Power Apps controls expose useful `data-control-name` values:

```css
[data-control-name="SubmitButton"]
```

Use the Power Apps control name rather than generated DOM paths.

### Preferred order

1. `data-control-name` for Canvas controls.
2. Accessible role/name where the rendered control exposes stable semantics.
3. Stable app-owned `data-*` attributes.
4. CSS selectors tied to stable component contracts.
5. Avoid XPath based on generated nesting/indexes.

## Model-Driven Apps

Prefer accessible locators and stable platform/app attributes:

```csharp
page.GetByRole(AriaRole.Button, new() { Name = "Save", Exact = true })
page.GetByLabel("Account Name", new() { Exact = true })
page.Locator("[data-id='your-stable-id']")
```

Do not freeze a generic locator library around undocumented generated class names. Create business-specific component objects for forms, grids, command bars, and custom pages as your app requires.

## Discovering Canvas control names

1. Open the app in play mode.
2. Open browser DevTools.
3. Inspect the required control inside the Power Apps player frame.
4. Locate the nearest `data-control-name` attribute.
5. Use the exact Power Apps control name in your feature/page object.
