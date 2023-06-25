# Estrutura da API
- ASP.NET Core 7.0: Framework para desenvolvimento da Microsoft.
- AutoMapper: Biblioteca para realizar mapeamento entre objetos.
- Swagger UI: Documentação para a API.
- Entity Framework Core 7.0
- XUnit
- FluentValidator
- MongoDb
- SQL Server
- Polly
- MediatR
- Serilog
- RabbitMQ
- Docker & Docker Compose

# Arquitetura
- CQRS
- Event Sourcing
- Unit of Work
- Repository Pattern
- Resut Pattern
- Domain Driven Design (Layers and Domain Model Pattern)
- Domain Events
- Domain Notification
- Domain Validations

# Executando a aplicação
- Add-Migration Inicial -Context MktDbContext
- Update-Database -Context MktDbContext
- docker-compose up --build

- http://localhost:5072/swagger/
- http://localhost:5072/WeatherForecast

# Twilio Docs
- https://www.twilio.com/pt-br/docs

# Mock Twilio para testar o consumo de api e o uso de RabbitMQ
- http://localhost:5073/TwilioWhatsApp

# API do SendGrid
- https://docs.sendgrid.com/pt-br/for-developers/sending-email/api-getting-started
- https://github.com/sendgrid/sendgrid-csharp

# Mock sendGrid para testar o consumo de api e o uso de RabbitMQ
- http://localhost:5074/sendgridemail

## Autor

- Guilherme Figueiras Maurila
 
[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila-58250026/)](https://www.linkedin.com/in/guilherme-maurila-58250026/)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)
