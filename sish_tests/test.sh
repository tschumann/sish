#!/bin/bash

set -eu

cd $(dirname "${BASH_SOURCE[0]}")

dotnet test --logger:"console;verbosity=detailed"
