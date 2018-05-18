FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016
LABEL multi.label1="value1"
WORKDIR /app
EXPOSE 80
COPY ./dist/portal portal/
WORKDIR /app/portal
ENTRYPOINT ["dotnet", "TestJenkins.dll"]
