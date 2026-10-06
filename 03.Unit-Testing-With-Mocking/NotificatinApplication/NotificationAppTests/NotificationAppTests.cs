using Moq;
using NUnit.Framework;
using NotificationApp.Entities;
using NotificationApp.Interfaces;
using NotificationApp.Services;
using System;

namespace NotificationApp.Tests
{
    public class NotificationAppTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<INotifier> _mockNotifier;
        private NotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            // Arrange: Create mock objects for the dependencies
            _mockUserRepo = new Mock<IUserRepository>();
            _mockNotifier = new Mock<INotifier>();
            _notificationService = new NotificationService(_mockUserRepo.Object, _mockNotifier.Object);
        }

        [Test]
        public void NotifyUser_WithValidActiveUser_CallsSend()
        {
            // Arrange: Mock an active user with valid email
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                IsActive = true,
            };

            _mockUserRepo.Setup(repo => repo.GetUserById(1)).Returns(user);

            // Act: Call the method
            _notificationService.NotifyUser(1, "Test Message");

            // Assert: Verify that Send was called once with correct parameters
            _mockNotifier.Verify(notifier => notifier.Send("test@example.com", "Test Message"), Times.Once);

        }

        [Test]
        public void NotifyUser_WithInactiveUser_ThrowsInvalidOperationException()
        {
            // Arrange: Mock an inactive user
            var user = new User
            {
                Id = 2,
                Email = "test@example.com",
                IsActive = false
            };
            _mockUserRepo.Setup(repo => repo.GetUserById(2)).Returns(user);



            // Act & Assert: Expect an exception, and Send should not be called
            var exception = Assert.Throws<InvalidOperationException>(() => _notificationService.NotifyUser(2, "Test Message"));
            Assert.That(exception.Message, Is.EqualTo("Cannot notify inactive user."));
            _mockNotifier.Verify(notifier => notifier.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [Test]
        public void NotifyUser_WithNonExistentUser_ThrowsArgumentException()
        {
            _mockUserRepo.Setup(repo => repo.GetUserById(99)).Returns((User)null!);
            var exception = Assert.Throws<ArgumentException>(() => _notificationService.NotifyUser(99, "Test message"));
            Assert.That(exception.Message, Is.EqualTo("User not found."));
            _mockNotifier.Verify(notifier => notifier.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void NotifyUser_WithEmptyMessage_ThrowsArgumentException(string? message)
        {

            var exception = Assert.Throws<ArgumentException>(() => _notificationService.NotifyUser(1, message));
            Assert.That(exception.Message, Is.EqualTo("Message cannot be empty."));
            _mockNotifier.Verify(notifier => notifier.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
