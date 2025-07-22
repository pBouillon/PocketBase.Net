# Contributing to PocketBase.Net

Thank you for considering contributing to PocketBase.Net!

## How Can I Contribute?

There are several ways you can contribute to PocketBase.Net:

- **Reporting Bugs**: If you find a bug, please report it by opening an issue.
- **Suggesting Enhancements**: If you have an idea for a new feature or improvement, open a discussion or issue.
- **Writing Code**: Submit pull requests with bug fixes, new features, or improvements to existing code.
- **Improving Documentation**: Help improve the documentation by fixing errors, clarifying explanations, or adding new content.
- **Writing Tests**: Ensure the reliability of the codebase by writing unit and integration tests.

## Getting Started

1. **Fork the Repository**: Fork the project on GitHub to your own account.
2. **Clone Your Fork**: Clone your fork to your local machine.
3. **Create a Branch:** Create a new branch for your changes.

## Code Contribution Guidelines

### Documentation

Ensure the code you are writing is properly documented using:
 
- **Class Documentation**: Every class should have a summary comment describing its purpose and functionality.
- **Method Documentation**: Each method should have comments describing its purpose, parameters, return values, and any exceptions that might be thrown.
- **Code Comments**: Use inline comments to explain complex logic or non-obvious code sections where needed.

> [!TIP]
> In most cases, code comments are a bad idea and extracting the logic into a dedicated, smaller method can help to increase readability.

### Testing

If you are helping to implement a feature or update the current code, chances are that you will also write some tests:

- **Unit Tests**: Each class should have corresponding unit tests that cover its methods and edge cases where relevant.
- **Integration Tests**: Features that add or change a feature should have integration tests that verify their functionality in a real-world scenario. In general, aim to at least test the happy path.

### Documentation

If the changes you add modify the public API, be sure to update the README accordingly.
As this is a guide for users, the README should reflect the current state of the project and how to use it.

### Branch Naming Conventions

For your branch, try to stick to the format `<issue id>-<title>`,
where `<issue id>` is the issue number without the `#` prefix, and `<title>` is a lowercase, dash-separated version of the feature or bug fix description.

> For example, `#8 feat(client): support sorting` should be implemented in a branch named `8-support-sorting`

### Commit Message Guidelines

Use clear and concise commit message, following [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/), with your issue number prefixed.

> For example: `#8 feat(client): add support for relationships`

## Submitting Changes

Congratulations on completing your task!

Here is how to submit it:

1. Push your changes to your fork on GitHub.
2. Navigate to the original repository on GitHub and open a pull request from your fork's branch to the main branch of the original repository.
3. In the pull request description, explain the changes you've made and why they're necessary, and if you've followed the guide (testing, documentation, ...).

## Reporting Bugs

If you find a bug, please open an issue with the following information:

1. A clear and concise description of what the bug is.
2. Provide a minimal reproduction of the bug if possible, or additional details if a reproduction is not possible.
3. What you expected to happen.
4. What actually happens.

## Suggesting Enhancements

If you have an idea for a new feature or improvement, open a discussion with the following information:

- What you would like to see as a feature or change.
- Why this enhancement would be useful and who it would benefit.
- If you have ideas on how this could be implemented, or if you are willing to help.

If the enhancement would be a great addition to the library, an issue will be created to add it to the backlog.

## Additional Guidelines

- **Be Respectful**.
- Before creating a new issue or pull request, check if there are existing ones that address the same topic.
- Each pull request should address a single issue or feature to make reviews easier.
