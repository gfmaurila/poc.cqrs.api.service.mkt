using Microsoft.AspNetCore.Mvc;
using poc.mock.sendgrid.Model;
using poc.mock.sendgrid.Service;

namespace poc.mock.sendgrid.Controllers;

[ApiController]
[Route("[controller]")]
public class SendGridEmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public SendGridEmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult<SendGridEmail>> Create(SendGridEmail email)
    {
        await _emailService.Create(email);

        return CreatedAtRoute("GetEmail", new { id = email.Id.ToString() }, email);
    }
}
