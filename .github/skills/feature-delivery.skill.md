---
name: feature-delivery
description: Deliver a change across the backend and Angular frontend while respecting repository architecture and real-time patterns.
---

# Feature delivery skill

Use this skill when building or modifying a feature that spans the API, business logic, infrastructure, or UI.

## Workflow

1. Confirm the feature goal and affected layers.
2. Identify the domain concepts and DTOs to update.
3. Add or update application logic before API exposure.
4. Update infrastructure code only when the feature needs persistence, external integration, or realtime streaming.
5. Adjust the Angular client to consume the new or updated API contract.
6. Validate the build and relevant behavior.

## Checkpoints

- Does the change preserve clean architecture boundaries?
- Are DTOs and contracts explicit and typed?
- Are real-time updates still coherent for the user?
- Are the docs and guidance still accurate?

## Output expectation

Provide implementation steps, file targets, and validation notes that align with the solving style used in this repository.
