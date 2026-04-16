---
name: UX Guidelines
description: "Use when creating UX flows, UI structure, forms, onboarding, dashboards, checkout, or usability improvements."
---
# UX Guidelines

## Core Principles
- Start from user intent, not from UI components.
- Keep one clear primary action per screen.
- Reduce cognitive load: short labels, progressive disclosure, clear grouping.
- Prefer recognition over recall: visible options, examples, defaults.
- Design for edge states early: empty, loading, error, no-permission.
- Use a modern, intentional visual language instead of generic default component styling.
- Keep typography, spacing, and contrast consistent across all screens.

## Style Direction
- Avoid standard Bootstrap look-and-feel and out-of-the-box Bootstrap visual patterns.
- Use modern layout composition with Grid/Flex, deliberate whitespace, and strong hierarchy.
- Define and reuse typography tokens (heading/body sizes, weights, line-heights).
- Use a clear color system with strong text-to-background contrast.

## Theme Defaults (Dog Walking)
- Use a playful retro pastel interface style where appropriate.
- Suggested palette:
	- Background: #fef6e9
	- Surface: #fffdf8
	- Primary accent: #ff8ba7
	- Secondary accent: #8bd3dd
	- Highlight: #f6c177
	- Text primary: #2b2d42
	- Text muted: #5c607a
- Suggested typography direction:
	- Heading style: expressive rounded retro display tone.
	- Body style: simple, highly legible sans-serif.
- Suggested component feel:
	- Rounded cards, soft shadows, outlined inputs, and bold but concise CTA labels.
	- Decorative retro accents should never reduce navigation clarity or readability.

## Form UX
- Ask only for data that is necessary for the current step.
- Place labels above fields; do not rely on placeholder-only labeling.
- Validate early when helpful, but avoid noisy errors while typing.
- Show errors next to fields with exact fix instructions.
- Preserve entered data after validation errors.

## Navigation and Flow
- Keep navigation depth shallow and predictable.
- Highlight current location and next best action.
- Use explicit step indicators for multi-step processes.
- Always provide a safe back/cancel path.
- Improve wayfinding with clear page titles, active states, and contextual breadcrumbs where useful.
- Reduce dead ends by always exposing a next action.

## Content and Microcopy
- Use action-oriented verbs on buttons.
- Write concrete labels: avoid generic text like "Submit" when possible.
- Explain consequences before destructive actions.
- Provide success feedback with what happens next.
- Break dense content into short sections for scanability.
- Favor plain language and sentence-case labels for readability.

## Accessibility Baseline
- Ensure keyboard operability for all interactive controls.
- Ensure visible focus indicators and logical tab order.
- Ensure sufficient contrast and scalable text.
- Pair color cues with text/icons (do not rely on color alone).
- Check contrast for body text, muted text, links, and button labels in all states.

## Responsive Baseline
- Mobile first for hierarchy and spacing.
- Keep primary actions thumb-reachable on mobile.
- Avoid horizontal scrolling for core content.
- Recheck empty and error states at all breakpoints.

## Delivery Standard
When giving UX recommendations, include:
1. Problem being solved
2. Proposed structure/flow
3. Navigation and readability improvements
4. Trade-offs and rationale
5. Accessibility and contrast checks
6. How to test success
