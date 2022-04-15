#!/bin/bash

set -e

cd $(dirname "${BASH_SOURCE[0]}")

# TODO: seems like there should be a better way of doing this, like a proper task runner
dotnet run $1 $2 $3
