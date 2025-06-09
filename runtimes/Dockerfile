FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0

RUN apt-get update && \
    apt-get install -y git ripgrep && \
    rm -rf /var/lib/apt/lists/*

# Runtime container for ShellMuse agent tools
