using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.Email;
using MediatR;

namespace Demo.Application.Users.Messages.SendUserInvitationEmail
{
    public class SendUserInvitationEmailMessageHandler : IRequestHandler<SendUserInvitationEmailMessage, Unit>
    {
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;
        private readonly IEmailSender _emailSender;
        private readonly IRazorViewRenderer _razorViewRenderer;
        private readonly IUserDomainEntity _userDomainEntity;

        public SendUserInvitationEmailMessageHandler(
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

        public async Task<Unit> Handle(SendUserInvitationEmailMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .WithOptions(x => x.AsNoTracking = true)
                .GetAsync(request.Data.UserId, cancellationToken);

            var toAddress = _userDomainEntity.Entity.Email;
            var subject = "Welcome to Demo!";

            var changePasswordUrl =
                await _auth0UserManagementClient.GetChangePasswordUrlAsync(_userDomainEntity.Entity, cancellationToken);

            var emailTemplateModel = new UserInvitationEmailBodyTemplateModel
            {
                ChangePasswordUrl = changePasswordUrl
            };

            var htmlContent = await _razorViewRenderer.RenderViewAsync(
                "~/Users/Messages/SendUserInvitationEmail/UserInvitationEmailBodyTemplate.cshtml", emailTemplateModel);

            await _emailSender.SendAsync(toAddress, subject, htmlContent, cancellationToken);

            return Unit.Value;
        }
    }
}