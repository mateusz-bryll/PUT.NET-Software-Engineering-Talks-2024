FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /opt/application
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o bin
EXPOSE 5274
ENTRYPOINT [ "./bin/Todo.WebApplication" ]