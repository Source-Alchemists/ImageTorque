# Contributing

First off, thank you for considering contributing to ImageTorque. We want to keep it as easy as possible to contribute changes that get things working in your environment.

## Where do I go from here?

If you've noticed a bug or have a feature request, [make one](https://github.com/Source-Alchemists/ImageTorque/issues/new)! It's generally best if you get confirmation of your bug or approval for your feature request this way before starting to code.

## Fork & create a branch

If this is something you think you can fix, then [fork ImageTorque](https://github.com/Source-Alchemists/ImageTorque/fork) and create a branch with a descriptive name.

A good branch name would be (where issue #777 is the ticket you're working on):

```sh
git checkout -b 777-png-resize
```

## Get the style right

We provding a [`.editorconfig`](https://editorconfig.org/) file to help you with the style of the project. Most editors support EditorConfig, but you may need to install a plugin.

## Make a Pull Request

At this point, you should switch back to your main branch and make sure it's up to date with ImageTorque's main branch:

```sh
git remote add upstream git@github.com:Source-Alchemists/ImageTorque.git
git checkout main
git pull upstream main
```

Then update your feature branch from your local main branch, and push it!

```sh
git checkout 777-fix-png-resize
git rebase main
git push --set-upstream origin 777-fix-png-resize
```

Finally, go to the [ImageTorque repo](https://github.com/Source-Alchemists/ImageTorque) and make a Pull Request.

## Keep your Pull Request updated

If we ask you to rebase your Pull Request, you can do it by:

```sh
git checkout 777-fix-png-resize
git pull --rebase upstream main
git push --force-with-lease 777-fix-png-resize
```
