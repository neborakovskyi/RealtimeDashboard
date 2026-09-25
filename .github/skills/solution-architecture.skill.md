---
name: solution-architecture
description: Understand RealtimeDashboard architecture, boundaries, and the real-time dashboard workflow.
---

# Solution architecture skill

Use this skill when working on repository-level questions, architecture review, or design decisions.

## Ask yourself

- What project boundary does this change belong to?
- Does this affect the domain, application, infrastructure, API, or UI layer?
- Does this change impact realtime behavior or AI integration?

## Apply the repo pattern

- Domain stays domain-only.
- Application owns use cases and orchestration.
- Infrastructure owns persistence and external services.
- API owns web exposure.
- UI owns dashboard rendering and live subscription behavior.

## Validate

- Confirm the change matches the layer boundaries.
- Check whether new docs or guidance are needed.
- Verify the final implementation remains consistent with the real-time dashboard design.
