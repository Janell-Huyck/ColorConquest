# Repo root is wherever this Makefile lives (so paths work when you use
#   make -f "$(git rev-parse --show-toplevel)/Makefile" run
# from any subdirectory).
REPO_ROOT := $(dir $(abspath $(firstword $(MAKEFILE_LIST))))
MAUI_PROJECT := $(REPO_ROOT)ColorConquest/ColorConquest.csproj

# Run unit tests (Core + Tests only; no MAUI workloads required)
test:
	cd "$(REPO_ROOT)" && dotnet test ColorConquest.Tests/ColorConquest.Tests.csproj

# Run the MAUI app (requires workloads for the target platform).
# From a subfolder: make -C "$(git rev-parse --show-toplevel)" run
run:
	cd "$(REPO_ROOT)" && dotnet run --project "$(MAUI_PROJECT)"

.PHONY: test run
