FROM microsoft/aspnetcore-build
ARG labelversion
LABEL labelversion $labelversion
WORKDIR /app
EXPOSE 80
COPY dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
