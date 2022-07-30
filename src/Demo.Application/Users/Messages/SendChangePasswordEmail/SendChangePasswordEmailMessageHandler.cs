using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.Email;
using MediatR;

namespace Demo.Application.Users.Messages.SendChangePasswordEmail;

public class SendChangePasswordEmailMessageHandler : IRequestHandler<SendChangePasswordEmailMessage, Unit>
{
    private readonly IAuth0UserManagementClient _auth0UserManagementClient;
    private readonly IEmailSender _emailSender;
    private readonly IRazorViewRenderer _razorViewRenderer;
    private readonly IUserDomainEntity _userDomainEntity;

    public SendChangePasswordEmailMessageHandler(
        IUserDomainEntity userDomainEntity,
        IEmailSender emailSender,
        IAuth0UserManagementClient auth0UserManagementClient,
        IRazorViewRenderer razorViewRenderer
    )
    {
        _userDomainEntity = userDomainEntity;
        _emailSender = emailSender;
        _auth0UserManagementClient = auth0UserManagementClient;
        _razorViewRenderer = razorViewRenderer;
    }

    public async Task<Unit> Handle(SendChangePasswordEmailMessage request, CancellationToken cancellationToken)
    {
        await _userDomainEntity
            .WithOptions(x => x.AsNoTracking = true)
            .GetAsync(request.Data.UserId, cancellationToken);

        var toAddress = _userDomainEntity.Entity.Email;
        var subject = "Reset password";

        var changePasswordUrl =
            await _auth0UserManagementClient.GetChangePasswordUrlAsync(_userDomainEntity.Entity, cancellationToken);

        var emailTemplateModel = new ChangePasswordEmailBodyTemplateModel { ChangePasswordUrl = changePasswordUrl };

        var htmlContent = await _razorViewRenderer.RenderViewAsync(
            "~/CurrentUser/Messages/SendChangePasswordEmail/ChangePasswordEmailBodyTemplate.cshtml",
            emailTemplateModel);

        await _emailSender.SendAsync(toAddress, subject, htmlContent, cancellationToken);

        return Unit.Value;
    }
}
