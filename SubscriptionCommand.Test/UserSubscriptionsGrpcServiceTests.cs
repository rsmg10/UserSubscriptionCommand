using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SubscriptionCommand.Commands.SendInvitation;
using SubscriptionCommand.Infrastructure.Presistance;
using SubscriptionCommandProto; 
using Xunit.Abstractions;

namespace SubscriptionCommand.Test
{
    public class UserSubscriptionsGrpcServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
         
        public UserSubscriptionsGrpcServiceTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput )
        {
            _factory = factory.WithDefaultConfigurations(testOutput, services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<ApplicationDatabase>));
                services.Remove(descriptor);
                var dbName = Guid.NewGuid().ToString();
                services.AddDbContext<ApplicationDatabase>(options => options.UseInMemoryDatabase(dbName));
            });
        } 

        // test cases 

        [Fact]
        public async Task SendInvitation_SendValidData_InvitationSent()
        {
            var client = new SubscriptionCommandProto.SubscriptionCommand.SubscriptionCommandClient(_factory.CreateGrpcChannel());
            var res = await client.SendInvitationAsync(new SubscriptionCommandProto.SendInvitationRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                MemberId = Guid.NewGuid().ToString(),
                Permission = 1,
                SubscriptionId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
            });
              

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDatabase>();
                await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(await context.Events.ToListAsync()));
                var eventCount = await context.Events.CountAsync();
                Assert.Equal(1, eventCount); 
            }
  
            Assert.NotNull(res);
            return;
        }


        [Theory]
        [InlineData("BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", "BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", 0, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "1377561B-1C64-4DF9-8767-29C1C05F75B1")]
        [InlineData("2E576522-615F-4B43-BEE2-14413F5011C0", "", 1, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "1377561B-1C64-4DF9-8767-29C1C05F75B1")]
        [InlineData("", "BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", 0, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "1377561B-1C64-4DF9-8767-29C1C05F75B1")]
        [InlineData("318525A4-0DE4-4A3B-BFD9-1928F43FB493", "BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", 1, "", "1377561B-1C64-4DF9-8767-29C1C05F75B1")] 
        [InlineData("AA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", "BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", 1, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "")] 

        public async Task SendInvitation_SendInvalidData_ValidationError(string accountId, string memberId, int permission, string subscriptionId, string userId)
        {
            // send invitation using grpc and assert validation exception 
 
            var client = new SubscriptionCommandProto.SubscriptionCommand.SubscriptionCommandClient(_factory.CreateGrpcChannel());
        
            var ex = await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationAsync(new SubscriptionCommandProto.SendInvitationRequest
                {
                    AccountId = accountId,
                    MemberId = memberId,
                    Permission = permission,
                    SubscriptionId = subscriptionId,
                    UserId = userId
                });
            });
            Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        }
        
        [Theory]
        [InlineData("BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", "1377561B-1C64-4DF9-8767-29C1C05F75B1", 7, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "1377561B-1C64-4DF9-8767-29C1C05F75B1")]
 
        public async Task SendInvitation_UserIdEqualMemberId_ValidationError(string accountId, string memberId, int permission, string subscriptionId, string userId)
        {
            // send invitation using grpc and assert validation exception 
 
            var client = new SubscriptionCommandProto.SubscriptionCommand.SubscriptionCommandClient(_factory.CreateGrpcChannel());
        
            var ex = await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationAsync(new SubscriptionCommandProto.SendInvitationRequest
                {
                    AccountId = accountId,
                    MemberId = memberId,
                    Permission = permission,
                    SubscriptionId = subscriptionId,
                    UserId = userId
                });
            });
            Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        }
        [Fact]
        // [InlineData("BA66601C-8A7E-43BC-BC95-BDCF6F5B79A3", "1377561B-1C64-4DF9-8767-29C1C05F75B1", 7, "9A687FB1-5555-4B39-BBDD-8202C0E77038", "1377561B-1C64-4DF9-8767-29C1C05F75B1")]
 
        public async Task SendAndAcceptInvitation_Valid()
        {
            var userId = Guid.NewGuid().ToString();
            var memberId = Guid.NewGuid().ToString();
            var subscriptionId = Guid.NewGuid().ToString();
            var accountId = Guid.NewGuid().ToString();
            
            var client = new SubscriptionCommandProto.SubscriptionCommand.SubscriptionCommandClient(_factory.CreateGrpcChannel());
            var sendInvitationAsync = await client.SendInvitationAsync(new SubscriptionCommandProto.SendInvitationRequest
            {
                AccountId = accountId,
                MemberId = memberId,
                Permission = 7,
                SubscriptionId = subscriptionId,
                UserId = userId
            });
            
            Assert.NotNull(sendInvitationAsync);
 
            var acceptInvitationAsync = await client.AcceptInvitationAsync(new SubscriptionCommandProto.AcceptInvitationRequest()
            {
                AccountId = accountId,
                MemberId = memberId, 
                SubscriptionId = subscriptionId,
                UserId = userId
            });
            
            Assert.NotNull(acceptInvitationAsync);
            
        }

        

    }
}
