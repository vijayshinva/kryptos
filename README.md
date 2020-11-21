# kryptos
A .NET core tool for cryptography.

![.NET Core](https://github.com/vijayshinva/kryptos/workflows/.NET%20Core/badge.svg)
[![CodeFactor](https://www.codefactor.io/repository/github/vijayshinva/kryptos/badge)](https://www.codefactor.io/repository/github/vijayshinva/kryptos)
[![NuGet version](https://badge.fury.io/nu/Kryptos.svg)](https://badge.fury.io/nu/Kryptos)

## Overview
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Features
- UUID generation
- Base64, Base64Url encoding and decoding
- MD5 Hash
- SHA-1, SHA-256, SHA-384, SHA-512 Hash
- JWT decoding
- HMAC-SHA1, HMAC-SHA256, HMAC-SHA384, HMAC-SHA512, HMAC-MD5
- Subresource Integrity

## Installation

**Kryptos** is available as a .NET Global Tool and can be installed using the .NET Core CLI.

```
dotnet tool install --global Kryptos
```

For offline installation, download the [nuget package][nuget-package] into a folder and run the following command.

```
dotnet tool install --global Kryptos --add-source FolderWithKryptosNuget\ 
```

**Kryptos** is under active development and new features are being added. Update to the latest version using the command below.
```
dotnet tool update --global Kryptos
```


## Usage

```
kryptos --help
```

## Examples

1. Decode Base64 encoded string
    ```
    kryptos base64 dec -t "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4="
    ```
2. Generate SHA-256 hash of file
    ```
    kryptos sha256 hash -i .\ubuntu-20.04-desktop-amd64.iso
    ```
3. Decode a JWT token
    ```
    kryptos jwt dec -t eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlZpamF5c2hpbnZhIEthcm51cmUiLCJpYXQiOjE1MTYyMzkwMjIsImF1ZCI6Imh0dHBzOi8vZ2l0aHViLmNvbS92aWpheXNoaW52YS9rcnlwdG9zIn0.ufklYra5bLYKM-FWnmxI0Tsw_ILmTIDK0cJ7ZkPfwfE
    ```
4. Generate Subresource Integrity
    ```
    kryptos sri hash -u https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css
    ```
5. Look up Cryptographic Object Identifier
    ```
    kryptos oid -t 1.3.6.1.5.5.7.3.1
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
[nuget-package]: https://www.nuget.org/packages/Kryptos