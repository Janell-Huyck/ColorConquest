# Run unit tests (Core + Tests only; no MAUI workloads required)
test:
	dotnet test ColorConquest.Tests/ColorConquest.Tests.csproj

.PHONY: test
