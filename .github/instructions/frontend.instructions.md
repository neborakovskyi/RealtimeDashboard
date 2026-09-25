# Frontend instructions

Apply these instructions when working in the Angular project under `realtime-dashboard-ui`.

## Frontend standards

- Keep components focused, small, and single-responsibility.
- Prefer signals over imperative state management when appropriate.
- Use standalone components and modern Angular APIs.
- Do not add `standalone: true` explicitly; it is the default in Angular 22+.
- Do not add `ChangeDetectionStrategy.OnPush` explicitly unless absolutely necessary.
- Use native template syntax (`@if`, `@for`, `@switch`) instead of `*ngIf` or `*ngFor`.
- Prefer bindings over `ngClass` and `ngStyle`.
- Use `input()`, `output()`, and `model()` patterns.
- Keep templates simple and readable.

## Accessibility

- Ensure the UI meets WCAG AA expectations.
- Maintain keyboard accessibility and focus flow.
- Prefer semantic HTML and accessible labels.
- Test for AXE issues when modifying components or forms.

## Real-time UI behavior

- Expect the dashboard to receive live metric updates and update the UI reactively.
- Keep the client aligned with the SignalR payload contract.
- Respect the existing category-based or live-update behavior when adding new dashboard content.
