# kryptos
A .NET core tool for cryptography.

![.NET Core](https://github.com/vijayshinva/kryptos/workflows/.NET%20Core/badge.svg)
[![CodeFactor](https://www.codefactor.io/repository/github/vijayshinva/kryptos/badge)](https://www.codefactor.io/repository/github/vijayshinva/kryptos)
[![NuGet version](https://badge.fury.io/nu/Kryptos.svg)](https://badge.fury.io/nu/Kryptos)

## Overview
- [Features](#features)
- [Installation](#installation)
- [Update](#update)
- [Usage](#usage)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Features
- Base64 Encoding and Decoding
- MD5 Hash
- SHA-1, SHA-256, SHA-384, SHA-512 Hash

## Installation

**Kryptos** is available as a .NET Global Tool and can be installed using the .NET Core CLI.

```
dotnet tool install --global Kryptos
```

## Update
**Kryptos** is under active development and new features are being added. Update to the latest version using the command below.
```
dotnet tool update --global Kryptos
```

## Usage

```
kryptos --help
```

## Examples
1. Generate Base64 encoded string
    ```
    kryptos base64 enc -t "The quick brown fox jumps over the lazy dog."
    ```
2. Decode Base64 encoded string
    ```
    kryptos base64 dec -t "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4="
    ```
3. Generate SHA-256 hash of file
    ```
    kryptos sha256 hash -i .\ubuntu-20.04-desktop-amd64.iso
    ```

## Contributing
- Fork the repo on [GitHub][git-repo]
- Clone the project to your own machine
- Commit changes to your own branch
- Push your work back up to your fork
- Be sure to pull the latest from "upstream" before making a pull request!
- Submit a Pull Request so that changes can be reviewed and merged

NOTE: By raising a Pull Request you grant the guardians of this project the necessary ownership or grants of rights over all contributions to distribute under the [chosen license](https://github.com/vijayshinva/kryptos/blob/main/LICENSE).

## LICENSE
[![license](https://img.shields.io/github/license/vijayshinva/kryptos.svg)](https://github.com/vijayshinva/kryptos/blob/main/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fvijayshinva%2Fkryptos.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fvijayshinva%2Fkryptos?ref=badge_shield)

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fvijayshinva%2Fkryptos.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fvijayshinva%2Fkryptos?ref=badge_large)



[git-repo]: https://github.com/vijayshinva/kryptos