# Security Guidance

This automation framework can handle authenticated browser sessions and Dataverse bearer tokens. Treat all such material as secrets.

## Never commit

- `.env`
- `.playwright-auth/`
- access/refresh tokens
- client secrets or certificates
- customer screenshots/traces/videos containing sensitive data

## Identity

Use dedicated non-production test identities with least privilege. Respect Microsoft Entra MFA and Conditional Access policies. Do not weaken tenant security controls solely to simplify automation.

## Destructive operations

Keep delete/update scenarios tagged and isolated. Require explicit approval before running destructive automation against shared or production-like environments.

## Evidence

Screenshots, traces, videos, downloads, and network logs can contain personal or confidential data. Apply the same access control and retention policy used for other test evidence.
